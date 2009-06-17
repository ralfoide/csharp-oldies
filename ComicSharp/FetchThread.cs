//*******************************************************************
/*
 *	Project:	Alfray.ComicsSharp
 * 
 *	File:		FetchThread.cs
 * 
 *	RM (c) 2003
 * 
 */
//*******************************************************************

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections;


//**************************
namespace Alfray.ComicsSharp
{
	//**********************
	public class FetchThread
	{
		// ------- public constants -------


		// ------- public types/enums -----


		//---------------------
		public enum EFetchState
		{
			kIdle,
			kStartedSynchronous,
			kStartedAsynchronous,
			kFinished
		}


		// ------- public delegates --------

		public delegate void UpdateHandler(ComicsBook book, string text, bool completed);


		// ------- public properties ------

		//********************
		public ComicsBook Book
		{
			get
			{
				return this.mBook;
			}
			set
			{
				this.mBook = value;
			}
		}

		//**********************
		public EFetchState State
		{
			get
			{
				return mState;
			}
			set
			{
				mState = value;
			}
		}


		//**********************
		public double Completion
		{
			get
			{
				return mCompletion;
			}
		}


		//******************
		public double KBRate
		{
			get
			{
				if (mDownloadTime > 0)
					return mDownloadBytes / 1024.0 / mDownloadTime;
				else
					return 0;
			}
		}

		// ------- public methods --------

		
		//*********************************
		public FetchThread(ComicsBook book)
		{
			mBook = book;
			mState = EFetchState.kIdle;
			mStopRequested = false;
			mThread = null;
			mCompletion = 0;
			mDownloadBytes = 0;
			mDownloadTime = 0;
			mSegmentList = new ArrayList();
			mUrlBlockList = new ArrayList();
		}


		//**********************************************
		public bool SetUpdateEvent(UpdateHandler method)
		{
			mUpdateEvent = method;

			return true;
		}


		//**********************************************
		/// <summary>
		/// Indicates to this fetcher that it is scheduled for fetch soon
		/// </summary>
		/// <returns>Always true</returns>
		//**********************************************
		public bool Schedule()
		{
			mDownloadBytes = 0;
			mDownloadTime = 0;

			updateUI("Scheduled for download");
			return true;
		}


		//**********************************
		/// <summary>
		/// Start fetching
		/// </summary>
		/// <param name="asynchronous">True if should use a thread for fetching, False if must fetch in caller's thread</param>
		/// <returns>True if asynchronous fetch started. False if fetch terminated (synchronous or error)</returns>
		//**********************************
		public bool Start(bool asynchronous)
		{
			mCompletion = 0;

			// need a valid book
			System.Diagnostics.Debug.Assert(mBook != null);
			if (mBook == null)
				return false;

			// need a valid image url
			if (!parseUrl())
				return false;

			// must not start twice
			System.Diagnostics.Debug.Assert(mThread == null);
			if (mThread != null)
				return false;

			if (asynchronous)
			{
				mThread = new Thread(new ThreadStart(internalFetch));
				mThread.Name = "Fetcher " + mBook.Name;
				mThread.Start();

				return true;	// asynchronous started
			}
			else
			{
				internalFetch();
				return false;	// synchronous fetch terminated
			}
		}


		//****************
		public bool Stop()
		{
			if (mState == EFetchState.kIdle || mState == EFetchState.kFinished)
				return true;

			if (mThread == null)
				return true;

			// lazy-notification for thread to stop when possible
			mStopRequested = true;
			updateUI("Aborting...");

			// Important: do not try to Join here!
			// If the subthread is updating the main window, waiting for it
			// seems to cause a deadlock (fair enough)

			return (mThread == null);
		}


		// ------- private methods ------- 


		//**************************
		private void internalFetch()
		{
			try
			{
				mState = (mThread == null ? EFetchState.kStartedSynchronous : EFetchState.kStartedAsynchronous);
				mFetchSuccess = false;
				mStopRequested = false;

				// get max index... TBDL
				if (mBook.UsesDate)
					mBook.MaxIndex = DateTime.Now.Ticks;

				mFetchSuccess = generateUrls(mBook.LastIndex, mBook.MaxIndex);
				if (mFetchSuccess)
					mFetchSuccess = fetchUrls();

				mState = EFetchState.kFinished;

				if (mStopRequested)
					updateUI("Aborted", true);
				else
					updateUI("Finished", true);

				// If the thread is aborted (using Thread.Abort), this
				// variable will not be nullified, which is then done in Stop().
				if (mThread != null)
					mThread = null;
			}
			catch(ThreadAbortException ex)
			{
				System.Diagnostics.Debug.Write(ex.Message);
			}
			finally
			{
				// done fetching

				mState = EFetchState.kFinished;
				mCompletion = 1;
			}
		}


