//*******************************************************************
/* 

 		Project:	Yasashii
 		File:		JPDict.cs

--------------------

This parses the JMDict.xml which has roughly the following format:

<entry> *
	<entry_seq> = unique number
	<k_ele>
		<keb> = kanji element
		<ke_inf> = coded info specific to this kanji
		<ke_pri> = app-specific priority
	<r_ele> *
		<reb> = reading element in kana [at least one per entry]
		<re_inf> = coded info specific to this element
		<re_pri> = app-specific priority
	<info>
		<lang> = ISO 639 2-letters origine language name for loan word
		<dial> = dialect name
		<bibl> + <bib_tag> <bib_txt> = bibliographic info
		<etym> = etymology
		<links> + <link_tag> <link_desc> <link_uri>
		<audit> + <upd_date> <upd_detl>
	<sense> * [at least one sense per entry]
		<stagk> = if present, restrict to keb
		<stagr> = if present, restrict to reb
		<ant>   = keb o reb of antonym
		<gram>  = grammar information (coded)
		<field> = field of application (general if absent)
		<gloss> = glosses, words or phrases equivalent in target language
		<gloss g_lang=NN> = NN is ISO-639 language code (2 letters), "en" implied
			<pri> = highlight this translation as primary meaning
		<example> = examples sentences
		<s_inf> = coded sense information

There will obviously be multiple <entry> and <sense>.
Eventually there can be
multiple <k_ele> and <r_ele>.

*/ 
//*******************************************************************

using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;


//*************************************
namespace Alfray.Yasashii
{
	//*********************
	public class SWordEntry
	{
		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		public string mKanji;
		public string mReading;
		public string mEnglish;
		public string mFrench;

		public ArrayList mDefList;		// array of string
		public ArrayList mExtraList;	// array of string

		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------


		//*****************
		public SWordEntry()
		{
			// should always be present, even if empty
			mKanji = "";
			mReading = "";
			mEnglish = "";
			mFrench = "";
		}

		//**********************************************
		public void AddExtra(string prefix, string xtra)
		{
			if (mExtraList == null)
				mExtraList = new ArrayList();
			mExtraList.Add((prefix.Length > 0 ? prefix + ": " : "") + xtra);
		}

		//***********************************
		public void AddEnglishDef(string def)
		{
			if (mDefList == null)
				mDefList = new ArrayList();
			mDefList.Add("en: " + def);

			if (mEnglish == null || mEnglish == "")
				mEnglish = def;
			else
				mEnglish += "; " + def;
		}

		//**********************************
		public void AddFrenchDef(string def)
		{
			if (mDefList == null)
				mDefList = new ArrayList();
			mDefList.Add("fr: " + def);

			if (mFrench == null || mFrench == "")
				mFrench = def;
			else
				mFrench += "; " + def;
		}
	}


	//*******************
	internal struct SItem
	{
		public enum EType
		{
			Entry,		// begining of an entry (no data associated)
			GlossEn,	// entry/sense/gloss
			GlossFr,	// entry/sense/gloss g_lang=fr
			Kanji,		// entry/k_ele/keb
			Reading,	// entry/r_ele/reb
			Gram,		// entry/sense/gram
			Example		// entry/sense/example
		}

		public EType mType;
		public string mData;

		public SItem(EType type, string str) { mType = type; mData = str; }
	}


	//***************************************************
	/// <summary>
	/// Summary description for JPDict.
	/// </summary>
	//***************************************************
	public class JPDict
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

		
		//*************
		public JPDict()
		{
		}


