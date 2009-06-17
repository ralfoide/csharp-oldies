/*
 * Simple service to perform auto-updates.
 * 
 * Test by Ralf.
 * 
 * Reference: http://msdn.microsoft.com/msdnmag/issues/01/12/NETServ/
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;

using System.IO;
using System.Net;

namespace DhdTest1
{
	public class DhdService : System.ServiceProcess.ServiceBase
	{
		private System.Windows.Forms.Timer mTimer;
		private System.ComponentModel.IContainer components;

		public DhdService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
			// WriteLog("Dhd Service Initialized: Test1");
		}

		// The main entry point for the process
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new DhdService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.mTimer = new System.Windows.Forms.Timer(this.components);
			// 
			// mTimer
			// 
			this.mTimer.Interval = 600000;  // 600 sec = 10 min
			this.mTimer.Tick += new System.EventHandler(this.mTimer_Tick);
			// 
			// DhdService
			// 
			this.CanPauseAndContinue = true;
			this.CanShutdown = true;
			this.ServiceName = "DHD1";

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			mTimer.Start();
			WriteLog("Dhd Service Started");
			mTimer_Tick(null, null);
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			mTimer.Stop();
			WriteLog("Dhd Service Stopped");
		}

		private void mTimer_Tick(object sender, System.EventArgs e)
		{
			// Check for updates
			try 
			{
				const string kUrl = "http://ralf.alfray.com/test/dhd/updates.txt";
				WriteLog("Check: " + kUrl);

				WebClient wc = new WebClient();
				Stream stream = wc.OpenRead(kUrl);
				StreamReader sr = new StreamReader(stream);
				string data = sr.ReadToEnd();

				WriteLog("Data: " + data);
				System.Diagnostics.Debug.WriteLine("Update Data: " + data);
			} 
			catch(WebException ex) 
			{
				WriteLog("Exception 1: " + ex.ToString());
				System.Diagnostics.Debug.WriteLine("WebClient Exception: " + ex.ToString());
			}
		}

		private void WriteLog(string s) 
		{
			EventLog.WriteEntry(s);
		}
	}
}
