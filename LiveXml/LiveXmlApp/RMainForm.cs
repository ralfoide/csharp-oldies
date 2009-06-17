//*******************************************************************
/* 

 		Project:	LiveXml
 		File:		RMainForm.cs

*/ 
//*******************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Alfray.LiveXml.Utils;

//*********************************
namespace Alfray.LiveXml.LiveXmlApp
{
	//**************************************
	/// <summary>
	/// Summary description for RMainForm.
	/// </summary>
	public class RMainForm : System.Windows.Forms.Form, RILog
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//****************
		public RMainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// Add any constructor code after InitializeComponent call
			//

			mVideoTimer.Enabled = false; // DEBUG

			init();

			// mVideoTimer.Enabled = true;
			// mVideoTimer.Interval = 2000;

		}

		#region RILog Members

		//***********************
		public void Log(object o)
		{
			Log(o.ToString());
		}

		//***********************
		public void Log(string s)
		{
			if (mDebugForm == null)
				createDebugWindow(false);
			if (mDebugForm != null)
				mDebugForm.Log(s);
		}

		#endregion




		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//*****************
		private void init()
		{
			// mVideoHandler = new RVideoHandler();
			// mVideoHandler.Start(this);
			// mCameraList = mVideoHandler.EnumerateCameras();
			mCameraList = new ArrayList();
		}


		//**********************
		private void terminate()
		{
			mCameraList.Clear();

			//mVideoHandler.Stop();
			//mVideoHandler = null;

			closeDebugWindow();
		}


		//******************************************
		private void createDebugWindow(bool visible)
		{
			if (mDebugForm == null)
			{
				mDebugForm = new RDebugForm();
				mDebugForm.Show();
			}
		}


		//******************************
		private void closeDebugWindow()
		{
			if (mDebugForm != null)
			{
				mDebugForm.CanClose = true;
				mDebugForm.Close();
				mDebugForm = null;
			}
		}


		//********************************
		private void showHideDebugWindow()
		{
			if (mDebugForm == null)
				createDebugWindow(true);
			else
				mDebugForm.Visible = !mDebugForm.Visible;
		}


		//*****************************
		private void updateLocalVideo()
		{
			// DEBUG
			if (true)
			{
				// Image img = mVideoHandler.GetImage((AcqLib.RVideoHandler.SCameraInfo)mCameraList[0]);
				// mPictureLocal.Image = img;
			}
		}


		//-------------------------------------------
		//----------- Private Callbacks -------------
		//-------------------------------------------


		//******************************************************************
		private void mMenuItemQuit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		//******************************************************************
		private void mMenuItemDebug_Click(object sender, System.EventArgs e)
		{
			showHideDebugWindow();
		}


		//******************************************************************
		private void RMainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			terminate();
		}

		
		//**************************************************************
		private void mVideoTimer_Tick(object sender, System.EventArgs e)
		{
			updateLocalVideo();
		}

		
		//**************************************************************
		private void mPictureLocal_Click(object sender, System.EventArgs e)
		{
			updateLocalVideo();
		}


		//-------------------------------------------
		//----------- Private WinForms --------------
		//-------------------------------------------

		//***********************************
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code

		//********************************
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RMainForm));
			this.mMenuMain = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mMenuItemConnect = new System.Windows.Forms.MenuItem();
			this.mMenuItemDisconnect = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mMenuItemQuit = new System.Windows.Forms.MenuItem();
			this.mMenuAide = new System.Windows.Forms.MenuItem();
			this.mMenuItemUpdate = new System.Windows.Forms.MenuItem();
			this.mMenuItemDebug = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.mMenuItemAbout = new System.Windows.Forms.MenuItem();
			this.mStatusBar = new System.Windows.Forms.StatusBar();
			this.mPictureLocal = new System.Windows.Forms.PictureBox();
			this.mVideoTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// mMenuMain
			// 
			this.mMenuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.mMenuAide});
			this.mMenuMain.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mMenuMain.RightToLeft")));
			// 
			// menuItem1
			// 
			this.menuItem1.Enabled = ((bool)(resources.GetObject("menuItem1.Enabled")));
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mMenuItemConnect,
																					  this.mMenuItemDisconnect,
																					  this.menuItem4,
																					  this.mMenuItemQuit});
			this.menuItem1.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem1.Shortcut")));
			this.menuItem1.ShowShortcut = ((bool)(resources.GetObject("menuItem1.ShowShortcut")));
			this.menuItem1.Text = resources.GetString("menuItem1.Text");
			this.menuItem1.Visible = ((bool)(resources.GetObject("menuItem1.Visible")));
			// 
			// mMenuItemConnect
			// 
			this.mMenuItemConnect.Enabled = ((bool)(resources.GetObject("mMenuItemConnect.Enabled")));
			this.mMenuItemConnect.Index = 0;
			this.mMenuItemConnect.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemConnect.Shortcut")));
			this.mMenuItemConnect.ShowShortcut = ((bool)(resources.GetObject("mMenuItemConnect.ShowShortcut")));
			this.mMenuItemConnect.Text = resources.GetString("mMenuItemConnect.Text");
			this.mMenuItemConnect.Visible = ((bool)(resources.GetObject("mMenuItemConnect.Visible")));
			// 
			// mMenuItemDisconnect
			// 
			this.mMenuItemDisconnect.Enabled = ((bool)(resources.GetObject("mMenuItemDisconnect.Enabled")));
			this.mMenuItemDisconnect.Index = 1;
			this.mMenuItemDisconnect.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemDisconnect.Shortcut")));
			this.mMenuItemDisconnect.ShowShortcut = ((bool)(resources.GetObject("mMenuItemDisconnect.ShowShortcut")));
			this.mMenuItemDisconnect.Text = resources.GetString("mMenuItemDisconnect.Text");
			this.mMenuItemDisconnect.Visible = ((bool)(resources.GetObject("mMenuItemDisconnect.Visible")));
			// 
			// menuItem4
			// 
			this.menuItem4.Enabled = ((bool)(resources.GetObject("menuItem4.Enabled")));
			this.menuItem4.Index = 2;
			this.menuItem4.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem4.Shortcut")));
			this.menuItem4.ShowShortcut = ((bool)(resources.GetObject("menuItem4.ShowShortcut")));
			this.menuItem4.Text = resources.GetString("menuItem4.Text");
			this.menuItem4.Visible = ((bool)(resources.GetObject("menuItem4.Visible")));
			// 
			// mMenuItemQuit
			// 
			this.mMenuItemQuit.Enabled = ((bool)(resources.GetObject("mMenuItemQuit.Enabled")));
			this.mMenuItemQuit.Index = 3;
			this.mMenuItemQuit.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemQuit.Shortcut")));
			this.mMenuItemQuit.ShowShortcut = ((bool)(resources.GetObject("mMenuItemQuit.ShowShortcut")));
			this.mMenuItemQuit.Text = resources.GetString("mMenuItemQuit.Text");
			this.mMenuItemQuit.Visible = ((bool)(resources.GetObject("mMenuItemQuit.Visible")));
			this.mMenuItemQuit.Click += new System.EventHandler(this.mMenuItemQuit_Click);
			// 
			// mMenuAide
			// 
			this.mMenuAide.Enabled = ((bool)(resources.GetObject("mMenuAide.Enabled")));
			this.mMenuAide.Index = 1;
			this.mMenuAide.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mMenuItemUpdate,
																					  this.mMenuItemDebug,
																					  this.menuItem10,
																					  this.mMenuItemAbout});
			this.mMenuAide.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuAide.Shortcut")));
			this.mMenuAide.ShowShortcut = ((bool)(resources.GetObject("mMenuAide.ShowShortcut")));
			this.mMenuAide.Text = resources.GetString("mMenuAide.Text");
			this.mMenuAide.Visible = ((bool)(resources.GetObject("mMenuAide.Visible")));
			// 
			// mMenuItemUpdate
			// 
			this.mMenuItemUpdate.Enabled = ((bool)(resources.GetObject("mMenuItemUpdate.Enabled")));
			this.mMenuItemUpdate.Index = 0;
			this.mMenuItemUpdate.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemUpdate.Shortcut")));
			this.mMenuItemUpdate.ShowShortcut = ((bool)(resources.GetObject("mMenuItemUpdate.ShowShortcut")));
			this.mMenuItemUpdate.Text = resources.GetString("mMenuItemUpdate.Text");
			this.mMenuItemUpdate.Visible = ((bool)(resources.GetObject("mMenuItemUpdate.Visible")));
			// 
			// mMenuItemDebug
			// 
			this.mMenuItemDebug.Enabled = ((bool)(resources.GetObject("mMenuItemDebug.Enabled")));
			this.mMenuItemDebug.Index = 1;
			this.mMenuItemDebug.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemDebug.Shortcut")));
			this.mMenuItemDebug.ShowShortcut = ((bool)(resources.GetObject("mMenuItemDebug.ShowShortcut")));
			this.mMenuItemDebug.Text = resources.GetString("mMenuItemDebug.Text");
			this.mMenuItemDebug.Visible = ((bool)(resources.GetObject("mMenuItemDebug.Visible")));
			this.mMenuItemDebug.Click += new System.EventHandler(this.mMenuItemDebug_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Enabled = ((bool)(resources.GetObject("menuItem10.Enabled")));
			this.menuItem10.Index = 2;
			this.menuItem10.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem10.Shortcut")));
			this.menuItem10.ShowShortcut = ((bool)(resources.GetObject("menuItem10.ShowShortcut")));
			this.menuItem10.Text = resources.GetString("menuItem10.Text");
			this.menuItem10.Visible = ((bool)(resources.GetObject("menuItem10.Visible")));
			// 
			// mMenuItemAbout
			// 
			this.mMenuItemAbout.Enabled = ((bool)(resources.GetObject("mMenuItemAbout.Enabled")));
			this.mMenuItemAbout.Index = 3;
			this.mMenuItemAbout.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemAbout.Shortcut")));
			this.mMenuItemAbout.ShowShortcut = ((bool)(resources.GetObject("mMenuItemAbout.ShowShortcut")));
			this.mMenuItemAbout.Text = resources.GetString("mMenuItemAbout.Text");
			this.mMenuItemAbout.Visible = ((bool)(resources.GetObject("mMenuItemAbout.Visible")));
			// 
			// mStatusBar
			// 
			this.mStatusBar.AccessibleDescription = resources.GetString("mStatusBar.AccessibleDescription");
			this.mStatusBar.AccessibleName = resources.GetString("mStatusBar.AccessibleName");
			this.mStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mStatusBar.Anchor")));
			this.mStatusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mStatusBar.BackgroundImage")));
			this.mStatusBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mStatusBar.Dock")));
			this.mStatusBar.Enabled = ((bool)(resources.GetObject("mStatusBar.Enabled")));
			this.mStatusBar.Font = ((System.Drawing.Font)(resources.GetObject("mStatusBar.Font")));
			this.mStatusBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mStatusBar.ImeMode")));
			this.mStatusBar.Location = ((System.Drawing.Point)(resources.GetObject("mStatusBar.Location")));
			this.mStatusBar.Name = "mStatusBar";
			this.mStatusBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mStatusBar.RightToLeft")));
			this.mStatusBar.Size = ((System.Drawing.Size)(resources.GetObject("mStatusBar.Size")));
			this.mStatusBar.TabIndex = ((int)(resources.GetObject("mStatusBar.TabIndex")));
			this.mStatusBar.Text = resources.GetString("mStatusBar.Text");
			this.mStatusBar.Visible = ((bool)(resources.GetObject("mStatusBar.Visible")));
			// 
			// mPictureLocal
			// 
			this.mPictureLocal.AccessibleDescription = resources.GetString("mPictureLocal.AccessibleDescription");
			this.mPictureLocal.AccessibleName = resources.GetString("mPictureLocal.AccessibleName");
			this.mPictureLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mPictureLocal.Anchor")));
			this.mPictureLocal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mPictureLocal.BackgroundImage")));
			this.mPictureLocal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureLocal.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mPictureLocal.Dock")));
			this.mPictureLocal.Enabled = ((bool)(resources.GetObject("mPictureLocal.Enabled")));
			this.mPictureLocal.Font = ((System.Drawing.Font)(resources.GetObject("mPictureLocal.Font")));
			this.mPictureLocal.Image = ((System.Drawing.Image)(resources.GetObject("mPictureLocal.Image")));
			this.mPictureLocal.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mPictureLocal.ImeMode")));
			this.mPictureLocal.Location = ((System.Drawing.Point)(resources.GetObject("mPictureLocal.Location")));
			this.mPictureLocal.Name = "mPictureLocal";
			this.mPictureLocal.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mPictureLocal.RightToLeft")));
			this.mPictureLocal.Size = ((System.Drawing.Size)(resources.GetObject("mPictureLocal.Size")));
			this.mPictureLocal.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("mPictureLocal.SizeMode")));
			this.mPictureLocal.TabIndex = ((int)(resources.GetObject("mPictureLocal.TabIndex")));
			this.mPictureLocal.TabStop = false;
			this.mPictureLocal.Text = resources.GetString("mPictureLocal.Text");
			this.mPictureLocal.Visible = ((bool)(resources.GetObject("mPictureLocal.Visible")));
			this.mPictureLocal.Click += new System.EventHandler(this.mPictureLocal_Click);
			// 
			// mVideoTimer
			// 
			this.mVideoTimer.Interval = 500;
			this.mVideoTimer.Tick += new System.EventHandler(this.mVideoTimer_Tick);
			// 
			// RMainForm
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.mPictureLocal);
			this.Controls.Add(this.mStatusBar);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.Menu = this.mMenuMain;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "RMainForm";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RMainForm_Closing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.MainMenu mMenuMain;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.StatusBar mStatusBar;
		private System.Windows.Forms.MenuItem mMenuItemConnect;
		private System.Windows.Forms.MenuItem mMenuItemDisconnect;
		private System.Windows.Forms.MenuItem mMenuItemQuit;
		private System.Windows.Forms.MenuItem mMenuAide;
		private System.Windows.Forms.MenuItem mMenuItemUpdate;
		private System.Windows.Forms.MenuItem mMenuItemDebug;
		private System.Windows.Forms.MenuItem mMenuItemAbout;
		private System.Windows.Forms.PictureBox mPictureLocal;
		private System.Windows.Forms.Timer mVideoTimer;


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		RDebugForm		mDebugForm;
		// RVideoHandler	mVideoHandler;

		ArrayList		mCameraList;




	} // class RMainForm
} // namespace Alfray.LiveXml.LiveXmlApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RMainForm.cs,v $
//	Revision 1.1.1.1  2005/02/18 22:54:53  ralf
//	A skeleton application template, with NUnit testing
//	
//---------------------------------------------------------------

