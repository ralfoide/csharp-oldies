//*******************************************************************
/*
 *	Project:	Alfray.ComicsSharp
 * 
 *	File:		ComicsBook.cs
 * 
 *	RM (c) 2003
 * 
 */
//*******************************************************************

using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

//*************************************
namespace Alfray.ComicsSharp
{


	//****************************************************
	[Serializable]
	public class ComicsBook
	{
		// ------- public constants -------

		// TBDL: make UI preference
		const bool kAllowMissingId = true;
		const bool kAllowMissingDates = true;

		// XML storage		
		public const string kElementComics = "comics-book";

		// ------- public properties ------

		//******************
		public bool UsesDate
		{
			get
			{
				return mIndexType == EIndexTypeEnum.kIndexDate;
			}
		}

		//****************
		public bool UsesId
		{
			get
			{
				return mIndexType == EIndexTypeEnum.kIndexId;
			}
		}

		//*****************************
		public EIndexTypeEnum IndexType
		{
			get
			{
				return mIndexType;
			}
			set
			{
				mIndexType = value;
			}
		}

		//********************
		public Int64 LastIndex
		{
			get
			{
				return mImageLastIndex;
			}
			set
			{
				mImageLastIndex = value;
			}
		}

		//********************
		public Int64 MaxIndex
		{
			get
			{
				return mImageMaxIndex;
			}
			set
			{
				mImageMaxIndex = value;
			}
		}


		//***************************
		public bool AllowMissingIndex
		{
			get
			{
				if (UsesDate)
					return kAllowMissingDates;
				else
					return kAllowMissingId;
			}
		}

		//********************
		public string ImageUrl
		{
			get
			{
				return mImageUrlPattern;
			}
			set
			{
				mImageUrlPattern = value;
			}
		}

		//********************
		public string BaseDir
		{
			get
			{
				return mBaseDir;
			}
			set
			{
				mBaseDir = value;

				// create directory if needed
				if (!Directory.Exists(mBaseDir))
					Directory.CreateDirectory(mBaseDir);
			}
		}

		//****************
		public string Name
		{
			get
			{
				return mName;
			}
			set
			{
				mName = value;
			}
		}

		//****************
		public bool Active
		{
			get
			{
				return mActive;
			}
			set
			{
				mActive = value;
			}
		}


		//************************
		public FetchThread Fetcher
		{
			get
			{
				return mFetchThread;
			}
		}

		// ------- public types --------

		//************************
		public enum EIndexTypeEnum
		{
			kIndexId	= 0,
			kIndexDate
		}

		// ------- public methods --------

		
		//********************************
		public ComicsBook()
		{
			mIndexType		= EIndexTypeEnum.kIndexId;
			mImageLastIndex	= 0;
			mImageMaxIndex	= 0;
			mActive			= true;
			mImageUrlPattern= "";
			mBaseDir		= "";
			mName			= "";

			mFetchThread = new FetchThread(this);
		}


		//****************************************
		public bool SetStorageBaseDir(string path)
		{
			if (!Directory.Exists(path))
				return false;

			if (mName == null || mName == "")
				return false;

			this.BaseDir = Path.Combine(path, mName);

			return true;
		}


		//********************************
		public bool Save(XmlTextWriter xw)
		{
			xw.WriteStartElement(kElementComics);	// start <comics-book>

			xw.WriteStartElement(kNodeName);
			xw.WriteString(this.Name);
			xw.WriteEndElement();

			xw.WriteStartElement(kNodeBase);
			xw.WriteString(this.BaseDir);
			xw.WriteEndElement();

			xw.WriteStartElement(kNodeType);
			xw.WriteString(((int)this.IndexType).ToString());
			xw.WriteEndElement();

			xw.WriteStartElement(kNodeImgUrl);
			xw.WriteString(this.ImageUrl);
			xw.WriteEndElement();

			xw.WriteStartElement(kNodeLast);
			xw.WriteString(this.LastIndex.ToString());
			xw.WriteEndElement();

			xw.WriteStartElement(kNodeMax);
			xw.WriteString(this.MaxIndex.ToString());
			xw.WriteEndElement();

			xw.WriteStartElement(kNodeActive);
			xw.WriteString(this.Active.ToString());
			xw.WriteEndElement();

			xw.WriteEndElement();			// end <comics-book>

			return true;
		}

		//**********************************
		public bool Load(XPathNavigator nav)
		{
			XPathNodeIterator n;

			n = nav.Select(kNodeName);
			if (n.MoveNext())
				this.Name = n.Current.Value;

			n = nav.Select(kNodeBase);
			if (n.MoveNext())
				this.BaseDir = n.Current.Value;

			n = nav.Select(kNodeType);
			if (n.MoveNext())
				this.IndexType = (ComicsBook.EIndexTypeEnum) System.Convert.ToInt32(n.Current.Value);

			n = nav.Select(kNodeImgUrl);
			if (n.MoveNext())
				this.ImageUrl = n.Current.Value;

			n = nav.Select(kNodeLast);
			if (n.MoveNext())
				this.LastIndex = System.Convert.ToInt64(n.Current.Value);

			n = nav.Select(kNodeMax);
			if (n.MoveNext())
				this.MaxIndex = System.Convert.ToInt64(n.Current.Value);
			else
				this.MaxIndex = this.LastIndex;

			n = nav.Select(kNodeActive);
			if (n.MoveNext())
			{
				try
				{
					// will thrown FormatException if not 'True' or 'False'
					this.Active = System.Convert.ToBoolean(n.Current.Value);
				}
				catch(FormatException /* ex */)
				{
					// try as a number or give up, 0 for false, otherwise true
					this.Active = (System.Convert.ToInt32(n.Current.Value) != 0);
				}
			}
			else
			{
				this.Active = true;
			}


			return true;
		}


		// ------- private methods ------- 


		// ------- private types --------


		// ------- private constants --------

		// XML storage
		
		private const string kNodeName		= "name";
		private const string kNodeBase		= "base";
		private const string kNodeType		= "type";
		private const string kNodeImgUrl	= "img_url";
		private const string kNodeLast		= "last";
		private const string kNodeMax		= "max";
		private const string kNodeActive	= "active";


		// ------- private properties ------- 

		EIndexTypeEnum	mIndexType;
		Int64			mImageLastIndex;
		Int64			mImageMaxIndex;
		bool			mActive;
		string			mImageUrlPattern;
		string			mBaseDir;
		string			mName;

		FetchThread		mFetchThread;

	} // class ComicsBook
} // namespace Alfray.ComicsSharp

//*******************************************************************
/*
 *	$Log: ComicsBook.cs,v $
 *	Revision 1.2  2003/08/04 21:37:27  ralf
 *	Upload with abort/feedback, active book, etc.
 *	
 *	Revision 1.1  2003/08/01 00:51:58  ralf
 *	Added ComicsBook, ComicsBookCollection, Edit form and main form.
 *	
 * 
 */
//*******************************************************************

