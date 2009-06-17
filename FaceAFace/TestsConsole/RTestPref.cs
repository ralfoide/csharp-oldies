//*******************************************************************
/* 

 		Project:	TestsConsole
 		File:		RTestPref.cs

*/ 
//*******************************************************************

using System;

using NUnit.Framework;
using Alfray.Faf.Utils;

//*************************************
namespace Alfray.Faf.TestsConsole
{
	//***************************************************
	/// <summary>
	/// Summary description for RTestPref.
	/// </summary>
	//***************************************************
	[TestFixture]
	public class RTestPref: RPref
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
			p = new RPref();
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
			Assert.IsNotNull(p);
			Assert.IsNotNull(p.Settings);

			// This key should not exist
			Assert.IsNull(p[kTestKey]);
		}

		//************************
		[Test]
		public void TestSetRemoveKey()
		{
			// This key should not exist
			Assert.IsNull(p[kTestKey]);

			const string kValue = "test-value-42-42";

			// add it
			p[kTestKey] = kValue;

			// it must exist now
			Assert.AreEqual(kValue, p[kTestKey]);

			// remove it
			p[kTestKey] = null;

			// This key should not exist
			Assert.IsNull(p[kTestKey]);

			// check it doesn't *really* exist anymore
			Assert.IsFalse(p.Settings.ContainsKey(kTestKey));
		}

		//**************************
		[Test]
		public void TestSettingFileName()
		{
			string s = this.settingFileName();
			Assert.IsTrue(s != null && s != "");
		}

		//************************
		[Test]
		public void TestLoad()
		{
			Assert.IsTrue(p.Load());
		}

		//************************
		[Test]
		public void TestSave()
		{
			Assert.IsTrue(p.Load());
			Assert.IsTrue(p.Save());
		}



		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------




		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		const string kTestKey = "reserved-test-key";

		private RPref p;

	} // class RTestPref
} // namespace Alfray.Faf.TestsConsole


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log$
//---------------------------------------------------------------
