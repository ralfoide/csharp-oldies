//*******************************************************************
/*

	Solution:	Xeres
	Project:	XeresApp
	File:		RMainForm.cs

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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Alfray.LibUtils.Misc;
using Alfray.Xeres.XeresLib;
using Alfray.Xeres.Device;

//*********************************
namespace Alfray.Xeres.XeresApp
{
	//**************************************
	/// <summary>
	/// Summary description for RMainForm.
	/// </summary>
	public class RMainForm : System.Windows.Forms.Form, RILog
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Events & Delegates -----
		//-------------------------------------------



		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------

		//*********************************
		public RDeviceFactory DeviceFactory
		{
			get
			{
				return mDevFactory;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//****************
		public RMainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// Add any constructor code after InitializeComponent call
			//

			init();
		}

		#region RILog Members

		//***********************
		public void Log(object o)
		{
			Log(o.ToString());
		}

		//***********************
		public void Log(string s)
		{
			if (mDebugForm == null)
				createDebugWindow(false);
			if (mDebugForm != null)
				mDebugForm.Log(s);
		}

		#endregion


		//***********************
		public void ReloadPrefs()
		{
			reloadPrefs();
		}



		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//*****************
		private void init()
		{
			// setup factory
			mDevFactory.SetLogger(this);
			mDevFactory.LocalVideo.CurrentDevice = RPlayerVideo.NewCallbackDevice(new RPlayerVideo.UpdateVideoCallback(updateLocalVideo));

			// load all settings
			loadSettings();

			// apply defaults
			reloadPrefs();
		}


		//**********************
		private void terminate()
		{
			// stop audio, video and comm
			if (mLocalState.AudioIsOn)
				mButtonStartStopAudio_Click(null, null);
			if (mLocalState.VideoIsOn)
				mButtonStartStopVideo_Click(null, null);
			if (mLocalState.CommIsOn)
				mButtonStartStopComm_Click(null, null);

			// close windows
			closePrefWindow();
			closeDebugWindow();

			// save settings
			saveSettings();
		}


		//*************************
		/// <summary>
		/// Loads settings specific to this window.
		/// Done only once when the window is created.
		/// </summary>
		//*************************
		private void loadSettings()
		{
			// load settings
			RMainModule.Pref.Load();

			// tell Windows not to change this position automatically
			this.StartPosition = FormStartPosition.Manual;

			// load position of this window
			Rectangle r;
			if (RMainModule.Pref.GetRect(RPrefConstants.kMainForm, out r))
			{
				// RM 20050307 No longer change the size of the window.
				// This is because the window cannot be resized manually,
				// instead it adapts to the size of the inner video preview.
				this.Location = r.Location;
			}

			// restore size of image previews

			Size sz;
			if (RMainModule.Pref.GetSize(RPrefConstants.kLocalPreviewSize, out sz))
				resizeLocalVideo(sz);

		}


		//*************************
		private void saveSettings()
		{
			// save position & size of this window
			RMainModule.Pref.SetRect(RPrefConstants.kMainForm, this.Bounds);

			// save settings
			RMainModule.Pref.Save();
		}


		//************************
		/// <summary>
		/// (Re)Loads app-wide preferences.
		/// Done anytime the user applies changes to the preference window
		/// or once at startup.
		/// </summary>
		//************************
		private void reloadPrefs()
		{
			// set video recorder device

			string uniq_id = RMainModule.Pref[RPrefConstants.kVideoRecorderId];

			// Update video pref.
			// [RM 20050324]
			// If local comm is playing video, the recorder must update
			// live. 
			// Only change the recorder if the selected device actually
			// exist in the recorder -- should not happen when the user makes
			// a selection in the pref panel but it can happen when this is
			// called from the init to restore initial prefs.
			
			RDeviceInfo curr = mDevFactory.RecorderVideo.CurrentDevice;

			if (curr == null || curr.UniqueId != uniq_id)
			{
				foreach(RDeviceInfo info in mDevFactory.RecorderVideo.EnumerateDevices())
				{
					if (info.UniqueId == uniq_id)
					{
						mDevFactory.RecorderVideo.CurrentDevice = info;
						break;
					}
				}
			}

			updateButtons();

			Log("Prefs reloaded");
		}


		//******************************************
		private void createDebugWindow(bool visible)
		{
			if (mDebugForm == null)
			{
				mDebugForm = new RDebugForm();
				mDebugForm.Show();
			}
		}


		//******************************
		private void closeDebugWindow()
		{
			if (mDebugForm != null)
			{
				mDebugForm.CanClose = true;
				mDebugForm.Close();
				mDebugForm = null;
			}
		}


		//********************************
		private void showHideDebugWindow()
		{
			if (mDebugForm == null)
				createDebugWindow(true);
			else
				mDebugForm.Visible = !mDebugForm.Visible;
		}


		//******************************************
		private void createPrefWindow(bool visible)
		{
			if (mPrefForm == null)
			{
				mPrefForm = new RPrefForm();
				mPrefForm.Show();
			}
		}


		//******************************
		private void closePrefWindow()
		{
			if (mPrefForm != null)
			{
				mPrefForm.CanClose = true;
				mPrefForm.Close();
				mPrefForm = null;
			}
		}


		//********************************
		private void showHidePrefWindow()
		{
			if (mPrefForm == null)
				createPrefWindow(true);
			else
				mPrefForm.Visible = !mPrefForm.Visible;
		}


		//**************************
		private void updateButtons()
		{
			if (mLocalState.CommIsOn)
			{
				mButtonStartStopComm.BackColor = Color.LightGreen;

				// [RM 20050324] Video is only enabled if a video device is selected
				mButtonStartStopVideo.Enabled = (mDevFactory.RecorderVideo.CurrentDevice != null);
				// [RM 20050324] Audio is only enabled if an audio device is selected
				mButtonStartStopAudio.Enabled = false /* (mDevFactory.RecorderAudio.CurrentDevice != null) */ ;

				mButtonStartStopVideo.BackColor = mLocalState.VideoIsOn ? Color.LightGreen : SystemColors.Control;
				mButtonStartStopAudio.BackColor = mLocalState.AudioIsOn ? Color.LightGreen : SystemColors.Control;				
			}
			else
			{
				mButtonStartStopComm.BackColor = SystemColors.Control;
				mButtonStartStopVideo.Enabled = false;
				mButtonStartStopAudio.Enabled = false;
				mButtonStartStopVideo.BackColor = SystemColors.Control;
				mButtonStartStopAudio.BackColor = SystemColors.Control;
			}
		}

		//**************************
		private void startStopComm()
		{
			if (mLocalState.CommIsOn)
				Log("Comm: Started");
			else
				Log("Comm: Stopped");

			mTimerStat.Enabled = mLocalState.VideoIsOn || mLocalState.AudioIsOn || mLocalState.CommIsOn;
		}

		//***************************
		private void startStopVideo()
		{
			if (mLocalState.VideoIsOn)
			{
				mDevFactory.LocalVideo.Start();
				mDevFactory.RecorderVideo.Start();
				Log("Video: Started");
			}
			else
			{
				mDevFactory.RecorderVideo.Stop();
				mDevFactory.LocalVideo.Stop();
				Log("Video: Stopped");
			}

			// DISABLED ** mTimerVideo.Enabled = mLocalState.VideoIsOn;
			mTimerStat.Enabled = mLocalState.VideoIsOn || mLocalState.AudioIsOn || mLocalState.CommIsOn;
		}

		//***************************
		private void startStopAudio()
		{
			if (mLocalState.AudioIsOn)
				Log("Audio: Started");
			else
				Log("Audio: Stopped");

			mTimerStat.Enabled = mLocalState.VideoIsOn || mLocalState.AudioIsOn || mLocalState.CommIsOn;
		}


		//*********************************************
		private void resizeLocalVideo(Size target_size)
		{
			if (target_size == Size.Empty)
				return;

			this.SuspendLayout();

			// delta between current size and new size

			int w = target_size.Width - mPixLocalCompressed.Width;
			int h = target_size.Height - mPixLocalCompressed.Height;

			// resize & move groups

			Size sz = mGroupLocal.Size;
			sz.Width += w;
			sz.Height += h;
			mGroupLocal.Size = sz;

			Rectangle r = this.Bounds;
			r.Width += w;
			if (sz.Height > mGroupRemote.Size.Height)
				r.Height += h;
			this.MinimumSize = r.Size;
			this.MaximumSize = r.Size;
			this.Bounds = r;

			this.ResumeLayout();

			// memorize new size in prefs

			RMainModule.Pref.SetSize(RPrefConstants.kLocalPreviewSize, target_size);
		}


		//****************************************
		/// <summary>
		/// Callback receiving images from RDevFactory.LocalVideo (RPlayerVideo)
		/// </summary>
		//****************************************
		private void updateLocalVideo(Image image)
		{
			Size sz = Size.Empty;
			if (mPixLocalCompressed.Image != null)
				sz = mPixLocalCompressed.Image.Size;

			// Update image
			mPixLocalCompressed.Image = image;

			// Update stats
			mLocalVideoFreq.Count++;

			// estime size of the video as RGB 24bpp data
			// long nb_bytes = b.Size.Width * b.Size.Height*3;
			// mVideoSentStat.BuffersBytes += nb_bytes;

			if (   mPixLocalCompressed.Image != null 
				&& sz != mPixLocalCompressed.Image.Size)
				resizeLocalVideo(mPixLocalCompressed.Image.Size);
		}


		//-------------------------------------------
		//----------- Private Callbacks -------------
		//-------------------------------------------


		//******************************************************************
		private void mMenuItemQuit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		//******************************************************************
		private void mMenuItemDebug_Click(object sender, System.EventArgs e)
		{
			showHideDebugWindow();
			mMenuItemDebug.Checked = mDebugForm.Visible;
		}

		//******************************************************************
		private void mMenuItemPrefs_Click(object sender, System.EventArgs e)
		{
			showHidePrefWindow();
			mMenuItemPrefs.Checked = mPrefForm.Visible;
		}

		//******************************************************************
		private void RMainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			terminate();
		}

		
		//******************************************************************
		private void mButtonStartStopComm_Click(object sender, System.EventArgs e)
		{
			mLocalState.CommIsOn = !mLocalState.CommIsOn;

			updateButtons();

			// start comm before audio/video
			if (mLocalState.CommIsOn)
				startStopComm();

			// put audio/video in same state as comm
			if (mLocalState.VideoIsOn != mLocalState.CommIsOn)
				mButtonStartStopVideo_Click(sender, e);
			if (mLocalState.AudioIsOn != mLocalState.CommIsOn)
				mButtonStartStopAudio_Click(sender, e);

			// stop comm after audio/video
			if (!mLocalState.CommIsOn)
				startStopComm();
		}

		//******************************************************************
		private void mButtonStartStopVideo_Click(object sender, System.EventArgs e)
		{
			// [RM 20050324] Video can only be turned on if a video device is selected
			if (!mLocalState.VideoIsOn && mDevFactory.RecorderVideo.CurrentDevice == null)
				return;

			mLocalState.VideoIsOn = !mLocalState.VideoIsOn;
			updateButtons();
			startStopVideo();
		}

		//******************************************************************
		private void mButtonStartStopAudio_Click(object sender, System.EventArgs e)
		{
			// [RM 20050324] Audio can only be turned on if an audio device is selected
			if (!mLocalState.AudioIsOn /* && mDevFactory.RecorderAudio.CurrentDevice == null */)
				return;
			

			mLocalState.AudioIsOn = !mLocalState.AudioIsOn;
			updateButtons();
			startStopAudio();
		}

		
		//******************************************************************
		private void mButtonMsgSend_Click(object sender, System.EventArgs e)
		{
		
		}	


		//******************************************************************
		private void mTimerStat_Tick(object sender, System.EventArgs e)
		{
			// update stat display

			string str = String.Format("{0:#.#} - {1:#.#} - {2:#.#}",
				mDevFactory.RecorderVideo.StatFps,
				mLocalVideoFreq.AvgPerSec,
				mVideoSentStat.AverageKbps);

			mStatTextVideoSent.Text = str;
		}

		

		//******************************************************************
		private void mTimerVideo_Tick(object sender, System.EventArgs e)
		{
			Queue q = mDevFactory.RecorderVideo.BufferQueue;

			Size sz = Size.Empty;
			if (mPixLocalCompressed.Image != null)
				sz = mPixLocalCompressed.Image.Size;

			RBufferVideo rb = null;
			Exception ex = null;

			// get the buffer out of the queue...
			// do only the minimun in the locked section and process data only
			// after being out of the locked section
			lock(q.SyncRoot)
			{
				if (q.Count > 0)
				{
					try
					{
						rb = q.Dequeue() as RBufferVideo;
					}
					catch(Exception ex1)
					{
						// Don't log in the locked section
						ex = ex1;
					}
				}
			}		

			if (ex != null)
				Log(ex.Message);

			// if we got a buffer and it contains a bitmap, use it
			if (rb != null 
				// && rb.MimeType == ".net/bitmap")
				&& rb.MimeType == "image/jpeg")
			{
				// Bitmap b = rb.Metadata[RBuffer.Key.kBitmap] as Bitmap;

				Image b = RBufferVideo.ImageFromJpeg(rb);
				long nb_bytes = rb.Data.Length;

				mPixLocalCompressed.Image = b;

				// Hack for stats
				mVideoSentStat.BuffersCount++;

				// estime size of the video as RGB 24bpp data
				// long nb_bytes = b.Size.Width * b.Size.Height*3;

				mVideoSentStat.BuffersBytes += nb_bytes;
			}

			if (   mPixLocalCompressed.Image != null 
				&& sz != mPixLocalCompressed.Image.Size)
				resizeLocalVideo(mPixLocalCompressed.Image.Size);
		}


		//-------------------------------------------
		//----------- Private WinForms --------------
		//-------------------------------------------

		//***********************************
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code

		//********************************
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RMainForm));
			this.mMenuMain = new System.Windows.Forms.MainMenu();
			this.mMenuConnection = new System.Windows.Forms.MenuItem();
			this.mMenuItemConnect = new System.Windows.Forms.MenuItem();
			this.mMenuItemDisconnect = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mMenuItemQuit = new System.Windows.Forms.MenuItem();
			this.mMenuTools = new System.Windows.Forms.MenuItem();
			this.mMenuItemPrefs = new System.Windows.Forms.MenuItem();
			this.mMenuHelp = new System.Windows.Forms.MenuItem();
			this.mMenuItemUpdate = new System.Windows.Forms.MenuItem();
			this.mMenuItemDebug = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.mMenuItemAbout = new System.Windows.Forms.MenuItem();
			this.mGroupLocal = new System.Windows.Forms.GroupBox();
			this.mSoundLevelLocalOriginal = new System.Windows.Forms.ProgressBar();
			this.label3 = new System.Windows.Forms.Label();
			this.mButtonStartStopAudio = new System.Windows.Forms.Button();
			this.mButtonStartStopVideo = new System.Windows.Forms.Button();
			this.mButtonStartStopComm = new System.Windows.Forms.Button();
			this.mPixLocalCompressed = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.mGroupRemote = new System.Windows.Forms.GroupBox();
			this.mSoundLevelRemoteReceived = new System.Windows.Forms.ProgressBar();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.mPixRemoteReceived = new System.Windows.Forms.PictureBox();
			this.mGroupStats = new System.Windows.Forms.GroupBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.mStatTextTotalReceived = new System.Windows.Forms.Label();
			this.mStatTextTotalSent = new System.Windows.Forms.Label();
			this.mStatTextAudioReceived = new System.Windows.Forms.Label();
			this.mStatTextAudioSent = new System.Windows.Forms.Label();
			this.mStatTextVideoReceived = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.mStatTextVideoSent = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.mGroupControl = new System.Windows.Forms.GroupBox();
			this.mControlTextKbps = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.mControlCheckMaxKbps = new System.Windows.Forms.CheckBox();
			this.mTooltips = new System.Windows.Forms.ToolTip(this.components);
			this.mGroupText = new System.Windows.Forms.GroupBox();
			this.mButtonMsgSend = new System.Windows.Forms.Button();
			this.mTextMsgSend = new System.Windows.Forms.TextBox();
			this.mTextMsgInOut = new System.Windows.Forms.TextBox();
			this.mTimerStat = new System.Windows.Forms.Timer(this.components);
			this.mTimerVideo = new System.Windows.Forms.Timer(this.components);
			this.mGroupLocal.SuspendLayout();
			this.mGroupRemote.SuspendLayout();
			this.mGroupStats.SuspendLayout();
			this.mGroupControl.SuspendLayout();
			this.mGroupText.SuspendLayout();
			this.SuspendLayout();
			// 
			// mMenuMain
			// 
			this.mMenuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mMenuConnection,
																					  this.mMenuTools,
																					  this.mMenuHelp});
			this.mMenuMain.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mMenuMain.RightToLeft")));
			// 
			// mMenuConnection
			// 
			this.mMenuConnection.Enabled = ((bool)(resources.GetObject("mMenuConnection.Enabled")));
			this.mMenuConnection.Index = 0;
			this.mMenuConnection.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.mMenuItemConnect,
																							this.mMenuItemDisconnect,
																							this.menuItem4,
																							this.mMenuItemQuit});
			this.mMenuConnection.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuConnection.Shortcut")));
			this.mMenuConnection.ShowShortcut = ((bool)(resources.GetObject("mMenuConnection.ShowShortcut")));
			this.mMenuConnection.Text = resources.GetString("mMenuConnection.Text");
			this.mMenuConnection.Visible = ((bool)(resources.GetObject("mMenuConnection.Visible")));
			// 
			// mMenuItemConnect
			// 
			this.mMenuItemConnect.Enabled = ((bool)(resources.GetObject("mMenuItemConnect.Enabled")));
			this.mMenuItemConnect.Index = 0;
			this.mMenuItemConnect.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemConnect.Shortcut")));
			this.mMenuItemConnect.ShowShortcut = ((bool)(resources.GetObject("mMenuItemConnect.ShowShortcut")));
			this.mMenuItemConnect.Text = resources.GetString("mMenuItemConnect.Text");
			this.mMenuItemConnect.Visible = ((bool)(resources.GetObject("mMenuItemConnect.Visible")));
			// 
			// mMenuItemDisconnect
			// 
			this.mMenuItemDisconnect.Enabled = ((bool)(resources.GetObject("mMenuItemDisconnect.Enabled")));
			this.mMenuItemDisconnect.Index = 1;
			this.mMenuItemDisconnect.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemDisconnect.Shortcut")));
			this.mMenuItemDisconnect.ShowShortcut = ((bool)(resources.GetObject("mMenuItemDisconnect.ShowShortcut")));
			this.mMenuItemDisconnect.Text = resources.GetString("mMenuItemDisconnect.Text");
			this.mMenuItemDisconnect.Visible = ((bool)(resources.GetObject("mMenuItemDisconnect.Visible")));
			// 
			// menuItem4
			// 
			this.menuItem4.Enabled = ((bool)(resources.GetObject("menuItem4.Enabled")));
			this.menuItem4.Index = 2;
			this.menuItem4.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem4.Shortcut")));
			this.menuItem4.ShowShortcut = ((bool)(resources.GetObject("menuItem4.ShowShortcut")));
			this.menuItem4.Text = resources.GetString("menuItem4.Text");
			this.menuItem4.Visible = ((bool)(resources.GetObject("menuItem4.Visible")));
			// 
			// mMenuItemQuit
			// 
			this.mMenuItemQuit.Enabled = ((bool)(resources.GetObject("mMenuItemQuit.Enabled")));
			this.mMenuItemQuit.Index = 3;
			this.mMenuItemQuit.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemQuit.Shortcut")));
			this.mMenuItemQuit.ShowShortcut = ((bool)(resources.GetObject("mMenuItemQuit.ShowShortcut")));
			this.mMenuItemQuit.Text = resources.GetString("mMenuItemQuit.Text");
			this.mMenuItemQuit.Visible = ((bool)(resources.GetObject("mMenuItemQuit.Visible")));
			this.mMenuItemQuit.Click += new System.EventHandler(this.mMenuItemQuit_Click);
			// 
			// mMenuTools
			// 
			this.mMenuTools.Enabled = ((bool)(resources.GetObject("mMenuTools.Enabled")));
			this.mMenuTools.Index = 1;
			this.mMenuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mMenuItemPrefs});
			this.mMenuTools.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuTools.Shortcut")));
			this.mMenuTools.ShowShortcut = ((bool)(resources.GetObject("mMenuTools.ShowShortcut")));
			this.mMenuTools.Text = resources.GetString("mMenuTools.Text");
			this.mMenuTools.Visible = ((bool)(resources.GetObject("mMenuTools.Visible")));
			// 
			// mMenuItemPrefs
			// 
			this.mMenuItemPrefs.Enabled = ((bool)(resources.GetObject("mMenuItemPrefs.Enabled")));
			this.mMenuItemPrefs.Index = 0;
			this.mMenuItemPrefs.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemPrefs.Shortcut")));
			this.mMenuItemPrefs.ShowShortcut = ((bool)(resources.GetObject("mMenuItemPrefs.ShowShortcut")));
			this.mMenuItemPrefs.Text = resources.GetString("mMenuItemPrefs.Text");
			this.mMenuItemPrefs.Visible = ((bool)(resources.GetObject("mMenuItemPrefs.Visible")));
			this.mMenuItemPrefs.Click += new System.EventHandler(this.mMenuItemPrefs_Click);
			// 
			// mMenuHelp
			// 
			this.mMenuHelp.Enabled = ((bool)(resources.GetObject("mMenuHelp.Enabled")));
			this.mMenuHelp.Index = 2;
			this.mMenuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mMenuItemUpdate,
																					  this.mMenuItemDebug,
																					  this.menuItem10,
																					  this.mMenuItemAbout});
			this.mMenuHelp.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuHelp.Shortcut")));
			this.mMenuHelp.ShowShortcut = ((bool)(resources.GetObject("mMenuHelp.ShowShortcut")));
			this.mMenuHelp.Text = resources.GetString("mMenuHelp.Text");
			this.mMenuHelp.Visible = ((bool)(resources.GetObject("mMenuHelp.Visible")));
			// 
			// mMenuItemUpdate
			// 
			this.mMenuItemUpdate.Enabled = ((bool)(resources.GetObject("mMenuItemUpdate.Enabled")));
			this.mMenuItemUpdate.Index = 0;
			this.mMenuItemUpdate.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemUpdate.Shortcut")));
			this.mMenuItemUpdate.ShowShortcut = ((bool)(resources.GetObject("mMenuItemUpdate.ShowShortcut")));
			this.mMenuItemUpdate.Text = resources.GetString("mMenuItemUpdate.Text");
			this.mMenuItemUpdate.Visible = ((bool)(resources.GetObject("mMenuItemUpdate.Visible")));
			// 
			// mMenuItemDebug
			// 
			this.mMenuItemDebug.Enabled = ((bool)(resources.GetObject("mMenuItemDebug.Enabled")));
			this.mMenuItemDebug.Index = 1;
			this.mMenuItemDebug.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemDebug.Shortcut")));
			this.mMenuItemDebug.ShowShortcut = ((bool)(resources.GetObject("mMenuItemDebug.ShowShortcut")));
			this.mMenuItemDebug.Text = resources.GetString("mMenuItemDebug.Text");
			this.mMenuItemDebug.Visible = ((bool)(resources.GetObject("mMenuItemDebug.Visible")));
			this.mMenuItemDebug.Click += new System.EventHandler(this.mMenuItemDebug_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Enabled = ((bool)(resources.GetObject("menuItem10.Enabled")));
			this.menuItem10.Index = 2;
			this.menuItem10.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("menuItem10.Shortcut")));
			this.menuItem10.ShowShortcut = ((bool)(resources.GetObject("menuItem10.ShowShortcut")));
			this.menuItem10.Text = resources.GetString("menuItem10.Text");
			this.menuItem10.Visible = ((bool)(resources.GetObject("menuItem10.Visible")));
			// 
			// mMenuItemAbout
			// 
			this.mMenuItemAbout.Enabled = ((bool)(resources.GetObject("mMenuItemAbout.Enabled")));
			this.mMenuItemAbout.Index = 3;
			this.mMenuItemAbout.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mMenuItemAbout.Shortcut")));
			this.mMenuItemAbout.ShowShortcut = ((bool)(resources.GetObject("mMenuItemAbout.ShowShortcut")));
			this.mMenuItemAbout.Text = resources.GetString("mMenuItemAbout.Text");
			this.mMenuItemAbout.Visible = ((bool)(resources.GetObject("mMenuItemAbout.Visible")));
			// 
			// mGroupLocal
			// 
			this.mGroupLocal.AccessibleDescription = resources.GetString("mGroupLocal.AccessibleDescription");
			this.mGroupLocal.AccessibleName = resources.GetString("mGroupLocal.AccessibleName");
			this.mGroupLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mGroupLocal.Anchor")));
			this.mGroupLocal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mGroupLocal.BackgroundImage")));
			this.mGroupLocal.Controls.Add(this.mSoundLevelLocalOriginal);
			this.mGroupLocal.Controls.Add(this.label3);
			this.mGroupLocal.Controls.Add(this.mButtonStartStopAudio);
			this.mGroupLocal.Controls.Add(this.mButtonStartStopVideo);
			this.mGroupLocal.Controls.Add(this.mButtonStartStopComm);
			this.mGroupLocal.Controls.Add(this.mPixLocalCompressed);
			this.mGroupLocal.Controls.Add(this.label1);
			this.mGroupLocal.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mGroupLocal.Dock")));
			this.mGroupLocal.Enabled = ((bool)(resources.GetObject("mGroupLocal.Enabled")));
			this.mGroupLocal.Font = ((System.Drawing.Font)(resources.GetObject("mGroupLocal.Font")));
			this.mGroupLocal.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mGroupLocal.ImeMode")));
			this.mGroupLocal.Location = ((System.Drawing.Point)(resources.GetObject("mGroupLocal.Location")));
			this.mGroupLocal.Name = "mGroupLocal";
			this.mGroupLocal.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mGroupLocal.RightToLeft")));
			this.mGroupLocal.Size = ((System.Drawing.Size)(resources.GetObject("mGroupLocal.Size")));
			this.mGroupLocal.TabIndex = ((int)(resources.GetObject("mGroupLocal.TabIndex")));
			this.mGroupLocal.TabStop = false;
			this.mGroupLocal.Text = resources.GetString("mGroupLocal.Text");
			this.mTooltips.SetToolTip(this.mGroupLocal, resources.GetString("mGroupLocal.ToolTip"));
			this.mGroupLocal.Visible = ((bool)(resources.GetObject("mGroupLocal.Visible")));
			// 
			// mSoundLevelLocalOriginal
			// 
			this.mSoundLevelLocalOriginal.AccessibleDescription = resources.GetString("mSoundLevelLocalOriginal.AccessibleDescription");
			this.mSoundLevelLocalOriginal.AccessibleName = resources.GetString("mSoundLevelLocalOriginal.AccessibleName");
			this.mSoundLevelLocalOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mSoundLevelLocalOriginal.Anchor")));
			this.mSoundLevelLocalOriginal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mSoundLevelLocalOriginal.BackgroundImage")));
			this.mSoundLevelLocalOriginal.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mSoundLevelLocalOriginal.Dock")));
			this.mSoundLevelLocalOriginal.Enabled = ((bool)(resources.GetObject("mSoundLevelLocalOriginal.Enabled")));
			this.mSoundLevelLocalOriginal.Font = ((System.Drawing.Font)(resources.GetObject("mSoundLevelLocalOriginal.Font")));
			this.mSoundLevelLocalOriginal.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mSoundLevelLocalOriginal.ImeMode")));
			this.mSoundLevelLocalOriginal.Location = ((System.Drawing.Point)(resources.GetObject("mSoundLevelLocalOriginal.Location")));
			this.mSoundLevelLocalOriginal.Name = "mSoundLevelLocalOriginal";
			this.mSoundLevelLocalOriginal.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mSoundLevelLocalOriginal.RightToLeft")));
			this.mSoundLevelLocalOriginal.Size = ((System.Drawing.Size)(resources.GetObject("mSoundLevelLocalOriginal.Size")));
			this.mSoundLevelLocalOriginal.TabIndex = ((int)(resources.GetObject("mSoundLevelLocalOriginal.TabIndex")));
			this.mSoundLevelLocalOriginal.Text = resources.GetString("mSoundLevelLocalOriginal.Text");
			this.mTooltips.SetToolTip(this.mSoundLevelLocalOriginal, resources.GetString("mSoundLevelLocalOriginal.ToolTip"));
			this.mSoundLevelLocalOriginal.Visible = ((bool)(resources.GetObject("mSoundLevelLocalOriginal.Visible")));
			// 
			// label3
			// 
			this.label3.AccessibleDescription = resources.GetString("label3.AccessibleDescription");
			this.label3.AccessibleName = resources.GetString("label3.AccessibleName");
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label3.Anchor")));
			this.label3.AutoSize = ((bool)(resources.GetObject("label3.AutoSize")));
			this.label3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label3.Dock")));
			this.label3.Enabled = ((bool)(resources.GetObject("label3.Enabled")));
			this.label3.Font = ((System.Drawing.Font)(resources.GetObject("label3.Font")));
			this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
			this.label3.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.ImageAlign")));
			this.label3.ImageIndex = ((int)(resources.GetObject("label3.ImageIndex")));
			this.label3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label3.ImeMode")));
			this.label3.Location = ((System.Drawing.Point)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label3.RightToLeft")));
			this.label3.Size = ((System.Drawing.Size)(resources.GetObject("label3.Size")));
			this.label3.TabIndex = ((int)(resources.GetObject("label3.TabIndex")));
			this.label3.Text = resources.GetString("label3.Text");
			this.label3.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.TextAlign")));
			this.mTooltips.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
			this.label3.Visible = ((bool)(resources.GetObject("label3.Visible")));
			// 
			// mButtonStartStopAudio
			// 
			this.mButtonStartStopAudio.AccessibleDescription = resources.GetString("mButtonStartStopAudio.AccessibleDescription");
			this.mButtonStartStopAudio.AccessibleName = resources.GetString("mButtonStartStopAudio.AccessibleName");
			this.mButtonStartStopAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonStartStopAudio.Anchor")));
			this.mButtonStartStopAudio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonStartStopAudio.BackgroundImage")));
			this.mButtonStartStopAudio.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonStartStopAudio.Dock")));
			this.mButtonStartStopAudio.Enabled = ((bool)(resources.GetObject("mButtonStartStopAudio.Enabled")));
			this.mButtonStartStopAudio.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonStartStopAudio.FlatStyle")));
			this.mButtonStartStopAudio.Font = ((System.Drawing.Font)(resources.GetObject("mButtonStartStopAudio.Font")));
			this.mButtonStartStopAudio.Image = ((System.Drawing.Image)(resources.GetObject("mButtonStartStopAudio.Image")));
			this.mButtonStartStopAudio.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonStartStopAudio.ImageAlign")));
			this.mButtonStartStopAudio.ImageIndex = ((int)(resources.GetObject("mButtonStartStopAudio.ImageIndex")));
			this.mButtonStartStopAudio.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonStartStopAudio.ImeMode")));
			this.mButtonStartStopAudio.Location = ((System.Drawing.Point)(resources.GetObject("mButtonStartStopAudio.Location")));
			this.mButtonStartStopAudio.Name = "mButtonStartStopAudio";
			this.mButtonStartStopAudio.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonStartStopAudio.RightToLeft")));
			this.mButtonStartStopAudio.Size = ((System.Drawing.Size)(resources.GetObject("mButtonStartStopAudio.Size")));
			this.mButtonStartStopAudio.TabIndex = ((int)(resources.GetObject("mButtonStartStopAudio.TabIndex")));
			this.mButtonStartStopAudio.Text = resources.GetString("mButtonStartStopAudio.Text");
			this.mButtonStartStopAudio.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonStartStopAudio.TextAlign")));
			this.mTooltips.SetToolTip(this.mButtonStartStopAudio, resources.GetString("mButtonStartStopAudio.ToolTip"));
			this.mButtonStartStopAudio.Visible = ((bool)(resources.GetObject("mButtonStartStopAudio.Visible")));
			this.mButtonStartStopAudio.Click += new System.EventHandler(this.mButtonStartStopAudio_Click);
			// 
			// mButtonStartStopVideo
			// 
			this.mButtonStartStopVideo.AccessibleDescription = resources.GetString("mButtonStartStopVideo.AccessibleDescription");
			this.mButtonStartStopVideo.AccessibleName = resources.GetString("mButtonStartStopVideo.AccessibleName");
			this.mButtonStartStopVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonStartStopVideo.Anchor")));
			this.mButtonStartStopVideo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonStartStopVideo.BackgroundImage")));
			this.mButtonStartStopVideo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonStartStopVideo.Dock")));
			this.mButtonStartStopVideo.Enabled = ((bool)(resources.GetObject("mButtonStartStopVideo.Enabled")));
			this.mButtonStartStopVideo.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonStartStopVideo.FlatStyle")));
			this.mButtonStartStopVideo.Font = ((System.Drawing.Font)(resources.GetObject("mButtonStartStopVideo.Font")));
			this.mButtonStartStopVideo.Image = ((System.Drawing.Image)(resources.GetObject("mButtonStartStopVideo.Image")));
			this.mButtonStartStopVideo.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonStartStopVideo.ImageAlign")));
			this.mButtonStartStopVideo.ImageIndex = ((int)(resources.GetObject("mButtonStartStopVideo.ImageIndex")));
			this.mButtonStartStopVideo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonStartStopVideo.ImeMode")));
			this.mButtonStartStopVideo.Location = ((System.Drawing.Point)(resources.GetObject("mButtonStartStopVideo.Location")));
			this.mButtonStartStopVideo.Name = "mButtonStartStopVideo";
			this.mButtonStartStopVideo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonStartStopVideo.RightToLeft")));
			this.mButtonStartStopVideo.Size = ((System.Drawing.Size)(resources.GetObject("mButtonStartStopVideo.Size")));
			this.mButtonStartStopVideo.TabIndex = ((int)(resources.GetObject("mButtonStartStopVideo.TabIndex")));
			this.mButtonStartStopVideo.Text = resources.GetString("mButtonStartStopVideo.Text");
			this.mButtonStartStopVideo.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonStartStopVideo.TextAlign")));
			this.mTooltips.SetToolTip(this.mButtonStartStopVideo, resources.GetString("mButtonStartStopVideo.ToolTip"));
			this.mButtonStartStopVideo.Visible = ((bool)(resources.GetObject("mButtonStartStopVideo.Visible")));
			this.mButtonStartStopVideo.Click += new System.EventHandler(this.mButtonStartStopVideo_Click);
			// 
			// mButtonStartStopComm
			// 
			this.mButtonStartStopComm.AccessibleDescription = resources.GetString("mButtonStartStopComm.AccessibleDescription");
			this.mButtonStartStopComm.AccessibleName = resources.GetString("mButtonStartStopComm.AccessibleName");
			this.mButtonStartStopComm.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonStartStopComm.Anchor")));
			this.mButtonStartStopComm.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonStartStopComm.BackgroundImage")));
			this.mButtonStartStopComm.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonStartStopComm.Dock")));
			this.mButtonStartStopComm.Enabled = ((bool)(resources.GetObject("mButtonStartStopComm.Enabled")));
			this.mButtonStartStopComm.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonStartStopComm.FlatStyle")));
			this.mButtonStartStopComm.Font = ((System.Drawing.Font)(resources.GetObject("mButtonStartStopComm.Font")));
			this.mButtonStartStopComm.Image = ((System.Drawing.Image)(resources.GetObject("mButtonStartStopComm.Image")));
			this.mButtonStartStopComm.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonStartStopComm.ImageAlign")));
			this.mButtonStartStopComm.ImageIndex = ((int)(resources.GetObject("mButtonStartStopComm.ImageIndex")));
			this.mButtonStartStopComm.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonStartStopComm.ImeMode")));
			this.mButtonStartStopComm.Location = ((System.Drawing.Point)(resources.GetObject("mButtonStartStopComm.Location")));
			this.mButtonStartStopComm.Name = "mButtonStartStopComm";
			this.mButtonStartStopComm.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonStartStopComm.RightToLeft")));
			this.mButtonStartStopComm.Size = ((System.Drawing.Size)(resources.GetObject("mButtonStartStopComm.Size")));
			this.mButtonStartStopComm.TabIndex = ((int)(resources.GetObject("mButtonStartStopComm.TabIndex")));
			this.mButtonStartStopComm.Text = resources.GetString("mButtonStartStopComm.Text");
			this.mButtonStartStopComm.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonStartStopComm.TextAlign")));
			this.mTooltips.SetToolTip(this.mButtonStartStopComm, resources.GetString("mButtonStartStopComm.ToolTip"));
			this.mButtonStartStopComm.Visible = ((bool)(resources.GetObject("mButtonStartStopComm.Visible")));
			this.mButtonStartStopComm.Click += new System.EventHandler(this.mButtonStartStopComm_Click);
			// 
			// mPixLocalCompressed
			// 
			this.mPixLocalCompressed.AccessibleDescription = resources.GetString("mPixLocalCompressed.AccessibleDescription");
			this.mPixLocalCompressed.AccessibleName = resources.GetString("mPixLocalCompressed.AccessibleName");
			this.mPixLocalCompressed.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mPixLocalCompressed.Anchor")));
			this.mPixLocalCompressed.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mPixLocalCompressed.BackgroundImage")));
			this.mPixLocalCompressed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPixLocalCompressed.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mPixLocalCompressed.Dock")));
			this.mPixLocalCompressed.Enabled = ((bool)(resources.GetObject("mPixLocalCompressed.Enabled")));
			this.mPixLocalCompressed.Font = ((System.Drawing.Font)(resources.GetObject("mPixLocalCompressed.Font")));
			this.mPixLocalCompressed.Image = ((System.Drawing.Image)(resources.GetObject("mPixLocalCompressed.Image")));
			this.mPixLocalCompressed.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mPixLocalCompressed.ImeMode")));
			this.mPixLocalCompressed.Location = ((System.Drawing.Point)(resources.GetObject("mPixLocalCompressed.Location")));
			this.mPixLocalCompressed.Name = "mPixLocalCompressed";
			this.mPixLocalCompressed.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mPixLocalCompressed.RightToLeft")));
			this.mPixLocalCompressed.Size = ((System.Drawing.Size)(resources.GetObject("mPixLocalCompressed.Size")));
			this.mPixLocalCompressed.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("mPixLocalCompressed.SizeMode")));
			this.mPixLocalCompressed.TabIndex = ((int)(resources.GetObject("mPixLocalCompressed.TabIndex")));
			this.mPixLocalCompressed.TabStop = false;
			this.mPixLocalCompressed.Text = resources.GetString("mPixLocalCompressed.Text");
			this.mTooltips.SetToolTip(this.mPixLocalCompressed, resources.GetString("mPixLocalCompressed.ToolTip"));
			this.mPixLocalCompressed.Visible = ((bool)(resources.GetObject("mPixLocalCompressed.Visible")));
			// 
			// label1
			// 
			this.label1.AccessibleDescription = resources.GetString("label1.AccessibleDescription");
			this.label1.AccessibleName = resources.GetString("label1.AccessibleName");
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
			this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
			this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
			this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
			this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
			this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
			this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
			this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
			this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
			this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
			this.mTooltips.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
			this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
			// 
			// mGroupRemote
			// 
			this.mGroupRemote.AccessibleDescription = resources.GetString("mGroupRemote.AccessibleDescription");
			this.mGroupRemote.AccessibleName = resources.GetString("mGroupRemote.AccessibleName");
			this.mGroupRemote.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mGroupRemote.Anchor")));
			this.mGroupRemote.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mGroupRemote.BackgroundImage")));
			this.mGroupRemote.Controls.Add(this.mSoundLevelRemoteReceived);
			this.mGroupRemote.Controls.Add(this.label4);
			this.mGroupRemote.Controls.Add(this.label6);
			this.mGroupRemote.Controls.Add(this.mPixRemoteReceived);
			this.mGroupRemote.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mGroupRemote.Dock")));
			this.mGroupRemote.Enabled = ((bool)(resources.GetObject("mGroupRemote.Enabled")));
			this.mGroupRemote.Font = ((System.Drawing.Font)(resources.GetObject("mGroupRemote.Font")));
			this.mGroupRemote.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mGroupRemote.ImeMode")));
			this.mGroupRemote.Location = ((System.Drawing.Point)(resources.GetObject("mGroupRemote.Location")));
			this.mGroupRemote.Name = "mGroupRemote";
			this.mGroupRemote.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mGroupRemote.RightToLeft")));
			this.mGroupRemote.Size = ((System.Drawing.Size)(resources.GetObject("mGroupRemote.Size")));
			this.mGroupRemote.TabIndex = ((int)(resources.GetObject("mGroupRemote.TabIndex")));
			this.mGroupRemote.TabStop = false;
			this.mGroupRemote.Text = resources.GetString("mGroupRemote.Text");
			this.mTooltips.SetToolTip(this.mGroupRemote, resources.GetString("mGroupRemote.ToolTip"));
			this.mGroupRemote.Visible = ((bool)(resources.GetObject("mGroupRemote.Visible")));
			// 
			// mSoundLevelRemoteReceived
			// 
			this.mSoundLevelRemoteReceived.AccessibleDescription = resources.GetString("mSoundLevelRemoteReceived.AccessibleDescription");
			this.mSoundLevelRemoteReceived.AccessibleName = resources.GetString("mSoundLevelRemoteReceived.AccessibleName");
			this.mSoundLevelRemoteReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mSoundLevelRemoteReceived.Anchor")));
			this.mSoundLevelRemoteReceived.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mSoundLevelRemoteReceived.BackgroundImage")));
			this.mSoundLevelRemoteReceived.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mSoundLevelRemoteReceived.Dock")));
			this.mSoundLevelRemoteReceived.Enabled = ((bool)(resources.GetObject("mSoundLevelRemoteReceived.Enabled")));
			this.mSoundLevelRemoteReceived.Font = ((System.Drawing.Font)(resources.GetObject("mSoundLevelRemoteReceived.Font")));
			this.mSoundLevelRemoteReceived.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mSoundLevelRemoteReceived.ImeMode")));
			this.mSoundLevelRemoteReceived.Location = ((System.Drawing.Point)(resources.GetObject("mSoundLevelRemoteReceived.Location")));
			this.mSoundLevelRemoteReceived.Name = "mSoundLevelRemoteReceived";
			this.mSoundLevelRemoteReceived.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mSoundLevelRemoteReceived.RightToLeft")));
			this.mSoundLevelRemoteReceived.Size = ((System.Drawing.Size)(resources.GetObject("mSoundLevelRemoteReceived.Size")));
			this.mSoundLevelRemoteReceived.TabIndex = ((int)(resources.GetObject("mSoundLevelRemoteReceived.TabIndex")));
			this.mSoundLevelRemoteReceived.Text = resources.GetString("mSoundLevelRemoteReceived.Text");
			this.mTooltips.SetToolTip(this.mSoundLevelRemoteReceived, resources.GetString("mSoundLevelRemoteReceived.ToolTip"));
			this.mSoundLevelRemoteReceived.Visible = ((bool)(resources.GetObject("mSoundLevelRemoteReceived.Visible")));
			// 
			// label4
			// 
			this.label4.AccessibleDescription = resources.GetString("label4.AccessibleDescription");
			this.label4.AccessibleName = resources.GetString("label4.AccessibleName");
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label4.Anchor")));
			this.label4.AutoSize = ((bool)(resources.GetObject("label4.AutoSize")));
			this.label4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label4.Dock")));
			this.label4.Enabled = ((bool)(resources.GetObject("label4.Enabled")));
			this.label4.Font = ((System.Drawing.Font)(resources.GetObject("label4.Font")));
			this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
			this.label4.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.ImageAlign")));
			this.label4.ImageIndex = ((int)(resources.GetObject("label4.ImageIndex")));
			this.label4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label4.ImeMode")));
			this.label4.Location = ((System.Drawing.Point)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label4.RightToLeft")));
			this.label4.Size = ((System.Drawing.Size)(resources.GetObject("label4.Size")));
			this.label4.TabIndex = ((int)(resources.GetObject("label4.TabIndex")));
			this.label4.Text = resources.GetString("label4.Text");
			this.label4.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.TextAlign")));
			this.mTooltips.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
			this.label4.Visible = ((bool)(resources.GetObject("label4.Visible")));
			// 
			// label6
			// 
			this.label6.AccessibleDescription = resources.GetString("label6.AccessibleDescription");
			this.label6.AccessibleName = resources.GetString("label6.AccessibleName");
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label6.Anchor")));
			this.label6.AutoSize = ((bool)(resources.GetObject("label6.AutoSize")));
			this.label6.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label6.Dock")));
			this.label6.Enabled = ((bool)(resources.GetObject("label6.Enabled")));
			this.label6.Font = ((System.Drawing.Font)(resources.GetObject("label6.Font")));
			this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
			this.label6.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.ImageAlign")));
			this.label6.ImageIndex = ((int)(resources.GetObject("label6.ImageIndex")));
			this.label6.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label6.ImeMode")));
			this.label6.Location = ((System.Drawing.Point)(resources.GetObject("label6.Location")));
			this.label6.Name = "label6";
			this.label6.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label6.RightToLeft")));
			this.label6.Size = ((System.Drawing.Size)(resources.GetObject("label6.Size")));
			this.label6.TabIndex = ((int)(resources.GetObject("label6.TabIndex")));
			this.label6.Text = resources.GetString("label6.Text");
			this.label6.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label6.TextAlign")));
			this.mTooltips.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
			this.label6.Visible = ((bool)(resources.GetObject("label6.Visible")));
			// 
			// mPixRemoteReceived
			// 
			this.mPixRemoteReceived.AccessibleDescription = resources.GetString("mPixRemoteReceived.AccessibleDescription");
			this.mPixRemoteReceived.AccessibleName = resources.GetString("mPixRemoteReceived.AccessibleName");
			this.mPixRemoteReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mPixRemoteReceived.Anchor")));
			this.mPixRemoteReceived.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mPixRemoteReceived.BackgroundImage")));
			this.mPixRemoteReceived.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPixRemoteReceived.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mPixRemoteReceived.Dock")));
			this.mPixRemoteReceived.Enabled = ((bool)(resources.GetObject("mPixRemoteReceived.Enabled")));
			this.mPixRemoteReceived.Font = ((System.Drawing.Font)(resources.GetObject("mPixRemoteReceived.Font")));
			this.mPixRemoteReceived.Image = ((System.Drawing.Image)(resources.GetObject("mPixRemoteReceived.Image")));
			this.mPixRemoteReceived.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mPixRemoteReceived.ImeMode")));
			this.mPixRemoteReceived.Location = ((System.Drawing.Point)(resources.GetObject("mPixRemoteReceived.Location")));
			this.mPixRemoteReceived.Name = "mPixRemoteReceived";
			this.mPixRemoteReceived.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mPixRemoteReceived.RightToLeft")));
			this.mPixRemoteReceived.Size = ((System.Drawing.Size)(resources.GetObject("mPixRemoteReceived.Size")));
			this.mPixRemoteReceived.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("mPixRemoteReceived.SizeMode")));
			this.mPixRemoteReceived.TabIndex = ((int)(resources.GetObject("mPixRemoteReceived.TabIndex")));
			this.mPixRemoteReceived.TabStop = false;
			this.mPixRemoteReceived.Text = resources.GetString("mPixRemoteReceived.Text");
			this.mTooltips.SetToolTip(this.mPixRemoteReceived, resources.GetString("mPixRemoteReceived.ToolTip"));
			this.mPixRemoteReceived.Visible = ((bool)(resources.GetObject("mPixRemoteReceived.Visible")));
			// 
			// mGroupStats
			// 
			this.mGroupStats.AccessibleDescription = resources.GetString("mGroupStats.AccessibleDescription");
			this.mGroupStats.AccessibleName = resources.GetString("mGroupStats.AccessibleName");
			this.mGroupStats.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mGroupStats.Anchor")));
			this.mGroupStats.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mGroupStats.BackgroundImage")));
			this.mGroupStats.Controls.Add(this.groupBox6);
			this.mGroupStats.Controls.Add(this.groupBox5);
			this.mGroupStats.Controls.Add(this.mStatTextTotalReceived);
			this.mGroupStats.Controls.Add(this.mStatTextTotalSent);
			this.mGroupStats.Controls.Add(this.mStatTextAudioReceived);
			this.mGroupStats.Controls.Add(this.mStatTextAudioSent);
			this.mGroupStats.Controls.Add(this.mStatTextVideoReceived);
			this.mGroupStats.Controls.Add(this.label11);
			this.mGroupStats.Controls.Add(this.mStatTextVideoSent);
			this.mGroupStats.Controls.Add(this.label9);
			this.mGroupStats.Controls.Add(this.label8);
			this.mGroupStats.Controls.Add(this.label7);
			this.mGroupStats.Controls.Add(this.label5);
			this.mGroupStats.Controls.Add(this.groupBox4);
			this.mGroupStats.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mGroupStats.Dock")));
			this.mGroupStats.Enabled = ((bool)(resources.GetObject("mGroupStats.Enabled")));
			this.mGroupStats.Font = ((System.Drawing.Font)(resources.GetObject("mGroupStats.Font")));
			this.mGroupStats.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mGroupStats.ImeMode")));
			this.mGroupStats.Location = ((System.Drawing.Point)(resources.GetObject("mGroupStats.Location")));
			this.mGroupStats.Name = "mGroupStats";
			this.mGroupStats.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mGroupStats.RightToLeft")));
			this.mGroupStats.Size = ((System.Drawing.Size)(resources.GetObject("mGroupStats.Size")));
			this.mGroupStats.TabIndex = ((int)(resources.GetObject("mGroupStats.TabIndex")));
			this.mGroupStats.TabStop = false;
			this.mGroupStats.Text = resources.GetString("mGroupStats.Text");
			this.mTooltips.SetToolTip(this.mGroupStats, resources.GetString("mGroupStats.ToolTip"));
			this.mGroupStats.Visible = ((bool)(resources.GetObject("mGroupStats.Visible")));
			// 
			// groupBox6
			// 
			this.groupBox6.AccessibleDescription = resources.GetString("groupBox6.AccessibleDescription");
			this.groupBox6.AccessibleName = resources.GetString("groupBox6.AccessibleName");
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox6.Anchor")));
			this.groupBox6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox6.BackgroundImage")));
			this.groupBox6.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox6.Dock")));
			this.groupBox6.Enabled = ((bool)(resources.GetObject("groupBox6.Enabled")));
			this.groupBox6.Font = ((System.Drawing.Font)(resources.GetObject("groupBox6.Font")));
			this.groupBox6.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox6.ImeMode")));
			this.groupBox6.Location = ((System.Drawing.Point)(resources.GetObject("groupBox6.Location")));
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox6.RightToLeft")));
			this.groupBox6.Size = ((System.Drawing.Size)(resources.GetObject("groupBox6.Size")));
			this.groupBox6.TabIndex = ((int)(resources.GetObject("groupBox6.TabIndex")));
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = resources.GetString("groupBox6.Text");
			this.mTooltips.SetToolTip(this.groupBox6, resources.GetString("groupBox6.ToolTip"));
			this.groupBox6.Visible = ((bool)(resources.GetObject("groupBox6.Visible")));
			// 
			// groupBox5
			// 
			this.groupBox5.AccessibleDescription = resources.GetString("groupBox5.AccessibleDescription");
			this.groupBox5.AccessibleName = resources.GetString("groupBox5.AccessibleName");
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox5.Anchor")));
			this.groupBox5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox5.BackgroundImage")));
			this.groupBox5.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox5.Dock")));
			this.groupBox5.Enabled = ((bool)(resources.GetObject("groupBox5.Enabled")));
			this.groupBox5.Font = ((System.Drawing.Font)(resources.GetObject("groupBox5.Font")));
			this.groupBox5.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox5.ImeMode")));
			this.groupBox5.Location = ((System.Drawing.Point)(resources.GetObject("groupBox5.Location")));
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox5.RightToLeft")));
			this.groupBox5.Size = ((System.Drawing.Size)(resources.GetObject("groupBox5.Size")));
			this.groupBox5.TabIndex = ((int)(resources.GetObject("groupBox5.TabIndex")));
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = resources.GetString("groupBox5.Text");
			this.mTooltips.SetToolTip(this.groupBox5, resources.GetString("groupBox5.ToolTip"));
			this.groupBox5.Visible = ((bool)(resources.GetObject("groupBox5.Visible")));
			// 
			// mStatTextTotalReceived
			// 
			this.mStatTextTotalReceived.AccessibleDescription = resources.GetString("mStatTextTotalReceived.AccessibleDescription");
			this.mStatTextTotalReceived.AccessibleName = resources.GetString("mStatTextTotalReceived.AccessibleName");
			this.mStatTextTotalReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mStatTextTotalReceived.Anchor")));
			this.mStatTextTotalReceived.AutoSize = ((bool)(resources.GetObject("mStatTextTotalReceived.AutoSize")));
			this.mStatTextTotalReceived.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mStatTextTotalReceived.Dock")));
			this.mStatTextTotalReceived.Enabled = ((bool)(resources.GetObject("mStatTextTotalReceived.Enabled")));
			this.mStatTextTotalReceived.Font = ((System.Drawing.Font)(resources.GetObject("mStatTextTotalReceived.Font")));
			this.mStatTextTotalReceived.Image = ((System.Drawing.Image)(resources.GetObject("mStatTextTotalReceived.Image")));
			this.mStatTextTotalReceived.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextTotalReceived.ImageAlign")));
			this.mStatTextTotalReceived.ImageIndex = ((int)(resources.GetObject("mStatTextTotalReceived.ImageIndex")));
			this.mStatTextTotalReceived.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mStatTextTotalReceived.ImeMode")));
			this.mStatTextTotalReceived.Location = ((System.Drawing.Point)(resources.GetObject("mStatTextTotalReceived.Location")));
			this.mStatTextTotalReceived.Name = "mStatTextTotalReceived";
			this.mStatTextTotalReceived.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mStatTextTotalReceived.RightToLeft")));
			this.mStatTextTotalReceived.Size = ((System.Drawing.Size)(resources.GetObject("mStatTextTotalReceived.Size")));
			this.mStatTextTotalReceived.TabIndex = ((int)(resources.GetObject("mStatTextTotalReceived.TabIndex")));
			this.mStatTextTotalReceived.Text = resources.GetString("mStatTextTotalReceived.Text");
			this.mStatTextTotalReceived.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextTotalReceived.TextAlign")));
			this.mTooltips.SetToolTip(this.mStatTextTotalReceived, resources.GetString("mStatTextTotalReceived.ToolTip"));
			this.mStatTextTotalReceived.Visible = ((bool)(resources.GetObject("mStatTextTotalReceived.Visible")));
			// 
			// mStatTextTotalSent
			// 
			this.mStatTextTotalSent.AccessibleDescription = resources.GetString("mStatTextTotalSent.AccessibleDescription");
			this.mStatTextTotalSent.AccessibleName = resources.GetString("mStatTextTotalSent.AccessibleName");
			this.mStatTextTotalSent.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mStatTextTotalSent.Anchor")));
			this.mStatTextTotalSent.AutoSize = ((bool)(resources.GetObject("mStatTextTotalSent.AutoSize")));
			this.mStatTextTotalSent.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mStatTextTotalSent.Dock")));
			this.mStatTextTotalSent.Enabled = ((bool)(resources.GetObject("mStatTextTotalSent.Enabled")));
			this.mStatTextTotalSent.Font = ((System.Drawing.Font)(resources.GetObject("mStatTextTotalSent.Font")));
			this.mStatTextTotalSent.Image = ((System.Drawing.Image)(resources.GetObject("mStatTextTotalSent.Image")));
			this.mStatTextTotalSent.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextTotalSent.ImageAlign")));
			this.mStatTextTotalSent.ImageIndex = ((int)(resources.GetObject("mStatTextTotalSent.ImageIndex")));
			this.mStatTextTotalSent.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mStatTextTotalSent.ImeMode")));
			this.mStatTextTotalSent.Location = ((System.Drawing.Point)(resources.GetObject("mStatTextTotalSent.Location")));
			this.mStatTextTotalSent.Name = "mStatTextTotalSent";
			this.mStatTextTotalSent.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mStatTextTotalSent.RightToLeft")));
			this.mStatTextTotalSent.Size = ((System.Drawing.Size)(resources.GetObject("mStatTextTotalSent.Size")));
			this.mStatTextTotalSent.TabIndex = ((int)(resources.GetObject("mStatTextTotalSent.TabIndex")));
			this.mStatTextTotalSent.Text = resources.GetString("mStatTextTotalSent.Text");
			this.mStatTextTotalSent.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextTotalSent.TextAlign")));
			this.mTooltips.SetToolTip(this.mStatTextTotalSent, resources.GetString("mStatTextTotalSent.ToolTip"));
			this.mStatTextTotalSent.Visible = ((bool)(resources.GetObject("mStatTextTotalSent.Visible")));
			// 
			// mStatTextAudioReceived
			// 
			this.mStatTextAudioReceived.AccessibleDescription = resources.GetString("mStatTextAudioReceived.AccessibleDescription");
			this.mStatTextAudioReceived.AccessibleName = resources.GetString("mStatTextAudioReceived.AccessibleName");
			this.mStatTextAudioReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mStatTextAudioReceived.Anchor")));
			this.mStatTextAudioReceived.AutoSize = ((bool)(resources.GetObject("mStatTextAudioReceived.AutoSize")));
			this.mStatTextAudioReceived.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mStatTextAudioReceived.Dock")));
			this.mStatTextAudioReceived.Enabled = ((bool)(resources.GetObject("mStatTextAudioReceived.Enabled")));
			this.mStatTextAudioReceived.Font = ((System.Drawing.Font)(resources.GetObject("mStatTextAudioReceived.Font")));
			this.mStatTextAudioReceived.Image = ((System.Drawing.Image)(resources.GetObject("mStatTextAudioReceived.Image")));
			this.mStatTextAudioReceived.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextAudioReceived.ImageAlign")));
			this.mStatTextAudioReceived.ImageIndex = ((int)(resources.GetObject("mStatTextAudioReceived.ImageIndex")));
			this.mStatTextAudioReceived.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mStatTextAudioReceived.ImeMode")));
			this.mStatTextAudioReceived.Location = ((System.Drawing.Point)(resources.GetObject("mStatTextAudioReceived.Location")));
			this.mStatTextAudioReceived.Name = "mStatTextAudioReceived";
			this.mStatTextAudioReceived.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mStatTextAudioReceived.RightToLeft")));
			this.mStatTextAudioReceived.Size = ((System.Drawing.Size)(resources.GetObject("mStatTextAudioReceived.Size")));
			this.mStatTextAudioReceived.TabIndex = ((int)(resources.GetObject("mStatTextAudioReceived.TabIndex")));
			this.mStatTextAudioReceived.Text = resources.GetString("mStatTextAudioReceived.Text");
			this.mStatTextAudioReceived.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextAudioReceived.TextAlign")));
			this.mTooltips.SetToolTip(this.mStatTextAudioReceived, resources.GetString("mStatTextAudioReceived.ToolTip"));
			this.mStatTextAudioReceived.Visible = ((bool)(resources.GetObject("mStatTextAudioReceived.Visible")));
			// 
			// mStatTextAudioSent
			// 
			this.mStatTextAudioSent.AccessibleDescription = resources.GetString("mStatTextAudioSent.AccessibleDescription");
			this.mStatTextAudioSent.AccessibleName = resources.GetString("mStatTextAudioSent.AccessibleName");
			this.mStatTextAudioSent.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mStatTextAudioSent.Anchor")));
			this.mStatTextAudioSent.AutoSize = ((bool)(resources.GetObject("mStatTextAudioSent.AutoSize")));
			this.mStatTextAudioSent.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mStatTextAudioSent.Dock")));
			this.mStatTextAudioSent.Enabled = ((bool)(resources.GetObject("mStatTextAudioSent.Enabled")));
			this.mStatTextAudioSent.Font = ((System.Drawing.Font)(resources.GetObject("mStatTextAudioSent.Font")));
			this.mStatTextAudioSent.Image = ((System.Drawing.Image)(resources.GetObject("mStatTextAudioSent.Image")));
			this.mStatTextAudioSent.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextAudioSent.ImageAlign")));
			this.mStatTextAudioSent.ImageIndex = ((int)(resources.GetObject("mStatTextAudioSent.ImageIndex")));
			this.mStatTextAudioSent.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mStatTextAudioSent.ImeMode")));
			this.mStatTextAudioSent.Location = ((System.Drawing.Point)(resources.GetObject("mStatTextAudioSent.Location")));
			this.mStatTextAudioSent.Name = "mStatTextAudioSent";
			this.mStatTextAudioSent.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mStatTextAudioSent.RightToLeft")));
			this.mStatTextAudioSent.Size = ((System.Drawing.Size)(resources.GetObject("mStatTextAudioSent.Size")));
			this.mStatTextAudioSent.TabIndex = ((int)(resources.GetObject("mStatTextAudioSent.TabIndex")));
			this.mStatTextAudioSent.Text = resources.GetString("mStatTextAudioSent.Text");
			this.mStatTextAudioSent.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextAudioSent.TextAlign")));
			this.mTooltips.SetToolTip(this.mStatTextAudioSent, resources.GetString("mStatTextAudioSent.ToolTip"));
			this.mStatTextAudioSent.Visible = ((bool)(resources.GetObject("mStatTextAudioSent.Visible")));
			// 
			// mStatTextVideoReceived
			// 
			this.mStatTextVideoReceived.AccessibleDescription = resources.GetString("mStatTextVideoReceived.AccessibleDescription");
			this.mStatTextVideoReceived.AccessibleName = resources.GetString("mStatTextVideoReceived.AccessibleName");
			this.mStatTextVideoReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mStatTextVideoReceived.Anchor")));
			this.mStatTextVideoReceived.AutoSize = ((bool)(resources.GetObject("mStatTextVideoReceived.AutoSize")));
			this.mStatTextVideoReceived.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mStatTextVideoReceived.Dock")));
			this.mStatTextVideoReceived.Enabled = ((bool)(resources.GetObject("mStatTextVideoReceived.Enabled")));
			this.mStatTextVideoReceived.Font = ((System.Drawing.Font)(resources.GetObject("mStatTextVideoReceived.Font")));
			this.mStatTextVideoReceived.Image = ((System.Drawing.Image)(resources.GetObject("mStatTextVideoReceived.Image")));
			this.mStatTextVideoReceived.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextVideoReceived.ImageAlign")));
			this.mStatTextVideoReceived.ImageIndex = ((int)(resources.GetObject("mStatTextVideoReceived.ImageIndex")));
			this.mStatTextVideoReceived.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mStatTextVideoReceived.ImeMode")));
			this.mStatTextVideoReceived.Location = ((System.Drawing.Point)(resources.GetObject("mStatTextVideoReceived.Location")));
			this.mStatTextVideoReceived.Name = "mStatTextVideoReceived";
			this.mStatTextVideoReceived.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mStatTextVideoReceived.RightToLeft")));
			this.mStatTextVideoReceived.Size = ((System.Drawing.Size)(resources.GetObject("mStatTextVideoReceived.Size")));
			this.mStatTextVideoReceived.TabIndex = ((int)(resources.GetObject("mStatTextVideoReceived.TabIndex")));
			this.mStatTextVideoReceived.Text = resources.GetString("mStatTextVideoReceived.Text");
			this.mStatTextVideoReceived.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextVideoReceived.TextAlign")));
			this.mTooltips.SetToolTip(this.mStatTextVideoReceived, resources.GetString("mStatTextVideoReceived.ToolTip"));
			this.mStatTextVideoReceived.Visible = ((bool)(resources.GetObject("mStatTextVideoReceived.Visible")));
			// 
			// label11
			// 
			this.label11.AccessibleDescription = resources.GetString("label11.AccessibleDescription");
			this.label11.AccessibleName = resources.GetString("label11.AccessibleName");
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label11.Anchor")));
			this.label11.AutoSize = ((bool)(resources.GetObject("label11.AutoSize")));
			this.label11.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label11.Dock")));
			this.label11.Enabled = ((bool)(resources.GetObject("label11.Enabled")));
			this.label11.Font = ((System.Drawing.Font)(resources.GetObject("label11.Font")));
			this.label11.Image = ((System.Drawing.Image)(resources.GetObject("label11.Image")));
			this.label11.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label11.ImageAlign")));
			this.label11.ImageIndex = ((int)(resources.GetObject("label11.ImageIndex")));
			this.label11.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label11.ImeMode")));
			this.label11.Location = ((System.Drawing.Point)(resources.GetObject("label11.Location")));
			this.label11.Name = "label11";
			this.label11.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label11.RightToLeft")));
			this.label11.Size = ((System.Drawing.Size)(resources.GetObject("label11.Size")));
			this.label11.TabIndex = ((int)(resources.GetObject("label11.TabIndex")));
			this.label11.Text = resources.GetString("label11.Text");
			this.label11.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label11.TextAlign")));
			this.mTooltips.SetToolTip(this.label11, resources.GetString("label11.ToolTip"));
			this.label11.Visible = ((bool)(resources.GetObject("label11.Visible")));
			// 
			// mStatTextVideoSent
			// 
			this.mStatTextVideoSent.AccessibleDescription = resources.GetString("mStatTextVideoSent.AccessibleDescription");
			this.mStatTextVideoSent.AccessibleName = resources.GetString("mStatTextVideoSent.AccessibleName");
			this.mStatTextVideoSent.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mStatTextVideoSent.Anchor")));
			this.mStatTextVideoSent.AutoSize = ((bool)(resources.GetObject("mStatTextVideoSent.AutoSize")));
			this.mStatTextVideoSent.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mStatTextVideoSent.Dock")));
			this.mStatTextVideoSent.Enabled = ((bool)(resources.GetObject("mStatTextVideoSent.Enabled")));
			this.mStatTextVideoSent.Font = ((System.Drawing.Font)(resources.GetObject("mStatTextVideoSent.Font")));
			this.mStatTextVideoSent.Image = ((System.Drawing.Image)(resources.GetObject("mStatTextVideoSent.Image")));
			this.mStatTextVideoSent.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextVideoSent.ImageAlign")));
			this.mStatTextVideoSent.ImageIndex = ((int)(resources.GetObject("mStatTextVideoSent.ImageIndex")));
			this.mStatTextVideoSent.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mStatTextVideoSent.ImeMode")));
			this.mStatTextVideoSent.Location = ((System.Drawing.Point)(resources.GetObject("mStatTextVideoSent.Location")));
			this.mStatTextVideoSent.Name = "mStatTextVideoSent";
			this.mStatTextVideoSent.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mStatTextVideoSent.RightToLeft")));
			this.mStatTextVideoSent.Size = ((System.Drawing.Size)(resources.GetObject("mStatTextVideoSent.Size")));
			this.mStatTextVideoSent.TabIndex = ((int)(resources.GetObject("mStatTextVideoSent.TabIndex")));
			this.mStatTextVideoSent.Text = resources.GetString("mStatTextVideoSent.Text");
			this.mStatTextVideoSent.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mStatTextVideoSent.TextAlign")));
			this.mTooltips.SetToolTip(this.mStatTextVideoSent, resources.GetString("mStatTextVideoSent.ToolTip"));
			this.mStatTextVideoSent.Visible = ((bool)(resources.GetObject("mStatTextVideoSent.Visible")));
			// 
			// label9
			// 
			this.label9.AccessibleDescription = resources.GetString("label9.AccessibleDescription");
			this.label9.AccessibleName = resources.GetString("label9.AccessibleName");
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label9.Anchor")));
			this.label9.AutoSize = ((bool)(resources.GetObject("label9.AutoSize")));
			this.label9.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label9.Dock")));
			this.label9.Enabled = ((bool)(resources.GetObject("label9.Enabled")));
			this.label9.Font = ((System.Drawing.Font)(resources.GetObject("label9.Font")));
			this.label9.Image = ((System.Drawing.Image)(resources.GetObject("label9.Image")));
			this.label9.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label9.ImageAlign")));
			this.label9.ImageIndex = ((int)(resources.GetObject("label9.ImageIndex")));
			this.label9.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label9.ImeMode")));
			this.label9.Location = ((System.Drawing.Point)(resources.GetObject("label9.Location")));
			this.label9.Name = "label9";
			this.label9.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label9.RightToLeft")));
			this.label9.Size = ((System.Drawing.Size)(resources.GetObject("label9.Size")));
			this.label9.TabIndex = ((int)(resources.GetObject("label9.TabIndex")));
			this.label9.Text = resources.GetString("label9.Text");
			this.label9.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label9.TextAlign")));
			this.mTooltips.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
			this.label9.Visible = ((bool)(resources.GetObject("label9.Visible")));
			// 
			// label8
			// 
			this.label8.AccessibleDescription = resources.GetString("label8.AccessibleDescription");
			this.label8.AccessibleName = resources.GetString("label8.AccessibleName");
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label8.Anchor")));
			this.label8.AutoSize = ((bool)(resources.GetObject("label8.AutoSize")));
			this.label8.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label8.Dock")));
			this.label8.Enabled = ((bool)(resources.GetObject("label8.Enabled")));
			this.label8.Font = ((System.Drawing.Font)(resources.GetObject("label8.Font")));
			this.label8.Image = ((System.Drawing.Image)(resources.GetObject("label8.Image")));
			this.label8.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label8.ImageAlign")));
			this.label8.ImageIndex = ((int)(resources.GetObject("label8.ImageIndex")));
			this.label8.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label8.ImeMode")));
			this.label8.Location = ((System.Drawing.Point)(resources.GetObject("label8.Location")));
			this.label8.Name = "label8";
			this.label8.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label8.RightToLeft")));
			this.label8.Size = ((System.Drawing.Size)(resources.GetObject("label8.Size")));
			this.label8.TabIndex = ((int)(resources.GetObject("label8.TabIndex")));
			this.label8.Text = resources.GetString("label8.Text");
			this.label8.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label8.TextAlign")));
			this.mTooltips.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
			this.label8.Visible = ((bool)(resources.GetObject("label8.Visible")));
			// 
			// label7
			// 
			this.label7.AccessibleDescription = resources.GetString("label7.AccessibleDescription");
			this.label7.AccessibleName = resources.GetString("label7.AccessibleName");
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label7.Anchor")));
			this.label7.AutoSize = ((bool)(resources.GetObject("label7.AutoSize")));
			this.label7.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label7.Dock")));
			this.label7.Enabled = ((bool)(resources.GetObject("label7.Enabled")));
			this.label7.Font = ((System.Drawing.Font)(resources.GetObject("label7.Font")));
			this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
			this.label7.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.ImageAlign")));
			this.label7.ImageIndex = ((int)(resources.GetObject("label7.ImageIndex")));
			this.label7.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label7.ImeMode")));
			this.label7.Location = ((System.Drawing.Point)(resources.GetObject("label7.Location")));
			this.label7.Name = "label7";
			this.label7.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label7.RightToLeft")));
			this.label7.Size = ((System.Drawing.Size)(resources.GetObject("label7.Size")));
			this.label7.TabIndex = ((int)(resources.GetObject("label7.TabIndex")));
			this.label7.Text = resources.GetString("label7.Text");
			this.label7.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label7.TextAlign")));
			this.mTooltips.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
			this.label7.Visible = ((bool)(resources.GetObject("label7.Visible")));
			// 
			// label5
			// 
			this.label5.AccessibleDescription = resources.GetString("label5.AccessibleDescription");
			this.label5.AccessibleName = resources.GetString("label5.AccessibleName");
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label5.Anchor")));
			this.label5.AutoSize = ((bool)(resources.GetObject("label5.AutoSize")));
			this.label5.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label5.Dock")));
			this.label5.Enabled = ((bool)(resources.GetObject("label5.Enabled")));
			this.label5.Font = ((System.Drawing.Font)(resources.GetObject("label5.Font")));
			this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
			this.label5.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.ImageAlign")));
			this.label5.ImageIndex = ((int)(resources.GetObject("label5.ImageIndex")));
			this.label5.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label5.ImeMode")));
			this.label5.Location = ((System.Drawing.Point)(resources.GetObject("label5.Location")));
			this.label5.Name = "label5";
			this.label5.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label5.RightToLeft")));
			this.label5.Size = ((System.Drawing.Size)(resources.GetObject("label5.Size")));
			this.label5.TabIndex = ((int)(resources.GetObject("label5.TabIndex")));
			this.label5.Text = resources.GetString("label5.Text");
			this.label5.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.TextAlign")));
			this.mTooltips.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
			this.label5.Visible = ((bool)(resources.GetObject("label5.Visible")));
			// 
			// groupBox4
			// 
			this.groupBox4.AccessibleDescription = resources.GetString("groupBox4.AccessibleDescription");
			this.groupBox4.AccessibleName = resources.GetString("groupBox4.AccessibleName");
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox4.Anchor")));
			this.groupBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox4.BackgroundImage")));
			this.groupBox4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox4.Dock")));
			this.groupBox4.Enabled = ((bool)(resources.GetObject("groupBox4.Enabled")));
			this.groupBox4.Font = ((System.Drawing.Font)(resources.GetObject("groupBox4.Font")));
			this.groupBox4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox4.ImeMode")));
			this.groupBox4.Location = ((System.Drawing.Point)(resources.GetObject("groupBox4.Location")));
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox4.RightToLeft")));
			this.groupBox4.Size = ((System.Drawing.Size)(resources.GetObject("groupBox4.Size")));
			this.groupBox4.TabIndex = ((int)(resources.GetObject("groupBox4.TabIndex")));
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = resources.GetString("groupBox4.Text");
			this.mTooltips.SetToolTip(this.groupBox4, resources.GetString("groupBox4.ToolTip"));
			this.groupBox4.Visible = ((bool)(resources.GetObject("groupBox4.Visible")));
			// 
			// mGroupControl
			// 
			this.mGroupControl.AccessibleDescription = resources.GetString("mGroupControl.AccessibleDescription");
			this.mGroupControl.AccessibleName = resources.GetString("mGroupControl.AccessibleName");
			this.mGroupControl.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mGroupControl.Anchor")));
			this.mGroupControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mGroupControl.BackgroundImage")));
			this.mGroupControl.Controls.Add(this.mControlTextKbps);
			this.mGroupControl.Controls.Add(this.label10);
			this.mGroupControl.Controls.Add(this.mControlCheckMaxKbps);
			this.mGroupControl.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mGroupControl.Dock")));
			this.mGroupControl.Enabled = ((bool)(resources.GetObject("mGroupControl.Enabled")));
			this.mGroupControl.Font = ((System.Drawing.Font)(resources.GetObject("mGroupControl.Font")));
			this.mGroupControl.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mGroupControl.ImeMode")));
			this.mGroupControl.Location = ((System.Drawing.Point)(resources.GetObject("mGroupControl.Location")));
			this.mGroupControl.Name = "mGroupControl";
			this.mGroupControl.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mGroupControl.RightToLeft")));
			this.mGroupControl.Size = ((System.Drawing.Size)(resources.GetObject("mGroupControl.Size")));
			this.mGroupControl.TabIndex = ((int)(resources.GetObject("mGroupControl.TabIndex")));
			this.mGroupControl.TabStop = false;
			this.mGroupControl.Text = resources.GetString("mGroupControl.Text");
			this.mTooltips.SetToolTip(this.mGroupControl, resources.GetString("mGroupControl.ToolTip"));
			this.mGroupControl.Visible = ((bool)(resources.GetObject("mGroupControl.Visible")));
			// 
			// mControlTextKbps
			// 
			this.mControlTextKbps.AccessibleDescription = resources.GetString("mControlTextKbps.AccessibleDescription");
			this.mControlTextKbps.AccessibleName = resources.GetString("mControlTextKbps.AccessibleName");
			this.mControlTextKbps.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mControlTextKbps.Anchor")));
			this.mControlTextKbps.AutoSize = ((bool)(resources.GetObject("mControlTextKbps.AutoSize")));
			this.mControlTextKbps.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mControlTextKbps.BackgroundImage")));
			this.mControlTextKbps.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mControlTextKbps.Dock")));
			this.mControlTextKbps.Enabled = ((bool)(resources.GetObject("mControlTextKbps.Enabled")));
			this.mControlTextKbps.Font = ((System.Drawing.Font)(resources.GetObject("mControlTextKbps.Font")));
			this.mControlTextKbps.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mControlTextKbps.ImeMode")));
			this.mControlTextKbps.Location = ((System.Drawing.Point)(resources.GetObject("mControlTextKbps.Location")));
			this.mControlTextKbps.MaxLength = ((int)(resources.GetObject("mControlTextKbps.MaxLength")));
			this.mControlTextKbps.Multiline = ((bool)(resources.GetObject("mControlTextKbps.Multiline")));
			this.mControlTextKbps.Name = "mControlTextKbps";
			this.mControlTextKbps.PasswordChar = ((char)(resources.GetObject("mControlTextKbps.PasswordChar")));
			this.mControlTextKbps.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mControlTextKbps.RightToLeft")));
			this.mControlTextKbps.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("mControlTextKbps.ScrollBars")));
			this.mControlTextKbps.Size = ((System.Drawing.Size)(resources.GetObject("mControlTextKbps.Size")));
			this.mControlTextKbps.TabIndex = ((int)(resources.GetObject("mControlTextKbps.TabIndex")));
			this.mControlTextKbps.Text = resources.GetString("mControlTextKbps.Text");
			this.mControlTextKbps.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("mControlTextKbps.TextAlign")));
			this.mTooltips.SetToolTip(this.mControlTextKbps, resources.GetString("mControlTextKbps.ToolTip"));
			this.mControlTextKbps.Visible = ((bool)(resources.GetObject("mControlTextKbps.Visible")));
			this.mControlTextKbps.WordWrap = ((bool)(resources.GetObject("mControlTextKbps.WordWrap")));
			// 
			// label10
			// 
			this.label10.AccessibleDescription = resources.GetString("label10.AccessibleDescription");
			this.label10.AccessibleName = resources.GetString("label10.AccessibleName");
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label10.Anchor")));
			this.label10.AutoSize = ((bool)(resources.GetObject("label10.AutoSize")));
			this.label10.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label10.Dock")));
			this.label10.Enabled = ((bool)(resources.GetObject("label10.Enabled")));
			this.label10.Font = ((System.Drawing.Font)(resources.GetObject("label10.Font")));
			this.label10.Image = ((System.Drawing.Image)(resources.GetObject("label10.Image")));
			this.label10.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label10.ImageAlign")));
			this.label10.ImageIndex = ((int)(resources.GetObject("label10.ImageIndex")));
			this.label10.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label10.ImeMode")));
			this.label10.Location = ((System.Drawing.Point)(resources.GetObject("label10.Location")));
			this.label10.Name = "label10";
			this.label10.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label10.RightToLeft")));
			this.label10.Size = ((System.Drawing.Size)(resources.GetObject("label10.Size")));
			this.label10.TabIndex = ((int)(resources.GetObject("label10.TabIndex")));
			this.label10.Text = resources.GetString("label10.Text");
			this.label10.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label10.TextAlign")));
			this.mTooltips.SetToolTip(this.label10, resources.GetString("label10.ToolTip"));
			this.label10.Visible = ((bool)(resources.GetObject("label10.Visible")));
			// 
			// mControlCheckMaxKbps
			// 
			this.mControlCheckMaxKbps.AccessibleDescription = resources.GetString("mControlCheckMaxKbps.AccessibleDescription");
			this.mControlCheckMaxKbps.AccessibleName = resources.GetString("mControlCheckMaxKbps.AccessibleName");
			this.mControlCheckMaxKbps.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mControlCheckMaxKbps.Anchor")));
			this.mControlCheckMaxKbps.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("mControlCheckMaxKbps.Appearance")));
			this.mControlCheckMaxKbps.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mControlCheckMaxKbps.BackgroundImage")));
			this.mControlCheckMaxKbps.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mControlCheckMaxKbps.CheckAlign")));
			this.mControlCheckMaxKbps.Checked = true;
			this.mControlCheckMaxKbps.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mControlCheckMaxKbps.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mControlCheckMaxKbps.Dock")));
			this.mControlCheckMaxKbps.Enabled = ((bool)(resources.GetObject("mControlCheckMaxKbps.Enabled")));
			this.mControlCheckMaxKbps.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mControlCheckMaxKbps.FlatStyle")));
			this.mControlCheckMaxKbps.Font = ((System.Drawing.Font)(resources.GetObject("mControlCheckMaxKbps.Font")));
			this.mControlCheckMaxKbps.Image = ((System.Drawing.Image)(resources.GetObject("mControlCheckMaxKbps.Image")));
			this.mControlCheckMaxKbps.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mControlCheckMaxKbps.ImageAlign")));
			this.mControlCheckMaxKbps.ImageIndex = ((int)(resources.GetObject("mControlCheckMaxKbps.ImageIndex")));
			this.mControlCheckMaxKbps.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mControlCheckMaxKbps.ImeMode")));
			this.mControlCheckMaxKbps.Location = ((System.Drawing.Point)(resources.GetObject("mControlCheckMaxKbps.Location")));
			this.mControlCheckMaxKbps.Name = "mControlCheckMaxKbps";
			this.mControlCheckMaxKbps.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mControlCheckMaxKbps.RightToLeft")));
			this.mControlCheckMaxKbps.Size = ((System.Drawing.Size)(resources.GetObject("mControlCheckMaxKbps.Size")));
			this.mControlCheckMaxKbps.TabIndex = ((int)(resources.GetObject("mControlCheckMaxKbps.TabIndex")));
			this.mControlCheckMaxKbps.Text = resources.GetString("mControlCheckMaxKbps.Text");
			this.mControlCheckMaxKbps.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mControlCheckMaxKbps.TextAlign")));
			this.mTooltips.SetToolTip(this.mControlCheckMaxKbps, resources.GetString("mControlCheckMaxKbps.ToolTip"));
			this.mControlCheckMaxKbps.Visible = ((bool)(resources.GetObject("mControlCheckMaxKbps.Visible")));
			// 
			// mGroupText
			// 
			this.mGroupText.AccessibleDescription = resources.GetString("mGroupText.AccessibleDescription");
			this.mGroupText.AccessibleName = resources.GetString("mGroupText.AccessibleName");
			this.mGroupText.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mGroupText.Anchor")));
			this.mGroupText.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mGroupText.BackgroundImage")));
			this.mGroupText.Controls.Add(this.mButtonMsgSend);
			this.mGroupText.Controls.Add(this.mTextMsgSend);
			this.mGroupText.Controls.Add(this.mTextMsgInOut);
			this.mGroupText.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mGroupText.Dock")));
			this.mGroupText.Enabled = ((bool)(resources.GetObject("mGroupText.Enabled")));
			this.mGroupText.Font = ((System.Drawing.Font)(resources.GetObject("mGroupText.Font")));
			this.mGroupText.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mGroupText.ImeMode")));
			this.mGroupText.Location = ((System.Drawing.Point)(resources.GetObject("mGroupText.Location")));
			this.mGroupText.Name = "mGroupText";
			this.mGroupText.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mGroupText.RightToLeft")));
			this.mGroupText.Size = ((System.Drawing.Size)(resources.GetObject("mGroupText.Size")));
			this.mGroupText.TabIndex = ((int)(resources.GetObject("mGroupText.TabIndex")));
			this.mGroupText.TabStop = false;
			this.mGroupText.Text = resources.GetString("mGroupText.Text");
			this.mTooltips.SetToolTip(this.mGroupText, resources.GetString("mGroupText.ToolTip"));
			this.mGroupText.Visible = ((bool)(resources.GetObject("mGroupText.Visible")));
			// 
			// mButtonMsgSend
			// 
			this.mButtonMsgSend.AccessibleDescription = resources.GetString("mButtonMsgSend.AccessibleDescription");
			this.mButtonMsgSend.AccessibleName = resources.GetString("mButtonMsgSend.AccessibleName");
			this.mButtonMsgSend.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonMsgSend.Anchor")));
			this.mButtonMsgSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonMsgSend.BackgroundImage")));
			this.mButtonMsgSend.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonMsgSend.Dock")));
			this.mButtonMsgSend.Enabled = ((bool)(resources.GetObject("mButtonMsgSend.Enabled")));
			this.mButtonMsgSend.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonMsgSend.FlatStyle")));
			this.mButtonMsgSend.Font = ((System.Drawing.Font)(resources.GetObject("mButtonMsgSend.Font")));
			this.mButtonMsgSend.Image = ((System.Drawing.Image)(resources.GetObject("mButtonMsgSend.Image")));
			this.mButtonMsgSend.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonMsgSend.ImageAlign")));
			this.mButtonMsgSend.ImageIndex = ((int)(resources.GetObject("mButtonMsgSend.ImageIndex")));
			this.mButtonMsgSend.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonMsgSend.ImeMode")));
			this.mButtonMsgSend.Location = ((System.Drawing.Point)(resources.GetObject("mButtonMsgSend.Location")));
			this.mButtonMsgSend.Name = "mButtonMsgSend";
			this.mButtonMsgSend.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonMsgSend.RightToLeft")));
			this.mButtonMsgSend.Size = ((System.Drawing.Size)(resources.GetObject("mButtonMsgSend.Size")));
			this.mButtonMsgSend.TabIndex = ((int)(resources.GetObject("mButtonMsgSend.TabIndex")));
			this.mButtonMsgSend.Text = resources.GetString("mButtonMsgSend.Text");
			this.mButtonMsgSend.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonMsgSend.TextAlign")));
			this.mTooltips.SetToolTip(this.mButtonMsgSend, resources.GetString("mButtonMsgSend.ToolTip"));
			this.mButtonMsgSend.Visible = ((bool)(resources.GetObject("mButtonMsgSend.Visible")));
			this.mButtonMsgSend.Click += new System.EventHandler(this.mButtonMsgSend_Click);
			// 
			// mTextMsgSend
			// 
			this.mTextMsgSend.AccessibleDescription = resources.GetString("mTextMsgSend.AccessibleDescription");
			this.mTextMsgSend.AccessibleName = resources.GetString("mTextMsgSend.AccessibleName");
			this.mTextMsgSend.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mTextMsgSend.Anchor")));
			this.mTextMsgSend.AutoSize = ((bool)(resources.GetObject("mTextMsgSend.AutoSize")));
			this.mTextMsgSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mTextMsgSend.BackgroundImage")));
			this.mTextMsgSend.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mTextMsgSend.Dock")));
			this.mTextMsgSend.Enabled = ((bool)(resources.GetObject("mTextMsgSend.Enabled")));
			this.mTextMsgSend.Font = ((System.Drawing.Font)(resources.GetObject("mTextMsgSend.Font")));
			this.mTextMsgSend.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mTextMsgSend.ImeMode")));
			this.mTextMsgSend.Location = ((System.Drawing.Point)(resources.GetObject("mTextMsgSend.Location")));
			this.mTextMsgSend.MaxLength = ((int)(resources.GetObject("mTextMsgSend.MaxLength")));
			this.mTextMsgSend.Multiline = ((bool)(resources.GetObject("mTextMsgSend.Multiline")));
			this.mTextMsgSend.Name = "mTextMsgSend";
			this.mTextMsgSend.PasswordChar = ((char)(resources.GetObject("mTextMsgSend.PasswordChar")));
			this.mTextMsgSend.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mTextMsgSend.RightToLeft")));
			this.mTextMsgSend.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("mTextMsgSend.ScrollBars")));
			this.mTextMsgSend.Size = ((System.Drawing.Size)(resources.GetObject("mTextMsgSend.Size")));
			this.mTextMsgSend.TabIndex = ((int)(resources.GetObject("mTextMsgSend.TabIndex")));
			this.mTextMsgSend.Text = resources.GetString("mTextMsgSend.Text");
			this.mTextMsgSend.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("mTextMsgSend.TextAlign")));
			this.mTooltips.SetToolTip(this.mTextMsgSend, resources.GetString("mTextMsgSend.ToolTip"));
			this.mTextMsgSend.Visible = ((bool)(resources.GetObject("mTextMsgSend.Visible")));
			this.mTextMsgSend.WordWrap = ((bool)(resources.GetObject("mTextMsgSend.WordWrap")));
			// 
			// mTextMsgInOut
			// 
			this.mTextMsgInOut.AccessibleDescription = resources.GetString("mTextMsgInOut.AccessibleDescription");
			this.mTextMsgInOut.AccessibleName = resources.GetString("mTextMsgInOut.AccessibleName");
			this.mTextMsgInOut.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mTextMsgInOut.Anchor")));
			this.mTextMsgInOut.AutoSize = ((bool)(resources.GetObject("mTextMsgInOut.AutoSize")));
			this.mTextMsgInOut.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mTextMsgInOut.BackgroundImage")));
			this.mTextMsgInOut.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mTextMsgInOut.Dock")));
			this.mTextMsgInOut.Enabled = ((bool)(resources.GetObject("mTextMsgInOut.Enabled")));
			this.mTextMsgInOut.Font = ((System.Drawing.Font)(resources.GetObject("mTextMsgInOut.Font")));
			this.mTextMsgInOut.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mTextMsgInOut.ImeMode")));
			this.mTextMsgInOut.Location = ((System.Drawing.Point)(resources.GetObject("mTextMsgInOut.Location")));
			this.mTextMsgInOut.MaxLength = ((int)(resources.GetObject("mTextMsgInOut.MaxLength")));
			this.mTextMsgInOut.Multiline = ((bool)(resources.GetObject("mTextMsgInOut.Multiline")));
			this.mTextMsgInOut.Name = "mTextMsgInOut";
			this.mTextMsgInOut.PasswordChar = ((char)(resources.GetObject("mTextMsgInOut.PasswordChar")));
			this.mTextMsgInOut.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mTextMsgInOut.RightToLeft")));
			this.mTextMsgInOut.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("mTextMsgInOut.ScrollBars")));
			this.mTextMsgInOut.Size = ((System.Drawing.Size)(resources.GetObject("mTextMsgInOut.Size")));
			this.mTextMsgInOut.TabIndex = ((int)(resources.GetObject("mTextMsgInOut.TabIndex")));
			this.mTextMsgInOut.Text = resources.GetString("mTextMsgInOut.Text");
			this.mTextMsgInOut.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("mTextMsgInOut.TextAlign")));
			this.mTooltips.SetToolTip(this.mTextMsgInOut, resources.GetString("mTextMsgInOut.ToolTip"));
			this.mTextMsgInOut.Visible = ((bool)(resources.GetObject("mTextMsgInOut.Visible")));
			this.mTextMsgInOut.WordWrap = ((bool)(resources.GetObject("mTextMsgInOut.WordWrap")));
			// 
			// mTimerStat
			// 
			this.mTimerStat.Interval = 1000;
			this.mTimerStat.Tick += new System.EventHandler(this.mTimerStat_Tick);
			// 
			// mTimerVideo
			// 
			this.mTimerVideo.Tick += new System.EventHandler(this.mTimerVideo_Tick);
			// 
			// RMainForm
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.mGroupText);
			this.Controls.Add(this.mGroupControl);
			this.Controls.Add(this.mGroupStats);
			this.Controls.Add(this.mGroupRemote);
			this.Controls.Add(this.mGroupLocal);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.Menu = this.mMenuMain;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "RMainForm";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.mTooltips.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RMainForm_Closing);
			this.mGroupLocal.ResumeLayout(false);
			this.mGroupRemote.ResumeLayout(false);
			this.mGroupStats.ResumeLayout(false);
			this.mGroupControl.ResumeLayout(false);
			this.mGroupText.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region Windows Form variables

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		private System.Windows.Forms.MainMenu mMenuMain;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem mMenuItemConnect;
		private System.Windows.Forms.MenuItem mMenuItemDisconnect;
		private System.Windows.Forms.MenuItem mMenuItemQuit;
		private System.Windows.Forms.MenuItem mMenuItemUpdate;
		private System.Windows.Forms.MenuItem mMenuItemDebug;
		private System.Windows.Forms.MenuItem mMenuItemAbout;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.GroupBox mGroupLocal;
		private System.Windows.Forms.GroupBox mGroupRemote;
		private System.Windows.Forms.PictureBox mPixLocalCompressed;
		private System.Windows.Forms.ProgressBar mSoundLevelLocalOriginal;
		private System.Windows.Forms.ProgressBar mSoundLevelRemoteReceived;
		private System.Windows.Forms.PictureBox mPixRemoteReceived;
		private System.Windows.Forms.GroupBox mGroupStats;
		private System.Windows.Forms.Label mStatTextVideoSent;
		private System.Windows.Forms.Label mStatTextVideoReceived;
		private System.Windows.Forms.Label mStatTextAudioReceived;
		private System.Windows.Forms.Label mStatTextAudioSent;
		private System.Windows.Forms.Label mStatTextTotalReceived;
		private System.Windows.Forms.Label mStatTextTotalSent;
		private System.Windows.Forms.GroupBox mGroupControl;
		private System.Windows.Forms.CheckBox mControlCheckMaxKbps;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox mControlTextKbps;
		private System.Windows.Forms.Button mButtonStartStopComm;
		private System.Windows.Forms.Button mButtonStartStopVideo;
		private System.Windows.Forms.Button mButtonStartStopAudio;
		private System.Windows.Forms.ToolTip mTooltips;
		private System.Windows.Forms.TextBox mTextMsgInOut;
		private System.Windows.Forms.TextBox mTextMsgSend;
		private System.Windows.Forms.Button mButtonMsgSend;
		private System.Windows.Forms.MenuItem mMenuConnection;
		private System.Windows.Forms.MenuItem mMenuHelp;
		private System.Windows.Forms.MenuItem mMenuTools;
		private System.Windows.Forms.MenuItem mMenuItemPrefs;

		private System.Windows.Forms.Timer mTimerStat;
		private System.Windows.Forms.Timer mTimerVideo;
		private System.Windows.Forms.GroupBox mGroupText;

		#endregion

		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		// forms
		private RDebugForm		mDebugForm;
		private RPrefForm		mPrefForm;

		private RDeviceFactory	mDevFactory		= new RDeviceFactory();
		private RCommState		mLocalState		= new RCommState();
		private RCommState		mRemoteState	= new RCommState();
		private RStatCounter	mVideoSentStat	= new RStatCounter();
		private RFreqCounter	mLocalVideoFreq	= new RFreqCounter();



	} // class RMainForm
} // namespace Alfray.Xeres.XeresApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RMainForm.cs,v $
//	Revision 1.14  2005/10/30 03:06:41  ralf
//	Fix to support DV input
//	
//	Revision 1.13  2005/04/30 22:41:30  ralf
//	Rebuilding own VideoCaptureNet project
//	Using separate LibUtils & LibUtilsTests
//	
//	Revision 1.12  2005/03/28 00:25:07  ralf
//	Using new LocalVideo player and RFreqCounter
//	
//	Revision 1.11  2005/03/24 19:47:04  ralf
//	Audio/video availability depends on current devices selection
//	
//	Revision 1.10  2005/03/10 22:10:20  ralf
//	Quick test with JPEG encoding
//	
//	Revision 1.9  2005/03/07 17:00:44  ralf
//	Update. Fixes. Doc.
//	
//	Revision 1.8  2005/03/07 16:12:00  ralf
//	Fixes
//	
//	Revision 1.7  2005/03/07 15:40:41  ralf
//	Fixed loading window size to work with auto resize window.
//	Using hardcoded constants for prefs.
//	
//	Revision 1.6  2005/03/07 07:17:00  ralf
//	RRecorderVideo, implement start, stop
//	RRecorderVideo, capture images in data queue
//	Display recorded images from RRecorderVideo data queue
//	Updating preferences for video recorder device
//	
//	Revision 1.5  2005/03/07 01:50:05  ralf
//	New files: RIRecorder, RIPlayer, RRecorderVideo, RRecorderFactory, RPrefForm
//	
//	Revision 1.4  2005/02/21 06:22:36  ralf
//	Using XeresLib.RCommState
//	
//	Revision 1.3  2005/02/21 04:43:27  ralf
//	GPL headers and CVS footers
//	
//	Revision 1.2  2005/02/21 04:30:03  ralf
//	Added text messaging group
//	
//	Revision 1.1.1.1  2005/02/21 03:39:10  ralf
//	no message
//	
//	Revision 1.1  2005/02/18 23:21:52  ralf
//	Creating both an App and a Class Lib
//	
//	Revision 1.1.1.1  2005/02/18 22:54:53  ralf
//	A skeleton application template, with NUnit testing
//	
//---------------------------------------------------------------

