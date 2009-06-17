//*******************************************************************
/* 

 		Project:	Desktop
 		File:		RMainModule.cs

*/ 
//*******************************************************************

using System;
using System.Windows.Forms;

//*************************************
namespace Alfray.LiveXml.LiveXmlApp
{
	//***************************************************
	/// <summary>
	/// Summary description for RMainModule.
	/// </summary>
	public class RMainModule
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
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//****************
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new RMainForm());
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

	} // class RMainModule
} // namespace Alfray.LiveXml.LiveXmlApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RMainModule.cs,v $
//	Revision 1.1.1.1  2005/02/18 22:54:53  ralf
//	A skeleton application template, with NUnit testing
//	
//---------------------------------------------------------------
