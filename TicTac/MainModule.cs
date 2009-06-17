//*******************************************************************
/* 

 		Project:	Simple Network Server
 		File:		MainModule.cs

*/ 
//*******************************************************************


using System;
using System.Windows.Forms;

namespace Alfray.TicTac
{
	/// <summary>
	/// Summary description for MainModule.
	/// </summary>
	public class MainModule
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			// test units
			Alfray.SimpleNetworkServer.EntityId.Test();

			Application.Run(new TestForm());
		}

	}
}


//---------------------------------------------------------------
//
//	$Log: MainModule.cs,v $
//	Revision 1.1  2003/06/10 04:43:32  ralf
//	Conforming to .Net naming rules
//	
//---------------------------------------------------------------

