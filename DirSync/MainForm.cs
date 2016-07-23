using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;
using System.Globalization;

namespace DirSync
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			Application.ThreadException += Application_ThreadException;
			
			if (File.Exists(Program.configPath))
			{
				string[] paths = File.ReadAllLines(Program.configPath, Encoding.Default);
				if (paths.Length == 2)
				{
					textBox_PathComputer.Text = paths[0];
					textBox_PathDesktop.Text = paths[1];
				}
			}
		}

		void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString(), "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void backgroundWorker_Sync_DoWork(object sender, DoWorkEventArgs e)
		{
			lock (syncLock)
			{
				SyncList result = new SyncList();
				DirectoryInfo dirInfo1 = new DirectoryInfo(dir1), dirInfo2 = new DirectoryInfo(dir2);

				try
				{
					Diff(dirInfo1, dirInfo2, result);
				}
				catch (CancelationException) { }

				e.Result = result;
			}
		}

		enum PairNullness
		{
			None,
			First,
			Second
		}

		public enum SyncAction
		{
			DoNothing,
			CopyToLeft,
			CopyToRight,
			Delete
		}

		const string toLeft = "<--", toRight = "-->", delete = "X", doNothing = "●", tire = "-";

		private void backgroundWorker_Sync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			button_Sync.Text = "Sync";
			toolStripStatusLabel_Dir.Text = "";
			if(e.Error != null)
			{
				MessageBox.Show(e.Error.ToString(), "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (e.Result != null)
			{
				SyncList result = (SyncList)e.Result;
				listView.Items.Clear();
				foreach (var filePair in result.filePairs)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Tag = filePair;

					if (filePair.Item1 != null)
					{
						lvi.Text = filePair.Item1.FullName;
						lvi.SubItems.Add(filePair.Item1.GetFileSizeString());
						lvi.SubItems.Add(filePair.Item1.LastWriteTime.ToString());
					}
					else
					{
						lvi.Text = tire;
 						lvi.SubItems.Add(tire);
						lvi.SubItems.Add(tire);
					}
					// Decide action
					if (filePair.Item1 != null && filePair.Item2 == null)
					{
						lvi.SubItems.Add(toRight).Tag = PairNullness.Second;
					}
					else if (filePair.Item1 == null && filePair.Item2 != null)
					{
						lvi.SubItems.Add(toLeft).Tag = PairNullness.First;
					}
					else if (filePair.Item1 != null && filePair.Item2 != null)
					{
						if(filePair.Item1.LastWriteTime > filePair.Item2.LastWriteTime)
							lvi.SubItems.Add(toRight).Tag = PairNullness.None;
						else if(filePair.Item1.LastWriteTime < filePair.Item2.LastWriteTime)
							lvi.SubItems.Add(toLeft).Tag = PairNullness.None;
					}
					else
					{
						throw new ApplicationException("Logic Error");
					}

					if (filePair.Item2 != null)
					{
						lvi.SubItems.Add(filePair.Item2.LastWriteTime.ToString());
						lvi.SubItems.Add(filePair.Item2.FullName);
 						lvi.SubItems.Add(filePair.Item2.GetFileSizeString());
					}
					else
					{
 						lvi.SubItems.Add(tire);
 						lvi.SubItems.Add(tire);
						lvi.SubItems.Add(tire);
					}

					lvi.Group = listView.Groups[1];
					listView.Items.Add(lvi);
				}
				
				foreach (var dirPair in result.dirPairs)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Tag = dirPair;

					if (dirPair.Item1 != null)
					{
						lvi.Text = dirPair.Item1.FullName;
						lvi.SubItems.Add(dirPair.Item1.GetFileSizeString());
						lvi.SubItems.Add(dirPair.Item1.LastWriteTime.ToString());
					}
					else
					{
						lvi.Text = tire;
 						lvi.SubItems.Add(tire);
						lvi.SubItems.Add(tire);
					}
					// Decide action
					if (dirPair.Item1 != null && dirPair.Item2 == null)
					{
						lvi.SubItems.Add(toRight).Tag = PairNullness.Second;
					}
					else if (dirPair.Item1 == null && dirPair.Item2 != null)
					{
						lvi.SubItems.Add(toLeft).Tag = PairNullness.First;
					}
					else if (dirPair.Item1 != null && dirPair.Item2 != null)
					{
						if(dirPair.Item1.LastWriteTime > dirPair.Item2.LastWriteTime)
						{
							lvi.SubItems.Add(toRight).Tag = PairNullness.None;
						}
						else if(dirPair.Item1.LastWriteTime < dirPair.Item2.LastWriteTime)
						{
							lvi.SubItems.Add(toLeft).Tag = PairNullness.None;
						}
					}
					else
					{
						throw new ApplicationException("Logic Error");
					}

					if (dirPair.Item2 != null)
					{
						lvi.SubItems.Add(dirPair.Item2.LastWriteTime.ToString());
						lvi.SubItems.Add(dirPair.Item2.FullName);
 						lvi.SubItems.Add(dirPair.Item2.GetFileSizeString());
					}
					else
					{
 						lvi.SubItems.Add(tire);
 						lvi.SubItems.Add(tire);
						lvi.SubItems.Add(tire);
					}

					lvi.Group = listView.Groups[0];
					listView.Items.Add(lvi);
				}
				listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		private void button_BrowseComputer_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog_Computer.ShowDialog() == DialogResult.OK)
				textBox_PathComputer.Text = folderBrowserDialog_Computer.SelectedPath;
		}

		private void button_BrowseDesktop_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog_Desktop.ShowDialog() == DialogResult.OK)
				textBox_PathDesktop.Text = folderBrowserDialog_Desktop.SelectedPath;
		}

		private void button_Sync_Click(object sender, EventArgs e)
		{
			if (!backgroundWorker_Sync.IsBusy)
			{
				if (!Directory.Exists(textBox_PathComputer.Text))
				{
					MessageBox.Show(this, "Directory does not exist:\r\n" + textBox_PathComputer.Text, "DirSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (!Directory.Exists(textBox_PathDesktop.Text))
				{
					MessageBox.Show(this, "Directory does not exist:\r\n" + textBox_PathDesktop.Text, "DirSync", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				dir1 = textBox_PathComputer.Text;
				dir2 = textBox_PathDesktop.Text;
				backgroundWorker_Sync.RunWorkerAsync();
				button_Sync.Text = "...";
				
			}
			else
				backgroundWorker_Sync.CancelAsync();
		}

		object syncLock = new object();
		string dir1, dir2;

		[Serializable]
		public class CancelationException : Exception
		{
			public CancelationException() { }
			public CancelationException(string message) : base(message) { }
			public CancelationException(string message, Exception inner) : base(message, inner) { }
			protected CancelationException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}

		private const int BYTES_TO_READ = sizeof(Int64);

		public static bool FilesAreEqual(FileInfo first, FileInfo second)
		{
			if (first.Length != second.Length)
				return false;

			int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

			using (FileStream fs1 = first.OpenRead())
			using (FileStream fs2 = second.OpenRead())
			{
				byte[] one = new byte[BYTES_TO_READ];
				byte[] two = new byte[BYTES_TO_READ];

				for (int i = 0; i < iterations; i++)
				{
					fs1.Read(one, 0, BYTES_TO_READ);
					fs2.Read(two, 0, BYTES_TO_READ);

					if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
						return false;
				}
			}

			return true;
		}

		private void Diff(DirectoryInfo node1, DirectoryInfo node2, SyncList syncList)
		{
			if (backgroundWorker_Sync.CancellationPending)
				return;
			if (statusStrip.InvokeRequired)
				statusStrip.Invoke(new Action(() => toolStripStatusLabel_Dir.Text = node1.FullName.Substring(dir1.Length)));
			var md5 = MD5.Create();
			var files1 = node1.EnumerateFiles();
			var files2 = node2.EnumerateFiles();
			foreach (FileInfo file in files1)
			{
				if (!file.isRelevant())
					continue;
				FileInfo mirror = files2.FirstOrDefault(f => string.Equals(f.Name, file.Name, StringComparison.InvariantCultureIgnoreCase));
				if (mirror == null)
				{
					syncList.filePairs.Add(new Pair<FileInfo>(file, null));
				}
				else
				{
					if (file.Length != mirror.Length
						|| (file.LastWriteTimeUtc - mirror.LastWriteTimeUtc).Duration() > TimeSpan.FromHours(1))
						syncList.filePairs.Add(new Pair<FileInfo>(file, mirror));
					else if (file.LastWriteTimeUtc != mirror.LastWriteTimeUtc)
						if (!FilesAreEqual(file, mirror))
								syncList.filePairs.Add(new Pair<FileInfo>(file, mirror));
				}
			}
			foreach (FileInfo file in files2)
			{
				if (!file.isRelevant())
					continue;
				if (!files1.Any(f => string.Equals(f.Name, file.Name, StringComparison.InvariantCultureIgnoreCase)))
					syncList.filePairs.Add(new Pair<FileInfo>(null, file));
			}
			//
			var dirs1 = node1.EnumerateDirectories();
			var dirs2 = node2.EnumerateDirectories();
			foreach (DirectoryInfo dir in dirs1)
			{
				if (!dir.isRelevant())
					continue;
				DirectoryInfo mirror = dirs2.FirstOrDefault(d => string.Equals(d.Name, dir.Name, StringComparison.InvariantCultureIgnoreCase));
				if (mirror == null)
					syncList.dirPairs.Add(new Pair<DirectoryInfo>(dir, null));
				else
					Diff(dir, mirror, syncList);
			}
			foreach (DirectoryInfo dir in dirs2)
			{
				if (!dir.isRelevant())
					continue;
				DirectoryInfo mirror = dirs1.FirstOrDefault(d => string.Equals(d.Name, dir.Name, StringComparison.InvariantCultureIgnoreCase));
				if (mirror == null)
					syncList.dirPairs.Add(new Pair<DirectoryInfo>(null, dir));
			}
		}
		
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (backgroundWorker_Sync.IsBusy)
			{
				backgroundWorker_Sync.CancelAsync();
				while (backgroundWorker_Sync.IsBusy) ;
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			File.WriteAllText(Program.configPath, textBox_PathComputer.Text + Environment.NewLine + textBox_PathDesktop.Text, Encoding.Default);
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			//if (e.KeyData == Keys.Escape)
			//{
			//	if (backgroundWorker_Sync.IsBusy)
			//		backgroundWorker_Sync.CancelAsync();
			//	else
			//		this.Close();
			//}
		}

		private void listView_MouseClick(object sender, MouseEventArgs e)
		{
			if (!Monitor.TryEnter(bakeLock))
				return;

			if (e.Button == MouseButtons.Right && listView.SelectedItems.Count == 1)
			{
				ListViewItem item = listView.SelectedItems[0];
				var sub = item.SubItems[3];
				string act = sub.Text;
				PairNullness nullness = (PairNullness)sub.Tag;

				switch (act)
				{
					case doNothing:
						if (nullness != PairNullness.First)
							sub.Text = toRight;
						else
							goto case toRight;
						break;
					case toRight:
						if (nullness != PairNullness.Second)
							sub.Text = toLeft;
						else
							goto case toLeft;
						break;
					case toLeft:
						sub.Text = delete;
						break;
					case delete:
						sub.Text = doNothing;
						break;
					default:
						Debug.Fail("Logic error");
						break;
				}
			}
		}
		
		class Bakery
		{
			public List<Tuple<DirectoryInfo, DirectoryInfo, SyncAction>> directories;
			public List<Tuple<FileInfo, FileInfo, SyncAction>> files;
		}
		
		object bakeLock = new object();
		Bakery bakery;

		private void MainForm_Load(object sender, EventArgs e)
		{

		}

		private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				foreach (ListViewItem item in listView.SelectedItems)
				{
					string path1, path2;
					if (item.Tag is Pair<FileInfo>)
					{
						var pair = ((Pair<FileInfo>)item.Tag);
						path1 = pair.Item1 != null ? pair.Item1.FullName : null;
						path2 = pair.Item2 != null ? pair.Item2.FullName : null;
					}
					else if (item.Tag is Pair<DirectoryInfo>)
					{
						var pair = ((Pair<DirectoryInfo>)item.Tag);
						path1 = pair.Item1 != null ? pair.Item1.FullName : null;
						path2 = pair.Item2 != null ? pair.Item2.FullName : null;
					}
					else
					{
						Debug.Fail("Logic error");
						return;
					}

					if (path1 != null)
					{
						var explorer = new ProcessStartInfo("explorer.exe", "/select," + path1);
						Process.Start(explorer);
					}
					if (path2 != null)
					{
						var explorer = new ProcessStartInfo("explorer.exe", "/select," + path2);
						Process.Start(explorer);
					}
				}
			}
		}

		private void button_Bake_Click(object sender, EventArgs e)
		{
			if (!backgroundWorker_Bake.IsBusy)
			{
				foreach (ListViewItem item in listView.Items)
				{

				}

				progressBar.Value = progressBar.Minimum;
				backgroundWorker_Bake.RunWorkerAsync();
			}
			else
				backgroundWorker_Bake.CancelAsync();
		}

		private void backgroundWorker_Bake_DoWork(object sender, DoWorkEventArgs e)
		{
			lock (bakeLock)
			{

			}
		}

		private void backgroundWorker_Bake_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{

		}

		private void backgroundWorker_Bake_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

		}
	}
}
