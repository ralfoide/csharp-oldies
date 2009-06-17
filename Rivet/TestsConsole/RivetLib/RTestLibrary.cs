//*******************************************************************
/*

	Solution:	Rivet
	Project:	TestsConsole
	File:		RTestLibrary.cs

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

using NUnit.Framework;

using Alfray.Rivet.RivetLib;

//*************************************
namespace Alfray.Rivet.TestsConsole.RivetLib
{
	//***************************************************
	/// <summary>
	/// RTestLibrary tests RLibrary.
	/// </summary>
	//***************************************************
	[TestFixture]
	//***************************************************
	public class RTestLibrary
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
		[SetUp]
		public void SetUp()
		{
			t = new RLibrary(mDefaultPath);
		}

		//********************
		[TearDown]
		public void TearDown()
		{
		}

		//****************
		[Test]
		public void TestInit()
		{
			Assert.IsNotNull(t);

			// ToString and AbsRootPath should be empty strings

			Assert.AreEqual(mDefaultPath, t.ToString());
			Assert.AreEqual(mDefaultPath, t.AbsRootPath);
		}


		//****************
		[Test]
		public void TestToString()
		{
			Assert.IsNotNull(t);

			// The path should be reflected by ToString

			Assert.AreEqual(mDefaultPath, t.ToString());

			// and by the property AbsRootPath

			Assert.AreEqual(t.ToString(), t.AbsRootPath);
		}



		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private RLibrary t;

		private string mDefaultPath = @"C:\Temp";

	} // class RTestLibrary
} // namespace Alfray.Rivet.TestsConsole.RivetLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RTestLibrary.cs,v $
//	Revision 1.3  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//	Revision 1.2  2005/05/30 20:44:55  ralf
//	Using uniform variable "t" for tested object
//	
//	Revision 1.1  2005/05/25 03:54:22  ralf
//	Implemented LocationCombo with storage in prefs.
//	Added RLibrary, RAppState, RTestSkeleton.
//	
//---------------------------------------------------------------
