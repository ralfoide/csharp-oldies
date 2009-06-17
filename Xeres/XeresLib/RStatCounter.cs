//*******************************************************************
/*

	Solution:	Xeres
	Project:	XeresLib
	File:		RStatCounter.cs

	Copyright 2005, Raphael MOLL.

	This file is part of Xeres.

	Xeres is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Xeres is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Xeres; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

*/
//*******************************************************************



using System;

//*************************************
namespace Alfray.Xeres.XeresLib
{
	//***************************************************
	/// <summary>
	/// RStatCounter provide simple buffer stats for one channel.
	/// Simply set in the number of buffers received or transmitted
	/// and their size and you can get an average kbps over time.
	/// </summary>
	//***************************************************
	public class RStatCounter
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//**************************
		/// <summary>
		/// Number of buffers sent or received over network.
		/// This value should only be incremented.
		/// </summary>
		//**************************
		public int BuffersCount = 0;


		//**************************
		/// <summary>
		/// Number of bytes total sent or received over network.
		/// This value should only be incremented.
		/// </summary>
		//**************************
		public long BuffersBytes
		{
			get
			{
				return mBuffersBytes;
			}
			set
			{
				mBuffersBytes = value;
				mLastTime = DateTime.Now;

				// Remember we'll need to update the average but only
				// do it when it is actually requested
				mNeedUpdate = true;

				if (mStartTime.Ticks == 0)
				{
					// First time, initialize to Now
					mStartTime = mLastTime;
					mStartBytes = mBuffersBytes;

					// DEBUG
					// System.Diagnostics.Debug.WriteLine("Reset tick = " + mStartTime.Ticks.ToString());
				}
			}
		}


		//**************************
		/// <summary>
		/// Returns the average kbps (kilo bit per second) since last reset
		/// </summary>
		//**************************
		public double AverageKbps
		{
			get
			{
				updateAverage();
				return mAverageKbps;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//********************
		/// <summary>
		/// Initialized an empty counter.
		/// Buffer bytes and count are initialized to zero.
		/// Average kpbs is zero at first (i.e. N/A)
		/// </summary>
		//********************
		public RStatCounter()
		{
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//**************************
		private void updateAverage()
		{
			if (mNeedUpdate)
			{
				// Get the time difference
				TimeSpan ts = mLastTime - mStartTime;
				long tts = (long)(ts.TotalSeconds);

				// Check we have valid start/last times
				// i.e. it must be positive non-null
				if (tts > 0)
				{
					long nb_bytes = mBuffersBytes - mStartBytes;
					mAverageKbps = (double)(nb_bytes * 8 / 1024) / (double)(tts);

					// DEBUG
					System.Diagnostics.Debug.WriteLine(String.Format("sec {0} - bytes {1} - avg {2:#.##}",
						tts,
						nb_bytes, mAverageKbps));

					// reset update flag only if average was really computed
					mNeedUpdate = false;
				}
			}
		}


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private bool		mNeedUpdate = true;
		private double		mAverageKbps = 0.0;
		private long		mBuffersBytes = 0;
		private DateTime	mStartTime = new DateTime(0);
		private DateTime	mLastTime  = new DateTime(0);
		private long		mStartBytes = 0;

	} // class RStatCounter
} // namespace Alfray.Xeres.XeresLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RStatCounter.cs,v $
//	Revision 1.4  2005/03/28 00:25:21  ralf
//	Fixed details
//	
//	Revision 1.3  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
//	
//	Revision 1.2  2005/03/20 20:00:45  ralf
//	Updated for NUnit 2.2.
//	
//	Revision 1.1  2005/03/10 22:09:16  ralf
//	Added stat counter with average kbps
//	
//---------------------------------------------------------------
