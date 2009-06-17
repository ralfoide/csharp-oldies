//*******************************************************************
/*
 *	Project:	Alfray.ComicsSharp
 * 
 *	File:		ComicsEditForm.cs
 * 
 *	RM (c) 2003
 * 
 */
//*******************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

//**************************
namespace Alfray.ComicsSharp
{
	//*****************************************************
	public class ComicsEditForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox mTextName;
		private System.Windows.Forms.TextBox mTextImageUrl;
		private System.Windows.Forms.TextBox mTextLastIndex;
		private System.Windows.Forms.TextBox mTextBaseDir;
		private System.Windows.Forms.RadioButton mRadioTypeIndex;
		private System.Windows.Forms.RadioButton mRadioTypeDate;
		private System.Windows.Forms.DateTimePicker mLastTimePicker;
		private System.Windows.Forms.Button mButtonOK;
		private System.Windows.Forms.Button mButtonCancel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox mTextMaxIndex;
		private System.Windows.Forms.ToolTip mToolTip;
		private System.ComponentModel.IContainer components;

		// ------- public properties ------

		//********************
		public ComicsBook Book
		{
			get
			{
				saveValues();
				return this.mBook;
			}
			set
			{
				if (value == null)
					this.mBook = new ComicsBook();
				else
					this.mBook = value;
				loadValues();
			}
		}

		// ------- public methods --------

		//************************************
		public ComicsEditForm(ComicsBook book)
		{
			// Required for Windows Form Designer support
			InitializeComponent();


			// Init tooltips

			mToolTip.SetToolTip(mTextImageUrl, "Image URL Pattern\n"
				+ "Placeholders format: { <code> [: format] }\n"
				+ "Code: id | date | ext\n"
				+ "Format: .Net numeric format (ex: 0000, default is 'G') or date (default is 'yyyyMMdd')\n"
				+ "For 'ext', format is a comma-separated list of extension (default is 'gif,jpg,png')");



			// Init
			mTextBaseDir.Enabled = false;

			if (book == null)
				this.mBook = new ComicsBook();
			else
				this.mBook = book;

			loadValues();
		}

