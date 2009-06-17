//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetLib
	File:		RDir.cs

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
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;


//*************************************
namespace Alfray.Rivet.RivetLib
{
	//***************************************************
	/// <summary>
	/// RDir lists the content of a directory.
	/// Items are fetched only when asked for.
	/// </summary>
	//***************************************************
	public class RDir: RFSItem
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//***********************
		/// <summary>
		/// Returns the list of directories contained in
		/// this directory.
		/// Items are loaded when first accessed.
		/// </summary>
		//***********************
		public RDir[] Directories
		{
			get
			{
				if (mDirs == null)
					loadDirs();
				return mDirs;
			}
		}


		//***********************
		/// <summary>
		/// Returns the list of images contained in
		/// this directory.
		/// Only images which name match the image filter
		/// will be loaded.
		/// Items are loaded when first accessed.
		/// </summary>
		//***********************
		public RImage[] Images
		{
			get
			{
				if (mImages == null)
					loadImages();
				return mImages;
			}
		}


		//***********************
		/// <summary>
		/// Regexp pattern used when loading images.
		/// The default is ".*\.je?pg$"
		/// </summary>
		//***********************
		public string ImageFilter
		{
			get
			{
				return mImageFilter;
			}
			set
			{
				if (value != mImageFilter)
				{
					mImageFilter = value;
					
					// will need to reload
					mImages = null;
				}
			}
		}



		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//***********************************
		/// <summary>
		/// Constructs a directory with the given name
		/// and parent
		/// </summary>
		//***********************************
		public RDir(RDir parent, string name)
			 : base(parent, name)
		{

		}

		
		
		#region IDisposable Members

		//*******************
		/// <summary>
		/// Release disposable resources (such as GDI bitmaps
		/// or file handles.)
		/// </summary>
		//*******************
		public override void Dispose()
		{
			// Dispose all resources

			if (mDirs != null)
			{
				foreach(RDir dir in mDirs)
					dir.Dispose();
				mDirs = null;
			}

			if (mImages != null)
			{
				foreach(RImage img in mImages)
					img.Dispose();
				mImages = null;
			}

			// Call base class

			base.Dispose();
		}

		#endregion


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//*********************
		/// <summary>
		/// Parse directory content and fill up mDirs.
		/// The array will be created even if empty.
		/// </summary>
		//*********************
		private void loadDirs()
		{
			DirectoryInfo di = new DirectoryInfo(FullPath);

			if (di.Exists)
			{
				DirectoryInfo[] dis = di.GetDirectories();

				mDirs = new RDir[dis.Length];

				int i = 0;

				foreach(DirectoryInfo d in dis)
				{
					if (d.Name != "." && d.Name != "..")
						mDirs[i++] = new RDir(this, d.Name);
				}
			}
			else
			{
				mDirs = new RDir[0];
			}
		}


		//***********************
		/// <summary>
		/// Parse directory content and fill up mImages.
		/// The array will be created even if empty.
		/// </summary>
		//***********************
		private void loadImages()
		{
			DirectoryInfo di = new DirectoryInfo(FullPath);

			if (di.Exists)
			{
				Regex re = new Regex(mImageFilter, RegexOptions.IgnoreCase | RegexOptions.Singleline);

				FileInfo[] fis = di.GetFiles();

				ArrayList list = new ArrayList();

				foreach(FileInfo fi in fis)
				{
					if (re.IsMatch(fi.Name))
						list.Add(new RImage(this, fi.Name));
				}

				mImages = list.ToArray(typeof(RImage)) as RImage[];
			}
			else
			{
				mImages = new RImage[0];
			}

		}


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private string		mImageFilter = @".*\.je?pg$";

		private RDir[]		mDirs;
		private	RImage[]	mImages;

	} // class RDir
} // namespace Alfray.Rivet.RivetLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RDir.cs,v $
//	Revision 1.2  2005/05/31 06:16:52  ralf
//	Implementinf IDisposable for RFSItem, RDir, RImage and RAppState
//	
//	Revision 1.1  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//---------------------------------------------------------------
