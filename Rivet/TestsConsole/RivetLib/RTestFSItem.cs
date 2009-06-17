//*******************************************************************
/*

	Solution:	Rivet
	Project:	TestsConsole
	File:		RTestFSItem.cs

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
	/// RTestFSItem tests RFSItem.
	/// This class derived from RFSItem in order to be
	/// able to instantiate it (the constructor is protected and
	/// thus by deriving from it we gain access to the constructor.)
	/// </summary>
	//***************************************************
	[TestFixture]
	public class RTestFSItem: RFSItem
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

        public RTestFSItem() : base() {
        }


        public RTestFSItem(RDir parent, string name) : base(parent, name) {
        }

		
		//****************
		[SetUp]
		public void SetUp()
		{
            t = new RTestFSItem();
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

			Assert.IsNull(t.Parent);
			Assert.AreEqual("", t.Name);

			// ToString and FullPath should be empty strings
			Assert.AreEqual("", t.ToString());
			Assert.AreEqual("", t.FullPath);
		}

		//****************
		[Test]
		public void TestInit2()
		{
			Assert.IsNotNull(t);

			// Reinstantiate a new test object using the other
			// constructor

			RDir d = new RDir(null, "root");
            RFSItem t2 = new RTestFSItem(d, "test");

			Assert.IsNotNull(t2);
			Assert.AreEqual(d, t2.Parent);
			Assert.AreEqual("test", t2.Name);

			// ToString and FullPath should be equal to the name
			// combined with the parent's full path (its name)

			string combined = System.IO.Path.Combine("root", "test");

			Assert.AreEqual(combined, t2.ToString());
			Assert.AreEqual(combined, t2.FullPath);
		}

		//****************
		[Test]
		public void TestName()
		{
			Assert.IsNotNull(t);

			t.Name = "test";

			// Since parent is null, ToString and FullPath should be
			// equal to the name

			Assert.AreEqual("test", t.ToString());
			Assert.AreEqual("test", t.FullPath);
		}

		//****************
		[Test]
		public void TestName2()
		{
			Assert.IsNotNull(t);

			t.Parent = new RDir(null, "root");

			t.Name = "test";

			// ToString and FullPath should be equal to the name
			// combined with the parent's full path (it's name)

			string combined = System.IO.Path.Combine("root", "test");

			Assert.AreEqual(combined, t.ToString());
			Assert.AreEqual(combined, t.FullPath);
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		private RFSItem t;


	} // class RTestFSItem
} // namespace Alfray.Rivet.TestsConsole.RivetLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RTestFSItem.cs,v $
//	Revision 1.1  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//---------------------------------------------------------------
