//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetLib
	File:		RFSItem.cs

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
using System.IO;

//*************************************
namespace Alfray.Rivet.RivetLib
{
	//***************************************************
	/// <summary>
	/// RFSItem is the abstract base class for a file system item
	/// which can be a file or a directory.
	/// Such an item as comomn attributes such as a name and a
	/// parent directory and common operations such as move, rename,
	/// etc.
	/// The constructor is protected.
	/// Instantiate classes RDir or RImage instead of this one.
	/// </summary>
	//***************************************************
	public class RFSItem: IDisposable
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//****************
		/// <summary>
		/// Gets or sets the name (leaf name, not full path)
		/// of this item.
		/// It is an error to set the name to null. 
		/// It must be at least an empty string.
		/// </summary>
		//****************
		public virtual string Name
		{
			get
			{
				return mName;
			}
			set
			{
				System.Diagnostics.Debug.Assert(value != null);
				if (value == null)
					mName = "";
				else
					mName = value;
			}
		}


		//****************
		/// <summary>
		/// Gets or sets the parent of this item
		/// or null if there is no parent.
		/// </summary>
		//****************
		public virtual RDir Parent
		{
			get
			{
				return mParent;
			}
			set
			{
				mParent = value;
			}
		}


		//********************
		/// <summary>
		/// Returns the full path of this item by combining
		/// the full path of the parent (if any) with this one.
		/// </summary>
		//********************
		public string FullPath
		{
			get
			{
				if (mParent != null)
					return Path.Combine(Parent.FullPath, mName);
				else
					return mName;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------


		//*****************************
		/// <summary>
		/// Override Object.ToString() to return the 
		/// full path
		/// </summary>
		//*****************************
		public override string ToString()
		{
			return FullPath;
		}


		//*************************************
		/// <summary>
		/// If both objects are RFSItem or derived,
		/// compares their full path.
		/// </summary>
		//*************************************
		public override bool Equals(object obj)
		{
			if (obj != null && obj is RFSItem)
				return FullPath == (obj as RFSItem).FullPath;

			return base.Equals(obj);
		}


		//*******************************
		/// <summary>
		/// Returns the hash code of the full path string
		/// </summary>
		//*******************************
		public override int GetHashCode()
		{
			return FullPath.GetHashCode();
		}



		
		#region IDisposable Members

		//***************************
		/// <summary>
		/// Release disposable resources (such as GDI bitmaps
		/// or file handles.)
		/// </summary>
		//***************************
		public virtual void Dispose()
		{
			// Noothing in base class.
		}

		#endregion

		// TBDL: Move(), Rename(), using standard System.IO operations
		// or returning NotImplementedInBaseClassException


		//-------------------------------------------
		//----------- Protected Methods -------------
		//-------------------------------------------

		//****************
		/// <summary>
		/// Constructs an empty FS item with no name
		/// and no parent.
		/// 
		/// Protected constructor.
		/// This class cannot be instantiated.
		/// </summary>
		//****************
		protected RFSItem()
		{
		}


		//**************************************
		/// <summary>
		/// Constructs a directory with the given name
		/// and parent
		/// 
		/// Protected constructor.
		/// This class cannot be instantiated.
		/// </summary>
		//*****************************************
		protected RFSItem(RDir parent, string name)
		{
			Parent = parent;
			Name = name;
		}

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		private string	mName = "";
		private RDir	mParent = null;

	} // class RFSItem
} // namespace Alfray.Rivet.RivetLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RFSItem.cs,v $
//	Revision 1.3  2005/07/12 14:32:15  ralf
//	Added GetHashCode and Equals to map on the FullName.
//	Ability to override Name and Parent properties.
//	
//	Revision 1.2  2005/05/31 06:16:52  ralf
//	Implementinf IDisposable for RFSItem, RDir, RImage and RAppState
//	
//	Revision 1.1  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//---------------------------------------------------------------
