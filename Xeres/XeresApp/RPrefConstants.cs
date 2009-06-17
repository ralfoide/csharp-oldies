//*******************************************************************
/* 

 		Project:	XeresApp
 		File:		RPrefConstants.cs

*/ 
//*******************************************************************

using System;

//*************************************
namespace Alfray.Xeres.XeresApp
{
	//***************************************************
	/// <summary>
	/// Summary description for RPrefConstants.
	/// </summary>
	public class RPrefConstants
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------

		// UI

		public const string kMainForm = "forms-main";
		public const string kPrefForm = "forms-pref";
		public const string kDebugForm = "forms-debug";

		public const string kLocalPreviewSize = "local-preview-size";

		// Devices

		public const string kVideoRecorderId = "video-recorder-dev-id";

	} // class RPrefConstants
} // namespace Alfray.Xeres.XeresApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RPrefConstants.cs,v $
//	Revision 1.3  2005/03/07 17:00:44  ralf
//	Update. Fixes. Doc.
//	
//	Revision 1.2  2005/03/07 15:40:49  ralf
//	Using hardcoded constants for prefs.
//	
//	Revision 1.1  2005/03/07 07:17:00  ralf
//	RRecorderVideo, implement start, stop
//	RRecorderVideo, capture images in data queue
//	Display recorded images from RRecorderVideo data queue
//	Updating preferences for video recorder device
//	
//---------------------------------------------------------------
