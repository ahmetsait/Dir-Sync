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

		object syncLock = new object();
		string dir1, dir2;

		private void backgroundWorker_Sync_DoWork(object sender, DoWorkEventArgs e)
		{
			lock (syncLock)
			{
				SyncList result = new SyncList();
				DirectoryInfo dirInfo1 = new DirectoryInfo(dir1), dirInfo2 = new DirectoryInfo(dir2);

				Diff(dirInfo1, dirInfo2, result);

				e.Result = result;
			}
		}

		private void backgroundWorker_Sync_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			toolStripStatusLabel.Text = ((string)e.UserState);
		}

		enum PairNullness
		{
			None,
			First,
			Second
		}

		const string toLeft = "<--", toRight = "-->", delete = "X", doNothing = "●", tire = "-";

		private void backgroundWorker_Sync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			button_Sync.Text = "Sync";
			button_Bake.Enabled = true;
			toolStripStatusLabel.Text = "";
			if(e.Error != null)
			{
				MessageBox.Show(e.Error.ToString(), "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (e.Result != null)
			{
				SyncList result = (SyncList)e.Result;
				try
				{
					listView.SuspendLayout();
					listView.SuspendDrawing();
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
							if (filePair.Item1.IsReadOnly || filePair.Item2.IsReadOnly)
							{
								lvi.SubItems.Add(doNothing).Tag = PairNullness.None;
								lvi.ForeColor = Color.Sienna;
							}
							else if (filePair.Item1.LastWriteTime > filePair.Item2.LastWriteTime)
								lvi.SubItems.Add(toRight).Tag = PairNullness.None;
							else if (filePair.Item1.LastWriteTime < filePair.Item2.LastWriteTime)
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
							if (dirPair.Item1.LastWriteTime > dirPair.Item2.LastWriteTime)
								lvi.SubItems.Add(toRight).Tag = PairNullness.None;
							else if (dirPair.Item1.LastWriteTime < dirPair.Item2.LastWriteTime)
								lvi.SubItems.Add(toLeft).Tag = PairNullness.None;
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
				}
				finally
				{
					listView.ResumeDrawing();
					listView.ResumeLayout();
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
				button_Sync.Text = "Stop";
				button_Bake.Enabled = false;
				progressBar.Value = progressBar.Minimum;
				backgroundWorker_Sync.RunWorkerAsync();
			}
			else
				backgroundWorker_Sync.CancelAsync();
		}

		private void Diff(DirectoryInfo node1, DirectoryInfo node2, SyncList syncList)
		{
			if (backgroundWorker_Sync.CancellationPending)
				return;
			backgroundWorker_Sync.ReportProgress(0, node1.FullName.Substring(dir1.Length));

			var files1 = node1.EnumerateFiles();
			var files2 = node2.EnumerateFiles();
			foreach (FileInfo file in files1)
			{
				if (backgroundWorker_Sync.CancellationPending)
					return;
				if (!file.isRelevant())
					continue;
				FileInfo mirror = files2.FirstOrDefault(f => string.Equals(f.Name, file.Name, StringComparison.InvariantCultureIgnoreCase));
				if (mirror == null)
					syncList.filePairs.Add(new Pair<FileInfo>(file, null));
				else if (mirror.isRelevant())
					if (file.Length != mirror.Length || (file.LastWriteTimeUtc - mirror.LastWriteTimeUtc).Duration() > TimeSpan.FromMinutes(1))	// Because fuck Windows
						syncList.filePairs.Add(new Pair<FileInfo>(file, mirror));
					//else if (file.LastWriteTimeUtc != mirror.LastWriteTimeUtc && !FilesAreEqual(file, mirror))	// This is unnecessarily slow
					//	syncList.filePairs.Add(new Pair<FileInfo>(file, mirror));									// And don't even think about hash checking
			}
			foreach (FileInfo file in files2)
			{
				if (!file.isRelevant())
					continue;
				if (!files1.Any(f => string.Equals(f.Name, file.Name, StringComparison.InvariantCultureIgnoreCase)))
					syncList.filePairs.Add(new Pair<FileInfo>(null, file));
			}
			
			var dirs1 = node1.EnumerateDirectories();
			var dirs2 = node2.EnumerateDirectories();
			foreach (DirectoryInfo dir in dirs1)
			{
				if (backgroundWorker_Sync.CancellationPending)
					return;
				if (!dir.isRelevant())
					continue;
				DirectoryInfo mirror = dirs2.FirstOrDefault(d => string.Equals(d.Name, dir.Name, StringComparison.InvariantCultureIgnoreCase));
				if (mirror == null)
					syncList.dirPairs.Add(new Pair<DirectoryInfo>(dir, null));
				else
					if (mirror.isRelevant())
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
			if (!Directory.Exists(Program.configPath))
				Directory.CreateDirectory(Path.GetDirectoryName(Program.configPath));
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
			if (e.Button == MouseButtons.Right)
			{
				foreach (ListViewItem item in listView.SelectedItems)
				{
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
							throw new ApplicationException("Logic Error");
					}
				}
			}
		}
		
		private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
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
						throw new ApplicationException("Logic Error");
					}

					path1 = path1 != null ? "/select," + path1 : Extensions.CombinePaths(dir1, Path.GetDirectoryName(path2).Substring(dir2.Length));
					path2 = path2 != null ? "/select," + path2 : Extensions.CombinePaths(dir2, Path.GetDirectoryName(path1).Substring(dir1.Length));
					Process.Start(new ProcessStartInfo("explorer.exe", path1));
					Process.Start(new ProcessStartInfo("explorer.exe", path2));
				}
			}
		}
		
		private void button_Bake_Click(object sender, EventArgs e)
		{
			if (!backgroundWorker_Bake.IsBusy)
			{
				BakeList bakeList = new BakeList();
				foreach (ListViewItem item in listView.Items)
				{
					if (item.Tag is Pair<FileInfo>)
					{
						Pair<FileInfo> filePair = (Pair<FileInfo>)item.Tag;
						string actionStr = item.SubItems[3].Text;
						switch (actionStr)
						{
							case toRight:
								bakeList.files.Add(Tuple.Create(filePair.Item1, filePair.Item2, SyncAction.CopyToRight));
								break;
							case toLeft:
								bakeList.files.Add(Tuple.Create(filePair.Item1, filePair.Item2, SyncAction.CopyToLeft));
								break;
							case delete:
								bakeList.files.Add(Tuple.Create(filePair.Item1, filePair.Item2, SyncAction.Delete));
								break;
							case doNothing:
								continue;
							default:
								break;
						}
					}
					else if (item.Tag is Pair<DirectoryInfo>)
					{
						Pair<DirectoryInfo> dirPair = (Pair<DirectoryInfo>)item.Tag;
						string actionStr = item.SubItems[3].Text;
						switch (actionStr)
						{
							case toLeft:
								bakeList.directories.Add(Tuple.Create(dirPair.Item1, dirPair.Item2, SyncAction.CopyToLeft));
								break;
							case toRight:
								bakeList.directories.Add(Tuple.Create(dirPair.Item1, dirPair.Item2, SyncAction.CopyToRight));
								break;
							case delete:
								bakeList.directories.Add(Tuple.Create(dirPair.Item1, dirPair.Item2, SyncAction.Delete));
								break;
							case doNothing:
								continue;
							default:
								break;
						}
					}
					else
						throw new ApplicationException("Logic Error");
				}
				progressBar.Value = progressBar.Minimum;
				button_Sync.Enabled = false;
				listView.Enabled = false;
				button_Bake.Text = "Stop";
				backgroundWorker_Bake.RunWorkerAsync(bakeList);
			}
			else
				backgroundWorker_Bake.CancelAsync();
		}

		private void backgroundWorker_Bake_DoWork(object sender, DoWorkEventArgs e)
		{
			lock (syncLock)
			{
				BakeList bakeList = (BakeList)e.Argument;
				int maxProgress = bakeList.directories.Count + bakeList.files.Count, progress = 0;
				foreach (var bake in bakeList.files)
				{
					bake.Item1?.Refresh();
					bake.Item2?.Refresh();
					switch (bake.Item3)
					{
						case SyncAction.CopyToLeft:
							if (bake.Item1.IsReadOnly)
								bake.Item1.IsReadOnly = false;
							bake.Item2.CopyTo(Extensions.CombinePaths(dir1, bake.Item2.FullName.Substring(dir2.Length)), true);
							break;
						case SyncAction.CopyToRight:
							if (bake.Item2.IsReadOnly)
								bake.Item2.IsReadOnly = false;
							bake.Item1.CopyTo(Extensions.CombinePaths(dir2, bake.Item1.FullName.Substring(dir1.Length)), true);
							break;
						case SyncAction.Delete:
							bake.Item1?.Delete();
							bake.Item2?.Delete();
							break;
						case SyncAction.DoNothing:
							continue;
						default:
							break;
					}
					backgroundWorker_Bake.ReportProgress(++progress * 100 / maxProgress, (bake.Item1 ?? bake.Item2).FullName.Substring(dir1.Length));
				}
				foreach (var bake in bakeList.directories)
				{
					bake.Item1?.Refresh();
					bake.Item2?.Refresh();
					switch (bake.Item3)
					{
						case SyncAction.CopyToLeft:
							FileSystem.CopyDirectory(bake.Item2.FullName, Extensions.CombinePaths(dir1, bake.Item2.FullName.Substring(dir2.Length)));
							break;
						case SyncAction.CopyToRight:
							FileSystem.CopyDirectory(bake.Item1.FullName, Extensions.CombinePaths(dir2, bake.Item1.FullName.Substring(dir1.Length)));
							break;
						case SyncAction.Delete:
							bake.Item1?.Delete(true);
							bake.Item2?.Delete(true);
							break;
						case SyncAction.DoNothing:
							continue;
						default:
							break;
					}
					backgroundWorker_Bake.ReportProgress(++progress * 100 / maxProgress, (bake.Item1 ?? bake.Item2).FullName.Substring(dir1.Length));
				}
			}
		}

		private void backgroundWorker_Bake_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar.Value = e.ProgressPercentage;
			toolStripStatusLabel.Text = (string)(e.UserState);
		}

		private void backgroundWorker_Bake_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			listView.Enabled = true;
			button_Sync.Enabled = true;
			button_Bake.Text = "Bake";
			toolStripStatusLabel.Text = "";
			if (e.Error != null)
				MessageBox.Show(e.Error.ToString(), "Bake Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
			{
				for (int i = listView.Items.Count - 1; i >= 0; i--)
				{
					var item = listView.Items[i];
					string actionStr = item.SubItems[3].Text;
					if (actionStr != doNothing)
						item.Remove();
				}
				progressBar.Value = progressBar.Maximum;
			}
		}
	}
}
