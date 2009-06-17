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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Collections;

//*************************************
namespace Alfray.ComicsSharp
{
	//*******************************************
	public class ComicsBookCollection : ArrayList
	{
		// ------- public properties ------

		// ------- public methods --------

		//***************************
		public ComicsBookCollection()
		{
		}

		//*******************************
		public new ComicsBook this[int index]
		{
			get
			{
				return (ComicsBook) base[index];
			}
			set
			{
				base[index] = value;
			}
		}

		//*******************************
		public int Add(ComicsBook value)
		{
			return base.Add(value);
		}

		//*******************************
		public void Insert(int index, ComicsBook value)
		{
			base.Insert(index, value);
		}

		//*******************************
		public void Remove(ComicsBook value)
		{
			base.Remove(value);
		}

		//*******************************
		public bool Contains(ComicsBook value)
		{
			return base.Contains(value);
		}

		//*******************************
		public int IndexOf(ComicsBook value)
		{
			return base.IndexOf(value);
		}

		
		//*****************************
		public bool Save(string folder)
		{
			// actually write to a ".tmp" file, then switch with original
			try
			{
				string fp = Path.Combine(folder, kFilePath);
				string tmp = fp + ".tmp";

				XmlTextWriter xw = new XmlTextWriter(tmp, null);

				xw.WriteStartDocument();
				xw.WriteStartElement(kElementRoot); // start <comics-books>
				xw.WriteAttributeString(kAttribCount, this.Count.ToString());

				foreach(ComicsBook bk in this)
					bk.Save(xw);

				xw.WriteEndElement();	// end <comics-books>
				xw.WriteEndDocument();
				xw.Flush();
				xw.Close();

				// now move to or replace original file
				if (File.Exists(fp))
				{
					string bak = fp + ".bak";
					if (File.Exists(bak))
						File.Delete(bak);
					File.Move(fp, bak);
				}
				File.Move(tmp, fp);

				return true;
			}
			catch(Exception ex)
			{
#if DEBUG
				System.Diagnostics.Debug.Fail("Error saving comics book colletion", ex.ToString());
#else
				System.Diagnostics.Trace.Fail("Error saving comics book colletion", ex.Message);
#endif
			}

			return false;
		}

		//*****************************
		public bool Load(string folder)
		{
			string fp = Path.Combine(folder, kFilePath);

			if (!File.Exists(fp))
				return false;

			XPathDocument doc = new XPathDocument(fp);
			XPathNavigator nav = doc.CreateNavigator();

			this.Clear();

			nav.MoveToRoot();
			XPathNodeIterator iter = nav.Select("/" + kElementRoot + "/" + ComicsBook.kElementComics);
			while(iter.MoveNext())
			{
				ComicsBook bk = new ComicsBook();

				if (bk.Load(iter.Current))
					this.Add(bk);
			}

			return true;
		}

		// ------- inherited methods ------


		// ------- private methods ------- 



		// ------- private properties ------- 

		// XML storage
		
		private const string kFilePath		= "comics_books.xml";
		private const string kElementRoot	= "comics-books";
		private const string kAttribCount	= "count";


	} // class ComicsBookCollection
} // namespace Alfray.ComicsSharp

//*******************************************************************
/*
 *	$Log: ComicsBookCollection.cs,v $
 *	Revision 1.2  2003/08/04 21:37:27  ralf
 *	Upload with abort/feedback, active book, etc.
 *	
 *	Revision 1.1  2003/08/01 00:51:58  ralf
 *	Added ComicsBook, ComicsBookCollection, Edit form and main form.
 *	
 * 
 */
//*******************************************************************

