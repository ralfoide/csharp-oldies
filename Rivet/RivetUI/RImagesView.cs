//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetUI
	File:		RImagesView.cs

	Copyright 2005, Raphael MOLL.

	This file is part of Rivet.

	Rivet is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Rivet is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Rivet; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

*/
//*******************************************************************



using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Alfray.LibUtils.Misc;
using Alfray.Rivet.RivetLib;

//****************************
namespace Alfray.Rivet.RivetUI
{
	//*****************************************************
	/// <summary>
	/// Summary description for RImagesView.
	/// </summary>
	//*****************************************************
	public class RImagesView : System.Windows.Forms.Control
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------

		//*********************
		/// <summary>
		/// Image items to be displayed by this control.
		/// The control must be redrawn after the item list
		/// is changed using Refresh().
		/// </summary>
		//*********************
		public ArrayList Items;


		//*********************
		/// <summary>
		/// Image items that are selected.
		/// This is updated by the control to respond to user
		/// selection.
		/// This can also be modified by the code using the control
		/// to manipulate the selection.
		/// Items present here MUST be present in the Items arraylist.
		/// If manipulated programmatically, the control must be refreshed
		/// to show the new selection.
		/// </summary>
		//*********************
		public ArrayList SelectedItems;


		//****************************
		/// <summary>
		/// Gets or sets the thumbnail size.
		/// If modified, you must call Refresh() after.
		/// Default thumbnail size is 96 pixels.
		/// </summary>
		//****************************
		public int ThumbnailSize = 96;


		//*****************
		/// <summary>
		/// Gets or sets the logger
		/// </summary>
		//*****************
		public RILog Logger = new RVoidLog();


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		//******************
		public RImagesView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Init

			Items = new ArrayList();
			SelectedItems = new ArrayList();

			// Create the static brush for background

			mBackgroundBrushGrad = new LinearGradientBrush(
				new Point(0, 0),
				new Point(0, 100),
				Color.FromArgb(48, 48, 128),
				Color.FromArgb(128, 128, 250));

			mBackgroundBrushSolid = new SolidBrush(mBackgroundBrushGrad.LinearColors[1]);

			// Attributes for thumbnail drawing

