//*******************************************************************
/* 

 		Project:	XeresApp
 		File:		RPrefForm.cs

*/ 
//*******************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Alfray.Xeres.Device;

//*********************************
namespace Alfray.Xeres.XeresApp
{
	//**************************************
	/// <summary>
	/// Summary description for RPrefForm.
	/// </summary>
	public class RPrefForm : System.Windows.Forms.Form
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//******************
		public bool CanClose
		{
			get
			{
				return mCanClose;
			}
			set
			{
				mCanClose = value;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//****************
		public RPrefForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Inits

			init();
		}




		//*****************
		private void init()
		{
			mCanClose = false;

			// load all settings
			loadSettings();

			// init device lists

			initRecorder(mComboVideoDev,
						 RMainModule.Pref[RPrefConstants.kVideoRecorderId],
						 RMainModule.MainForm.DeviceFactory.RecorderVideo);

			// check UI

			mNeedApply = false;
			validateUi();
		}


		//********************************************
		private void initRecorder(ComboBox combo,
								  string pref_id,
								  RIRecorder recorder)
		{
			// previous selection...
			RDeviceInfo previous = null;
			if (combo.SelectedItem is RDeviceInfo)
				previous = combo.SelectedItem as RDeviceInfo;


			// reset list based on device enumeration
			// if there's a pref id, find it
			RDeviceInfo curr = null;
			combo.Items.Clear();
			foreach(RDeviceInfo info in recorder.EnumerateDevices())
			{
				combo.Items.Add(info);
				if (pref_id != null && info.UniqueId == pref_id)
					curr = info;
			}

			// select preference or default device

			if (curr == null)
				curr = recorder.CurrentDevice;

			combo.SelectedItem = curr;

			// memorize if the selection has changed

			if (!mNeedApply
				&& ((previous == null && curr != null)
				|| (previous != null && curr == null)
				|| (previous != null && curr != null && previous.UniqueId != curr.UniqueId)))
			{
				mNeedApply = true;
				validateUi();
			}
		}


		//**********************
		private void terminate()
		{
			// save settings
			saveSettings();
		}


		//*************************
		private void loadSettings()
		{
			// load position & size of this window
			Rectangle r;
			if (RMainModule.Pref.GetRect(RPrefConstants.kPrefForm, out r))
			{
				this.Bounds = r;
				// tell Windows not to change this position
				this.StartPosition = FormStartPosition.Manual;
			}
		}


		//*************************
		private void saveSettings()
		{
			// save position & size of this window
			RMainModule.Pref.SetRect(RPrefConstants.kPrefForm, this.Bounds);

			// save settings
			RMainModule.Pref.Save();
		}


		//***********************
		private void validateUi()
		{
			mButtonApply.Enabled = mNeedApply;
		}

		//******************
		private void apply()
		{
			// update prefs

			RDeviceInfo dev = null;
			if (mComboVideoDev.SelectedItem is RDeviceInfo)
				dev = mComboVideoDev.SelectedItem as RDeviceInfo;

			RMainModule.Pref[RPrefConstants.kVideoRecorderId] = (dev != null ? dev.UniqueId : null);

		
			// tell main form to reload prefs

			RMainModule.MainForm.ReloadPrefs();

			mNeedApply = false;
			validateUi();
		}


		//-------------------------------------------
		//----------- Private Callback ---------------
		//-------------------------------------------


		//****************************************************************
		private void RPrefForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!mCanClose)
			{
				// Simply hide it
				e.Cancel = true;
				this.Visible = false;
			}
			else
			{
				terminate();
			}		
		}

		//****************************************************************
		private void mButtonApply_Click(object sender, System.EventArgs e)
		{
			apply();
		}

		//****************************************************************
		private void mButtonOk_Click(object sender, System.EventArgs e)
		{
			apply();
			this.Close();
		}

		//****************************************************************
		private void mButtonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		
		//****************************************************************
		private void mComboVideoDev_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (!mNeedApply)
			{
				mNeedApply = true;
				validateUi();
			}
		}

		//****************************************************************
		private void mComboAudioDev_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (!mNeedApply)
			{
				mNeedApply = true;
				validateUi();
			}
		}

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------

		
		
		//-------------------------------------------
		//----------- Private Methods ---------------
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RPrefForm));
			this.mTabControl = new System.Windows.Forms.TabControl();
			this.mTabDevices = new System.Windows.Forms.TabPage();
			this.mComboAudioDev = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.mComboVideoDev = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.mButtonCancel = new System.Windows.Forms.Button();
			this.mButtonOk = new System.Windows.Forms.Button();
			this.mButtonApply = new System.Windows.Forms.Button();
			this.mTabControl.SuspendLayout();
			this.mTabDevices.SuspendLayout();
			this.SuspendLayout();
			// 
			// mTabControl
			// 
			this.mTabControl.AccessibleDescription = resources.GetString("mTabControl.AccessibleDescription");
			this.mTabControl.AccessibleName = resources.GetString("mTabControl.AccessibleName");
			this.mTabControl.Alignment = ((System.Windows.Forms.TabAlignment)(resources.GetObject("mTabControl.Alignment")));
			this.mTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mTabControl.Anchor")));
			this.mTabControl.Appearance = ((System.Windows.Forms.TabAppearance)(resources.GetObject("mTabControl.Appearance")));
			this.mTabControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mTabControl.BackgroundImage")));
			this.mTabControl.Controls.Add(this.mTabDevices);
			this.mTabControl.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mTabControl.Dock")));
			this.mTabControl.Enabled = ((bool)(resources.GetObject("mTabControl.Enabled")));
			this.mTabControl.Font = ((System.Drawing.Font)(resources.GetObject("mTabControl.Font")));
			this.mTabControl.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mTabControl.ImeMode")));
			this.mTabControl.ItemSize = ((System.Drawing.Size)(resources.GetObject("mTabControl.ItemSize")));
			this.mTabControl.Location = ((System.Drawing.Point)(resources.GetObject("mTabControl.Location")));
			this.mTabControl.Name = "mTabControl";
			this.mTabControl.Padding = ((System.Drawing.Point)(resources.GetObject("mTabControl.Padding")));
			this.mTabControl.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mTabControl.RightToLeft")));
			this.mTabControl.SelectedIndex = 0;
			this.mTabControl.ShowToolTips = ((bool)(resources.GetObject("mTabControl.ShowToolTips")));
			this.mTabControl.Size = ((System.Drawing.Size)(resources.GetObject("mTabControl.Size")));
			this.mTabControl.TabIndex = ((int)(resources.GetObject("mTabControl.TabIndex")));
			this.mTabControl.Text = resources.GetString("mTabControl.Text");
			this.mTabControl.Visible = ((bool)(resources.GetObject("mTabControl.Visible")));
			// 
			// mTabDevices
			// 
			this.mTabDevices.AccessibleDescription = resources.GetString("mTabDevices.AccessibleDescription");
			this.mTabDevices.AccessibleName = resources.GetString("mTabDevices.AccessibleName");
			this.mTabDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mTabDevices.Anchor")));
			this.mTabDevices.AutoScroll = ((bool)(resources.GetObject("mTabDevices.AutoScroll")));
			this.mTabDevices.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("mTabDevices.AutoScrollMargin")));
			this.mTabDevices.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("mTabDevices.AutoScrollMinSize")));
			this.mTabDevices.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mTabDevices.BackgroundImage")));
			this.mTabDevices.Controls.Add(this.mComboAudioDev);
			this.mTabDevices.Controls.Add(this.label2);
			this.mTabDevices.Controls.Add(this.mComboVideoDev);
			this.mTabDevices.Controls.Add(this.label1);
			this.mTabDevices.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mTabDevices.Dock")));
			this.mTabDevices.Enabled = ((bool)(resources.GetObject("mTabDevices.Enabled")));
			this.mTabDevices.Font = ((System.Drawing.Font)(resources.GetObject("mTabDevices.Font")));
			this.mTabDevices.ImageIndex = ((int)(resources.GetObject("mTabDevices.ImageIndex")));
			this.mTabDevices.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mTabDevices.ImeMode")));
			this.mTabDevices.Location = ((System.Drawing.Point)(resources.GetObject("mTabDevices.Location")));
			this.mTabDevices.Name = "mTabDevices";
			this.mTabDevices.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mTabDevices.RightToLeft")));
			this.mTabDevices.Size = ((System.Drawing.Size)(resources.GetObject("mTabDevices.Size")));
			this.mTabDevices.TabIndex = ((int)(resources.GetObject("mTabDevices.TabIndex")));
			this.mTabDevices.Text = resources.GetString("mTabDevices.Text");
			this.mTabDevices.ToolTipText = resources.GetString("mTabDevices.ToolTipText");
			this.mTabDevices.Visible = ((bool)(resources.GetObject("mTabDevices.Visible")));
			// 
			// mComboAudioDev
			// 
			this.mComboAudioDev.AccessibleDescription = resources.GetString("mComboAudioDev.AccessibleDescription");
			this.mComboAudioDev.AccessibleName = resources.GetString("mComboAudioDev.AccessibleName");
			this.mComboAudioDev.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mComboAudioDev.Anchor")));
			this.mComboAudioDev.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mComboAudioDev.BackgroundImage")));
			this.mComboAudioDev.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mComboAudioDev.Dock")));
			this.mComboAudioDev.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.mComboAudioDev.Enabled = ((bool)(resources.GetObject("mComboAudioDev.Enabled")));
			this.mComboAudioDev.Font = ((System.Drawing.Font)(resources.GetObject("mComboAudioDev.Font")));
			this.mComboAudioDev.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mComboAudioDev.ImeMode")));
			this.mComboAudioDev.IntegralHeight = ((bool)(resources.GetObject("mComboAudioDev.IntegralHeight")));
			this.mComboAudioDev.ItemHeight = ((int)(resources.GetObject("mComboAudioDev.ItemHeight")));
			this.mComboAudioDev.Location = ((System.Drawing.Point)(resources.GetObject("mComboAudioDev.Location")));
			this.mComboAudioDev.MaxDropDownItems = ((int)(resources.GetObject("mComboAudioDev.MaxDropDownItems")));
			this.mComboAudioDev.MaxLength = ((int)(resources.GetObject("mComboAudioDev.MaxLength")));
			this.mComboAudioDev.Name = "mComboAudioDev";
			this.mComboAudioDev.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mComboAudioDev.RightToLeft")));
			this.mComboAudioDev.Size = ((System.Drawing.Size)(resources.GetObject("mComboAudioDev.Size")));
			this.mComboAudioDev.TabIndex = ((int)(resources.GetObject("mComboAudioDev.TabIndex")));
			this.mComboAudioDev.Text = resources.GetString("mComboAudioDev.Text");
			this.mComboAudioDev.Visible = ((bool)(resources.GetObject("mComboAudioDev.Visible")));
			this.mComboAudioDev.SelectedIndexChanged += new System.EventHandler(this.mComboAudioDev_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
			this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
			this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
			this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
			this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
			this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
			this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
			this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
			this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
			this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
			this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
			this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
			this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
			this.label2.Text = resources.GetString("label2.Text");
			this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
			this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
			// 
			// mComboVideoDev
			// 
			this.mComboVideoDev.AccessibleDescription = resources.GetString("mComboVideoDev.AccessibleDescription");
			this.mComboVideoDev.AccessibleName = resources.GetString("mComboVideoDev.AccessibleName");
			this.mComboVideoDev.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mComboVideoDev.Anchor")));
			this.mComboVideoDev.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mComboVideoDev.BackgroundImage")));
			this.mComboVideoDev.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mComboVideoDev.Dock")));
			this.mComboVideoDev.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.mComboVideoDev.Enabled = ((bool)(resources.GetObject("mComboVideoDev.Enabled")));
			this.mComboVideoDev.Font = ((System.Drawing.Font)(resources.GetObject("mComboVideoDev.Font")));
			this.mComboVideoDev.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mComboVideoDev.ImeMode")));
			this.mComboVideoDev.IntegralHeight = ((bool)(resources.GetObject("mComboVideoDev.IntegralHeight")));
			this.mComboVideoDev.ItemHeight = ((int)(resources.GetObject("mComboVideoDev.ItemHeight")));
			this.mComboVideoDev.Location = ((System.Drawing.Point)(resources.GetObject("mComboVideoDev.Location")));
			this.mComboVideoDev.MaxDropDownItems = ((int)(resources.GetObject("mComboVideoDev.MaxDropDownItems")));
			this.mComboVideoDev.MaxLength = ((int)(resources.GetObject("mComboVideoDev.MaxLength")));
			this.mComboVideoDev.Name = "mComboVideoDev";
			this.mComboVideoDev.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mComboVideoDev.RightToLeft")));
			this.mComboVideoDev.Size = ((System.Drawing.Size)(resources.GetObject("mComboVideoDev.Size")));
			this.mComboVideoDev.TabIndex = ((int)(resources.GetObject("mComboVideoDev.TabIndex")));
			this.mComboVideoDev.Text = resources.GetString("mComboVideoDev.Text");
			this.mComboVideoDev.Visible = ((bool)(resources.GetObject("mComboVideoDev.Visible")));
			this.mComboVideoDev.SelectedIndexChanged += new System.EventHandler(this.mComboVideoDev_SelectedIndexChanged);
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
			this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
			// 
			// mButtonCancel
			// 
			this.mButtonCancel.AccessibleDescription = resources.GetString("mButtonCancel.AccessibleDescription");
			this.mButtonCancel.AccessibleName = resources.GetString("mButtonCancel.AccessibleName");
			this.mButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonCancel.Anchor")));
			this.mButtonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonCancel.BackgroundImage")));
			this.mButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.mButtonCancel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonCancel.Dock")));
			this.mButtonCancel.Enabled = ((bool)(resources.GetObject("mButtonCancel.Enabled")));
			this.mButtonCancel.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonCancel.FlatStyle")));
			this.mButtonCancel.Font = ((System.Drawing.Font)(resources.GetObject("mButtonCancel.Font")));
			this.mButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("mButtonCancel.Image")));
			this.mButtonCancel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonCancel.ImageAlign")));
			this.mButtonCancel.ImageIndex = ((int)(resources.GetObject("mButtonCancel.ImageIndex")));
			this.mButtonCancel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonCancel.ImeMode")));
			this.mButtonCancel.Location = ((System.Drawing.Point)(resources.GetObject("mButtonCancel.Location")));
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonCancel.RightToLeft")));
			this.mButtonCancel.Size = ((System.Drawing.Size)(resources.GetObject("mButtonCancel.Size")));
			this.mButtonCancel.TabIndex = ((int)(resources.GetObject("mButtonCancel.TabIndex")));
			this.mButtonCancel.Text = resources.GetString("mButtonCancel.Text");
			this.mButtonCancel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonCancel.TextAlign")));
			this.mButtonCancel.Visible = ((bool)(resources.GetObject("mButtonCancel.Visible")));
			this.mButtonCancel.Click += new System.EventHandler(this.mButtonCancel_Click);
			// 
			// mButtonOk
			// 
			this.mButtonOk.AccessibleDescription = resources.GetString("mButtonOk.AccessibleDescription");
			this.mButtonOk.AccessibleName = resources.GetString("mButtonOk.AccessibleName");
			this.mButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonOk.Anchor")));
			this.mButtonOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonOk.BackgroundImage")));
			this.mButtonOk.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonOk.Dock")));
			this.mButtonOk.Enabled = ((bool)(resources.GetObject("mButtonOk.Enabled")));
			this.mButtonOk.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonOk.FlatStyle")));
			this.mButtonOk.Font = ((System.Drawing.Font)(resources.GetObject("mButtonOk.Font")));
			this.mButtonOk.Image = ((System.Drawing.Image)(resources.GetObject("mButtonOk.Image")));
			this.mButtonOk.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonOk.ImageAlign")));
			this.mButtonOk.ImageIndex = ((int)(resources.GetObject("mButtonOk.ImageIndex")));
			this.mButtonOk.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonOk.ImeMode")));
			this.mButtonOk.Location = ((System.Drawing.Point)(resources.GetObject("mButtonOk.Location")));
			this.mButtonOk.Name = "mButtonOk";
			this.mButtonOk.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonOk.RightToLeft")));
			this.mButtonOk.Size = ((System.Drawing.Size)(resources.GetObject("mButtonOk.Size")));
			this.mButtonOk.TabIndex = ((int)(resources.GetObject("mButtonOk.TabIndex")));
			this.mButtonOk.Text = resources.GetString("mButtonOk.Text");
			this.mButtonOk.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonOk.TextAlign")));
			this.mButtonOk.Visible = ((bool)(resources.GetObject("mButtonOk.Visible")));
			this.mButtonOk.Click += new System.EventHandler(this.mButtonOk_Click);
			// 
			// mButtonApply
			// 
			this.mButtonApply.AccessibleDescription = resources.GetString("mButtonApply.AccessibleDescription");
			this.mButtonApply.AccessibleName = resources.GetString("mButtonApply.AccessibleName");
			this.mButtonApply.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonApply.Anchor")));
			this.mButtonApply.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonApply.BackgroundImage")));
			this.mButtonApply.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonApply.Dock")));
			this.mButtonApply.Enabled = ((bool)(resources.GetObject("mButtonApply.Enabled")));
			this.mButtonApply.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonApply.FlatStyle")));
			this.mButtonApply.Font = ((System.Drawing.Font)(resources.GetObject("mButtonApply.Font")));
			this.mButtonApply.Image = ((System.Drawing.Image)(resources.GetObject("mButtonApply.Image")));
			this.mButtonApply.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonApply.ImageAlign")));
			this.mButtonApply.ImageIndex = ((int)(resources.GetObject("mButtonApply.ImageIndex")));
			this.mButtonApply.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonApply.ImeMode")));
			this.mButtonApply.Location = ((System.Drawing.Point)(resources.GetObject("mButtonApply.Location")));
			this.mButtonApply.Name = "mButtonApply";
			this.mButtonApply.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonApply.RightToLeft")));
			this.mButtonApply.Size = ((System.Drawing.Size)(resources.GetObject("mButtonApply.Size")));
			this.mButtonApply.TabIndex = ((int)(resources.GetObject("mButtonApply.TabIndex")));
			this.mButtonApply.Text = resources.GetString("mButtonApply.Text");
			this.mButtonApply.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonApply.TextAlign")));
			this.mButtonApply.Visible = ((bool)(resources.GetObject("mButtonApply.Visible")));
			this.mButtonApply.Click += new System.EventHandler(this.mButtonApply_Click);
			// 
			// RPrefForm
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.mButtonApply);
			this.Controls.Add(this.mButtonOk);
			this.Controls.Add(this.mButtonCancel);
			this.Controls.Add(this.mTabControl);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "RPrefForm";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RPrefForm_Closing);
			this.mTabControl.ResumeLayout(false);
			this.mTabDevices.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mTabControl;
		private System.Windows.Forms.TabPage mTabDevices;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox mComboVideoDev;
		private System.Windows.Forms.ComboBox mComboAudioDev;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button mButtonCancel;
		private System.Windows.Forms.Button mButtonOk;
		private System.Windows.Forms.Button mButtonApply;


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		//*****************************
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		//*****************************


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private bool	mCanClose;
		private bool	mNeedApply;

	} // class RPrefForm
} // namespace Alfray.Xeres.XeresApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RPrefForm.cs,v $
//	Revision 1.4  2005/10/30 03:06:41  ralf
//	Fix to support DV input
//	
//	Revision 1.3  2005/03/07 15:40:49  ralf
//	Using hardcoded constants for prefs.
//	
//	Revision 1.2  2005/03/07 07:17:00  ralf
//	RRecorderVideo, implement start, stop
//	RRecorderVideo, capture images in data queue
//	Display recorded images from RRecorderVideo data queue
//	Updating preferences for video recorder device
//	
//	Revision 1.1  2005/03/07 01:50:05  ralf
//	New files: RIRecorder, RIPlayer, RRecorderVideo, RRecorderFactory, RPrefForm
//	
//---------------------------------------------------------------

