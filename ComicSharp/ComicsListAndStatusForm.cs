//*******************************************************************
/*
 *	Project:	Alfray.ComicsSharp
 * 
 *	File:		ComicsBookCollection.cs
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
using System.Data;

using System.IO;

//**************************
namespace Alfray.ComicsSharp
{
	//********************************************************
	public class FormListAndStatus : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.StatusBar mStatusBar;
		private System.Windows.Forms.ProgressBar mProgressBar;
		private System.Windows.Forms.ListView mListView;
		private System.Windows.Forms.Button mButtonAdd;
		private System.Windows.Forms.Button mButtonEdit;
		private System.Windows.Forms.Button mButtonRemove;
		private System.Windows.Forms.Button mButtonStart;
		private System.Windows.Forms.Button mButtonStop;
		private System.Windows.Forms.Button mButtonView;
		private System.Windows.Forms.Button mButtonViewAll;
		private System.Windows.Forms.ColumnHeader mColumnName;
		private System.Windows.Forms.ColumnHeader mColumnLast;
		private System.Windows.Forms.ColumnHeader mColumnStatus;
		private System.Windows.Forms.ColumnHeader mColumnMax;

		// Required designer variable.
		private System.ComponentModel.IContainer components;

		// ------- public constants ------

		const bool kFetchAllInParallel = false;
		private System.Windows.Forms.ColumnHeader mColumnRate;
		const bool kFetchAsynchronous = true;
		
		// ------- public properties ------


		// ------- public methods --------

		//************************
		public FormListAndStatus()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Init

			mAppDataPath = ShellAccess.GetFolderPath(ShellAccess.CSIDL_APPDATA);
			
			// Get the namespace of this class, typically "Alfray.ComicsSharp"
			// and transform it into two subdirs "Alfray\ComicsSharp"
			string subdir = this.GetType().Namespace.Replace('.', '\\');

			// append this to the app data path to get an app-specific directory
			mAppDataPath = Path.Combine(mAppDataPath, subdir);

			// create the directory if it does not exists
			if (!Directory.Exists(mAppDataPath))
				Directory.CreateDirectory(mAppDataPath);


			// create lists
			mFetchList = new ComicsBookCollection();
			mBookList = new ComicsBookCollection();

			// get main list
			mBookList.Load(mAppDataPath);

			foreach(ComicsBook bk in mBookList)
			{
				// set storage dir
				if (bk.BaseDir == "")
					bk.SetStorageBaseDir(mAppDataPath);

				// populate all items in view list
				ListViewItem it = new ListViewItem(bk.Name);
				it = mListView.Items.Add(it);
				it.Tag = bk;
				updateItem(it);
			}

			// Select first item, if any
			if (mListView.Items.Count > 0)
				mListView.Items[0].Selected = true;

			updateListView();

			// update
			mStatusBar.Text = "List loaded";
		}

		//*************************************
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
			this.components = new System.ComponentModel.Container();
			this.mStatusBar = new System.Windows.Forms.StatusBar();
			this.mProgressBar = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.mListView = new System.Windows.Forms.ListView();
			this.mColumnName = new System.Windows.Forms.ColumnHeader();
			this.mColumnLast = new System.Windows.Forms.ColumnHeader();
			this.mColumnMax = new System.Windows.Forms.ColumnHeader();
			this.mColumnStatus = new System.Windows.Forms.ColumnHeader();
			this.mButtonAdd = new System.Windows.Forms.Button();
			this.mButtonEdit = new System.Windows.Forms.Button();
			this.mButtonRemove = new System.Windows.Forms.Button();
			this.mButtonStart = new System.Windows.Forms.Button();
			this.mButtonStop = new System.Windows.Forms.Button();
			this.mButtonView = new System.Windows.Forms.Button();
			this.mButtonViewAll = new System.Windows.Forms.Button();
			this.mColumnRate = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// mStatusBar
			// 
			this.mStatusBar.Location = new System.Drawing.Point(0, 224);
			this.mStatusBar.Name = "mStatusBar";
			this.mStatusBar.Size = new System.Drawing.Size(504, 22);
			this.mStatusBar.TabIndex = 0;
			this.mStatusBar.Text = "statusBar1";
			// 
			// mProgressBar
			// 
			this.mProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mProgressBar.Location = new System.Drawing.Point(200, 195);
			this.mProgressBar.Name = "mProgressBar";
			this.mProgressBar.Size = new System.Drawing.Size(296, 16);
			this.mProgressBar.TabIndex = 1;
			// 
			// mListView
			// 
			this.mListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mListView.CheckBoxes = true;
			this.mListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.mColumnName,
																						this.mColumnLast,
																						this.mColumnMax,
																						this.mColumnStatus,
																						this.mColumnRate});
			this.mListView.FullRowSelect = true;
			this.mListView.GridLines = true;
			this.mListView.HideSelection = false;
			this.mListView.LabelWrap = false;
			this.mListView.Location = new System.Drawing.Point(8, 8);
			this.mListView.MultiSelect = false;
			this.mListView.Name = "mListView";
			this.mListView.Size = new System.Drawing.Size(488, 144);
			this.mListView.TabIndex = 2;
			this.mListView.View = System.Windows.Forms.View.Details;
			this.mListView.DoubleClick += new System.EventHandler(this.mButtonView_Click);
			this.mListView.SelectedIndexChanged += new System.EventHandler(this.mListView_SelectedIndexChanged);
			this.mListView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.mListView_ItemCheck);
			// 
			// mColumnName
			// 
			this.mColumnName.Text = "Name";
			this.mColumnName.Width = 120;
			// 
			// mColumnLast
			// 
			this.mColumnLast.Text = "Last";
			this.mColumnLast.Width = 75;
			// 
			// mColumnMax
			// 
			this.mColumnMax.Text = "Max";
			this.mColumnMax.Width = 75;
			// 
			// mColumnStatus
			// 
			this.mColumnStatus.Text = "Status";
			this.mColumnStatus.Width = 134;
			// 
			// mButtonAdd
			// 
			this.mButtonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonAdd.Location = new System.Drawing.Point(8, 160);
			this.mButtonAdd.Name = "mButtonAdd";
			this.mButtonAdd.TabIndex = 3;
			this.mButtonAdd.Text = "Add...";
			this.mButtonAdd.Click += new System.EventHandler(this.mButtonAdd_Click);
			// 
			// mButtonEdit
			// 
			this.mButtonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonEdit.Location = new System.Drawing.Point(96, 160);
			this.mButtonEdit.Name = "mButtonEdit";
			this.mButtonEdit.TabIndex = 4;
			this.mButtonEdit.Text = "Edit...";
			this.mButtonEdit.Click += new System.EventHandler(this.mButtonEdit_Click);
			// 
			// mButtonRemove
			// 
			this.mButtonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mButtonRemove.Location = new System.Drawing.Point(416, 160);
			this.mButtonRemove.Name = "mButtonRemove";
			this.mButtonRemove.TabIndex = 5;
			this.mButtonRemove.Text = "Remove...";
			this.mButtonRemove.Click += new System.EventHandler(this.mButtonRemove_Click);
			// 
			// mButtonStart
			// 
			this.mButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonStart.Location = new System.Drawing.Point(8, 192);
			this.mButtonStart.Name = "mButtonStart";
			this.mButtonStart.TabIndex = 6;
			this.mButtonStart.Text = "Start";
			this.mButtonStart.Click += new System.EventHandler(this.mButtonStart_Click);
			// 
			// mButtonStop
			// 
			this.mButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonStop.Location = new System.Drawing.Point(96, 192);
			this.mButtonStop.Name = "mButtonStop";
			this.mButtonStop.TabIndex = 7;
			this.mButtonStop.Text = "Stop";
			this.mButtonStop.Click += new System.EventHandler(this.mButtonStop_Click);
			// 
			// mButtonView
			// 
			this.mButtonView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonView.Location = new System.Drawing.Point(200, 160);
			this.mButtonView.Name = "mButtonView";
			this.mButtonView.TabIndex = 8;
			this.mButtonView.Text = "View";
			this.mButtonView.Click += new System.EventHandler(this.mButtonView_Click);
			// 
			// mButtonViewAll
			// 
			this.mButtonViewAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonViewAll.Location = new System.Drawing.Point(288, 160);
			this.mButtonViewAll.Name = "mButtonViewAll";
			this.mButtonViewAll.TabIndex = 9;
			this.mButtonViewAll.Text = "View All";
			this.mButtonViewAll.Click += new System.EventHandler(this.mButtonViewAll_Click);
			// 
			// mColumnRate
			// 
			this.mColumnRate.Text = "kB/s";
			this.mColumnRate.Width = 80;
			// 
			// FormListAndStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(504, 246);
			this.Controls.Add(this.mButtonViewAll);
			this.Controls.Add(this.mButtonView);
			this.Controls.Add(this.mButtonStop);
			this.Controls.Add(this.mButtonStart);
			this.Controls.Add(this.mButtonRemove);
			this.Controls.Add(this.mButtonEdit);
			this.Controls.Add(this.mButtonAdd);
			this.Controls.Add(this.mListView);
			this.Controls.Add(this.mProgressBar);
			this.Controls.Add(this.mStatusBar);
			this.MinimumSize = new System.Drawing.Size(512, 280);
			this.Name = "FormListAndStatus";
			this.Text = "ComicSharp - Status";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormListAndStatus_Closing);
			this.ResumeLayout(false);

		}
		#endregion


		// ------- private properties ------- 

		ComicsBookCollection mBookList;		// main storage
		ComicsBookCollection mFetchList;	// items currently fetching

		string mAppDataPath;

		// ------- private methods ------- 

		//**************************************
		private bool updateItem(ListViewItem it)
		{
			try
			{
				if (it == null)
					return false;

				if (it.Tag == null || !it.Tag.GetType().Equals(typeof(ComicsBook)))
					return false;

				ComicsBook book = (ComicsBook)it.Tag;

				it.Text = book.Name;
				it.Checked = book.Active;

				// remove all but the first subitem
				while(it.SubItems.Count > 1)
					it.SubItems.RemoveAt(it.SubItems.Count-1);

				if (book.UsesDate)
				{
					DateTime dt = new DateTime(book.LastIndex);
					it.SubItems.Add(dt.ToShortDateString());
					it.SubItems.Add(DateTime.Now.ToShortDateString());
				}
				else
				{
					it.SubItems.Add(book.LastIndex.ToString());
					it.SubItems.Add(book.MaxIndex.ToString());
				}

				it.SubItems.Add("Idle"); // status
				it.SubItems.Add(""); // kB/s
			}
			catch(Exception ex)
			{
				System.Diagnostics.Trace.Fail(ex.Message);
				return false;
			}

			return true;
		}


		//***************************
		private bool updateListView()
		{
			bool is_fetching = (ArrayList.Synchronized(mFetchList).Count > 0);
			mButtonStop.Enabled = is_fetching;
            
			bool has_seletion = mListView.SelectedItems.Count > 0;
			mButtonEdit.Enabled = has_seletion && !is_fetching;
			mButtonView.Enabled = has_seletion && !is_fetching;
			mButtonRemove.Enabled = has_seletion && !is_fetching;

			bool has_items = mListView.Items.Count > 0;
			mButtonStart.Enabled = has_items && !is_fetching;
			mButtonViewAll.Enabled = has_items && !is_fetching;


			return true;
		}


		//************************************
		private bool viewItem(ComicsBook book)
		{
			return true;
		}


		//**************************
		private bool startFetching()
		{
			// don't start if nothing to do
			if (mBookList.Count < 1)
				return false;

			// get thread-safe wrapper around fetchlist
			ArrayList safe_fetch_list = ArrayList.Synchronized(mFetchList);

			// for each, tell to start
			foreach(ComicsBook bk in mBookList)
			{
				// ...only if active [RM 20030803]
				if (bk.Active)
				{
					bool already_started = false;

					// ...and not already in the fetch list
					// the list must be locked before using an enumerator
					lock(safe_fetch_list.SyncRoot)
					{
						foreach(ComicsBook bf in safe_fetch_list)
							if (Object.ReferenceEquals(bf, bk))
								already_started = true;
					}

					// add to the fetch list (but does not start yet)
					if (!already_started)
					{
						// set fetcher update callback
						bk.Fetcher.SetUpdateEvent(new FetchThread.UpdateHandler(this.fetchUpdateCallback));

						// set fetcher initial state
						bk.Fetcher.State = FetchThread.EFetchState.kIdle;

						// notify this book it is scheduled for later...
						bk.Fetcher.Schedule();

						// add to list for later start
						safe_fetch_list.Add(bk);
					}
				}
			}


			// start fetching... all at once or only the first one
			if (kFetchAllInParallel)
			{
				// start'em all
				// the list must be locked before using an enumerator
				lock(safe_fetch_list.SyncRoot)
				{
					foreach(ComicsBook bk in safe_fetch_list)
						bk.Fetcher.Start(kFetchAsynchronous);
				}
			}
			else if (safe_fetch_list.Count > 0)
			{
				// start only the first idle one
				// (completion of the first one will start the other ones)

				// the list must be locked before using an enumerator
				lock(safe_fetch_list.SyncRoot)
				{
					foreach(ComicsBook bk in safe_fetch_list)
					{
						if (bk.Fetcher.State == FetchThread.EFetchState.kIdle)
						{
							bk.Fetcher.Start(kFetchAsynchronous);
							break;
						}
					} // foreach
				} // lock
			} // else

			// asynchronouse fetch started if list not empty
			return safe_fetch_list.Count > 0;
		}


		//*************************
		private bool stopFetching()
		{
			// get thread-safe wrapper around fetchlist
			ArrayList safe_fetch_list = ArrayList.Synchronized(mFetchList);

			// already all stopped?
			if (safe_fetch_list.Count < 1)
			{
				mStatusBar.Text = "All downloads stopped";
				return true;
			}
			else
			{
				mStatusBar.Text = "Stopping downloads...";
			}

			// the list must be locked before using an enumerator
			lock(safe_fetch_list.SyncRoot)
			{
				foreach(ComicsBook bk in safe_fetch_list)
					bk.Fetcher.Stop();
			}

			return false;
		}


		//****************************************************************************
		private void fetchUpdateCallback(ComicsBook book, string text, bool completed)
		{
			if (book == null)
				return;

			// locate book in main list view
			ListViewItem it = null;
			foreach(ListViewItem i in mListView.Items)
			{
				if (Object.ReferenceEquals(i.Tag, book))
				{
					it = i;
					break;
				}
			}

			// if book not located, give up
			System.Diagnostics.Debug.Assert(it != null);
			if (it == null)
				return;

			// Update status of item
			if (text != null)
				it.SubItems[mColumnStatus.Index].Text = text;

			// update the last and max index
			if (book.UsesDate)
			{
				it.SubItems[mColumnLast.Index].Text = (new DateTime(book.LastIndex)).ToShortDateString();
				if (book.Fetcher.Completion == 0)
					it.SubItems[mColumnMax.Index].Text =(new DateTime(book.MaxIndex)).ToShortDateString();
			}
			else
			{
				it.SubItems[mColumnLast.Index].Text = book.LastIndex.ToString();
				if (book.Fetcher.Completion == 0)
					it.SubItems[mColumnMax.Index].Text = book.MaxIndex.ToString();
			}

			// update the data rate
			it.SubItems[mColumnRate.Index].Text = book.Fetcher.KBRate.ToString("F");

			// Update progress bar
			ArrayList safe_book_list = ArrayList.Synchronized(mBookList);
			if (safe_book_list.Count > 0)
			{
				double p = 0;
				double n = 0;

				// books cannot be removed during fetching, so this list should not
				// change... no lock needed here
				foreach(ComicsBook bk in safe_book_list)
				{
					if (bk.Active)
					{
						n++;
						p += bk.Fetcher.Completion;
					}
				}

				if (n > 0)
					mProgressBar.Value = (int)(100.0 * p / n);
				else
					mProgressBar.Value = 0;
			}

			// if item has been completed, remove from internal list
			if (completed)
			{
				// get thread-safe wrapper around fetchlist
				ArrayList safe_fetch_list = ArrayList.Synchronized(mFetchList);

				int n = safe_fetch_list.Count-1;
				for(; n >= 0; n--)
				{
					if (Object.ReferenceEquals(safe_fetch_list[n], book))
					{
						safe_fetch_list.RemoveAt(n);
						break;
					}
				}

				// if everything was removed, update the stop-ui
				if (n >= 0 && safe_fetch_list.Count == 0)
					mButtonStop_Click(this, null);

				// if not parallelizing fetchs, start the next idle one if any

				// the list must be locked before using an enumerator
				lock(safe_fetch_list.SyncRoot)
				{
					foreach(ComicsBook bk in safe_fetch_list)
					{
						if (bk.Fetcher.State == FetchThread.EFetchState.kIdle)
						{
							bk.Fetcher.Start(kFetchAsynchronous);
							break;
						}
					} // foreach
				} // lock

			} // if completed

			// Update UI
			this.Update();
		}



		// ------- private UI callbacks ------- 


		//**************************************************************
		private void mButtonAdd_Click(object sender, System.EventArgs e)
		{
			ComicsBook book = new ComicsBook();

			ComicsEditForm form = new ComicsEditForm(book);
			if (form.ShowDialog() == DialogResult.OK)
			{
				// set storage dir
				book.SetStorageBaseDir(mAppDataPath);

				// add to internal list
				mBookList.Add(book);

				// add to list view
				ListViewItem it = new ListViewItem(book.Name);
				it = mListView.Items.Add(it);
				it.Tag = book;
				updateItem(it);

				// select in list view
				mListView.SelectedItems.Clear();
				it.Selected = true;

				mStatusBar.Text = String.Format("New book '{0}' added", book.Name);
			}
		}


		//**************************************************************
		private void mButtonEdit_Click(object sender, System.EventArgs e)
		{
			// get selected item(s) (note: multiselect is off so should be unique)
			foreach(ListViewItem it in mListView.SelectedItems)
			{
				// get the book
				ComicsBook book = (ComicsBook)it.Tag;

				// edit it
				mStatusBar.Text = String.Format("Editing book '{0}'...", book.Name);
				ComicsEditForm form = new ComicsEditForm(book);
				if (form.ShowDialog() == DialogResult.OK)
				{
					// update
					updateItem(it);

					mStatusBar.Text = String.Format("Book '{0}' edited", book.Name);
				}
			}
		}


		//**************************************************************
		private void mButtonView_Click(object sender, System.EventArgs e)
		{
			// get selected item(s) (note: multiselect is off so should be unique)
			foreach(ListViewItem it in mListView.SelectedItems)
			{
				// get the book
				ComicsBook book = (ComicsBook)it.Tag;

				// view it
				viewItem(book);
			}
		}


		//**************************************************************
		private void mButtonViewAll_Click(object sender, System.EventArgs e)
		{
			// get selected item(s) (note: multiselect is off so should be unique)
			foreach(ComicsBook book in mBookList)
			{
				// view it
				viewItem(book);
			}
		}


		//**************************************************************
		private void mButtonRemove_Click(object sender, System.EventArgs e)
		{
			// can't do that if fetching is going on
			System.Diagnostics.Debug.Assert(mFetchList.Count == 0);
			if (mFetchList.Count != 0)
				return;

			// get selected item(s) (note: multiselect is off so should be unique)
			// start by last item... cannot use foreach when modifying the collection
			for(int n = mListView.Items.Count-1; n >= 0; n--)
			{
				ListViewItem it = mListView.Items[n];
				if (!it.Selected)
					continue;

				// get the book
				ComicsBook book = (ComicsBook)it.Tag;

				mStatusBar.Text = "Removing book '" + book.Name + "'...";

				if (MessageBox.Show("Are you sure you want to delete '" + book.Name + "'?",
					"Delete Comics Book",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) == DialogResult.Yes)
				{
					// remove view item
					it.Remove();

					// remove the book from the main collection
					// ArrayList.IndexOf uses object equality (i.e. compare value)
					int c = mBookList.Count-1;
					for(; c >= 0; c--)
					{
						if (Object.ReferenceEquals(mBookList[c], book))
						{
							mBookList.RemoveAt(c);
							break;
						}
					}

					System.Diagnostics.Debug.Assert(c >= 0);

					if (c < 0)
						mBookList.Remove(book);

					mStatusBar.Text = "Book '" + book.Name + "' removed";
				}
				else
				{
					mStatusBar.Text = "Book '" + book.Name + "' not removed";
				}
			}

			updateListView();	
		}


		//**************************************************************
		private void mButtonStart_Click(object sender, System.EventArgs e)
		{
			startFetching();
			updateListView();
		}


		//**************************************************************
		private void mButtonStop_Click(object sender, System.EventArgs e)
		{
			stopFetching();
			updateListView();
		}


		//**************************************************************
		private void FormListAndStatus_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// don't close if still fetching comics
			if (mFetchList.Count > 0)
			{
				mStatusBar.Text = "Downloads currently active. Stop them first.";
				e.Cancel = true;
				return;
			}

			// don't close if can't save (is that such a good idea?)
			if (mBookList.Save(mAppDataPath))
			{
				mStatusBar.Text = "List saved";
			}
			else
			{
				mStatusBar.Text = "Error saving list, close aborted";
				e.Cancel = true;
			}
		}

		//**************************************************************
		private void mListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			updateListView();
		}

		//**************************************************************
		private void mListView_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			// get the book
			int index = e.Index;

			ComicsBook book = (ComicsBook)mListView.Items[index].Tag;

			// get the new state
			bool will_be_active = (e.NewValue == CheckState.Checked);
			book.Active = will_be_active;
		}


	} // class FormListAndStatus
} // namespace Alfray.ComicsSharp

//*******************************************************************
/*
 *	$Log: ComicsListAndStatusForm.cs,v $
 *	Revision 1.2  2003/08/04 21:37:27  ralf
 *	Upload with abort/feedback, active book, etc.
 *	
 *	Revision 1.1  2003/08/01 00:51:58  ralf
 *	Added ComicsBook, ComicsBookCollection, Edit form and main form.
 */
//*******************************************************************

