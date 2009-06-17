//*******************************************************************
/* 

 		Project:	$safeprojectname$
 		File:		RPref.cs

		Platform:	.Net 2.0 with generics

*/ 
//*******************************************************************



using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;


//*********************************
namespace Alfray.Faf.Utils
{
	//***************************************************
	/// <summary>
	/// RPref:
	/// - Maintains a dictionnary of settings.
	/// - Settings are loaded/saved in an XML File.
	/// - The file is located in the app's UserAppData, RPref.xml
	/// </summary>
	//***************************************************
	public class RPref
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//*****************************
		public string this[string name]
		{
			get
			{
				if (mSettings.ContainsKey(name))
					return (string) mSettings[name];
				else
					return null;
			}

			set
			{
				// RM 20050129 setting an existing key to NULL removes it
				if (value == null)
				{
					if (mSettings.ContainsKey(name))
						mSettings.Remove(name);
				}
				else
				{
					mSettings[name] = value;
				}
			}
		}



		//***********************
		public Hashtable Settings
		{
			get
			{
				return mSettings;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//************
		public RPref()
		{
			mSettings = new Hashtable();
		}



		//****************
		public bool Load()
		{
			string f = settingFileName();

			if (!File.Exists(f))
				return true;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(f);

				// not an XML?
				if (doc.DocumentElement == null)
					return true;

				foreach (XmlNode node in doc.SelectNodes("/r-prefs/pref"))
				{
					try
					{
						XmlNode key_node = node.SelectSingleNode("name");
						XmlNode val_node = node.SelectSingleNode("value");

						string key = cleanup(key_node.InnerText.ToString());
						string val = cleanup(val_node.InnerText.ToString());

						if (key != null && val != null)
							mSettings[key] = val;
					}
					catch(Exception ex)
					{
						System.Diagnostics.Debug.WriteLine(ex.Message);
					}
				}


				return true;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return false;
		}


		//****************
		public bool Save()
		{
			string f = settingFileName();

			// Check if the directory exists, and if not create it
			if (!File.Exists(f))
			{
				string dir = Path.GetDirectoryName(f);
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);
			}

			XmlWriter w = new XmlTextWriter(f, System.Text.Encoding.UTF8);

			w.WriteStartDocument();
			w.WriteStartElement("r-prefs");
			w.WriteAttributeString("version", "1.0");
			
			IDictionaryEnumerator de = mSettings.GetEnumerator();
			while(de.MoveNext())
			{
				w.WriteStartElement("pref");
				
				w.WriteStartElement("name");
				w.WriteString(de.Key.ToString());
				w.WriteEndElement();	// name

				w.WriteStartElement("value");
				w.WriteString(de.Value.ToString());
				w.WriteEndElement();	// value

				w.WriteEndElement();	// pref
			}

			w.WriteEndElement(); // rprefs
			w.WriteEndDocument();
			w.Close();

			return true;
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//******************************
		protected string settingFileName()
		{
			// UserAppDataPath uses the version number (f.ex 1.0.42.1234)
			// so let's just keep the major.minor numbers
			string path = System.Windows.Forms.Application.UserAppDataPath;

			Regex re = new Regex("^(.*[0-9]+\\.[0-9]+)\\.[0-9]+\\.[0-9]+$");

			Match m = re.Match(path);
			if (m.Success)
			{
				CaptureCollection c1 = m.Groups[1].Captures;
				if (c1 != null && c1[0] != null)
					path = c1[0].Value;
			}


			return Path.Combine(path, kPrefFile);
		}


		//********************************
		protected string cleanup(string str)
		// cleanup any trailing or leading space, \n \r \t
		{
			if (str == null || str.Length < 1)
				return str;

			const string ws = " \n\r\t\f";

			while(str.Length > 0 && ws.IndexOf(str[0]) >= 0)
				str = str.Remove(0, 1);

			while(str.Length > 0 && ws.IndexOf(str[str.Length-1]) >= 0)
				str = str.Remove(str.Length-1, 1);

			return str;
		}



		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private const string	kPrefFile = "RPref.xml";

		private	Hashtable		mSettings;


	} // class RPref
} // namespace Alfray.Faf.Utils


//---------------------------------------------------------------
//	[C# Template RM 20041110]
//	$Log: RPref.cs,v $
//---------------------------------------------------------------