		//*********************
		private bool parseUrl()
		{
			string url = mBook.ImageUrl;

			// url cannot be empty
			if (url == null || url == "")
				return false;

			// clear segment list
			mSegmentList.Clear();

			// parse url...
			while(url != "")
			{
				SSegment seg = new SSegment(ESegmentType.kSegmentString);

				int pos = url.IndexOf('{');
				if (pos < 0)
				{
					// url is one big string segment
					seg.mParam = url;
					url = "";
				}
				else if (pos > 0)
				{
					// get string segment
					seg.mParam = url.Substring(0, pos);
					url = url.Remove(0, pos);
				}
				else if (pos == 0)
				{
					// url starts by a new segment... which one?
					// remove { separator
					url = url.Remove(0, 1);

					// get the full segment... i.e. till next } separator
					int end = url.IndexOf('}');
					System.Diagnostics.Trace.Assert(end >= 0, "Non-closed URL segment: missing } character!");
					System.Diagnostics.Trace.Assert(end != 0, "Invalid empty segment: missing id|seg between { and }!");
					if (end < 1)
						return false;

					// get segment
					string seg_str = url.Substring(0, end);
					// remove from url
					url = url.Remove(0, end+1);

					// segment has format "<code> [ : <format> ]?"
					int sep = seg_str.IndexOf(':');

					// System.Diagnostics.Trace.Assert(sep != 0, "Invalid segment format: '<code> [ : <format> ]?'");

					string code   = (sep >= 0 ? seg_str.Substring(0, sep) : seg_str);
					string format = (sep <  0 ? ""                        : seg_str.Substring(sep+1));

					// check code
					if (code == "id")
					{
						seg.mType = ESegmentType.kSegmentId;
						System.Diagnostics.Trace.Assert(mBook.UsesId, "Id segment found but book is not id-based!");
					}
					else if (code == "date")
					{
						seg.mType = ESegmentType.kSegmentDate;
						System.Diagnostics.Trace.Assert(mBook.UsesDate, "Date segment found but book is not date-based!");
					}
					else if (code == "ext")
					{
						seg.mType = ESegmentType.kSegmentExt;
						// if none given, set the default
						if (format == "")
							format = "gif,jpg,png";
					}

					seg.mParam = format;
				}

				// add segment to list
				mSegmentList.Add(seg);

			} // while url not empty

			return true;
		}


		//*****************************************************
		private bool generateUrls(Int64 min_val, Int64 max_val)
		{
			bool uses_date = mBook.UsesDate;
			DateTime dt_min;

			System.Diagnostics.Debug.Assert(max_val >= min_val);

			mUrlBlockList.Clear();

			if (uses_date)
				dt_min = new DateTime(min_val);
			else
				dt_min = DateTime.Now; // useless but otherwise C# compiler complains dt_min is not always assigned below -- RM 20030801

			updateUI("Generating URLs...");

			// for all values or dates...
			while (min_val <= max_val)
			{
				// process stop requests
				if (mStopRequested)
					break;

				// process min_val or dt_min
				ArrayList url_list = new ArrayList();	// array of StringBuilder

				// process the segment list and rebuild the string
				foreach(SSegment seg in mSegmentList)
				{
					// process stop requests
					if (mStopRequested)
						break;

					// by design the parameter cannot be null
					System.Diagnostics.Debug.Assert(seg.mParam != null);

					// segment reconstructed
					string seg_str = null;

					switch(seg.mType)
					{
						case ESegmentType.kSegmentString:

							seg_str = seg.mParam;
							
							break;


						case ESegmentType.kSegmentId:

							System.Diagnostics.Debug.Assert(!uses_date);

							// example of valid numeric format: 0000 (300 => 0300)

							if (seg.mParam != "")
								seg_str = min_val.ToString(seg.mParam);
							else
								seg_str += min_val.ToString();
							break;


						case ESegmentType.kSegmentDate:

							System.Diagnostics.Debug.Assert(uses_date);

							// example of valid date format: yyyyMMdd => 20030801

							if (seg.mParam != "")
								seg_str = dt_min.ToString(seg.mParam);
							else
								seg_str = dt_min.ToString("yyyyMMdd");
							break;


						case ESegmentType.kSegmentExt:

							// by design the parameter cannot be empty
							System.Diagnostics.Debug.Assert(seg.mParam != "");

							string [] ext_list = seg.mParam.Split(',', ' ', '|');

							if (url_list.Count == 0)
							{
								foreach(string ext in ext_list)
									url_list.Add(new StringBuilder(ext));
							}
							else
							{
								ArrayList tmp2 = new ArrayList();

								foreach(StringBuilder sb in url_list)
								{
									string s = sb.ToString();
									foreach(string ext in ext_list)
										tmp2.Add(new StringBuilder(s + ext));
								}

								url_list = tmp2;
							}

							break;
					} // end switch

					if (seg_str != null)
					{
						if (url_list.Count == 0)
							url_list.Add(new StringBuilder(seg_str));
						else
							for(int i = url_list.Count-1; i>=0; i--)
								((StringBuilder) url_list[i]).Append(seg_str);
					}
				} // end foreach segment

				// Create a new URL block
				SUrlBlock block = new SUrlBlock(min_val);

				// Append the new URL(s) to the block
				foreach(StringBuilder str in url_list)
					block.mUrlList.Add(str.ToString());

				// add the block to the global fetch list
				mUrlBlockList.Add(block);

				// advance to next value or date
				if (uses_date)
				{
					dt_min = dt_min.AddDays(1);
					min_val = dt_min.Ticks;
				}
				else
				{
					min_val++;
				}
			} // while min<=max

			return !mStopRequested;
		}


