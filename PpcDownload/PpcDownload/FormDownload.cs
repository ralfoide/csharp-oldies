//-----------------------------------------------------------
/*
	Project:	PpcDownload
	Subproject:	PpcDownload

	Ralf (c) 2003
*/
//-----------------------------------------------------------

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;


//-----------------------------------------------------------
//-----------------------------------------------------------

//--------------------------
namespace Alfray.PpcDownload
{
	/// <summary>
	/// Summary description for mFormDownload.
	/// </summary>
	public class mFormDownload : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.MainMenu mMainMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mMenuExit;
		private System.Windows.Forms.MenuItem mMenuDisconnect;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mMenuConnect;
		private System.Windows.Forms.Button mButtonConnect;
		private System.Windows.Forms.SaveFileDialog mSaveFileDialog;
		private System.Windows.Forms.ColumnHeader mColumnFile;
		private System.Windows.Forms.ColumnHeader mColumnSize;
		private System.Windows.Forms.ColumnHeader mColumnDir;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mMenuSelect;
		private System.Windows.Forms.MenuItem mMenuUnselect;
		private System.Windows.Forms.MenuItem mMenuDownload;
		private System.Windows.Forms.ListView mListViewFiles;
		private System.Windows.Forms.TextBox mTextHost;
		private System.Windows.Forms.ContextMenu mContextMenu;
		private System.Windows.Forms.MenuItem mContextMenuDownload;
		private System.Windows.Forms.MenuItem mContextMenuSelect;
		private System.Windows.Forms.StatusBar mStatusBar;

		//-----------------------------------------------------------
		//-----------------------------------------------------------

		//--------------------
		public mFormDownload()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			mStatusBar.Text = "Idle";