		//*************************************
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.mTextName = new System.Windows.Forms.TextBox();
			this.mTextImageUrl = new System.Windows.Forms.TextBox();
			this.mTextLastIndex = new System.Windows.Forms.TextBox();
			this.mTextBaseDir = new System.Windows.Forms.TextBox();
			this.mRadioTypeIndex = new System.Windows.Forms.RadioButton();
			this.mRadioTypeDate = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.mLastTimePicker = new System.Windows.Forms.DateTimePicker();
			this.mButtonOK = new System.Windows.Forms.Button();
			this.mButtonCancel = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.mTextMaxIndex = new System.Windows.Forms.TextBox();
			this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 24);
			this.label2.TabIndex = 1;
			this.label2.Text = "Image URL:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 152);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 24);
			this.label3.TabIndex = 2;
			this.label3.Text = "Storage Dir:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 24);
			this.label4.TabIndex = 3;
			this.label4.Text = "Last:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mTextName
			// 
			this.mTextName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mTextName.Location = new System.Drawing.Point(88, 8);
			this.mTextName.Name = "mTextName";
			this.mTextName.Size = new System.Drawing.Size(304, 20);
			this.mTextName.TabIndex = 4;
			this.mTextName.Text = "";
			// 
			// mTextImageUrl
			// 
			this.mTextImageUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mTextImageUrl.Location = new System.Drawing.Point(88, 32);
			this.mTextImageUrl.Name = "mTextImageUrl";
			this.mTextImageUrl.Size = new System.Drawing.Size(304, 20);
			this.mTextImageUrl.TabIndex = 5;
			this.mTextImageUrl.Text = "";
			// 
			// mTextLastIndex
			// 
			this.mTextLastIndex.Location = new System.Drawing.Point(88, 88);
			this.mTextLastIndex.Name = "mTextLastIndex";
			this.mTextLastIndex.Size = new System.Drawing.Size(80, 20);
			this.mTextLastIndex.TabIndex = 6;
			this.mTextLastIndex.Text = "";
			// 
			// mTextBaseDir
			// 
			this.mTextBaseDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mTextBaseDir.Location = new System.Drawing.Point(88, 152);
			this.mTextBaseDir.Name = "mTextBaseDir";
			this.mTextBaseDir.Size = new System.Drawing.Size(304, 20);
			this.mTextBaseDir.TabIndex = 7;
			this.mTextBaseDir.Text = "";
			// 
			// mRadioTypeIndex
			// 
			this.mRadioTypeIndex.Location = new System.Drawing.Point(88, 64);
			this.mRadioTypeIndex.Name = "mRadioTypeIndex";
			this.mRadioTypeIndex.Size = new System.Drawing.Size(104, 16);
			this.mRadioTypeIndex.TabIndex = 8;
			this.mRadioTypeIndex.Text = "Index based";
			this.mRadioTypeIndex.CheckedChanged += new System.EventHandler(this.mRadioTypeIndex_CheckedChanged);
			// 
			// mRadioTypeDate
			// 
			this.mRadioTypeDate.Location = new System.Drawing.Point(192, 64);
			this.mRadioTypeDate.Name = "mRadioTypeDate";
			this.mRadioTypeDate.Size = new System.Drawing.Size(104, 16);
			this.mRadioTypeDate.TabIndex = 9;
			this.mRadioTypeDate.Text = "Date based";
			this.mRadioTypeDate.CheckedChanged += new System.EventHandler(this.mRadioTypeDate_CheckedChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 64);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 23);
			this.label5.TabIndex = 10;
			this.label5.Text = "Type:";
			// 
			// mLastTimePicker
			// 
			this.mLastTimePicker.CustomFormat = "";
			this.mLastTimePicker.Location = new System.Drawing.Point(192, 88);
			this.mLastTimePicker.Name = "mLastTimePicker";
			this.mLastTimePicker.TabIndex = 11;
			// 
			// mButtonOK
			// 
			this.mButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.mButtonOK.Location = new System.Drawing.Point(232, 184);
			this.mButtonOK.Name = "mButtonOK";
			this.mButtonOK.TabIndex = 12;
			this.mButtonOK.Text = "OK";
			this.mButtonOK.Click += new System.EventHandler(this.mButtonOK_Click);
			// 
			// mButtonCancel
			// 
			this.mButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.mButtonCancel.Location = new System.Drawing.Point(320, 184);
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.TabIndex = 13;
			this.mButtonCancel.Text = "Cancel";
			this.mButtonCancel.Click += new System.EventHandler(this.mButtonCancel_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 112);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 24);
			this.label6.TabIndex = 3;
			this.label6.Text = "Max:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mTextMaxIndex
			// 
			this.mTextMaxIndex.Location = new System.Drawing.Point(88, 120);
			this.mTextMaxIndex.Name = "mTextMaxIndex";
			this.mTextMaxIndex.Size = new System.Drawing.Size(80, 20);
			this.mTextMaxIndex.TabIndex = 6;
			this.mTextMaxIndex.Text = "";
			// 
			// mToolTip
			// 
			this.mToolTip.AutoPopDelay = 20000;
			this.mToolTip.InitialDelay = 500;
			this.mToolTip.ReshowDelay = 100;
			// 
			// ComicsEditForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 214);
			this.Controls.Add(this.mButtonCancel);
			this.Controls.Add(this.mButtonOK);
			this.Controls.Add(this.mLastTimePicker);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.mRadioTypeDate);
			this.Controls.Add(this.mRadioTypeIndex);
			this.Controls.Add(this.mTextBaseDir);
			this.Controls.Add(this.mTextLastIndex);
			this.Controls.Add(this.mTextImageUrl);
			this.Controls.Add(this.mTextName);
			this.Controls.Add(this.mTextMaxIndex);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label6);
			this.MaximumSize = new System.Drawing.Size(1024, 248);
			this.MinimumSize = new System.Drawing.Size(408, 0);
			this.Name = "ComicsEditForm";
			this.Text = "ComicsSharp - Edit";
			this.ResumeLayout(false);

		}
		#endregion

		// ------- private properties ------- 

		private ComicsBook mBook;


		// ------- private methods ------- 

		//***********************
		private bool saveValues()
		{
			mBook.Name      = mTextName.Text;
			mBook.ImageUrl  = mTextImageUrl.Text;

			if (mTextBaseDir.Enabled)
				mBook.BaseDir   = mTextBaseDir.Text;

			if (mRadioTypeDate.Checked)
			{
				mBook.IndexType = ComicsBook.EIndexTypeEnum.kIndexDate;
				mBook.LastIndex = mLastTimePicker.Value.Ticks;
			}
			else
			{
				mBook.IndexType = ComicsBook.EIndexTypeEnum.kIndexId;
				mBook.LastIndex = System.Convert.ToInt64(mTextLastIndex.Text);
				mBook.MaxIndex = System.Convert.ToInt64(mTextMaxIndex.Text);
			}

			return true;
		}

		//***********************
		private bool loadValues()
		{
			mTextName.Text      = mBook.Name;
			mTextImageUrl.Text  = mBook.ImageUrl;
			mTextLastIndex.Text = mBook.LastIndex.ToString();
			mTextMaxIndex.Text	= mBook.MaxIndex.ToString();
			mTextBaseDir.Text   = mBook.BaseDir;

			if (mBook.UsesDate)
			{
				mRadioTypeIndex.Checked = false;
				mRadioTypeDate.Checked  = true;

				if (mBook.LastIndex <= 0)
					mLastTimePicker.Value = DateTime.Now;
				else
					mLastTimePicker.Value = new DateTime(mBook.LastIndex);
			}
			else
			{
				mRadioTypeDate.Checked  = false;
				mRadioTypeIndex.Checked = true;
			}

			return true;
		}

		// ------- private UI callbacks ------- 

		//****************************************************************************
		private void mRadioTypeIndex_CheckedChanged(object sender, System.EventArgs e)
		{
			mTextLastIndex.Enabled  = true;
			mTextMaxIndex.Enabled   = true;
			mLastTimePicker.Enabled = false;
		}

		//****************************************************************************
		private void mRadioTypeDate_CheckedChanged(object sender, System.EventArgs e)
		{
			mTextLastIndex.Enabled  = false;
			mTextMaxIndex.Enabled   = false;
			mLastTimePicker.Enabled = true;		
		}

		//*************************************************************
		private void mButtonOK_Click(object sender, System.EventArgs e)
		{
			if (saveValues())
				this.Close();
		}

		//*****************************************************************
		private void mButtonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	} // class ComicsEditForm
} // namespace Alfray.ComicsSharp


//*******************************************************************
/*
 *	$Log: ComicsEditForm.cs,v $
 *	Revision 1.2  2003/08/04 21:37:27  ralf
 *	Upload with abort/feedback, active book, etc.
 *	
 *	Revision 1.1  2003/08/01 00:51:58  ralf
 *	Added ComicsBook, ComicsBookCollection, Edit form and main form.
 *	
 * 
 */
//*******************************************************************