		//**********************
		private bool fetchUrls()
		{
			int nb = mUrlBlockList.Count;

			if (nb <= 0)
				return false;

			double p1 = 1.0 / (double)nb;
			double i1 = 0;

			// keep track of all downloaded blocks...
			// i.e. bump the "last" index each time one block could be
			// fetched successfully with all the predecessors.
			bool got_all_blocks = true;

			// allow missing indexes (generally true for ids and dates)
			bool allow_missing = mBook.AllowMissingIndex;



			// for every block...
			for(int ib = 0; ib<nb; ib++, i1 += p1)
			{
				// process stop requests
				if (mStopRequested)
					break;

				SUrlBlock block = (SUrlBlock) mUrlBlockList[ib];

				// set progress bar
				mCompletion = i1;
				updateUI(null);

				// block shound not have already been processed here...
				System.Diagnostics.Debug.Assert(block.mState == EBlockState.kUnprocessed);
				if (block.mState != EBlockState.kUnprocessed)
					continue;

				// can block be empty?
				System.Diagnostics.Debug.Assert(block.mUrlList.Count != 0);
				if (block.mUrlList.Count == 0)
					continue;

				double p2 = p1 / (double)(block.mUrlList.Count+1);

				// for each URL...
				foreach(string url in block.mUrlList)
				{
					// process stop requests
					if (mStopRequested)
						break;

					if (fetchUrl(url, block.mIndex, mBook.UsesDate))
					{
						// need only one url per block...
						block.mState = EBlockState.kFetchSome;
						break;
					}
					else
					{
						// block processed... even if couldn't actually download
						block.mState = EBlockState.kFetchNone;
					}

					// update progress bar
					mCompletion += p2;
					updateUI(null);
				}

				if (allow_missing)
				{
					// blocks can be total failures but must have been processed
					// (by design)
					if (got_all_blocks && block.mState == EBlockState.kUnprocessed)
						got_all_blocks = false;
				}
				else
				{
					// is the block sequence fully downloaded?
					if (got_all_blocks && block.mState != EBlockState.kFetchSome)
						got_all_blocks = false;
				}

				// if got something and still on track, bump last index
				if (got_all_blocks && block.mState == EBlockState.kFetchSome)
					mBook.LastIndex = block.mIndex;

			} // for block

			return !mStopRequested;
		}