			mListViewFiles.Activation = ItemActivation.TwoClick;
		}


		//-----------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mMainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mMenuSelect = new System.Windows.Forms.MenuItem();
			this.mMenuUnselect = new System.Windows.Forms.MenuItem();
			this.mMenuDownload = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mMenuConnect = new System.Windows.Forms.MenuItem();
			this.mMenuDisconnect = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mMenuExit = new System.Windows.Forms.MenuItem();
			this.label2 = new System.Windows.Forms.Label();
			this.mTextHost = new System.Windows.Forms.TextBox();
			this.mStatusBar = new System.Windows.Forms.StatusBar();
			this.mButtonConnect = new System.Windows.Forms.Button();
			this.mListViewFiles = new System.Windows.Forms.ListView();
			this.mColumnFile = new System.Windows.Forms.ColumnHeader();
			this.mColumnSize = new System.Windows.Forms.ColumnHeader();
			this.mColumnDir = new System.Windows.Forms.ColumnHeader();
			this.mSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.mContextMenu = new System.Windows.Forms.ContextMenu();
			this.mContextMenuDownload = new System.Windows.Forms.MenuItem();
			this.mContextMenuSelect = new System.Windows.Forms.MenuItem();
			// 
			// mMainMenu
			// 
			this.mMainMenu.MenuItems.Add(this.menuItem2);
			this.mMainMenu.MenuItems.Add(this.menuItem1);
			// 
			// menuItem2
			// 
			this.menuItem2.MenuItems.Add(this.mMenuSelect);
			this.menuItem2.MenuItems.Add(this.mMenuUnselect);
			this.menuItem2.MenuItems.Add(this.mMenuDownload);
			this.menuItem2.Text = "Edit";
			// 
			// mMenuSelect
			// 
			this.mMenuSelect.Text = "Select";
			this.mMenuSelect.Click += new System.EventHandler(this.mMenuSelect_Click);
			// 
			// mMenuUnselect
			// 
			this.mMenuUnselect.Text = "Unselect";
			this.mMenuUnselect.Click += new System.EventHandler(this.mMenuUnselect_Click);
			// 
			// mMenuDownload
			// 
			this.mMenuDownload.Text = "Download...";
			this.mMenuDownload.Click += new System.EventHandler(this.mMenuDownload_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.MenuItems.Add(this.mMenuConnect);
			this.menuItem1.MenuItems.Add(this.mMenuDisconnect);
			this.menuItem1.MenuItems.Add(this.menuItem4);
			this.menuItem1.MenuItems.Add(this.mMenuExit);
			this.menuItem1.Text = "Tools";
			// 
			// mMenuConnect
			// 
			this.mMenuConnect.Text = "Connect";
			this.mMenuConnect.Click += new System.EventHandler(this.mMenuConnect_Click);
			// 
			// mMenuDisconnect
			// 
			this.mMenuDisconnect.Text = "Disconnect";
			this.mMenuDisconnect.Click += new System.EventHandler(this.mMenuDisconnect_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Text = "-";
			// 
			// mMenuExit
			// 
			this.mMenuExit.Text = "Exit";
			this.mMenuExit.Click += new System.EventHandler(this.mMenuExit_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 10);
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.Text = "Host:";
			// 
			// mTextHost
			// 
			this.mTextHost.Location = new System.Drawing.Point(56, 8);
			this.mTextHost.Text = "192.168.2.42";
			// 
			// mStatusBar
			// 
			this.mStatusBar.Location = new System.Drawing.Point(0, 248);
			this.mStatusBar.Size = new System.Drawing.Size(240, 22);
			this.mStatusBar.Text = "<status>";
			// 
			// mButtonConnect
			// 
			this.mButtonConnect.Location = new System.Drawing.Point(160, 8);
			this.mButtonConnect.Text = "Connect";
			this.mButtonConnect.Click += new System.EventHandler(this.mButtonConnect_Click);
			// 
			// mListViewFiles
			// 
			this.mListViewFiles.CheckBoxes = true;
			this.mListViewFiles.Columns.Add(this.mColumnFile);
			this.mListViewFiles.Columns.Add(this.mColumnSize);
			this.mListViewFiles.Columns.Add(this.mColumnDir);
			this.mListViewFiles.ContextMenu = this.mContextMenu;
			this.mListViewFiles.FullRowSelect = true;
			this.mListViewFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.mListViewFiles.Location = new System.Drawing.Point(8, 40);
			this.mListViewFiles.Size = new System.Drawing.Size(224, 200);
			this.mListViewFiles.View = System.Windows.Forms.View.Details;
			this.mListViewFiles.ItemActivate += new System.EventHandler(this.mListViewFiles_ItemActivate);
			this.mListViewFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.mListViewFiles_ItemCheck);
			// 
			// mColumnFile
			// 
			this.mColumnFile.Text = "File";
			this.mColumnFile.Width = 60;
			// 
			// mColumnSize
			// 
			this.mColumnSize.Text = "Size";
			this.mColumnSize.Width = 60;
			// 
			// mColumnDir
			// 
			this.mColumnDir.Text = "Directory";
			this.mColumnDir.Width = 100;
			// 
			// mSaveFileDialog
			// 
			this.mSaveFileDialog.FileName = "doc1";
			// 
			// mContextMenu
			// 
			this.mContextMenu.MenuItems.Add(this.mContextMenuDownload);
			this.mContextMenu.MenuItems.Add(this.mContextMenuSelect);
			// 
			// mContextMenuDownload
			// 
			this.mContextMenuDownload.Text = "Download";
			this.mContextMenuDownload.Click += new System.EventHandler(this.mContextMenuDownload_Click);
			// 
			// mContextMenuSelect
			// 
			this.mContextMenuSelect.Text = "Select";
			// 
			// mFormDownload
			// 
			this.Controls.Add(this.mListViewFiles);
			this.Controls.Add(this.mButtonConnect);
			this.Controls.Add(this.mStatusBar);
			this.Controls.Add(this.mTextHost);
			this.Controls.Add(this.label2);
			this.Menu = this.mMainMenu;
			this.Text = "PocketPC Download";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.mFormDownload_Closing);

		}
		#endregion


		//-------------------------------------------
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main() 
		{
			Application.Run(new Alfray.PpcDownload.mFormDownload());
		}


		//-----------------------------------------------------------
		//-----------------------------------------------------------
		// Protocol and Network Methods


		public const int kServerPort = 13456;
		public const string kProtocolHeader = "Alfray.PpcUpload-1.0\n";
		public const string kProtocolAskList = "list\n";
		public const string kProtocolEndList = "<end>\n";
		public const char kProtocolSep = '\n';



		//--------------------------
		protected bool connect()
		{
			return getFileList();
		}


		//--------------------------
		protected bool disconnect()
		{
			// nothing to do right now (not asynchronous)
			return true;
		}


		//--------------------------
		protected bool getFileList()
		{
			mListViewFiles.Items.Clear();

			TcpClient tc = new TcpClient(mTextHost.Text, kServerPort);
			NetworkStream ns = tc.GetStream();

			byte [] buffer = System.Text.Encoding.ASCII.GetBytes(kProtocolHeader);
			ns.Write(buffer, 0, buffer.Length);

			buffer = System.Text.Encoding.ASCII.GetBytes(kProtocolAskList);
			ns.Write(buffer, 0, buffer.Length);

			string reply;
			do
			{
				reply = readLine(ns);

				if (reply != null)
				{
					// watch for the end line
					if (reply == kProtocolEndList)
						break;

					// remove trailing \n
					reply = reply.Remove(reply.Length-1, 1);

					// get file size
					int pos = reply.LastIndexOf(" <");
					string size = "";
					if (pos >= 0)
					{
						size = reply.Substring(pos+2).Replace(">", "").Trim();
						reply = reply.Substring(0, pos);
					}

					// get dir & file
					pos = reply.LastIndexOf('/');
					string dir = "";
					if (pos > 0)
						dir = reply.Substring(0, pos);
					string file = reply.Substring(pos+1);

					string [] s = new string[3] { file, size, dir };
					ListViewItem it = new ListViewItem(s);
					mListViewFiles.Items.Add(it);
				}
			} while(reply != null);

			tc.Close();
			return true;
		}


		//--------------------------------------------------------------------
		protected string readLine(NetworkStream ns)
			// reads a line of arbitrary length, terminated by a \n character
			// returns null if not valid string can be read
		{
			byte [] buffer = new byte[128];
			string result = "";
			int index = 0;

			while(true)
			{
				int b = ns.ReadByte();

				// end of stream... not a valid string, give up
				if (b == -1)
					return null;

				buffer[index++] = (byte)b;

				// end of line found, stop here
				if (b == kProtocolSep)
					break;

				if (index >= buffer.Length)
				{
					result += System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
					index = 0;
				}
			}

			if (index > 0)
				result += System.Text.Encoding.UTF8.GetString(buffer, 0, index);

			return result;
		}


		//-----------------------------------------------------------
		protected bool checkItems(bool check)
		{
			IEnumerable lvic = mListViewFiles.SelectedIndices;

			if (lvic != null)
				foreach(int index in lvic)
					mListViewFiles.Items[index].Checked = check;

			return true;
		}


		//-----------------------------------------------------------
		protected bool downloadChecked()
		{
			foreach(ListViewItem it in mListViewFiles.Items)
				if (it.Checked)
					downloadItem(it);

			return true;
		}


		//-----------------------------------------------------------
		protected bool downloadItem(ListViewItem it)
		{
			if (it.SubItems.Count == 3)
			{
				string file = it.SubItems[0].Text;
				string dir  = it.SubItems[2].Text;

				MessageBox.Show("file: " + file + "\r\ndir: " + dir);
			}

			return true;
		}


		//-----------------------------------------------------------
		//-----------------------------------------------------------
		// UI Callback


		private void mButtonConnect_Click(object sender, System.EventArgs e)
		{
			connect();
		}

		private void mListViewFiles_ItemActivate(object sender, System.EventArgs e)
		{
			IEnumerable lvic = mListViewFiles.SelectedIndices;

			if (lvic != null)
				foreach(int index in lvic)
				{
					ListViewItem it = mListViewFiles.Items[index];
					it.Checked = ! it.Checked;
				}
		}

		private void mListViewFiles_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			checkItems(true);
		}

		private void mMenuSelect_Click(object sender, System.EventArgs e)
		{
			checkItems(true);
		}

		private void mMenuUnselect_Click(object sender, System.EventArgs e)
		{
			checkItems(false);
		}

		private void mMenuDownload_Click(object sender, System.EventArgs e)
		{
			downloadChecked();	
		}

		private void mMenuConnect_Click(object sender, System.EventArgs e)
		{
			connect();
		}

		private void mMenuDisconnect_Click(object sender, System.EventArgs e)
		{
			disconnect();
		}

		private void mMenuExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void mFormDownload_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!disconnect())
				e.Cancel = true;
		}

		private void mContextMenuDownload_Click(object sender, System.EventArgs e)
		{
			downloadChecked();
		}
	}
}


//-----------------------------------------------------------
//	$Log: FormDownload.cs,v $
//	Revision 1.2  2003/07/14 15:57:07  ralf
//	Menu handling, download, etc.
//	
//	Revision 1.1.1.1  2003/07/07 16:00:58  ralf
//	Initial Version
//	
//-----------------------------------------------------------

