//*******************************************************************
/* 

 		Project:	LuaTest
 		File:		TestExec.cs

*/ 
//*******************************************************************

using System;
using Alfray.LuaManaged;
using NUnit.Framework;

//*********************************
namespace Alfray.LuaManaged.LuaTest
{
	//*******************
	[TestFixture]
	public class TestExec
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

		
		//**********************
		[Test]
		public void TestNUnit()
		{
			// Assert.IsTrue(false);
			Assert.IsTrue(true);
		}


		//**********************
		[Test]
		public void TestEmpty()
		{
			RLuaScript script = new RLuaScript();

			String cmd = "10+2";

			int status = script.ExecuteString(cmd);

			Assert.AreEqual(status, 0);
		}

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

	} // class TestExec
} // namespace Alfray.LuaManaged.LuaTest


//---------------------------------------------------------------
//
//	$Log: TestExec.cs,v $
//	Revision 1.1  2004/01/21 19:11:58  ralf
//	Added LuaTest (NUnit) project
//	
//---------------------------------------------------------------
