//*******************************************************************
/* 

 		Project:	AcqLib
 		File:		RVideoHandler.cs

*/ 
//*******************************************************************

using System;
using System.IO;
using System.Drawing;
using System.Collections;

using WIA;

//*************************************
namespace Alfray.Faf.AcqLib
{
	//***************************************************
	/// <summary>
	/// Summary description for RVideoHandler.
	/// </summary>
	public class RVideoHandler
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------

		//***********************
		public struct SCameraInfo
		{
			public int mIndex;
			public string mName;
			public string mSystemId;
			public WIA.Device mDevice;
		}

		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//********************
		public RVideoHandler()
		{
		}

		//**************************
		public void Start(RILog log)
		{
			if (log == null)
				mLog = new RVoidLog();
			else
				mLog = log;
			
			mLog.Log("Started");

			mDeviceMan = new WIA.DeviceManager();

			mDeviceMan.OnEvent += new _IDeviceManagerEvents_OnEventEventHandler(this.OnEvent);

			register(null);
		}

		//****************
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

		//*****************************************************************
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


		//**********************************
		public ArrayList EnumerateCameras()
		{
			ArrayList camera_list = new ArrayList();
			mLog.Log("\nenumerateCameras");

			mLog.Log("Devices Present = " + mDeviceMan.DeviceInfos.Count.ToString());
			
			try
			{
				int index = 0;

				if (mDeviceMan.DeviceInfos.Count >= 1)
				{
					// mLog.Log("Connecting to device 0...");
					IEnumerator e = mDeviceMan.DeviceInfos.GetEnumerator();

					while(e.MoveNext())
					{
						WIA.Device dev = (e.Current as WIA.DeviceInfo).Connect();

						if (dev.Type == WIA.WiaDeviceType.VideoDeviceType)
						{
							mLog.Log(String.Format("Device {0} is a Video", index));

							list_properties(dev.Properties);
							list_commands(dev);
							list_events(dev);

							SCameraInfo ci = new SCameraInfo();
							ci.mIndex = index++;
							ci.mName = get_prop(dev.Properties, "Name");
							ci.mSystemId = get_prop(dev.Properties, "Remote Device ID");
							ci.mDevice = dev;
							camera_list.Add(ci);
						}
						/*
						else if (dev.Type == WIA.WiaDeviceType.CameraDeviceType)
						{
							mLog.Log(String.Format("Device {0} is a Camera", index));

							list_properties(dev.Properties);

							SCameraInfo ci = new SCameraInfo();
							ci.mIndex = index++;
							ci.mName = get_prop(dev.Properties, "Name");
							ci.mSystemId = get_prop(dev.Properties, "Remote Device ID");
							ci.mDevice = dev;
							camera_list.Add(ci);
						}
						*/
						else
						{
							mLog.Log(String.Format("Connected device is not a camera ({1})",
								dev.Type.ToString()));
							dev = null;
						}
					}
				}
			}
			catch(Exception ex)
			{
				mLog.Log(ex.ToString());
			}

			return camera_list;
		}	


		//***********************************
		public Image GetImage(SCameraInfo ci)
		{
			RTraceTimer rt = new RTraceTimer(mLog, "GetImage");

			Image img = null;

			try
			{
				string ctp = WIA.CommandID.wiaCommandTakePicture;
				string take_picture = "{AF933CAC-ACAD-11D2-A093-00C04F72DC3C}";

				WIA.Item item = ci.mDevice.ExecuteCommand(take_picture);

				rt.CheckPoint("ExecuteCommand");

				WIA.ImageFile img_file = item.Transfer(WIA.FormatID.wiaFormatBMP) as WIA.ImageFile;

				rt.CheckPoint("Transfer");

				mLog.Log(String.Format("ImgFile: {0}x{1}x{2}",
					img_file.Width, img_file.Height, img_file.PixelDepth));

				rt.SetPoint();

				WIA.Vector vector = img_file.FileData;
				object obj = vector.get_BinaryData();
				byte [] bin = (byte []) obj;

				rt.CheckPoint("GetBinaryData");

				MemoryStream ms = new MemoryStream(bin);
				img = new Bitmap(ms);

				rt.CheckPoint("Make Bitmap");
			}
			catch(Exception ex)
			{
				mLog.Log("GetImage:" + ex.ToString());
			}

			rt.EndTotal();
			return img;
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------

		//***********************************
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


		//*************************************
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


		
		//-------------------------------------------


		private void list_commands(WIA.Device dev)
		{
			mLog.Log("-- Command count: " + dev.Commands.Count.ToString());

			IEnumerator e = dev.Commands.GetEnumerator();
			for(int i = 0; e.MoveNext(); i++)
			{
				WIA.DeviceCommand dc = e.Current as WIA.DeviceCommand;
				mLog.Log(String.Format("  Command [{0}] = '{1}', id='{2}, desc={3}",
					i, dc.Name, dc.CommandID, dc.Description));
			}
		}


		private void list_events(WIA.Device dev)
		{
			mLog.Log("-- Event count: " + dev.Events.Count.ToString());

			IEnumerator e = dev.Events.GetEnumerator();
			for(int i = 0; e.MoveNext(); i++)
			{
				WIA.DeviceEvent ev = e.Current as WIA.DeviceEvent;
				mLog.Log(String.Format("  Event [{0}] = '{1}', type='{2}', id='{3}, desc={4}",
					i, ev.Name, ev.Type.ToString(), ev.EventID, ev.EventID));
			}
		}


		//-------------------------------------------

/*		
		private void list_items(WIA.Device dev)
		{
			string [] names = { "Item Name", "Item Size", "Thumbnail Data", "File Time Count", "Item Time Stamp" };

			IEnumerator e = dev.Items.GetEnumerator();
			for(int i = 0; e.MoveNext(); i++)
			{
				WIA.Item item = e.Current as WIA.Item;

				mLog.Log(String.Format("Item[{0}]", i));
				list_properties(item.Properties);

				display_props(names, item);

				if (i == 3)
				{
					mLog.Log("WARNING: only displaying 3 items for tests");
					break;
				}
			}
		}
*/
		
		private string get_prop(WIA.Properties properties, string prop_name)
		{
			IEnumerator e = properties.GetEnumerator();
			for(int i = 0; e.MoveNext(); i++)
			{
				WIA.Property p = (e.Current as WIA.Property);

				if (p.Name == prop_name)
				{
					return p.get_Value().ToString();
				}
			}

			return "";
		}

		
		private void list_properties(WIA.Properties properties)
		{
			IEnumerator e = properties.GetEnumerator();
			for(int i = 0; e.MoveNext(); i++)
			{
				WIA.Property p = (e.Current as WIA.Property);

				mLog.Log(String.Format(" {0} = {1}{2}{3}", p.Name, p.get_Value(),
					get_prop_type(p.Type),
					p.IsReadOnly ? ", R/O" : ""));
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

		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private RILog mLog;
		private WIA.DeviceManager mDeviceMan;

		private string [,] mEvents = 
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

	} // class RVideoHandler
} // namespace Alfray.Faf.AcqLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log$
//---------------------------------------------------------------
