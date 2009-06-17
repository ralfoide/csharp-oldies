//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetLib
	File:		RAppState.cs

	Copyright 2003, 2004, Raphael MOLL.

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
using System.Diagnostics;
using System.Collections;

using Alfray.LibUtils.Misc;


//*****************************
namespace Alfray.Rivet.RivetLib
{
	//***************************************************
	/// <summary>
	/// RAppState constains the global state of the application.
	/// </summary>
	//***************************************************
	public class RAppState: IDisposable
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//************************
		/// <summary>
		/// LibraryPaths is an array-list of RLibrary objects
		/// </summary>
		//************************
		public ArrayList Libraries
		{
			get
			{
				return mLibraries;
			}
		}


		//****************************
		/// <summary>
		/// Gets or sets the current library.
		/// When setting the library, asserts that it is present in the
		/// Libraries array list.
		/// When no library is selected, this is null.
		/// To "deselect" the library, affect null.
		/// </summary>
		//****************************
		public RLibrary CurrentLibrary
		{
			get
			{
				return mCurrentLib;
			}

			set
			{
				mCurrentLib = value;

				if (mCurrentLib != null)
					System.Diagnostics.Debug.Assert(mLibraries.IndexOf(mCurrentLib) != -1);
			}
		}


		//****************************
		/// <summary>
		/// Gets or sets the current directory.
		/// When setting the directory, asserts that it is present in the
		/// current library.
		/// When no directory is selected, this is null.
		/// To deselect the directory, affect null.
		/// </summary>
		//****************************
		public RDir CurrentDirectory
		{
			get
			{
				return mCurrentDir;
			}

			set
			{
				mCurrentDir = value;

				if (mCurrentDir != null)
				{
					System.Diagnostics.Debug.Assert(CurrentLibrary != null);
					assertDirInLib(mCurrentDir, CurrentLibrary);
				}
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//****************
		public RAppState()
		{
			mLibraries = new ArrayList();
		}

		#region IDisposable Members

		//*******************
		/// <summary>
		/// Dispose of resources (bitmaps, files, etc.)
		/// </summary>
		//*******************
		public void Dispose()
		{
			if (mLibraries != null)
			{
				foreach(RLibrary lib in mLibraries)
					lib.Dispose();
				mLibraries = null;
			}
		}

		#endregion

		//**********************************
		/// <summary>
		/// Loads state from the current pref object
		/// </summary>
		//**********************************
		public void LoadFromPrefs(RPref pref)
		{
			// Load all libraries as string

			string[] lib_paths;
			if (pref.GetEnumeration(RPrefConstants.kStateLibList, out lib_paths))
			{
				Libraries.Clear();

				foreach(string path in lib_paths)
					Libraries.Add(new RLibrary(path));
			}

			// Get selected library, if any

			CurrentLibrary = null;

			string name_curr_lib = pref[RPrefConstants.kStateLibCurr];
			if (name_curr_lib != null && name_curr_lib != "")
			{
				// Try to locate the lib in the list
				foreach(RLibrary lib in Libraries)
				{
					// Note: check using ToString() and not AbsRootPath
					// as ToString is what gets displayed and what the user must type.
					if (lib.ToString() == name_curr_lib)
					{
						CurrentLibrary = lib;
						break;
					}
				}
			}
		}


		//********************************
		/// <summary>
		/// Saves state into the current pref object
		/// </summary>
		//********************************
		public void SaveToPrefs(RPref pref)
		{
			// Save all libraries as string

			pref.SetEnumeration(RPrefConstants.kStateLibList, Libraries);

			// Save name of current selected library
			// Note: check using ToString() and not AbsRootPath
			// as ToString is what gets displayed and what the user must type.

			pref[RPrefConstants.kStateLibCurr] = CurrentLibrary != null ? CurrentLibrary.ToString() : "";

		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//*******************************************
		/// <summary>
		/// Check that a given directory exits in a given
		/// library, recursively.
		/// This method is conditional and exists only when
		/// the DEBUG conditional flag is defined.
		/// </summary>
		//*******************************************
		[Conditional("DEBUG")]
		private void assertDirInLib(RDir dir, RLibrary lib)
		{
			bool found;

			assertDirInDirList(dir, lib.Directories, out found);

			if (!found)
				System.Diagnostics.Debug.Fail("Current Directory not in Current Library",
					"Current Directory " + dir.ToString() + " not found in current library " + lib.ToString());
		}


		//*******************************************
		/// <summary>
		/// Check that a given directory exits in a given
		/// list of directories, recursively.
		/// This method is conditional and exists only when
		/// the DEBUG conditional flag is defined.
		/// </summary>
		//*******************************************
		//--RM 20090616--[Conditional("DEBUG")]
		private void assertDirInDirList(RDir dir, RDir[] list, out bool found)
		{
			// Search at the first level
			foreach(RDir d in list)
			{
				if (d.FullPath == dir.FullPath)
				{
					found = true;
					return;
				}
			}

			// If not search in inner levels
			foreach(RDir d in list)
			{
				assertDirInDirList(dir, d.Directories, out found);

				if (found)
					return;
			}

			// not found
			found = false;
		}

		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private ArrayList	mLibraries	= null;
		private RLibrary	mCurrentLib	= null;
		private RDir		mCurrentDir	= null;

	} // class RAppState
} // namespace Alfray.Rivet.RivetLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RAppState.cs,v $
//	Revision 1.4  2005/05/31 06:16:52  ralf
//	Implementinf IDisposable for RFSItem, RDir, RImage and RAppState
//	
//	Revision 1.3  2005/05/31 00:12:16  ralf
//	Current selected directory in RAppState.
//	Display image for select directory in main form.
//	
//	Revision 1.2  2005/05/29 23:02:13  ralf
//	Fixes. Load last current library at startup.
//	
//	Revision 1.1  2005/05/25 03:54:22  ralf
//	Implemented LocationCombo with storage in prefs.
//	Added RLibrary, RAppState, RTestSkeleton.
//	
//---------------------------------------------------------------