		//************************************************************
		private bool fetchUrl(string url, Int64 index, bool uses_date)
		{
			if (url == null || url == "")
				return false;

			// get destination file and path

			string display_name;
			if (uses_date)
			{
				DateTime dt = new DateTime(index);
				display_name = dt.ToString("yyyyMMdd");
			}
			else
			{
				display_name = index.ToString("0000");
			}

			string dest_name = Path.Combine(mBook.BaseDir, display_name);

			// if file already exists, don't download twice
			if (File.Exists(dest_name))
			{
				updateUI(display_name + ": Already in cache");
				return true;
			}

			try
			{
				// download file using the WebClient
				try
				{
					// process stop requests
					if (mStopRequested)
						return false;

					updateUI(display_name + ": Downloading...");

					WebClient web = new WebClient();

#if false
					web.DownloadFile(url, dest_name);
#else
					// download using a stream, KB per KB (for feedback
					// and ability to abort
					Stream st = web.OpenRead(url);

					if (st == null || !st.CanRead)
						return false;

					FileStream fs = new FileStream(
						dest_name,
						FileMode.Create,
						FileAccess.Write,
						FileShare.Read);

					System.Diagnostics.Debug.Assert(fs != null && fs.CanWrite);
					if (fs == null || !fs.CanWrite)
						return false;

					byte [] buffer = new byte[1024];
					int nb = 0;

					Int64 start_tick = DateTime.Now.Ticks;
					double start_dt = mDownloadTime;

					// read until requested to stop
					while (!mStopRequested)
					{
						nb = st.Read(buffer, 0, buffer.Length);

						// end of file reached
						if (nb == 0)
							break;

						// write the data to the binary file
						fs.Write(buffer, 0, nb);

						// update rate and UI
						mDownloadBytes += nb;
						mDownloadTime = start_dt + (DateTime.Now.Ticks - start_tick) / TimeSpan.TicksPerSecond;

						if (!mStopRequested)
							updateUI(null);
					}

					// close streams
					fs.Close();
					st.Close();

					// if stopped writing because a stop was requested
					// but read was not finished, remove the partial file
					if (mStopRequested && nb > 0 && File.Exists(dest_name))
						File.Delete(dest_name);
#endif

					// process stop requests
					if (mStopRequested)
						return false;
				}
				catch(WebException ex)
				{
					// if a file was downloaded, remove it (probably unfinished)
					if (File.Exists(dest_name))
						File.Delete(dest_name);

					updateUI(display_name + ": " + ex.Message);

					System.Diagnostics.Debug.Write(ex.Message);
					return false;
				}
			}
			catch(ThreadAbortException ex)
			{
				// if a file was downloaded, remove it (probably unfinished)
				if (File.Exists(dest_name))
					File.Delete(dest_name);

				System.Diagnostics.Debug.Write(ex.Message);

				return false;
			}

			updateUI(display_name + ": Success");

			return true;
		}


		//********************************
		private void updateUI(string text)
		{
			updateUI(text, false);
		}

		//************************************************
		private void updateUI(string text, bool completed)
		{
			if (mUpdateEvent != null)
			{
#if false
				// non-thread-safe event call
				// mUpdateEvent(mBook, text, completed);
#else
				// *asynhrnous* thread-safe event call
				// (will invoke the method from the main form in the context
				// of the main thread)
				object[] args = { mBook, text, completed };
				Startup.MainForm.BeginInvoke(mUpdateEvent, args);
#endif
			}
		}

		// ------- private properties ------- 

		private EFetchState	mState;
		private bool		mFetchSuccess;
		private bool		mStopRequested;
		private double		mCompletion;	// 0..1
		private double		mDownloadBytes;
		private double		mDownloadTime;
		private ComicsBook	mBook;
		private ArrayList	mSegmentList;	// array of SSegment
		private ArrayList	mUrlBlockList;	// array of SUrlBlock

		private Thread		mThread;

		private event UpdateHandler mUpdateEvent;


		//-----------------------
		private enum ESegmentType
		{
			kSegmentString,
			kSegmentId,
			kSegmentDate,
			kSegmentExt
		}
		
		//---------------------
		private struct SSegment
		{
			public ESegmentType mType;
			public string		 mParam;

			public SSegment(ESegmentType type)
			{
				mType  = type;
				mParam = "";
			}
		}

		//----------------------
		private enum EBlockState
		{
			kUnprocessed,
			kFetchSome,
			kFetchNone
		}

		//----------------------
		private struct SUrlBlock
		{
			public Int64		mIndex;
			public ArrayList	mUrlList;
			public EBlockState	mState;

			public SUrlBlock(Int64 index)
			{
				mIndex = index;
				mUrlList = new ArrayList();
				mState = EBlockState.kUnprocessed;
			}

			public SUrlBlock(Int64 index, string url)
			{
				mIndex = index;
				mUrlList = new ArrayList();
				mUrlList.Add(url);
				mState = EBlockState.kUnprocessed;
			}

			public SUrlBlock(Int64 index, ArrayList url_list)
			{
				mIndex = index;
				mUrlList = new ArrayList();
				mUrlList.AddRange(url_list);
				mState = EBlockState.kUnprocessed;
			}
		}
	
	} // class FetchThread
} // namespace Alfray.ComicsSharp

//*******************************************************************
/*
 *	$Log: FetchThread.cs,v $
 *	Revision 1.2  2003/08/04 21:37:27  ralf
 *	Upload with abort/feedback, active book, etc.
 *	
 *	Revision 1.1  2003/08/01 00:51:58  ralf
 *	Added ComicsBook, ComicsBookCollection, Edit form and main form.
 *	
 * 
 */
//*******************************************************************

