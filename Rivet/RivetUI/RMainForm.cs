//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetUI
	File:		RMainForm.cs

	Copyright 2005, Raphael MOLL.

	This file is part of Rivet.

	Rivet is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Rivet is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Rivet; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

*/
//*******************************************************************



using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Alfray.Rivet.RivetLib;

//*********************************
namespace Alfray.Rivet.RivetUI
{
	//**************************************
	/// <summary>
	/// Summary description for RMainForm.
	/// </summary>
	public class RMainForm : System.Windows.Forms.Form
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

			// Setup the images view in the placeholder

			mImagesView = new RImagesView();
			mImagesView.Location = new Point(0,0);
			mImagesView.Size = mPanelPlaceholder.Size;
			mImagesView.Anchor = mPanelPlaceholder.Anchor;

			mPanelPlaceholder.Controls.Add(mImagesView);
			//mPanelPlaceholder.AutoScroll = true;
			//mPanelPlaceholder.AutoScrollMargin = new Size(5,5);

			// Inits done in derived class BWMainFormEx

		}


		//-------------------------------------------
		//----------- Protected Properties ----------
		//-------------------------------------------

		//******************************
		/// <summary>
		/// Gives complete access to the Library Locations combo box
		/// </summary>
		//******************************
		protected ComboBox LocationCombo
		{
			get
			{
				return mLocationCombo;
			}
		}


		//*****************************
		/// <summary>
		/// Changes the label on top of the image list
		/// </summary>
		//*****************************
		protected string ImageListLabel
		{
			get
			{
				return mImageListLabel.Text;
			}

			set
			{
				mImageListLabel.Text = value;
			}
		}

		//******************************
		/// <summary>
		/// Gives complete access to the Directory Tree View
		/// </summary>
		//******************************
		protected TreeView DirectoryTree
		{
			get
			{
				return mDirTreeView;
			}
		}

		//******************************
		/// <summary>
		/// Gives complete access to the Image List View
		/// </summary>
		//******************************
		protected RImagesView ImagesView
		{
			get
			{
				return mImagesView;
			}
		}


		//-------------------------------------------
		//----------- Proteced Methods --------------
		//-------------------------------------------

		//*********************************
		/// <summary>
		/// This callback notifies when the application is being closed.
		/// </summary>
		//*********************************
		protected virtual void Terminate()
		{
		}

		//*******************************************
		/// <summary>
		/// This callback notifies when the debug window should
		/// be displayed or hidden.
		/// </summary>
		//*******************************************
		protected virtual void ShowHideDebugWindow()
		{
		}

		//******************************************
		/// <summary>
		/// This callback notifies when the pref window should
		/// be displayed or hidden.
		/// </summary>
		//******************************************
		protected virtual void ShowHidePrefWindow()
		{
		}

		//**********************************************************
		/// <summary>
		/// This callback is called when the library path has 
		/// been changed in the library combo box.
		/// It may be a new path or one in the existing library list.
		/// </summary>
		//**********************************************************
		protected virtual void OnLibraryPathChanged(string libPath)
		{
		}

		//*****************************************
		/// <summary>
		/// This callback is called to update the library path 
		/// combo box
		/// </summary>
		//*****************************************
		protected virtual void UpdateLibraryList()
		{
		}

		//*****************************************
		/// <summary>
		/// This callback is called when the window is activated
		/// the first time.
		/// You may need to force a redraw for it to be completly 
		/// and correctly drawn by using this code:
		///   this.Refresh(); Application.DoEvents();
		/// </summary>
		//*****************************************
		protected virtual void OnFirstActivated()
		{
		}

		//**********************************************************
		/// <summary>
		/// This callback is called when a node has been selected 
		/// in the directory tree.
		/// The selected node is given as argument.
		/// </summary>
		//**********************************************************
		protected virtual void OnDirTreeNodeSelected(TreeNode node)
		{
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------



		

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
			ShowHideDebugWindow();
		}


		//******************************************************************
		private void RMainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Terminate();
		}

		
		//******************************************************************
		private void mDirTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			OnDirTreeNodeSelected(e.Node);
		}

		//******************************************************************
		private void mImageListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		//******************************************************************
		private void mLargeIconListButton_Click(object sender, System.EventArgs e)
		{
		
		}

		//******************************************************************
		private void mDetailListButton_Click(object sender, System.EventArgs e)
		{
		
		}

		//******************************************************************
		private void mLocationCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			OnLibraryPathChanged(mLocationCombo.Text);
		}

		//******************************************************************
		private void mLocationCombo_Validated(object sender, System.EventArgs e)
		{
			string s = mLocationCombo.Text;
			if (mLocationCombo.FindStringExact(s) == -1)
				OnLibraryPathChanged(s);
		}

		//******************************************************************
		private void mMenuItemPrefs_Click(object sender, System.EventArgs e)
		{
			ShowHidePrefWindow();
		}

		//******************************************************************
		private void RMainForm_Activated(object sender, System.EventArgs e)
		{
			if (mNeedsFirstActivated)
			{
				mNeedsFirstActivated = false;
				OnFirstActivated();
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RMainForm));
			this.mMenuMain = new System.Windows.Forms.MainMenu();
			this.mMenuFile = new System.Windows.Forms.MenuItem();
			this.mMenuItemLocation = new System.Windows.Forms.MenuItem();
			this.mMenuItemPrefs = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mMenuItemQuit = new System.Windows.Forms.MenuItem();
			this.mMenuHelp = new System.Windows.Forms.MenuItem();
			this.mMenuItemUpdate = new System.Windows.Forms.MenuItem();
			this.mMenuItemDebug = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.mMenuItemAbout = new System.Windows.Forms.MenuItem();
			this.mStatusBar = new System.Windows.Forms.StatusBar();
			this.mDirTreeView = new System.Windows.Forms.TreeView();
			this.mLocationCombo = new System.Windows.Forms.ComboBox();
			this.mDetailListButton = new System.Windows.Forms.Button();
			this.mLargeIconListButton = new System.Windows.Forms.Button();
			this.mImageListLabel = new System.Windows.Forms.Label();
			this.mPanelPlaceholder = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// mMenuMain
			// 
			this.mMenuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mMenuFile,
																					  this.mMenuHelp});
			// 
			// mMenuFile
			// 
			this.mMenuFile.Index = 0;
			this.mMenuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mMenuItemLocation,
																					  this.mMenuItemPrefs,
																					  this.menuItem4,
																					  this.mMenuItemQuit});
			this.mMenuFile.Text = "File";
			// 
			// mMenuItemLocation
			// 
			this.mMenuItemLocation.Index = 0;
			this.mMenuItemLocation.Text = "Change Location...";
			// 
			// mMenuItemPrefs
			// 
			this.mMenuItemPrefs.Index = 1;
			this.mMenuItemPrefs.Text = "Preferences...";
			this.mMenuItemPrefs.Click += new System.EventHandler(this.mMenuItemPrefs_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "-";
			// 
			// mMenuItemQuit
			// 
			this.mMenuItemQuit.Index = 3;
			this.mMenuItemQuit.Text = "Quit";
			this.mMenuItemQuit.Click += new System.EventHandler(this.mMenuItemQuit_Click);
			// 
			// mMenuHelp
			// 
			this.mMenuHelp.Index = 1;
			this.mMenuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mMenuItemUpdate,
																					  this.mMenuItemDebug,
																					  this.menuItem10,
																					  this.mMenuItemAbout});
			this.mMenuHelp.Text = "Help";
			// 
			// mMenuItemUpdate
			// 
			this.mMenuItemUpdate.Index = 0;
			this.mMenuItemUpdate.Text = "Update...";
			// 
			// mMenuItemDebug
			// 
			this.mMenuItemDebug.Index = 1;
			this.mMenuItemDebug.Text = "Debug";
			this.mMenuItemDebug.Click += new System.EventHandler(this.mMenuItemDebug_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 2;
			this.menuItem10.Text = "-";
			// 
			// mMenuItemAbout
			// 
			this.mMenuItemAbout.Index = 3;
			this.mMenuItemAbout.Text = "About...";
			// 
			// mStatusBar
			// 
			this.mStatusBar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.mStatusBar.Location = new System.Drawing.Point(0, 424);
			this.mStatusBar.Name = "mStatusBar";
			this.mStatusBar.Size = new System.Drawing.Size(712, 22);
			this.mStatusBar.TabIndex = 0;
			// 
			// mDirTreeView
			// 
			this.mDirTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.mDirTreeView.FullRowSelect = true;
			this.mDirTreeView.HideSelection = false;
			this.mDirTreeView.HotTracking = true;
			this.mDirTreeView.ImageIndex = -1;
			this.mDirTreeView.LabelEdit = true;
			this.mDirTreeView.Location = new System.Drawing.Point(8, 40);
			this.mDirTreeView.Name = "mDirTreeView";
			this.mDirTreeView.SelectedImageIndex = -1;
			this.mDirTreeView.Size = new System.Drawing.Size(216, 376);
			this.mDirTreeView.TabIndex = 2;
			this.mDirTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mDirTreeView_AfterSelect);
			// 
			// mLocationCombo
			// 
			this.mLocationCombo.Location = new System.Drawing.Point(8, 8);
			this.mLocationCombo.Name = "mLocationCombo";
			this.mLocationCombo.Size = new System.Drawing.Size(216, 21);
			this.mLocationCombo.TabIndex = 4;
			this.mLocationCombo.Text = "<Click to select a library>";
			this.mLocationCombo.Validated += new System.EventHandler(this.mLocationCombo_Validated);
			this.mLocationCombo.SelectedIndexChanged += new System.EventHandler(this.mLocationCombo_SelectedIndexChanged);
			// 
			// mDetailListButton
			// 
			this.mDetailListButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mDetailListButton.Image = ((System.Drawing.Image)(resources.GetObject("mDetailListButton.Image")));
			this.mDetailListButton.Location = new System.Drawing.Point(672, 4);
			this.mDetailListButton.Name = "mDetailListButton";
			this.mDetailListButton.Size = new System.Drawing.Size(32, 32);
			this.mDetailListButton.TabIndex = 5;
			this.mDetailListButton.Click += new System.EventHandler(this.mDetailListButton_Click);
			// 
			// mLargeIconListButton
			// 
			this.mLargeIconListButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mLargeIconListButton.Image = ((System.Drawing.Image)(resources.GetObject("mLargeIconListButton.Image")));
			this.mLargeIconListButton.Location = new System.Drawing.Point(640, 4);
			this.mLargeIconListButton.Name = "mLargeIconListButton";
			this.mLargeIconListButton.Size = new System.Drawing.Size(32, 32);
			this.mLargeIconListButton.TabIndex = 6;
			this.mLargeIconListButton.Click += new System.EventHandler(this.mLargeIconListButton_Click);
			// 
			// mImageListLabel
			// 
			this.mImageListLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mImageListLabel.Location = new System.Drawing.Point(232, 8);
			this.mImageListLabel.Name = "mImageListLabel";
			this.mImageListLabel.Size = new System.Drawing.Size(400, 23);
			this.mImageListLabel.TabIndex = 7;
			this.mImageListLabel.Text = "<label>";
			this.mImageListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// mPanelPlaceholder
			// 
			this.mPanelPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mPanelPlaceholder.AutoScroll = true;
			this.mPanelPlaceholder.Location = new System.Drawing.Point(232, 40);
			this.mPanelPlaceholder.Name = "mPanelPlaceholder";
			this.mPanelPlaceholder.Size = new System.Drawing.Size(472, 368);
			this.mPanelPlaceholder.TabIndex = 8;
			// 
			// RMainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 446);
			this.Controls.Add(this.mPanelPlaceholder);
			this.Controls.Add(this.mImageListLabel);
			this.Controls.Add(this.mLargeIconListButton);
			this.Controls.Add(this.mDetailListButton);
			this.Controls.Add(this.mLocationCombo);
			this.Controls.Add(this.mDirTreeView);
			this.Controls.Add(this.mStatusBar);
			this.Menu = this.mMenuMain;
			this.MinimumSize = new System.Drawing.Size(552, 480);
			this.Name = "RMainForm";
			this.Text = "RMainForm";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RMainForm_Closing);
			this.Activated += new System.EventHandler(this.RMainForm_Activated);
			this.ResumeLayout(false);

		}

		#endregion

		private System.ComponentModel.IContainer components = null;

		private System.Windows.Forms.MainMenu mMenuMain;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.StatusBar mStatusBar;
		private System.Windows.Forms.MenuItem mMenuItemQuit;
		private System.Windows.Forms.MenuItem mMenuItemDebug;
		private System.Windows.Forms.MenuItem mMenuItemAbout;
		private System.Windows.Forms.MenuItem mMenuFile;
		private System.Windows.Forms.MenuItem mMenuItemLocation;
		private System.Windows.Forms.MenuItem mMenuHelp;
		private System.Windows.Forms.MenuItem mMenuItemUpdate;
		private System.Windows.Forms.TreeView mDirTreeView;
		private System.Windows.Forms.ComboBox mLocationCombo;
		private System.Windows.Forms.Button mDetailListButton;
		private System.Windows.Forms.Button mLargeIconListButton;
		private System.Windows.Forms.Label mImageListLabel;
		private System.Windows.Forms.MenuItem mMenuItemPrefs;
		private System.Windows.Forms.Panel mPanelPlaceholder;



		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private bool		mNeedsFirstActivated = true;
		private RImagesView	mImagesView = null;


	} // class RMainForm
} // namespace Alfray.Rivet.RivetUI


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RMainForm.cs,v $
//	Revision 1.6  2005/07/12 14:35:24  ralf
//	Display images in custom view.
//	
//	Revision 1.5  2005/06/26 22:22:13  ralf
//	Using custom RImageView class
//	
//	Revision 1.4  2005/05/31 06:16:52  ralf
//	Implementinf IDisposable for RFSItem, RDir, RImage and RAppState
//	
//	Revision 1.3  2005/05/31 00:12:16  ralf
//	Current selected directory in RAppState.
//	Display image for select directory in main form.
//	
//	Revision 1.2  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//	Revision 1.1  2005/05/30 00:48:33  ralf
//	Reorganized source. Added RivetUI library.
//	
//	Revision 1.3  2005/05/29 23:02:13  ralf
//	Fixes. Load last current library at startup.
//	
//	Revision 1.2  2005/05/25 03:54:21  ralf
//	Implemented LocationCombo with storage in prefs.
//	Added RLibrary, RAppState, RTestSkeleton.
//	
//	Revision 1.1.1.1  2005/05/23 02:48:59  ralf
//	no message
//	
//	Revision 1.3  2005/04/28 21:31:14  ralf
//	Using new LibUtils project
//	
//	Revision 1.2  2005/03/20 19:48:39  ralf
//	Added GPL headers.
//	
//	Revision 1.1  2005/02/18 23:21:52  ralf
//	Creating both an App and a Class Lib
//	
//	Revision 1.1.1.1  2005/02/18 22:54:53  ralf
//	A skeleton application template, with NUnit testing
//	
//---------------------------------------------------------------

