//*******************************************************************
/*

	Solution:	Xeres
	Project:	TestsConsole
	File:		RTestCommState.cs

	Copyright 2005, Raphael MOLL.

	This file is part of Xeres.

	Xeres is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Xeres is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Xeres; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

*/
//*******************************************************************



using System;

using NUnit.Framework;

using Alfray.Xeres.XeresLib;

//*************************************
namespace Alfray.Xeres.TestsConsole.XeresLib
{
	//***************************************************
	/// <summary>
	/// Summary description for RTestCommState.
	/// </summary>
	//***************************************************
	[TestFixture]
	public class RTestCommState
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
			s = new RCommState();
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
			Assert.IsNotNull(s);
			Assert.IsFalse(s.CommIsOn);
			Assert.IsFalse(s.VideoIsOn);
			Assert.IsFalse(s.AudioIsOn);
		}

		//****************
		[Test]
		public void TestChangeProperties()
		{
			Assert.IsFalse(s.CommIsOn);
			s.CommIsOn = true;
			Assert.IsTrue(s.CommIsOn);

			Assert.IsFalse(s.VideoIsOn);
			s.VideoIsOn = true;
			Assert.IsTrue(s.VideoIsOn);

			Assert.IsFalse(s.AudioIsOn);
			s.AudioIsOn = true;
			Assert.IsTrue(s.AudioIsOn);
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		RCommState s;

	} // class RTestCommState
} // namespace Alfray.Xeres.TestsConsole.XeresLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RTestCommState.cs,v $
//	Revision 1.2  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
//	
//	Revision 1.1  2005/02/21 06:22:51  ralf
//	New tests: RCommState, RDeviceInfo
//	
//---------------------------------------------------------------
