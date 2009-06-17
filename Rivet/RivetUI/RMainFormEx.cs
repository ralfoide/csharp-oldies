//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetUI
	File:		RMainFormEx.cs

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
using System.Windows.Forms;

using Alfray.LibUtils.Misc;
using Alfray.Rivet.RivetLib;


//*************************************
namespace Alfray.Rivet.RivetUI
{
	//***************************************************
	/// <summary>
	/// RMainFormEx override RMainForm and contains all the
	/// logic of the UI.
	/// UI elements are declared (via the VS.Net resource editor)
	/// in RMainForm. As much of possible all actions performed
	/// by the UI are implemented in RMainFormEx rather than
	/// directly in RMainForm.
	/// The goal is to separate the pure UI widget management
	/// from the logic of the window, in the hope it may be easier
	/// later to use a different UI kit (GTK#, etc.)
	/// In a first test, RMainFormEx directly derives from RMainForm
	/// and thus will access WinForm widgets directly.
	/// Note that the distinction used here is more or less similar
	/// to what VS.Net 2005 does with partial classes for WinForms.
	/// </summary>
	//***************************************************
	public class RMainFormEx: RMainForm, RILog
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

		
		//***************************
		public RMainFormEx() : base()
		{
			// Post-base UI Init
			init();
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


		//***********************
		public void ReloadPrefs()
		{
			reloadPrefs();
		}

		
		//-------------------------------------------
		//----------- Proteced Methods --------------
		//-------------------------------------------

		
		//*********************************
		/// <summary>
		/// Closes the application
		/// </summary>
		//*********************************
		protected override void Terminate()
		{
			// close windows
			closePrefWindow();
			closeDebugWindow();

			// save settings
			saveSettings();
		
			// make sure we remove current selections if any
			resetDirTree();

			// dispose app state
			mState.Dispose();
		}


		//*******************************************
		/// <summary>
		/// Shows or hides the debug window.
		/// Creates it the first time.
		/// </summary>
		//*******************************************
		protected override void ShowHideDebugWindow()
		{
			if (mDebugForm == null)
				createDebugWindow(true);
			else
				mDebugForm.Visible = !mDebugForm.Visible;
		}


		//******************************************
		/// <summary>
		/// Shows or hides the pref window.
		/// Creates it the first time.
		/// </summary>
		//******************************************
		protected override void ShowHidePrefWindow()
		{
			if (mPrefForm == null)
				createPrefWindow(true);
			else
				mPrefForm.Visible = !mPrefForm.Visible;
		}

		//**********************************************************
		/// <summary>
		/// Callback called when the library path has been changed
		/// in the library combo box.
		/// It may be a new path or one in the existing library list.
		/// </summary>
		/// <param name="libPath"></param>
		//**********************************************************
		protected override void OnLibraryPathChanged(string libPath)
		{
			// Check if this is library is already known

			foreach(RLibrary lib in mState.Libraries)
			{
				// Note: check using ToString() and not AbsRootPath
				// as ToString is what gets displayed and what the user must type.
				if (lib.ToString() == libPath)
				{
					// Change current library
					// (will do nothing if this is the current library)
					displayLibrary(lib);

					// We're done here
					return;
				}
			}

			// This is a new library path
			// Create a new library and add it to the list

			RLibrary new_lib = new RLibrary(libPath);
			mState.Libraries.Add(new_lib);

			// Update combo box

			UpdateLibraryList();

			// Select the new library if not already selected

			if (LocationCombo.SelectedIndex == -1
				|| LocationCombo.SelectedItem == null
				|| LocationCombo.SelectedItem.ToString() != libPath)
			{
				LocationCombo.SelectedIndex = LocationCombo.FindStringExact(libPath);
			}

			// Change current library
			// (will do nothing if this is the current library)
			displayLibrary(new_lib);
		}


		//*****************************************
		/// <summary>
		/// Updates the library path combo box
		/// </summary>
		//*****************************************
		protected override void UpdateLibraryList()
		{
			// Prevent updates
			LocationCombo.BeginUpdate();

			// Get currently selected object or null
			object selection = LocationCombo.SelectedItem;

			// Remove all items
			LocationCombo.Items.Clear();

			// Add all items
			// We directly add all RLibrary references.
			// Items will be displayed using their ToString() method.
			LocationCombo.Items.AddRange(mState.Libraries.ToArray());

			// If there was a selection, try to reselect the same
			// object or same path

			if (selection != null)
			{
				// Note: check using ToString() and not AbsRootPath
				// as ToString is what gets displayed and what the user must type.
				int n = LocationCombo.FindStringExact(selection.ToString());
				if (n >= 0)
					LocationCombo.SelectedIndex = n;
			}

			// End updates
			LocationCombo.EndUpdate();
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
		protected override void OnFirstActivated()
		{
			// load all settings
			loadSettings();

			// apply defaults
			reloadPrefs();

			// Force a redraw
			this.Refresh(); 
			Application.DoEvents();

			// Populate the library popup first
			UpdateLibraryList();

			if (mState.CurrentLibrary != null)
			{
				// If we can find it in the popup, select it
				// Note: check using ToString() and not AbsRootPath
				// as ToString is what gets displayed and what the user must type.
				int n = LocationCombo.FindStringExact(mState.CurrentLibrary.ToString());

				if (n >= 0)
				{
					// Selecting the index will update the popup and display the library
					// but displayLibrary would do nothing as it would notice this
					// library is already the current one. So let's trick it here
					// by nullifying the current library.
					mState.CurrentLibrary = null;
					LocationCombo.SelectedIndex = n;

					System.Diagnostics.Debug.Assert(mState.CurrentLibrary != null);
				}
			}
		}

		//**********************************************************
		/// <summary>
		/// This callback is called when a node has been selected 
		/// in the directory tree.
		/// The selected node is given as argument.
		/// </summary>
		//**********************************************************
		protected override void OnDirTreeNodeSelected(TreeNode node)
		{
			System.Diagnostics.Debug.Assert(node != null && node.Tag != null);
			
			if (node != null && node.Tag != null)
			{
				System.Diagnostics.Debug.Assert(node.Tag is RDir);
				
				if (node.Tag is RDir)
					displayDirImages(node.Tag as RDir);
			}
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//*****************
		private void init()
		{
			// create state
			mState = new RAppState();

			// set loggers
			// Must use async logger pass-thru if the object might log
			// from a worker thread.
			// DEBUG
			// ImagesView.Logger = new RAsyncLog(this);

			// setup UI -- most of it is done in OnFirstActivated
		}


		//*************************
		/// <summary>
		/// Loads settings specific to this window.
		/// Done only once when the window is created.
		/// </summary>
		//*************************
		private void loadSettings()
		{
			// load settings
			RMainModule.Pref.Load();

			// tell Windows not to change this position automatically
			this.StartPosition = FormStartPosition.Manual;

			// load position of this window
			Rectangle r;
			if (RMainModule.Pref.GetRect(RPrefConstants.kMainForm, out r))
			{
				// RM 20050307 No longer change the size of the window.
				// This is because the window cannot be resized manually,
				// instead it adapts to the size of the inner video preview.
				this.Location = r.Location;
			}

			// load state
			mState.LoadFromPrefs(RMainModule.Pref);

			// <insert other setting stuff here>

		}


		//*************************
		private void saveSettings()
		{
			// save state
			mState.SaveToPrefs(RMainModule.Pref);

			// save position & size of this window
			RMainModule.Pref.SetRect(RPrefConstants.kMainForm, this.Bounds);

			// save settings
			RMainModule.Pref.Save();
		}


		//************************
		/// <summary>
		/// (Re)Loads app-wide preferences.
		/// Done anytime the user applies changes to the preference window
		/// or once at startup.
		/// </summary>
		//************************
		private void reloadPrefs()
		{
			updateButtons();

			Log("Prefs reloaded");
		}


		//-------------------------------------------
		//-------------------------------------------


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


		//******************************************
		private void createPrefWindow(bool visible)
		{
			if (mPrefForm == null)
			{
				mPrefForm = new RPrefForm();
				mPrefForm.Show();
			}
		}


		//******************************
		private void closePrefWindow()
		{
			if (mPrefForm != null)
			{
				mPrefForm.CanClose = true;
				mPrefForm.Close();
				mPrefForm = null;
			}
		}


		//**************************
		/// <summary>
		/// Not used
		/// </summary>
		//**************************
		private void updateButtons()
		{
		}


		//-------------------------------------------
		//-------------------------------------------


		//***************************************
		/// <summary>
		/// Change the current library to be this library
		/// and display it.
		/// 
		/// Note that lib could be null to deselect the
		/// existing library.
		/// </summary>
		//***************************************
		private void displayLibrary(RLibrary lib)
		{
			if (mState.CurrentLibrary != lib)
			{
				resetDirTree();

				mState.CurrentLibrary = lib;

				if (lib != null)
				{
					ImageListLabel = lib.ToString();

					fillDirTree(lib.Directories);

					// select the first node, if any
					if (DirectoryTree.Nodes.Count > 0)
						DirectoryTree.SelectedNode = DirectoryTree.Nodes[0];
				}
				else
				{
					ImageListLabel = "Please select a library first.";
				}
			}
		}


		//*************************
		/// <summary>
		/// Empty the dir tree.
		/// Deselect current stuff first if necessary.
		/// </summary>
		//*************************
		private void resetDirTree()
		{
			// remove selection
			DirectoryTree.SelectedNode = null;

			// empty
			DirectoryTree.Nodes.Clear();

		}


		//***************************************
		/// <summary>
		/// Fill the directory tree with a new dir list
		/// from the current library
		/// </summary>
		//***************************************
		private void fillDirTree(RDir[] dir_list)
		{
			// Block updates
			DirectoryTree.BeginUpdate();

			// fill tree recursively
			addDirs2Tree(DirectoryTree.Nodes, dir_list);

			// End block updates
			DirectoryTree.EndUpdate();
		}


		//**********************************
		/// <summary>
		/// Utility method for fillDirTree that actually
		/// fills the tree recursively with directory items.
		/// </summary>
		//**********************************
		private void addDirs2Tree(TreeNodeCollection tree_node, RDir[] dir_list)
		{
			foreach(RDir dir in dir_list)
			{
				// create a tree node. Put the directory object
				// as the tag of the tree node.
				TreeNode tn = new TreeNode(dir.Name);
				tn.Tag = dir;
				tree_node.Add(tn);

				addDirs2Tree(tn.Nodes, dir.Directories);
			}
		}

		//-------------------------------------------
		//-------------------------------------------


		//*************************************
		/// <summary>
		/// Display images for a selected directory.
		/// 
		/// Note that the directory can be null to deselect
		/// the existing directory.
		/// </summary>
		//*************************************
		private void displayDirImages(RDir dir)
		{
			if (mState.CurrentDirectory != dir)
			{
				resetImagesView();

				mState.CurrentDirectory = dir;

				if (dir != null)
				{
					ImageListLabel = mState.CurrentLibrary.ToString() + " => " + dir.Name;

					fillImagesView(dir.Images);
				}
			}
		}


		//***************************
		/// <summary>
		/// Clear the image view.
		/// Remove any selected stuff if necessary
		/// </summary>
		//***************************
		private void resetImagesView()
		{
			// remove selection
			ImagesView.SelectedItems.Clear();

			// empty
			ImagesView.Items.Clear();

			// redraw
			ImagesView.Refresh();
		}


		//*********************************************
		/// <summary>
		/// Fill the image view with the images
		/// </summary>
		//*********************************************
		private void fillImagesView(RImage[] image_list)
		{
			foreach(RImage img in image_list)
			{
				// add it
				ImagesView.Items.Add(img);
			}

			// redraw
			ImagesView.Refresh();
		}


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		// forms
		private RDebugForm		mDebugForm = null;
		private RPrefForm		mPrefForm = null;

		// global data
		private RAppState		mState = null;

	} // class RMainFormEx
} // namespace Alfray.Rivet.RivetUI


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RMainFormEx.cs,v $
//	Revision 1.6  2005/07/23 15:00:22  ralf
//	Fix: Using Control.BeginInvoke instead of Control.Invoke in working thread
//	
//	Revision 1.5  2005/07/12 14:35:24  ralf
//	Display images in custom view.
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
//	Revision 1.2  2005/05/25 03:54:22  ralf
//	Implemented LocationCombo with storage in prefs.
//	Added RLibrary, RAppState, RTestSkeleton.
//	
//	Revision 1.1.1.1  2005/05/23 02:48:59  ralf
//	no message
//	
//---------------------------------------------------------------
