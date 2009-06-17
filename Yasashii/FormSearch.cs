//*******************************************************************
/* 

 		Project:	Yasashii
 		File:		FormSearch.cs

*/ 
//*******************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

//*************************************
namespace Alfray.Yasashii
{
	//*************************************
	/// <summary>
	/// Summary description for FormSearch.
	/// </summary>
	//*************************************
	public class FormSearch : System.Windows.Forms.Form
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


		public FormSearch()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormSearch));
			this.mGroupSearch = new System.Windows.Forms.GroupBox();
			this.mButtonAbout = new System.Windows.Forms.Button();
			this.mComboSearch = new System.Windows.Forms.TextBox();
			this.mCheckStartOnly = new System.Windows.Forms.CheckBox();
			this.mRadioKana = new System.Windows.Forms.RadioButton();
			this.mButtonSearch = new System.Windows.Forms.Button();
			this.mRadioFrench = new System.Windows.Forms.RadioButton();
			this.mRadioEnglish = new System.Windows.Forms.RadioButton();
			this.mRadioRomaji = new System.Windows.Forms.RadioButton();
			this.mCheckWholeWord = new System.Windows.Forms.CheckBox();
			this.mCheckMatchCase = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.mListDef = new System.Windows.Forms.ListBox();
			this.mLabelRomaji = new System.Windows.Forms.Label();
			this.mLabelKana = new System.Windows.Forms.Label();
			this.mLabelKanji = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.mListWord = new System.Windows.Forms.ListView();
			this.mResultColumnKanji = new System.Windows.Forms.ColumnHeader();
			this.mResultColumnReading = new System.Windows.Forms.ColumnHeader();
			this.mResultColumnEnglish = new System.Windows.Forms.ColumnHeader();
			this.mResultColumnFrench = new System.Windows.Forms.ColumnHeader();
			this.mStatusBar = new System.Windows.Forms.StatusBar();
			this.mGroupSearch.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// mGroupSearch
			// 
			this.mGroupSearch.Controls.Add(this.mButtonAbout);
			this.mGroupSearch.Controls.Add(this.mComboSearch);
			this.mGroupSearch.Controls.Add(this.mCheckStartOnly);
			this.mGroupSearch.Controls.Add(this.mRadioKana);
			this.mGroupSearch.Controls.Add(this.mButtonSearch);
			this.mGroupSearch.Controls.Add(this.mRadioFrench);
			this.mGroupSearch.Controls.Add(this.mRadioEnglish);
			this.mGroupSearch.Controls.Add(this.mRadioRomaji);
			this.mGroupSearch.Controls.Add(this.mCheckWholeWord);
			this.mGroupSearch.Controls.Add(this.mCheckMatchCase);
			this.mGroupSearch.Location = new System.Drawing.Point(8, 8);
			this.mGroupSearch.Name = "mGroupSearch";
			this.mGroupSearch.Size = new System.Drawing.Size(208, 192);
			this.mGroupSearch.TabIndex = 0;
			this.mGroupSearch.TabStop = false;
			this.mGroupSearch.Text = "Search";
			// 
			// mButtonAbout
			// 
			this.mButtonAbout.Location = new System.Drawing.Point(112, 96);
			this.mButtonAbout.Name = "mButtonAbout";
			this.mButtonAbout.Size = new System.Drawing.Size(88, 23);
			this.mButtonAbout.TabIndex = 10;
			this.mButtonAbout.Text = "About...";
			this.mButtonAbout.Click += new System.EventHandler(this.mButtonAbout_Click);
			// 
			// mComboSearch
			// 
			this.mComboSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mComboSearch.Location = new System.Drawing.Point(8, 24);
			this.mComboSearch.Name = "mComboSearch";
			this.mComboSearch.Size = new System.Drawing.Size(192, 26);
			this.mComboSearch.TabIndex = 9;
			this.mComboSearch.Text = "";
			this.mComboSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mComboSearch_KeyPress);
			// 
			// mCheckStartOnly
			// 
			this.mCheckStartOnly.Location = new System.Drawing.Point(8, 112);
			this.mCheckStartOnly.Name = "mCheckStartOnly";
			this.mCheckStartOnly.TabIndex = 8;
			this.mCheckStartOnly.Text = "S&tart only";
			// 
			// mRadioKana
			// 
			this.mRadioKana.Location = new System.Drawing.Point(96, 136);
			this.mRadioKana.Name = "mRadioKana";
			this.mRadioKana.TabIndex = 7;
			this.mRadioKana.Text = "&Kana reading";
			this.mRadioKana.CheckedChanged += new System.EventHandler(this.mRadioKana_CheckedChanged);
			// 
			// mButtonSearch
			// 
			this.mButtonSearch.Location = new System.Drawing.Point(112, 64);
			this.mButtonSearch.Name = "mButtonSearch";
			this.mButtonSearch.Size = new System.Drawing.Size(88, 23);
			this.mButtonSearch.TabIndex = 6;
			this.mButtonSearch.Text = "&Search";
			this.mButtonSearch.Click += new System.EventHandler(this.mButtonSearch_Click);
			// 
			// mRadioFrench
			// 
			this.mRadioFrench.Location = new System.Drawing.Point(8, 160);
			this.mRadioFrench.Name = "mRadioFrench";
			this.mRadioFrench.Size = new System.Drawing.Size(80, 24);
			this.mRadioFrench.TabIndex = 5;
			this.mRadioFrench.Text = "&French";
			this.mRadioFrench.CheckedChanged += new System.EventHandler(this.mRadioFrench_CheckedChanged);
			// 
			// mRadioEnglish
			// 
			this.mRadioEnglish.Checked = true;
			this.mRadioEnglish.Location = new System.Drawing.Point(8, 136);
			this.mRadioEnglish.Name = "mRadioEnglish";
			this.mRadioEnglish.Size = new System.Drawing.Size(80, 24);
			this.mRadioEnglish.TabIndex = 4;
			this.mRadioEnglish.TabStop = true;
			this.mRadioEnglish.Text = "&English";
			this.mRadioEnglish.CheckedChanged += new System.EventHandler(this.mRadioEnglish_CheckedChanged);
			// 
			// mRadioRomaji
			// 
			this.mRadioRomaji.Enabled = false;
			this.mRadioRomaji.Location = new System.Drawing.Point(96, 160);
			this.mRadioRomaji.Name = "mRadioRomaji";
			this.mRadioRomaji.TabIndex = 3;
			this.mRadioRomaji.Text = "&Romaji";
			this.mRadioRomaji.CheckedChanged += new System.EventHandler(this.mRadioRomaji_CheckedChanged);
			// 
			// mCheckWholeWord
			// 
			this.mCheckWholeWord.Location = new System.Drawing.Point(8, 88);
			this.mCheckWholeWord.Name = "mCheckWholeWord";
			this.mCheckWholeWord.TabIndex = 2;
			this.mCheckWholeWord.Text = "&Whole word";
			// 
			// mCheckMatchCase
			// 
			this.mCheckMatchCase.Checked = true;
			this.mCheckMatchCase.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mCheckMatchCase.Location = new System.Drawing.Point(8, 64);
			this.mCheckMatchCase.Name = "mCheckMatchCase";
			this.mCheckMatchCase.TabIndex = 1;
			this.mCheckMatchCase.Text = "&Match case";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.mListDef);
			this.groupBox1.Controls.Add(this.mLabelRomaji);
			this.groupBox1.Controls.Add(this.mLabelKana);
			this.groupBox1.Controls.Add(this.mLabelKanji);
			this.groupBox1.Location = new System.Drawing.Point(224, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(448, 192);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Definition";
			// 
			// mListDef
			// 
			this.mListDef.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mListDef.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mListDef.IntegralHeight = false;
			this.mListDef.ItemHeight = 20;
			this.mListDef.Location = new System.Drawing.Point(8, 56);
			this.mListDef.Name = "mListDef";
			this.mListDef.ScrollAlwaysVisible = true;
			this.mListDef.Size = new System.Drawing.Size(432, 128);
			this.mListDef.TabIndex = 4;
			// 
			// mLabelRomaji
			// 
			this.mLabelRomaji.AccessibleDescription = "Romaji reading";
			this.mLabelRomaji.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mLabelRomaji.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mLabelRomaji.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mLabelRomaji.Location = new System.Drawing.Point(320, 16);
			this.mLabelRomaji.Name = "mLabelRomaji";
			this.mLabelRomaji.Size = new System.Drawing.Size(120, 32);
			this.mLabelRomaji.TabIndex = 3;
			this.mLabelRomaji.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// mLabelKana
			// 
			this.mLabelKana.AccessibleDescription = "Kana reading";
			this.mLabelKana.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mLabelKana.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mLabelKana.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mLabelKana.Location = new System.Drawing.Point(168, 16);
			this.mLabelKana.Name = "mLabelKana";
			this.mLabelKana.Size = new System.Drawing.Size(144, 32);
			this.mLabelKana.TabIndex = 2;
			this.mLabelKana.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// mLabelKanji
			// 
			this.mLabelKanji.AccessibleDescription = "Kanji writting";
			this.mLabelKanji.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mLabelKanji.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mLabelKanji.Location = new System.Drawing.Point(8, 16);
			this.mLabelKanji.Name = "mLabelKanji";
			this.mLabelKanji.Size = new System.Drawing.Size(152, 32);
			this.mLabelKanji.TabIndex = 1;
			this.mLabelKanji.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.mListWord);
			this.groupBox2.Location = new System.Drawing.Point(8, 208);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(664, 176);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Results";
			// 
			// mListWord
			// 
			this.mListWord.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.mListWord.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mListWord.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.mResultColumnKanji,
																						this.mResultColumnReading,
																						this.mResultColumnEnglish,
																						this.mResultColumnFrench});
			this.mListWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mListWord.FullRowSelect = true;
			this.mListWord.GridLines = true;
			this.mListWord.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.mListWord.HideSelection = false;
			this.mListWord.HoverSelection = true;
			this.mListWord.LabelWrap = false;
			this.mListWord.Location = new System.Drawing.Point(8, 16);
			this.mListWord.MultiSelect = false;
			this.mListWord.Name = "mListWord";
			this.mListWord.Size = new System.Drawing.Size(648, 152);
			this.mListWord.TabIndex = 0;
			this.mListWord.View = System.Windows.Forms.View.Details;
			this.mListWord.ItemActivate += new System.EventHandler(this.mListWord_ItemActivate);
			this.mListWord.SelectedIndexChanged += new System.EventHandler(this.mListWord_SelectedIndexChanged);
			// 
			// mResultColumnKanji
			// 
			this.mResultColumnKanji.Text = "Kanji";
			this.mResultColumnKanji.Width = 126;
			// 
			// mResultColumnReading
			// 
			this.mResultColumnReading.Text = "Reading";
			this.mResultColumnReading.Width = 132;
			// 
			// mResultColumnEnglish
			// 
			this.mResultColumnEnglish.Text = "English";
			this.mResultColumnEnglish.Width = 200;
			// 
			// mResultColumnFrench
			// 
			this.mResultColumnFrench.Text = "Français";
			this.mResultColumnFrench.Width = 186;
			// 
			// mStatusBar
			// 
			this.mStatusBar.Location = new System.Drawing.Point(0, 392);
			this.mStatusBar.Name = "mStatusBar";
			this.mStatusBar.Size = new System.Drawing.Size(680, 22);
			this.mStatusBar.TabIndex = 3;
			this.mStatusBar.Text = "Idle";
			// 
			// FormSearch
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 414);
			this.Controls.Add(this.mStatusBar);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.mGroupSearch);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(632, 392);
			this.Name = "FormSearch";
			this.Text = "Yasashii Japanese-English Dictionnary";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormSearch_KeyDown);
			this.Load += new System.EventHandler(this.FormSearch_Load);
			this.Activated += new System.EventHandler(this.FormSearch_Activated);
			this.mGroupSearch.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		
		//*************************************************************
		private void FormSearch_Load(object sender, System.EventArgs e)
		{


		}


		//******************************************************************
		private void FormSearch_Activated(object sender, System.EventArgs e)
		{
			if (mInitActivated)
			{
				// do this only once
				mInitActivated = false;

				mStatusBar.Text = "Loading dictionnary...";

				// force app to redraw the first time
				this.Refresh();
				Application.DoEvents();


				// load the dictionnary... may take a while
				DateTime dt = DateTime.Now;
				mDict = new JPDict();


				// load cache if already exists
				bool ok = mDict.LoadCache();

				if (!ok)
				{
					MessageBox.Show("Yasashii is now going to load the XML dictionnary from the "
						+ "external jmdict file for the first time. This process may take around "
						+ "40 seconds and is done only once. Thank you for your patience.",
						"First dictionnary load",
						MessageBoxButtons.OK, MessageBoxIcon.Information);

					// otherwise load data in memory
					ok = mDict.LoadData();

					// and recreate the cache
					if (ok)
					{
						ok = mDict.CreateCache();
					}
					else
					{
						MessageBox.Show("Yasashii failed to load the JMdict data file!"
							+ "\nIs the file 'jmdict.xml' "
							+ "present in the application directory?",
							"Loading XML dictionnary",
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}

				if (!ok)
				{
					MessageBox.Show("Yasashii does not have its dictionnary data and will now quit.",
						"Loading dictionnary",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					Application.Exit();
				}


				TimeSpan ts = DateTime.Now - dt;
				string dur = " (" + ts.ToString() + "s)";

				if (ok)
					mStatusBar.Text = "Dictionnary loaded successfully" + dur;
				else
					mStatusBar.Text = "Error while loading dictionnary!" + dur;
			}
		}

		//*********************************************************************
		private void FormSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Control)
			{
				e.Handled = true;

				if ((e.KeyData & Keys.E) == Keys.E)
					mComboSearch.Focus();
				else if ((e.KeyData & Keys.D) == Keys.D)
					mListDef.Focus();
				else if ((e.KeyData & Keys.R) == Keys.R)
					mListWord.Focus();
				else if ((e.KeyData & Keys.Oemplus) == Keys.Oemplus)
					changeFont(1);
				else if ((e.KeyData & Keys.OemMinus) == Keys.OemMinus)
					changeFont(-1);
				else
					e.Handled = false;
			}
		}

		//*****************************************************************
		private void mButtonSearch_Click(object sender, System.EventArgs e)
		{
			performSearch(true);
		}


		//********************************************************************
		private void mComboSearch_Validated(object sender, System.EventArgs e)
		{
			mButtonSearch_Click(sender, e);
		}


		//********************************************************************
		private void mListWord_ItemActivate(object sender, System.EventArgs e)
		{
			// get selected entry from the word list

			if (mListWord.SelectedItems.Count > 0)
			{
				fillDefList((SWordEntry) mListWord.SelectedItems[0].Tag);
			}
		}

		
		//********************************************************************
		private void mListWord_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			mListWord_ItemActivate(sender, e);
		}


		//********************************************************************
		private void mComboSearch_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
		
		}


		//********************************************************************
		private void mComboSearch_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')			// return key
				performSearch(false);
		}


		//********************************************************************
		private void mButtonAbout_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("Yasashii v1.0.0 -- 2003 (c) Raphael MOLL"
				+ "\n\nYasashii is a simple Japanese/English two-way dictionnary based on the JMdict data file."
				+ "\nhttp://www.alfray.com"
				+ "\n\nJMdict (c) James William Breen & EDRDG, Monash University"
				+ "\nhttp://www.csse.monash.edu.au/~jwb/jmdict.html",
				"About Yasashii",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}


		//********************************************************************
		private void mRadioEnglish_CheckedChanged(object sender, System.EventArgs e)
		{
			mComboSearch.ImeMode = ImeMode.Alpha;
		}


		//********************************************************************
		private void mRadioFrench_CheckedChanged(object sender, System.EventArgs e)
		{
			mComboSearch.ImeMode = ImeMode.Alpha;
		}


		//********************************************************************
		private void mRadioKana_CheckedChanged(object sender, System.EventArgs e)
		{
			mComboSearch.ImeMode = ImeMode.Hiragana;
		}


		//********************************************************************
		private void mRadioRomaji_CheckedChanged(object sender, System.EventArgs e)
		{
			mComboSearch.ImeMode = ImeMode.Alpha;
		}


		//-----------------------------


		//********************************
		private void changeFont(int delta)
		{
			Control [] items = { mListDef, mListWord, mLabelKanji, mLabelKana, mLabelRomaji };

			foreach(Control c in items)
			{
				if (c.Font.SizeInPoints + delta > 8)
					c.Font = new Font(c.Font.FontFamily, c.Font.SizeInPoints + delta, GraphicsUnit.Point);
			}
		}


		//***************************************
		private void performSearch(bool complain)
		{
			string sword = mComboSearch.Text;
			bool use_case = mCheckMatchCase.Checked;
			bool whole_word = mCheckWholeWord.Checked;
			bool start_only = mCheckStartOnly.Checked;


			if (sword.Length < 1)
			{
				if (complain)
					MessageBox.Show("You must enter a search word first!", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}


			// look for the word
			mStatusBar.Text = "Word lookup for " + sword;

			ArrayList word_list = null;

			// timing...
			DateTime dt = DateTime.Now;

			if (mRadioEnglish.Checked)
				word_list = mDict.SearchWordEnglish(sword, use_case, whole_word, start_only);
			else if (mRadioFrench.Checked)
				word_list = mDict.SearchWordFrench(sword, use_case, whole_word, start_only);
			else if (mRadioKana.Checked)
				word_list = mDict.SearchWordReading(sword, whole_word, start_only);
			else if (mRadioRomaji.Checked)
				word_list = mDict.SearchWordRomaji(sword, whole_word, start_only);

			if (word_list == null || word_list.Count < 1)
			{
				mStatusBar.Text = "Word not found!";
			}
			else
			{ 
				TimeSpan ts = (DateTime.Now - dt);
				mStatusBar.Text = word_list.Count + " word" + (word_list.Count > 1 ? "s" : "") + " found in " + ts.ToString();

				fillWordList(word_list);
			}	
		}


		//********************************************
		private void fillWordList(ArrayList word_list)
		{
			mListWord.Items.Clear();

			if (word_list == null || word_list.Count < 1)
				return;

			/*
			int idx_jp = mResultColumnKanji.Index;
			int idx_kn = mResultColumnKana.Index;
			int idx_en = mResultColumnEnglish.Index;
			int idx_fr = mResultColumnFrench.Index;
			*/

			foreach(SWordEntry we in word_list)
			{
				ListViewItem lvi = new ListViewItem(we.mKanji);
				lvi.SubItems.Add(we.mReading);
				lvi.SubItems.Add(we.mEnglish);
				lvi.SubItems.Add(we.mFrench);

				// store entry in tag
				lvi.Tag = we;

				mListWord.Items.Add(lvi);
			}
		}


		//*************************************
		private void fillDefList(SWordEntry we)
		{
			mListDef.Items.Clear();

			// set entry kanji/kana/romaji
			mLabelKanji.Text = we.mKanji;
			mLabelKana.Text = we.mReading;
			mLabelRomaji.Text = "<romaji>";	// TBDL

			// insert all definitions
			if (we.mDefList != null)
				foreach(string str in we.mDefList)
					mListDef.Items.Add(str);

			// insert all extras
			if (we.mExtraList != null)
				foreach(string str in we.mExtraList)
					mListDef.Items.Add(str);
		}


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		protected	JPDict	mDict;
		private		bool	mInitActivated = true;

		private System.Windows.Forms.GroupBox mGroupSearch;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ColumnHeader mResultColumnEnglish;
		private System.Windows.Forms.ColumnHeader mResultColumnFrench;
		private System.Windows.Forms.CheckBox mCheckMatchCase;
		private System.Windows.Forms.RadioButton mRadioRomaji;
		private System.Windows.Forms.RadioButton mRadioEnglish;
		private System.Windows.Forms.RadioButton mRadioFrench;
		private System.Windows.Forms.Button mButtonSearch;
		private System.Windows.Forms.Label mLabelKana;
		private System.Windows.Forms.Label mLabelRomaji;
		private System.Windows.Forms.StatusBar mStatusBar;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		//-----------------------

		private System.Windows.Forms.ListView mListWord;
		private System.Windows.Forms.ColumnHeader mResultColumnKanji;
		private System.Windows.Forms.ListBox mListDef;
		private System.Windows.Forms.Label mLabelKanji;
		private System.Windows.Forms.CheckBox mCheckWholeWord;
		private System.Windows.Forms.ColumnHeader mResultColumnReading;
		private System.Windows.Forms.RadioButton mRadioKana;
		private System.Windows.Forms.TextBox mComboSearch;
		private System.Windows.Forms.Button mButtonAbout;
		private System.Windows.Forms.CheckBox mCheckStartOnly;



	} // class FormSearch
} // namespace Alfray.Yasashii


//---------------------------------------------------------------
//
//	$Log: FormSearch.cs,v $
//	Revision 1.6  2003/11/16 08:51:07  ralf
//	Keyboard shortcuts, change font
//	
//	Revision 1.5  2003/10/17 09:08:05  ralf
//	Added about box
//	
//	Revision 1.4  2003/10/17 08:18:38  ralf
//	Dialogs if XML jmdict data is missing
//	
//	Revision 1.3  2003/10/17 07:00:53  ralf
//	Started work on romaji-kana conversion
//	
//	Revision 1.2  2003/10/13 23:24:53  ralf
//	Implemented SItem lookup
//	
//	Revision 1.1.1.1  2003/10/13 21:33:42  ralf
//	Working prototype with cache of XML data for faster load
//	
//---------------------------------------------------------------