			mImgAttr.ClearBrushRemapTable();
			mImgAttr.ClearColorKey();
			mImgAttr.ClearColorMatrix();
			mImgAttr.ClearGamma();
			mImgAttr.ClearOutputChannel();
			mImgAttr.ClearOutputChannelColorProfile();
			mImgAttr.ClearRemapTable();
			mImgAttr.ClearThreshold();
		}

		//-------------------------------------------
		//----------- Protected Methods -------------
		//-------------------------------------------


		//*********************************************
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		//*********************************************
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				stopAsyncUpdate();

				if (components != null)
					components.Dispose();

				if (mImgAttr != null)
					mImgAttr.Dispose();
				mImgAttr = null;

				if (mBackgroundBrushGrad != null)
					mBackgroundBrushGrad.Dispose();
				mBackgroundBrushGrad = null;

				if (mBackgroundBrushSolid != null)
					mBackgroundBrushSolid.Dispose();
				mBackgroundBrushSolid = null;
			}

			base.Dispose(disposing);
		}


		//************************************************
		protected override void OnSizeChanged(EventArgs e)
		{
			Refresh();
			base.OnSizeChanged(e);
		}


		//************************************************
		protected override void OnPaint(PaintEventArgs pe)
		{
			// Calling the base class OnPaint
			base.OnPaint(pe);

			// Custom Paint Code
			Point pt = this.Location;
			Size sz = this.Size;

			// Fill with background color
			Graphics g = pe.Graphics;
			int h = Math.Min(sz.Height, (int) mBackgroundBrushGrad.Rectangle.Height);
			if (pe.ClipRectangle.Y <= h)
				g.FillRectangle(mBackgroundBrushGrad, 0, 0, sz.Width, h);
			if (pe.ClipRectangle.Bottom >= h && sz.Height > h)
				g.FillRectangle(mBackgroundBrushSolid, 0, h, sz.Width, sz.Height);

			// Precompute the position of all elements as needed
			bool needs_refresh = mInfoRefresh;
			if (needs_refresh)
				computePositions(sz);

			// Draw the elements that intersect the cliping region
			// DEBUG
			Logger.Log("[ImgView] OnPaint: before Items' lock");
			lock(Items)
			{
				lock(mInfo)
				{
					foreach(RImage img in Items)
					{
						SImgInfo info = (SImgInfo)(mInfo[img]);
						if (pe.ClipRectangle.IntersectsWith(info.mBounds))
						{
							if (info.mThumbnail != null)
							{
								// g.DrawImage(info.mThumbnail, info.mRect.Location);
								g.DrawImage(info.mThumbnail,
											info.mRect,
											0, 0,
											info.mThumbnail.Width,
											info.mThumbnail.Height,
											GraphicsUnit.Pixel,
											mImgAttr);
							}

							g.DrawRectangle(new Pen(Color.Black), Rectangle.Inflate(info.mBounds, 1, 1));
						}
					}
				}
			}

			if (needs_refresh)
				startAsyncUpdate();
		}


		//****************************
		public override void Refresh()
		{
			mInfoRefresh = true;

			// Since the item array may have changed,
			// remove all info from images no longer in the items
			// array.

			Logger.Log("[ImgView] Refresh: before Items' lock");
			lock(Items)
			{
				lock(mInfo)
				{
					RImage [] old_img = new RImage[mInfo.Keys.Count];
					mInfo.Keys.CopyTo(old_img, 0);
					for(int n = old_img.Length - 1; n >= 0; n--)
						if (!Items.Contains(old_img[n]))
							mInfo.Remove(old_img[n]);
				}
			}

			base.Refresh();
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//************************************
		/// <summary>
		/// Compute the position of each image in the view.
		/// Information is stored in the mPixInfo hash table,
		/// with the item reference as key.
		/// </summary>
		//************************************
		private void computePositions(Size sz)
		{
			// the info hash table will be refreshed
			mInfoRefresh = false;

			// How many images can fit in a row?
			int nbCols = (sz.Width + kHSpace) / (ThumbnailSize + kHSpace);
			// Must be a least one...
			if (nbCols < 0)
				nbCols = 0;

			int x = 0;
			int y = 0;

			Logger.Log("[ImgView] computePositions: before Items' lock");
			lock(Items)
			{
				lock(mInfo)
				{
					foreach(RImage img in Items)
					{
						// Either add new infos or update existing
						SImgInfo info = SImgInfo.Empty;

						if (mInfo.ContainsKey(img))
							info = (SImgInfo) mInfo[img];

						// Right now we don't know anything about the image
						// just get the bounding rect
						info.mCol = x;
						info.mRow = y;

						if (info.mRect == Rectangle.Empty)
							info.mRect = new Rectangle(0, 0, ThumbnailSize, ThumbnailSize);
						computePos(ref info.mRect, x, y);
						
						if (info.mBounds.IsEmpty)
							info.mBounds = computeRect(x, y, ThumbnailSize, ThumbnailSize);
						else
							computePos(ref info.mBounds, x, y);

						// Add to or update hash table
						mInfo[img] = info;

						// Next row/col
						x++;
						if (x >= nbCols)
						{
							x = 0;
							y++;
						}
					} // foreach Items
				} // lock mInfo
			} // lock Items
		}


		//***************************************************************
		/// <summary>
		/// Computes the position of the rectangle where an image should fit
		/// </summary>
		//***************************************************************
		private void computePos(ref Rectangle r, int col, int row)
		{
			// Location
			r.X = kHSpace / 2 
				+ col * (ThumbnailSize + kHSpace) 
				+ (ThumbnailSize - r.Width) / 2;
			r.Y = kVSpace / 2
				+ row * (ThumbnailSize + kVSpace)
				+ (ThumbnailSize - r.Height) / 2;
		}


		//***************************************************************
		/// <summary>
		/// Computes the position & size of a rectangle where an image should fit.
		/// </summary>
		//***************************************************************
		private Rectangle computeRect(int col, int row, int width, int height)
		{
			Rectangle r = Rectangle.Empty;

			// Size
			r.Size = new Size(width, height);

			// Location
			r.X = kHSpace / 2 
				+ col * (ThumbnailSize + kHSpace) 
				+ (ThumbnailSize - width) / 2;
			r.Y = kVSpace / 2
				+ row * (ThumbnailSize + kVSpace)
				+ (ThumbnailSize - height) / 2;

			return r;
		}


		//*****************************
		/// <summary>
		/// Start the async update thread
		/// </summary>
		//*****************************
		private void startAsyncUpdate()
		{
			stopAsyncUpdate();

			Logger.Log("[ImgView] startAsyncUpdate");

			mThread = new Thread(new ThreadStart(this.asyncUpdateThread));
			mThreadStopRequested = false;
			mThread.Start();
		}


		//****************************
		/// <summary>
		/// Stop the async update thread
		/// </summary>
		//****************************
		private void stopAsyncUpdate()
		{
			Logger.Log("[ImgView] stopAsyncUpdate - Thread "
				+ (mThread == null ? "is" : "is not") + " null"
				+ " - bool " + mThreadStopRequested.ToString());
			if (mThread != null)
			{
				if (mThreadStopRequested == false)
				{
					Logger.Log("[ImgView] stopAsyncUpdate - Before join");

					mThreadStopRequested = true;
					mThread.Join();

					Logger.Log("[ImgView] stopAsyncUpdate - After join");
				}

				mThread = null;
			}
		}


		//******************************
		/// <summary>
		/// Async Update Thread
		/// </summary>
		//******************************
		private void asyncUpdateThread()
		{
			while (!mThreadStopRequested)
			{
				// look for at least one incomplete info object
				SImgInfo info = SImgInfo.Empty;
				RImage img = null;

				Logger.Log("[ImgView] asyncUpdate - Before search");
				lock(Items)
				{
					lock(mInfo)
					{
						foreach(RImage i in Items)
						{
							SImgInfo ii = (SImgInfo) mInfo[i];
							if (ii.mThumbnail == null)
							{
								// SImgInfo is a structure, not a class, so what
								// we have here is a stack copy and not a reference
								// to the structures in the hashtable
								info = ii;
								img = i;
								break;
							}
						}
					} // lock mInfo
				} // lock Items

				if (info.IsEmpty && img != null)
				{
					Logger.Log("[ImgView] asyncUpdate - Update col " 
						+ info.mCol.ToString() + ", row " + info.mRow.ToString());

					// load this info object.
					// We're explicitly not modifying the info object
					// out of the critical section.
					Rectangle rect = info.mRect;

					// -- get the thumbnail
					Image thumb = img.GetThumbnail(ThumbnailSize);

					// -- If the object size is not properly set, do it now
					if (   rect == Rectangle.Empty
						|| rect.Width != thumb.Width
						|| rect.Height != thumb.Height)
					{
						rect = computeRect(info.mCol, info.mRow, thumb.Width, thumb.Height);
					}

					// -- Update the info object (write back to it)
					lock(mInfo)
					{
						info.mRect = rect;
						info.mThumbnail = thumb;
						mInfo[img] = info;
					}

					Logger.Log("[ImgView] asyncUpdate - Updated");

					// -- Update display in the control's thread

					this.BeginInvoke(new invalidateDelegate(this.Invalidate), new object[] { info.mBounds });
				}
				else
				{
					// no object left to update, abort the while loop
					mThreadStopRequested = true;
					Logger.Log("[ImgView] asyncUpdate - Terminated");
				}
			}

		}

	
		#region Component Designer generated code

		//*********************************************
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		//*********************************************
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		
		//*********************************************
		/// <summary>
		/// Required designer variable.
		/// </summary>
		//*********************************************
		private System.ComponentModel.Container components = null;




		//-------------------------------------------
		//----------- Private Constants -------------
		//-------------------------------------------
		
		// Some constants that may be modifiable later
		private const int kHSpace = 10;
		private const int kVSpace = 10;


		//-------------------------------------------
		//----------- Private Types -----------------
		//-------------------------------------------

		private delegate void invalidateDelegate(Rectangle r);


		//---------------------
		/// <summary>
		/// View specific information for drawing an RImage
		/// </summary>
		//---------------------
		private struct SImgInfo
		{
			public Rectangle	mRect;
			public Rectangle	mBounds;
			public Image		mThumbnail;
			public int			mCol;
			public int			mRow;

			//---------------------
			/// <summary>
			/// Returns an SImgInfo with all fields initialized to defaults
			/// </summary>
			//---------------------
			public static SImgInfo Empty
			{
				get
				{
					SImgInfo info;
					info.mBounds	= Rectangle.Empty;
					info.mRect		= Rectangle.Empty;
					info.mThumbnail	= null;
					info.mRow		= 0;
					info.mCol		= 0;
					return info;
				}
			}

			//-----------------
			/// <summary>
			/// Indicates if this img info is empty.
			/// This is determined by mThumbnail and mRect
			/// being null of empty.
			/// </summary>
			//-----------------
			public bool IsEmpty
			{
				get
				{
					// This is a or-test. As long as one of these
					// members is NOT set, the structure is "empty"
					// (i.e. not fully initialized)
					return mThumbnail == null
						|| mBounds.IsEmpty
						|| mRect.IsEmpty;
				}
			}
		} // SImgInfo


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		// Background
		static private LinearGradientBrush	mBackgroundBrushGrad;
		static private SolidBrush			mBackgroundBrushSolid;

		// Drawing attributes
		static private ImageAttributes		mImgAttr = new ImageAttributes();

		// Hashtable with some extra info for each image for layout
		private Hashtable	mInfo				= new Hashtable();
		private bool		mInfoRefresh		= true;

		// Async Update Thread
		private Thread		mThread				= null;
		private bool		mThreadStopRequested= false;
	}
} // namespace Alfray.Rivet.RivetUI

//---------------------------------------------------------------
//	$Log: RImagesView.cs,v $
//	Revision 1.4  2005/07/23 15:00:22  ralf
//	Fix: Using Control.BeginInvoke instead of Control.Invoke in working thread
//	
//	Revision 1.3  2005/07/22 14:53:52  ralf
//	Adding asynchronous update of thumbnails.
//	
//	Revision 1.2  2005/07/12 14:34:32  ralf
//	Display images
//	
//	Revision 1.1  2005/06/26 22:22:13  ralf
//	Using custom RImageView class
//	
//---------------------------------------------------------------