		//*********************
		public bool LoadCache()
		{
			string cache_filename = Path.Combine(System.Windows.Forms.Application.UserAppDataPath, mJmdictXml +".dat");

			DateTime dt;
			TimeSpan ts;

			if (!File.Exists(cache_filename))
				return false;

			// --- read cache file ---

			mItems = null;

			// DEBUG
			dt = DateTime.Now;

			//string cache_filename = Path.Combine(System.Windows.Forms.Application.UserAppDataPath, "JMdict.dat");
			StreamReader sr = File.OpenText(cache_filename);

			// get header and check

			string r_head = sr.ReadLine();
			System.Diagnostics.Debug.Assert(r_head == mCacheHeader, "Cache header mismatch");
			if (r_head != mCacheHeader)
				return false;

			// get items count
			int r_items = Convert.ToInt32(sr.ReadLine());
			System.Diagnostics.Debug.Assert(r_items > 0, "Zero items in cache");
			if (r_items <= 0)
				return false;

			// preallocate -- the item count is a hint for the array list size
			mItems = new ArrayList(r_items);

			while(r_items-- > 0)
			{
				// get code

				char c = (char)sr.Read();
				SItem.EType t = SItem.EType.GlossEn;

				switch(c)
				{
					case '@':
						t = SItem.EType.Entry;
						break;
					case 'e':
						t = SItem.EType.GlossEn;
						break;
					case 'f':
						t = SItem.EType.GlossFr;
						break;
					case 'k':
						t = SItem.EType.Kanji;
						break;
					case 'r':
						t = SItem.EType.Reading;
						break;
					case 'g':
						t = SItem.EType.Gram;
						break;
					case 'x':
						t = SItem.EType.Example;
						break;
					case '=':
						// end of file reached
						r_items = 0;
						break;
					default:
						System.Diagnostics.Debug.Fail("Unknown char type: " + c);
						break;
				}

				if (t == SItem.EType.Entry)
					mItems.Add(new SItem(t, null));
				else
					mItems.Add(new SItem(t, sr.ReadLine()));
			}

			sr.Close();
			sr = null;

			// DEBUG
			ts = (DateTime.Now - dt);
			System.Diagnostics.Debug.Write("\nRead Cache: " + ts.Seconds.ToString() + "s");

			return (mItems.Count > 0);
		}


		//********************
		public bool LoadData()
		{
			// DEBUG
			DateTime dt;
			TimeSpan ts;

			System.Diagnostics.Debug.Write("\nReading XML: " + mJmdictXml);
			dt = DateTime.Now;

			// where to store the content
			// entry = 100000
			// gloss = 350000
			// keb   =  90000
			// reb   = 100000
			// gram  =  80000
			mItems = new ArrayList(560000); // prealocate... a lot

			// get stream from file
			string path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
			path = Path.Combine(path, mJmdictXml);

			if (!File.Exists(path))
				return false;

			try
			{
				// create xml text reader
				XmlTextReader reader = new XmlTextReader(path);

				reader.WhitespaceHandling = WhitespaceHandling.None;

				while(reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element)
						continue;

					switch(reader.Name)
					{
						case "entry":
							mItems.Add(new SItem(SItem.EType.Entry, null));
							break;
						case "gloss":
							if (reader.HasAttributes)
							{
								if (reader.MoveToAttribute("g_lang")
									&& reader.ReadAttributeValue()
									&& reader.Value == "fr"
									&& reader.MoveToElement())
									mItems.Add(new SItem(SItem.EType.GlossFr, reader.ReadString()));
							}
							else
							{
								mItems.Add(new SItem(SItem.EType.GlossEn, reader.ReadString()));
							}
							break;
						case "keb":
							mItems.Add(new SItem(SItem.EType.Kanji, reader.ReadString()));
							break;
						case "reb":
							mItems.Add(new SItem(SItem.EType.Reading, reader.ReadString()));
							break;
						case "gram":
							string s = reader.ReadInnerXml();
							if (s.StartsWith("&"))
								s = s.Substring(1);
							if (s.EndsWith(";"))
								s = s.Substring(0, s.Length-1);
							mItems.Add(new SItem(SItem.EType.Gram, s));
							break;
						case "example":
							mItems.Add(new SItem(SItem.EType.Example, reader.ReadString()));
							break;
					}

					// System.Diagnostics.Debug.Write("\n" + reader.NodeType.ToString() + "= " + reader.Name + "; " + stream.Position.ToString());
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.Write(ex.Message, "\nError loading XML: ");
			}

			ts = (DateTime.Now - dt);
			System.Diagnostics.Debug.Write("\nread XML: nb = " + mItems.Count + " -- " + ts.Seconds.ToString() + "s");

			return mItems.Count > 0;
		}


