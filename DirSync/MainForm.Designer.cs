namespace DirSync
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Directories", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Files", System.Windows.Forms.HorizontalAlignment.Left);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.backgroundWorker_Sync = new System.ComponentModel.BackgroundWorker();
			this.listView = new System.Windows.Forms.ListView();
			this.columnHeader_Path1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader_Size1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader_LastChange1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader_Action = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader_LastChange2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader_Path2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader_Size2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.button_Sync = new System.Windows.Forms.Button();
			this.textBox_PathDesktop = new System.Windows.Forms.TextBox();
			this.button_BrowseDesktop = new System.Windows.Forms.Button();
			this.button_BrowseComputer = new System.Windows.Forms.Button();
			this.textBox_PathComputer = new System.Windows.Forms.TextBox();
			this.folderBrowserDialog_Computer = new System.Windows.Forms.FolderBrowserDialog();
			this.folderBrowserDialog_Desktop = new System.Windows.Forms.FolderBrowserDialog();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.button_Bake = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.backgroundWorker_Bake = new System.ComponentModel.BackgroundWorker();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel_Dir = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// backgroundWorker_Sync
			// 
			this.backgroundWorker_Sync.WorkerSupportsCancellation = true;
			this.backgroundWorker_Sync.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_Sync_DoWork);
			this.backgroundWorker_Sync.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_Sync_RunWorkerCompleted);
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Path1,
            this.columnHeader_Size1,
            this.columnHeader_LastChange1,
            this.columnHeader_Action,
            this.columnHeader_LastChange2,
            this.columnHeader_Path2,
            this.columnHeader_Size2});
			this.listView.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			listViewGroup1.Header = "Directories";
			listViewGroup1.Name = "dirs";
			listViewGroup2.Header = "Files";
			listViewGroup2.Name = "files";
			this.listView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
			this.listView.Location = new System.Drawing.Point(16, 15);
			this.listView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(745, 408);
			this.listView.TabIndex = 1;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseClick);
			this.listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
			// 
			// columnHeader_Path1
			// 
			this.columnHeader_Path1.Text = "Path 1";
			this.columnHeader_Path1.Width = 73;
			// 
			// columnHeader_Size1
			// 
			this.columnHeader_Size1.Text = "Size";
			// 
			// columnHeader_LastChange1
			// 
			this.columnHeader_LastChange1.Text = "Last Change";
			this.columnHeader_LastChange1.Width = 98;
			// 
			// columnHeader_Action
			// 
			this.columnHeader_Action.Text = "Act";
			this.columnHeader_Action.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader_Action.Width = 37;
			// 
			// columnHeader_LastChange2
			// 
			this.columnHeader_LastChange2.Text = "Last Change";
			this.columnHeader_LastChange2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader_LastChange2.Width = 93;
			// 
			// columnHeader_Path2
			// 
			this.columnHeader_Path2.Text = "Path 2";
			this.columnHeader_Path2.Width = 90;
			// 
			// columnHeader_Size2
			// 
			this.columnHeader_Size2.Text = "Size";
			// 
			// button_Sync
			// 
			this.button_Sync.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button_Sync.Location = new System.Drawing.Point(340, 464);
			this.button_Sync.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button_Sync.Name = "button_Sync";
			this.button_Sync.Size = new System.Drawing.Size(100, 28);
			this.button_Sync.TabIndex = 3;
			this.button_Sync.Text = "Sync";
			this.toolTip.SetToolTip(this.button_Sync, "Find conflicts of the directories");
			this.button_Sync.UseVisualStyleBackColor = true;
			this.button_Sync.Click += new System.EventHandler(this.button_Sync_Click);
			// 
			// textBox_PathDesktop
			// 
			this.textBox_PathDesktop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox_PathDesktop.Location = new System.Drawing.Point(448, 466);
			this.textBox_PathDesktop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox_PathDesktop.Name = "textBox_PathDesktop";
			this.textBox_PathDesktop.Size = new System.Drawing.Size(271, 22);
			this.textBox_PathDesktop.TabIndex = 4;
			// 
			// button_BrowseDesktop
			// 
			this.button_BrowseDesktop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_BrowseDesktop.AutoSize = true;
			this.button_BrowseDesktop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_BrowseDesktop.Location = new System.Drawing.Point(733, 465);
			this.button_BrowseDesktop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button_BrowseDesktop.Name = "button_BrowseDesktop";
			this.button_BrowseDesktop.Size = new System.Drawing.Size(30, 27);
			this.button_BrowseDesktop.TabIndex = 3;
			this.button_BrowseDesktop.Text = "...";
			this.button_BrowseDesktop.UseVisualStyleBackColor = true;
			this.button_BrowseDesktop.Click += new System.EventHandler(this.button_BrowseDesktop_Click);
			// 
			// button_BrowseComputer
			// 
			this.button_BrowseComputer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button_BrowseComputer.AutoSize = true;
			this.button_BrowseComputer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_BrowseComputer.Location = new System.Drawing.Point(16, 465);
			this.button_BrowseComputer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button_BrowseComputer.Name = "button_BrowseComputer";
			this.button_BrowseComputer.Size = new System.Drawing.Size(30, 27);
			this.button_BrowseComputer.TabIndex = 3;
			this.button_BrowseComputer.Text = "...";
			this.button_BrowseComputer.UseVisualStyleBackColor = true;
			this.button_BrowseComputer.Click += new System.EventHandler(this.button_BrowseComputer_Click);
			// 
			// textBox_PathComputer
			// 
			this.textBox_PathComputer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textBox_PathComputer.Location = new System.Drawing.Point(59, 466);
			this.textBox_PathComputer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox_PathComputer.Name = "textBox_PathComputer";
			this.textBox_PathComputer.Size = new System.Drawing.Size(272, 22);
			this.textBox_PathComputer.TabIndex = 4;
			// 
			// folderBrowserDialog_Computer
			// 
			this.folderBrowserDialog_Computer.RootFolder = System.Environment.SpecialFolder.MyComputer;
			this.folderBrowserDialog_Computer.ShowNewFolderButton = false;
			// 
			// folderBrowserDialog_Desktop
			// 
			this.folderBrowserDialog_Desktop.ShowNewFolderButton = false;
			// 
			// button_Bake
			// 
			this.button_Bake.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_Bake.Location = new System.Drawing.Point(663, 431);
			this.button_Bake.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button_Bake.Name = "button_Bake";
			this.button_Bake.Size = new System.Drawing.Size(100, 28);
			this.button_Bake.TabIndex = 6;
			this.button_Bake.Text = "Bake";
			this.toolTip.SetToolTip(this.button_Bake, "Execute the file actions");
			this.button_Bake.UseVisualStyleBackColor = true;
			this.button_Bake.Click += new System.EventHandler(this.button_Bake_Click);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(16, 431);
			this.progressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(639, 28);
			this.progressBar.TabIndex = 5;
			// 
			// backgroundWorker_Bake
			// 
			this.backgroundWorker_Bake.WorkerReportsProgress = true;
			this.backgroundWorker_Bake.WorkerSupportsCancellation = true;
			this.backgroundWorker_Bake.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_Bake_DoWork);
			this.backgroundWorker_Bake.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_Bake_ProgressChanged);
			this.backgroundWorker_Bake.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_Bake_RunWorkerCompleted);
			// 
			// statusStrip
			// 
			this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Dir});
			this.statusStrip.Location = new System.Drawing.Point(0, 509);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(779, 22);
			this.statusStrip.TabIndex = 0;
			// 
			// toolStripStatusLabel_Dir
			// 
			this.toolStripStatusLabel_Dir.Name = "toolStripStatusLabel_Dir";
			this.toolStripStatusLabel_Dir.Size = new System.Drawing.Size(0, 17);
			// 
			// MainForm
			// 
			this.AcceptButton = this.button_Sync;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(779, 531);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.button_Bake);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.textBox_PathComputer);
			this.Controls.Add(this.textBox_PathDesktop);
			this.Controls.Add(this.button_BrowseComputer);
			this.Controls.Add(this.button_BrowseDesktop);
			this.Controls.Add(this.button_Sync);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DirSync";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.ComponentModel.BackgroundWorker backgroundWorker_Sync;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.Button button_Sync;
		private System.Windows.Forms.ColumnHeader columnHeader_Path1;
		private System.Windows.Forms.TextBox textBox_PathDesktop;
		private System.Windows.Forms.Button button_BrowseDesktop;
		private System.Windows.Forms.Button button_BrowseComputer;
		private System.Windows.Forms.TextBox textBox_PathComputer;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_Computer;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_Desktop;
		private System.Windows.Forms.ColumnHeader columnHeader_Path2;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ColumnHeader columnHeader_Size1;
		private System.Windows.Forms.ColumnHeader columnHeader_LastChange1;
		private System.Windows.Forms.ColumnHeader columnHeader_Action;
		private System.Windows.Forms.ColumnHeader columnHeader_LastChange2;
		private System.Windows.Forms.ColumnHeader columnHeader_Size2;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button button_Bake;
		private System.ComponentModel.BackgroundWorker backgroundWorker_Bake;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Dir;
	}
}

