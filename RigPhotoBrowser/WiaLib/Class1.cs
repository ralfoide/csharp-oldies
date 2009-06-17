using System;

using System.IO;
using System.Drawing;
using System.Collections;

using WIA;

namespace Alfray.Rig.AcqLib
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Class1
	{
		public Class1()
		{
			//
			// TODO: Add constructor logic here
			//

			mImageList = new Hashtable();
		}


		public void Start(RILog log)
		{
			mLog = log;
			mLog.Log("Started");

			mDeviceMan = new WIA.DeviceManager();

			mDeviceMan.OnEvent += new _IDeviceManagerEvents_OnEventEventHandler(this.OnEvent);

			register(null);
		}

		public void Stop()
		{
			if (mDeviceMan != null)
			{
				unregister(null);
			}

			if (mLog != null)
			{
				mLog.Log("Stopped");
				mLog = null;
			}
		}


		public void OnEvent(string eventID, string deviceID, string itemID)
		{
			string s = eventID;

			for(int i = 0, k = (mEvents.Length / 2); i < k; i++)
			{
				if (s == mEvents[i,0])
				{
					s = mEvents[i,1];
					break;
				}
			}

			mLog.Log(String.Format("OnEvent: EventID: {1} {2}, DeviceID: {3}, ItemID: {4}",
				s, eventID, deviceID, itemID));

		}


		public void Test2()
		{
			mLog.Log("Test2");
			RILog log = mLog;
			//Stop();
			//Start(log);
			mLog.Log("Devices Present = " + mDeviceMan.DeviceInfos.Count.ToString());

			WIA.CommonDialog cd = new WIA.CommonDialog();

			WIA.Device dev = null;
			
			try
			{
				if (mDeviceMan.DeviceInfos.Count == 1)
				{
					mLog.Log("Connecting to device 0...");
					IEnumerator e = mDeviceMan.DeviceInfos.GetEnumerator();

					while(dev == null && e.MoveNext())
					{
						dev = (e.Current as WIA.DeviceInfo).Connect();

						if (dev.Type != WIA.WiaDeviceType.CameraDeviceType)
						{
							mLog.Log(String.Format("Connected device is not a camera ({0}, {1})",
								(int)dev.Type, dev.Type.ToString()));
							dev = null;
						}
					}
				}

				if (dev == null)
				{
					mLog.Log("ShowSelectDevice -- not available");
					dev = cd.ShowSelectDevice(WIA.WiaDeviceType.CameraDeviceType, false, false);
				}

				if (dev != null)
				{
					mLog.Log(dev.ToString());
				}

				/*if (dev != null)
				{
					mLog.Log("ShowAcquisitionWizard");
					cd.ShowAcquisitionWizard(dev);
				}
				*/

				list_items(dev);

			}
			catch(Exception ex)
			{
				mLog.Log(ex.ToString());
			}
		}	


		private void register(WIA.Device dev)
		{
			string d = "*";

			if (dev != null)
				d = dev.DeviceID;

			for(int i = 0, k = (mEvents.Length / 2); i < k; i++)
			{
				try
				{
					mDeviceMan.RegisterEvent(mEvents[i,0], d);
					mLog.Log("Register event: " + mEvents[i,1]);
				}
				catch(Exception)
				{
					mLog.Log("Register event: " + mEvents[i,1] + " failed");
				}
			}
		}


		private void unregister(WIA.Device dev)
		{
			string d = "*";

			if (dev != null)
				d = dev.DeviceID;

			for(int i = 0, k = (mEvents.Length / 2); i < k; i++)
			{
				try
				{
					mDeviceMan.UnregisterEvent(mEvents[i,0], d);
					mLog.Log("Unregister event: " + mEvents[i,1]);
				}
				catch(Exception)
				{
					mLog.Log("Unregister event: " + mEvents[i,1] + " failed");
				}
			}
		}


		private void list_items(WIA.Device dev)
		{
			string [] names = { "Item Name", "Item Size", "Thumbnail Data", "File Time Count", "Item Time Stamp" };

			IEnumerator e = dev.Items.GetEnumerator();
			for(int i = 0; e.MoveNext(); i++)
			{
				WIA.Item item = e.Current as WIA.Item;

				mLog.Log(String.Format("Item[{0}]", i));
				list_properties(item);

				display_props(names, item);
				get_thumbnail(item);

				if (i == 3)
				{
					mLog.Log("WARNING: only displaying 3 items for tests");
					break;
				}
			}
		}


		private void list_properties_sample(WIA.Item item)
		{
			/*
			For Each p In dev.Properties
				s = p.Name & "(" & p.PropertyID & ") = "
				If p.IsVector Then
					s = s & "[vector of data]"
				Else
					s = s & p.Value
					If p.SubType <> UnspecifiedSubType Then       
    					If p.Value <> p.SubTypeDefault Then
    						s = s & "(Default = " & p.SubTypeDefault & ")"
    					End If
					End If
				End If

				If p.IsReadOnly then
					s= s & " [READ ONLY]"
				else
					Select Case p.SubType
					Case FlagSubType
						s = s & " [ valid flags include:"
						For i = 1 To p.SubTypeValues.Count
							s = s & p.SubTypeValues(i)
							If i <> p.SubTypeValues.Count Then
								s = s & ", "
							End If
						Next
						s = s & " ]"
					Case ListSubType
						s = s & " [ valid values include:"
						For i = 1 To p.SubTypeValues.Count
							s = s & p.SubTypeValues(i)
							If i <> p.SubTypeValues.Count Then
								s = s & ", "
							End If
						Next
						s = s & " ]"
					Case RangeSubType
						s = s & " [ valid values in the range from " & _
							p.SubTypeMin & " to " & p.SubTypeMax & _
							" in increments of " & p.SubTypeStep & " ]"
					Case Else 'UnspecifiedSubType
					End Select
				End If

				MsgBox s
			Next
			*/
		}


		private void list_properties(WIA.Item item)
		{
			IEnumerator e = item.Properties.GetEnumerator();
			for(int i = 0; e.MoveNext(); i++)
			{
				WIA.Property p = (e.Current as WIA.Property);

				mLog.Log(String.Format(" {0} = {1}{2}{3}", p.Name, p.get_Value(),
					get_prop_type(p.Type),
					p.IsReadOnly ? ", R/O" : ""));
			}
		}


		private void display_props(string [] names, WIA.Item item)
		{
			IEnumerator e = item.Properties.GetEnumerator();
			for(int i = 0; e.MoveNext(); i++)
			{
				WIA.Property p = (e.Current as WIA.Property);

				if (Array.IndexOf(names, p.Name) >= 0)
				{
					mLog.Log(String.Format(" {0} = {1}{2}{3}", p.Name, p.get_Value(),
						get_prop_type(p.Type),
						p.IsReadOnly ? ", R/O" : ""));
				}
			}
		}


		private string get_prop_type(int t)
		{
			string s = t.ToString();

			try
			{
				if (t >= 0 && t < 120)
					s = ((WIA.WiaPropertyType) t).ToString();

				if (t >= 1000 && t <= 1106)
					s = ((WIA.WiaImagePropertyType) t).ToString();
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return ", type: " + s;
		}


		private void get_thumbnail(WIA.Item item)
		{
			// Note: C# only automatically marshalls "System.Object" to a COM Variant.
			object name = "Thumbnail Data";
			WIA.Property td = item.Properties.get_Item(ref name);

			name = "Item Name";
			WIA.Property nm = item.Properties.get_Item(ref name);

			name = "Thumbnail Width";
			WIA.Property tw = item.Properties.get_Item(ref name);

			name = "Thumbnail Height";
			WIA.Property th = item.Properties.get_Item(ref name);

			if (td != null && tw != null && th != null && nm != null)
			{
				int w = Convert.ToInt32(tw.get_Value());
				int h = Convert.ToInt32(th.get_Value());

				string item_name = nm.get_Value().ToString();

				WIA.Vector v = (WIA.Vector) td.get_Value();
	
				// Image i = new Bitmap( v.BinaryData

				object o1 = v.get_BinaryData();
				byte [] b = (byte []) o1;
				mLog.Log(String.Format("  thmb data: {0} bytes, type: {1}", 
					b.Length,
					b.GetType().ToString()));

				// MemoryStream ms = new MemoryStream(b);

				string file_name = @"c:\temp\thumb_" + item_name;

				FileStream fs = new FileStream(file_name, FileMode.Create);
				BinaryWriter bw = new BinaryWriter(fs);
				bw.Write(b);
				bw.Close();
				fs.Close();
				mLog.Log("  thumbnail: " + file_name);


				//Image img = new Bitmap(ms);
				//mImageList.Add(item_name, img);



				object o2 = v.get_Picture(w, h);
				mLog.Log(o2.GetType().ToString());
				mLog.Log(o2.ToString());
			}
		}


		// -----------------

		RILog mLog;
		WIA.DeviceManager mDeviceMan;

		string [,] mEvents = 
			{
				{ WIA.EventID.wiaEventDeviceConnected, "wiaEventDeviceConnected" },
				{ WIA.EventID.wiaEventDeviceDisconnected, "wiaEventDeviceDisconnected" },
				{ WIA.EventID.wiaEventItemCreated, "wiaEventItemCreated" },
				{ WIA.EventID.wiaEventItemDeleted, "wiaEventItemDeleted" },
				{ WIA.EventID.wiaEventScanImage, "wiaEventScanImage" },
				{ WIA.EventID.wiaEventScanPrintImage, "wiaEventScanPrintImage" },
				{ WIA.EventID.wiaEventScanFaxImage, "wiaEventScanFaxImage" },
				{ WIA.EventID.wiaEventScanOCRImage, "wiaEventScanOCRImage" },
				{ WIA.EventID.wiaEventScanEmailImage, "wiaEventScanEmailImage" },
				{ WIA.EventID.wiaEventScanFilmImage, "wiaEventScanFilmImage" },
				{ WIA.EventID.wiaEventScanImage2, "wiaEventScanImage2" },
				{ WIA.EventID.wiaEventScanImage3, "wiaEventScanImage3" },
				{ WIA.EventID.wiaEventScanImage4, "wiaEventScanImage4" }
			};

		Hashtable mImageList;
	}
}
