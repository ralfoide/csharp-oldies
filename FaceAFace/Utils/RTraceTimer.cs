//*******************************************************************
/* 

 		Project:	AcqLib
 		File:		RTraceTimer.cs

*/ 
//*******************************************************************

using System;

//*************************************
namespace Alfray.Faf.Utils
{
	//***************************************************
	/// <summary>
	/// Summary description for RTraceTimer.
	/// </summary>
	//***************************************************
	public class RTraceTimer
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

		
		//****************************************
		public RTraceTimer(RILog log, string name)
		{
			mLog = log;
			mName = name;
			mStart = DateTime.Now;
			mPoint = mStart;
		}


		//********************
		public void SetPoint()
		{
			mPoint = DateTime.Now;
		}


		//********************************
		public void CheckPoint(string msg)
		{
			DateTime now = DateTime.Now;
			TimeSpan from_start = mStart - now;
			TimeSpan from_point = mPoint - now;

			mLog.Log(String.Format("[RT: {0}/{1}] - Last: {2} - Total: {3}",
				mName, msg, from_point, from_start));

			SetPoint();
		}


		//********************************
		public void EndTotal()
		{
			TimeSpan from_start = mStart - DateTime.Now;

			mLog.Log(String.Format("[RT: {0}/End] - Total: {1}",
				mName, from_start));
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		private DateTime	mStart;
		private DateTime	mPoint;

		private string		mName;
		private RILog		mLog;


	} // class RTraceTimer
} // namespace Alfray.Faf.Utils


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log$
//---------------------------------------------------------------
