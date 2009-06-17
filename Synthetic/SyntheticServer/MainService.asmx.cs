//******************************************************************
/*  
 		Project:	Synthetic
		Subproject:	SyntheticServer
 		File:		MainService.cs
  
*/
//******************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace Alfray.Synthetic.Server
{
	/// <summary>
	/// Summary description for Service1.
	/// </summary>
	[WebService(Namespace="urn://alfray.com/synthetic/2003/M1/WebService",
				Name="SyntheticServer",
				Description="Serves a repository of SyntheticPlans")]
	public class Service1 : System.Web.Services.WebService
	{
		public Service1()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		// WEB SERVICE EXAMPLE
		// The HelloWorld() example service returns the string Hello World
		// To build, uncomment the following lines then save and build the project
		// To test this web service, press F5

		[WebMethod]
		public string HelloWorld()
		{
			return "Hello World";
		}
	}
}


//---------------------------------------------------------------
//
//	$Log: MainService.asmx.cs,v $
//	Revision 1.2  2003/06/10 04:47:24  ralf
//	Conforming to .Net naming rules
//	
//---------------------------------------------------------------