		//***********************
		public bool CreateCache()
		{
			string cache_filename = Path.Combine(System.Windows.Forms.Application.UserAppDataPath, mJmdictXml + ".dat");

			// DEBUG
			DateTime dt;
			TimeSpan ts;

			// --- create a cache file from the data ---

			dt = DateTime.Now;

			StreamWriter sw = File.CreateText(cache_filename);

			// write the header
			sw.WriteLine(mCacheHeader);
			// write a rough _estimate_ of the number of entries (there may be less, but no more!)
			sw.WriteLine(mItems.Count.ToString());

			foreach(SItem si in mItems)
			{
				if (si.mType == SItem.EType.Entry)
				{
					// special case -- no data to store
					sw.Write((char) '@');
					continue;
				}

				// others entry must have some data with with
				if (si.mData == null || si.mData.Length <= 0)
					continue;

				char c = (char)0;
				switch(si.mType)
				{
					case SItem.EType.GlossEn:
						c = 'e';
						break;
					case SItem.EType.GlossFr:
						c = 'f';
						break;
					case SItem.EType.Kanji:
						c = 'k';
						break;
					case SItem.EType.Reading:
						c = 'r';
						break;
					case SItem.EType.Gram:
						c = 'g';
						break;
					case SItem.EType.Example:
						c = 'x';
						break;
				}

				sw.Write(c);
				sw.WriteLine(si.mData);
			}

			// indicate end of file
			sw.Write((char) '=');

			sw.Close();
			sw = null;

			// DEBUG
			ts = (DateTime.Now - dt);
			System.Diagnostics.Debug.Write("\nWrite Cache: " + ts.Seconds.ToString() + "s");

			return true;
		}



		//**********************************
		/// <returns>An ArrayList of SWordEntry or null if nothing found</returns>
		//**********************************
		internal ArrayList searchCache(SItem.EType type_filter,
										string search_word,
										bool use_case,
										bool whole_word,
										bool start_only)
		{
			// need items first
			if (mItems == null)
				return null;

			// DEBUG
			DateTime dt = DateTime.Now;

			// destination
			ArrayList word_list = new ArrayList();

			// prepare a regexp for matching the search word
			Regex regexp;

			//RegexOptions regopt = RegexOptions.Compiled;
			RegexOptions regopt = RegexOptions.None;
			if (!use_case)
				regopt |= RegexOptions.IgnoreCase;

			string regstr = search_word;
			if (whole_word)
				regstr = @"(?:^|\s)" + regstr + @"(?:$|\s)";
			if (start_only)
				regstr = "^" + regstr;

			regexp = new Regex(regstr, regopt);

			// look thru all items of the requested type
			int n = mItems.Count;
			for(int i=0; i<n; i++)
			{
				SItem it = (SItem) mItems[i];

				if (it.mType != type_filter)
					continue;

				if (regexp.IsMatch(it.mData))
				{
					SWordEntry we = getCacheEntry(i);
					if (we != null)
						word_list.Add(we);
				}
			}


			// DEBUG
			TimeSpan ts = (DateTime.Now - dt);
			System.Diagnostics.Debug.Write("\nSearch: " + ts.ToString() + "s");


			if (word_list.Count > 0)
				return word_list;
			else
				return null;
		}


