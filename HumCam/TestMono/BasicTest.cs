//********************************************************************
//	Project:	HumCam
//	File:		BasicTests.cs
//********************************************************************

using System;
using System.Diagnostics;

namespace Alfray.HumCam.TestMono
{
	/// <summary>
	/// Test that a simple class can be executed under both Mono and VS.Met
	/// </summary>
	class BasicTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			System.Console.Out.WriteLine("Alfray.HumCam.TestMono > Main - * BEGIN *");

			BasicTest bt = new BasicTest();
			bt.DoSomething();
			bt.TestSystemCall();

			System.Console.Out.WriteLine("Alfray.HumCam.TestMono > Main - * END *");
		}

		static void DebugLog(string s)
		{
			// [Debug]
			System.Console.Out.WriteLine(s);
		}


		private void DoSomething()
		{
			DebugLog("Alfray.HumCam.TestMono > BasicTest.DoSomething");
		}

		private void TestSystemCall()
		{
			DebugLog("Alfray.HumCam.TestMono > BasicTest.TestSystemCall");

			Process ps = new Process();
			ps.StartInfo.FileName = "/bin/bash";
			ps.StartInfo.Arguments = "-c ls";

			// -- Do this to redirect input/output: --
			ps.StartInfo.ErrorDialog = false;
			ps.StartInfo.UseShellExecute = false;
			ps.StartInfo.RedirectStandardOutput = true;

			ps.Start();    

			Console.WriteLine("Output: ", ps.StandardOutput.ReadToEnd());

			ps.WaitForExit();

			DebugLog("TestSystemCall: Done");

		}
	}
}

//********************************************************************
//	$Log: BasicTest.cs,v $
//	Revision 1.3  2004/05/03 05:27:42  ralf
//	Tests
//	
//********************************************************************

