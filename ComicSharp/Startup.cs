//*******************************************************************
/*
 *	Project:	Alfray.ComicsSharp
 * 
 *	File:		Startup.cs
 * 
 *	RM (c) 2003
 * 
 */
//*******************************************************************

using System;
using System.Windows.Forms;

//*************************************
namespace Alfray.ComicsSharp
{
	//****************************************************
	/// <summary>
	/// Summary description for Startup.
	/// </summary>
	public class Startup
	{
		// ----- public properties ----

		public static FormListAndStatus MainForm
		{
			get
			{
				return mMainForm;
			}
		}

		// ----- private properties ----

		private static FormListAndStatus mMainForm;

		// ----- public methods ----

		//*************************************
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();	// RM 20030807
			mMainForm = new FormListAndStatus();
			Application.Run(mMainForm);
			mMainForm = null;
		}
	
	} // class Startup
} // namespace Alfray.ComicsSharp

//*******************************************************************
/*
 *	$Log: Startup.cs,v $
 *	Revision 1.3  2003/08/12 14:11:34  ralf
 *	Enabled XP Visual Themes
 *	
 *	Revision 1.2  2003/08/04 21:37:27  ralf
 *	Upload with abort/feedback, active book, etc.
 *	
 *	Revision 1.1  2003/08/01 00:51:59  ralf
 *	Added ComicsBook, ComicsBookCollection, Edit form and main form.
 *	
 * 
 */
//*******************************************************************

