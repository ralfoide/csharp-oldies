//*******************************************************************
/* 

 		Project:	SneLib
 		File:		RSneListener.cs

*/ 
//*******************************************************************

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//*********************
namespace Alfray.SneLib
{
	//***************************************
	/// <summary>
	/// Creates an SNE server that waits for incoming connections.
	/// </summary>
	//***************************************
	public class RSneListener
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//************************
		/// <summary>
		/// Indicate if incoming connections will be handled asynchronously
		/// (i.e. each in a different thread) or directly.
		/// The default is "true" when the lib is in Release mode and "false"
		/// when in debug mode. In this later case, only one connection can be
		/// handled at a time and some other features (such as asynchronous
		/// host notification) may be disabled too.
		/// </summary>
		//************************
		public bool IsAsynchronous
		{
			get
			{
				return mIsAsynchronous;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//*******************
		public RSneListener()
		{
#if DEBUG
			mIsAsynchronous = false;
#else
			mIsAsynchronous = true;
#endif
		}


		//***********************
		public bool BeginListen()
		{
			return BeginListen(RSneClient.kDefaultPort);
		}


		//*******************************
		public bool BeginListen(int port)
		{
			// if already listening, close existing stuff first
			if (mTcpListener != null)
				EndListen();

			try
			{
				// open new one
				mTcpListener = new TcpListener(IPAddress.Any, port);

				// start it
				mTcpListener.Start();
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message, "RSneListener.BeginListen: ");
				mTcpListener = null;
			}

			return (mTcpListener != null);
		}


		//*********************
		public void EndListen()
		{
			// silently ignore if not listening
			if (mTcpListener != null)
			{
				mTcpListener.Stop();
				mTcpListener = null;
			}
		}


		//-------------------------------------------
		//----------- Protected Methods -------------
		//-------------------------------------------


		//********************************
		protected bool startAcceptThread()
		{
			// don't start it twice
			if (mAcceptThread == null)
			{
				try
				{
					ThreadStart thread_delegate = new ThreadStart(this.acceptThread);
					mAcceptThread = new Thread(thread_delegate);
					mAcceptThread.Name = "SneListener.AcceptThread";
					mAcceptThreadMustQuit = false;

					mAcceptThread.Start();
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message, "RSneListener.startAcceptThread: ");
					mAcceptThread = null;
				}
			}

			return (mAcceptThread != null);
		}


		//*******************************
		protected void stopAcceptThread()
		{
			// nothing to do if not running thread
			if (mAcceptThread != null)
			{
				// tell accept thread to stop
				mAcceptThreadMustQuit = true;

				// join... wait for end for a full second
				if (!mAcceptThread.Join(1000))
				{
					// the thread hasn't stopped after a second, force abort
					mAcceptThread.Abort();
					mAcceptThread.Join();
				}
			
				// thread is gone
				mAcceptThread = null;
			}
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//*************************
		private void acceptThread()
		{
			try
			{
				// loop till mAcceptThreadMustQuit is true
				// (there is no need to synchronize on a value, but all accesses
				//  to other instance members such as mTcpListener should be
				//  synchronized if necessary).

				while(!mAcceptThreadMustQuit && mTcpListener != null)
				{
					if (mTcpListener.Pending())
					{
						TcpClient client = mTcpListener.AcceptTcpClient();
						if (IsAsynchronous)
						{
							// TBDL: handle the connection in a separate thread
							// Requirements: list of threads, close when listener stops, etc.

							client.Close();
						}
					}
					else
					{
						Thread.Sleep(100);	// sleeps a 1/10th of a second
					}
				}
			}
			catch(System.Threading.ThreadAbortException)
			{
			}
			finally
			{
				// cleanup
			}
		}


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private	TcpListener	mTcpListener;
		private	Thread		mAcceptThread;
		private bool		mIsAsynchronous;				// should connections spawn threads
		private	bool		mAcceptThreadMustQuit = false;	// accept loop must quit

	} // class RSneListener
} // namespace Alfray.SneLib


//---------------------------------------------------------------
//
//	$Log: RSneListener.cs,v $
//	Revision 1.1  2004/01/05 06:29:14  ralf
//	Asynchronous handling of listen.
//	Added RSneClient vs RSneListerner and RSneConnection and test classes.
//	
//---------------------------------------------------------------
