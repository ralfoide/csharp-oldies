//*******************************************************************
/* 

 		Project:	HintTest
 		File:		RMain.cs

*/ 
//*******************************************************************

using System;
using System.IO;
using System.Text;

using NUnit.Core;
using NUnit.Util;
using NUnit.Framework;

//*************************************
namespace Alfray.LiveXml.TestsConsole
{
	//***************************************************
	/// <summary>
	/// RMain is the entry point of the test application.
	/// </summary>
	public class RMain
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

		//*******************************************
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//*******************************************
		[STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine("FacaAFace Main");
			System.Diagnostics.Trace.WriteLine("FacaAFace Main");

			System.Console.Out.WriteLine("Running NUnit tests, please wait.");
 
			TestDomain test_domain = new TestDomain();
 
			string assembly_path = System.Reflection.Assembly.GetExecutingAssembly().Location;
			NUnitProject project = NUnitProject.FromAssembly(assembly_path);
 
			Test test = project.LoadTest(test_domain);
 
			// Extracted from NUnit.ConsoleUI.Execute()
			EventListener collector = null;
			collector = new NullListener(); 
 
			ConsoleWriter outStream = new ConsoleWriter(Console.Out);
			ConsoleWriter errorStream = new ConsoleWriter(Console.Error);
 
			string savedDirectory = Environment.CurrentDirectory;
			TestResult result = test_domain.Run(collector, outStream, errorStream);
			Directory.SetCurrentDirectory(savedDirectory);
  
			Console.WriteLine();
 
			StringBuilder builder = new StringBuilder();
			XmlResultVisitor resultVisitor = new XmlResultVisitor(new StringWriter( builder ), result);
			result.Accept(resultVisitor);
			resultVisitor.Write();
		}

		
		//*****************
		public RMain()
		{
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

	} // class RMain
} // namespace Alfray.LiveXml.TestsConsole


//---------------------------------------------------------------
//
//	$Log: RMain.cs,v $
//	Revision 1.1.1.1  2005/02/18 22:54:53  ralf
//	A skeleton application template, with NUnit testing
//	
//	
//---------------------------------------------------------------
