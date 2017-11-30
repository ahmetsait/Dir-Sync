using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DirSync
{
	public static class Extensions
	{
		/// <summary>
		/// Calculates total file or directory size by recursively traversing the file system.
		/// This function omits files and directories that cannot be enumerated succesfully.
		/// </summary>
		/// <param name="fileSystemInfo">The file system object to calculate.</param>
		/// <returns>Calculated file system size</returns>
		public static long GetTotalSize(this FileSystemInfo fileSystemInfo)
		{
			if (fileSystemInfo == null)
				throw new ArgumentNullException("fileSystemInfo");
			fileSystemInfo.Refresh();
			if (fileSystemInfo is FileInfo)
				return ((FileInfo)fileSystemInfo).Length;
			else if (fileSystemInfo is DirectoryInfo)
			{
				DirectoryInfo folder = (DirectoryInfo)fileSystemInfo;
				long folderSize = 0L;
				try
				{
					foreach (var file in folder.EnumerateFiles())
						if (file.isRelevant())
							folderSize += file.Length;

					foreach (var dir in folder.EnumerateDirectories())
						folderSize += dir.GetTotalSize();
				}
				catch (UnauthorizedAccessException) { }
				catch (NotSupportedException) { }

				return folderSize;
			}
			else return 0L;
		}

		/// <summary>
		/// Returns human readable representation of a file size.
		/// </summary>
		public static string GetFileSizeString(this FileSystemInfo fileSystemInfo)
		{
			return GetFileSizeString(fileSystemInfo.GetTotalSize());
		}

		/// <summary>
		/// Returns human readable representation of a file size.
		/// </summary>
		public static string GetFileSizeString(long size)
		{
			if (size < 1024 * 1024)
				return string.Format("{0:F2} KB", size / 1024f);
			else if (size < 1024 * 1024 * 1024)
				return string.Format("{0:F2} MB", size / 1024f / 1024f);
			else
				return string.Format("{0:F3} GB", size / 1024f / 1024f / 1024f);
		}

		/// <summary>
		/// Returns whether the file system object is worth synchronizing.
		/// </summary>
		public static bool isRelevant(this FileSystemInfo fileSystemInfo)
		{
			return (!fileSystemInfo.Attributes.HasFlag(FileAttributes.System) &&
				!fileSystemInfo.Attributes.HasFlag(FileAttributes.ReadOnly));
		public static string CombinePaths(string path1, string path2)
		{
			return path1.TrimEnd('\\') + '\\' + path2.TrimStart('\\');
		}

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

		private const int WM_SETREDRAW = 11;

		public static void SuspendDrawing(this Control parent)
		{
			SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
		}

		public static void ResumeDrawing(this Control parent)
		{
			SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
			parent.Refresh();
		}
	}
}
