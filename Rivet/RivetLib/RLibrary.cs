//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetLib
	File:		RLibrary.cs

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

//*************************************
namespace Alfray.Rivet.RivetLib
{
	//***************************************************
	/// <summary>
	/// Summary description for RLibrary.
	/// </summary>
	//***************************************************
	public class RLibrary: IDisposable
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//***********************
		/// <summary>
		/// Return the absolute root path of the library
		/// or "" if there isn't any
		/// </summary>
		//***********************
		public string AbsRootPath
		{
			get
			{
				return mRootDir == null ? "" : mRootDir.Name;
			}
		}


		//***********************
		/// <summary>
		/// Returns the list of directories of the library.
		/// </summary>
		//***********************
		public RDir[] Directories
		{
			get
			{
				return mRootDir.Directories;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//******************************
		/// <summary>
		/// Construct an RLibrary with a known root path.
		/// The path must be absolute.
		/// Content will not be loaded until either Load()
		/// is called or items are accessed.
		/// </summary>
		//******************************
		public RLibrary(string abs_path)
		{
			mRootDir = new RDir(null, abs_path);
		}


		//*****************************
		/// <summary>
		/// Override Object.ToString() to return the absolute
		/// root path.
		/// </summary>
		//*****************************
		public override string ToString()
		{
			return AbsRootPath;
		}

		#region IDisposable Members

		//*******************
		/// <summary>
		/// Dispose methods associated with resources
		/// (bitmaps, files, etc.)
		/// </summary>
		//*******************
		public void Dispose()
		{
			if (mRootDir != null)
			{
				mRootDir.Dispose();
				mRootDir = null;
			}
		}

		#endregion

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private RDir	mRootDir = null;

	} // class RLibrary
} // namespace Alfray.Rivet.RivetLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RLibrary.cs,v $
//	Revision 1.3  2005/05/31 06:16:52  ralf
//	Implementinf IDisposable for RFSItem, RDir, RImage and RAppState
//	
//	Revision 1.2  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//	Revision 1.1  2005/05/25 03:54:22  ralf
//	Implemented LocationCombo with storage in prefs.
//	Added RLibrary, RAppState, RTestSkeleton.
//	
//---------------------------------------------------------------
