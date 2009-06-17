//*******************************************************************
/* 

 		Project:	SneLibTest
 		File:		RMain.cs

*/ 
//*******************************************************************

using System;
using System.IO;
using System.Text;

using NUnit.Core;
using NUnit.Util;

//*************************
namespace Alfray.SneLibTest
{
	//***************************************************
	/// <summary>
	/// Summary description for RMain.
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

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			System.Console.Out.WriteLine("Running NUnit tests, please wait.");

			TestDomain test_domain = new TestDomain();

			string assembly_path = System.Reflection.Assembly.GetExecutingAssembly().Location;
			NUnitProject project = NUnitProject.FromAssembly(assembly_path);

			Test test = project.LoadTest(test_domain);

			// string xmlResult = "TestResult.xml";

			// Extracted from NUnit.ConsoleUI.Execute()
			if (true)
			{
				EventListener collector = null;
				
				// if silent
					collector = new NullListener(); 
				//collector = new EventCollector();

				ConsoleWriter outStream = new ConsoleWriter(Console.Out);
				ConsoleWriter errorStream = new ConsoleWriter(Console.Error);
			
				string savedDirectory = Environment.CurrentDirectory;
				TestResult result = test_domain.Run(collector, outStream, errorStream);
				Directory.SetCurrentDirectory( savedDirectory );
			
				Console.WriteLine();

				StringBuilder builder = new StringBuilder();
				XmlResultVisitor resultVisitor = new XmlResultVisitor(new StringWriter( builder ), result);
				result.Accept(resultVisitor);
				resultVisitor.Write();

				// xmlOutput = builder.ToString();

				//if (!silent)
				//	CreateSummaryDocument();

				//int resultCode = 0;
				//if(result.IsFailure)
				//	resultCode = 1;
				//return resultCode;
			}
		}

		
		//****************
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
} // namespace SneLibTest


//---------------------------------------------------------------
//
//	$Log: RMain.cs,v $
//	Revision 1.3  2004/12/04 05:34:46  ralf
//	NUnit
//	
//	Revision 1.2  2004/01/05 06:29:14  ralf
//	Asynchronous handling of listen.
//	Added RSneClient vs RSneListerner and RSneConnection and test classes.
//	
//	Revision 1.1.1.1  2003/12/24 08:03:31  ralf
//	Empty skeleton with NUnit module
//	
//---------------------------------------------------------------