		//*******************************************
		protected SWordEntry getCacheEntry(int index)
		{
			// 'index' refers to an mIndex entry that
			// is after only EType.Entry and before another one

			// So first, let's backtrack the item array till
			// we hit the beginning of the entry

			for(; index >= 0; index--)
			{
				SItem it = (SItem) mItems[index];

				if (it.mType == SItem.EType.Entry)
					break;
			}

			// failed to find an entry tag? not good.
			if (index < 0)
				return null;

			// destination storage
			SWordEntry we = new SWordEntry();

			// now let's parse this entry...
			index++; // skip the entry tag
			for(int n = mItems.Count; index < n; index++)
			{
				SItem it = (SItem) mItems[index];

				// stop as soon as we hit the beginning of the following entry
				if (it.mType == SItem.EType.Entry)
					break;

				// all other entries must have data associated
				if (it.mData == null || it.mData.Length <= 0)
					continue;

				string str = it.mData;

				switch(it.mType)
				{
					case SItem.EType.GlossEn:
						we.AddEnglishDef(str);
						break;
					case SItem.EType.GlossFr:
						we.AddFrenchDef(str);
						break;
					case SItem.EType.Kanji:
						// right now let's get only the first kanji
						if (we.mKanji.Length == 0)
							we.mKanji = str;
						// let's store the others as extra info
						else
							we.AddExtra("kanji", str);
						break;
					case SItem.EType.Reading:
						// right now let's get only the first reading
						if (we.mReading.Length == 0)
							we.mReading = str;
						// let's store the others as extra info
						else
							we.AddExtra("reading", str);
						break;
					case SItem.EType.Gram:
						we.AddExtra("gram", str);
						break;
					case SItem.EType.Example:
						we.AddExtra("ex", str);
						break;
				} // end switch type
			} // end for items

			// if there's no kanji, use the first reading
			if (we.mKanji.Length == 0)
				we.mKanji = we.mReading;

			// return item if not empty
			if (we.mReading != "" && we.mDefList != null && we.mDefList.Count > 0)
				return we;
			else
				return null;
		}



		//**********************************
		/// <returns>An ArrayList of SWordEntry or null if nothing found</returns>
		//**********************************
		public ArrayList SearchWordEnglish(string search_word, bool use_case, bool whole_word, bool start_only)
		{
			return searchCache(SItem.EType.GlossEn, search_word, use_case, whole_word, start_only);
		}


		//**********************************
		/// <returns>An ArrayList of SWordEntry or null if nothing found</returns>
		//**********************************
		public ArrayList SearchWordFrench(string search_word, bool use_case, bool whole_word, bool start_only)
		{
			return searchCache(SItem.EType.GlossFr, search_word, use_case, whole_word, start_only);
		}

		
		//**********************************
		/// <returns>An ArrayList of SWordEntry or null if nothing found</returns>
		//**********************************
		public ArrayList SearchWordReading(string search_word, bool whole_word, bool start_only)
		{
			return searchCache(SItem.EType.Reading, search_word, false, whole_word, start_only);
		}

		
		//**********************************
		/// <returns>An ArrayList of SWordEntry or null if nothing found</returns>
		//**********************************
		public ArrayList SearchWordRomaji(string search_word, bool whole_word, bool start_only)
		{
			return searchCache(SItem.EType.Reading, search_word, false, whole_word, start_only);
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------



		//-------------------------------------------
		//----------- Private Constants -------------
		//-------------------------------------------

		const string mCacheHeader = "alfray_yasashii-jmdict_cache-v100";
		const string mJmdictXml   = "JMdict.xml";
		// const string mJmdictXml   = "JMdict_redux.xml";

		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private ArrayList		mItems;

	} // class JPDict
} // namespace Alfray.Yasashii


//---------------------------------------------------------------
//
//	$Log: JPDict.cs,v $
//	Revision 1.4  2003/10/17 08:18:27  ralf
//	Removed obsolete XML/XPath search code
//	
//	Revision 1.3  2003/10/17 07:00:53  ralf
//	Started work on romaji-kana conversion
//	
//	Revision 1.2  2003/10/13 23:24:53  ralf
//	Implemented SItem lookup
//	
//	Revision 1.1.1.1  2003/10/13 21:33:55  ralf
//	Working prototype with cache of XML data for faster load
//	
//---------------------------------------------------------------
