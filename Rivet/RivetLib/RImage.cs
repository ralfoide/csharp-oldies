//*******************************************************************
/* 

 		Project:	RivetLib
 		File:		RImage.cs

*/ 
//*******************************************************************

using System;
using System.Drawing;
using System.Collections;

using Alfray.LibUtils.Misc;

//*************************************
namespace Alfray.Rivet.RivetLib
{
	//***************************************************
	/// <summary>
	/// Summary description for RImage.
	/// </summary>
	//***************************************************
	public class RImage: RFSItem
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//*************************
		/// <summary>
		/// Gets the leaf name (not full path) of this image.
		/// Unlike RFSItem, this property is get-only.
		/// If you want to rename an image, use the Move() method.
		/// </summary>
		//*************************
		public override string Name
		{
			get
			{
				return base.Name;
			}
		}


		//*************************
		/// <summary>
		/// Gets the parent of this image 
		/// or null if there is no parent.
		/// Unlike RFSItem, this property is get-only.
		/// If you want to move an image, use the Move() method.
		/// </summary>
		//*************************
		public override RDir Parent
		{
			get
			{
				return base.Parent;
			}
		}


		//****************
		/// <summary>
		/// Loads the image and return a reference to it.
		/// The inner image may be automatically disposed 
		/// so do not store the reference around.
		/// Once you are done with the image, call Release()
		/// to make sure the image is unloaded.
		/// </summary>
		//****************
		public Image Image
		{
			get
			{
				if (mImage == null)
				{
					try
					{
						mImage = new Bitmap(FullPath);
					}
					catch(Exception ex)
					{
						System.Console.Out.WriteLine(ex.Message);
					}
				}

				return mImage;
			}
		}


		//**************
		/// <summary>
		/// Returns the image size, in pixels
		/// or Size.Empty if no valid image is found.
		/// The image size is extracted once and cached.
		/// </summary>
		//**************
		public Size Size
		{
			get
			{
				if (mSize == Size.Empty)
				{
					try
					{
						mSize = Image.Size;
						Release();
					}
					catch(Exception ex)
					{
						System.Console.Out.WriteLine(ex.Message);
					}
				}

				return mSize;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//***********************************
		/// <summary>
		/// Constructs a directory with the given name
		/// and parent
		/// </summary>
		//***********************************
		public RImage(RDir parent, string name)
			: base(parent, name)
		{

		}


		//*******************
		/// <summary>
		/// Purges inner image references to free some memory.
		/// </summary>
		//*******************
		public void Release()
		{
			if (mImage != null)
			{
				mImage.Dispose();
				mImage = null;
			}
		}


		//********************************
		/// <summary>
		/// Returns a thumbnail bitmap of the requested size
		/// or null if no valid image is found.
		/// The thumbnail will be sized to fix maxSize and
		/// conserve the aspect ratio.
		/// </summary>
		//********************************
		public Image GetThumbnail(int maxSize)
		{
			if (   mThumbnail == null
				|| (mThumbnail.Size.Width != maxSize && mThumbnail.Size.Height != maxSize))
			{
				try
				{
					// create or recreate the thumbnail

					if (mThumbnail != null)
						mThumbnail.Dispose();

					// Make sure to cache the size if not already done
					if (mSize == Size.Empty)
						mSize = Image.Size;

					// Get aspect ratio
					Size sz = RUtils.AspectRatio(mSize, maxSize);

					// Create thumbnail
					mThumbnail = new Bitmap(Image, sz);
					
					// Release original image to free memory
					Release();
				}
				catch(Exception ex)
				{
					System.Console.Out.WriteLine(ex.Message);
				}
			}

			return mThumbnail;
		}

		
		#region IDisposable Members

		//*******************
		/// <summary>
		/// Release resources associated with thumbnails
		/// </summary>
		//*******************
		public override void Dispose()
		{
			if (mThumbnail != null)
			{
				mThumbnail.Dispose();
				mThumbnail = null;
			}

			if (mImage != null)
			{
				mImage.Dispose();
				mImage = null;
			}

			// Call base class

			base.Dispose();
		}

		#endregion



		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Constants -------------
		//-------------------------------------------

		

		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private Bitmap		mImage		= null;
		private Bitmap		mThumbnail	= null;
		private Size		mSize		= Size.Empty;
		private Hashtable	mExifInfo	= new Hashtable();


	} // class RImage
} // namespace Alfray.Rivet.RivetLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RImage.cs,v $
//	Revision 1.4  2005/07/22 14:52:58  ralf
//	Removed useless Collect and replaced with manual Release.
//	Internal image always released avec a GetThumb/Size.
//	GetThumb always updates cached size.
//	
//	Revision 1.3  2005/07/12 14:32:52  ralf
//	Keep image loaded for a while. Auto flush it.
//	
//	Revision 1.2  2005/05/31 06:16:52  ralf
//	Implementinf IDisposable for RFSItem, RDir, RImage and RAppState
//	
//	Revision 1.1  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//---------------------------------------------------------------
