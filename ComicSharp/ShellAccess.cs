//*******************************************************************
/*
 *	Project:	Alfray.ComicsSharp
 * 
 *	File:		ShellAccess.cs
 * 
 *	RM (c) 2003
 * 
 */
//*******************************************************************

using System;
using System.Runtime.InteropServices;

//*************************************
namespace Alfray.ComicsSharp
{
	//****************************************************
	/// <summary>
	/// Summary description for ShellAccess.
	/// </summary>
	public class ShellAccess
	{
		// ------- public properties ------

		// ------- public methods --------

		//*****************************************
		public static string GetFolderPath(int folder_csidl)
		{
			try
			{
				String path = new String('\0', 1024);

				uint hresult = SHGetFolderPath(
					IntPtr.Zero,
					folder_csidl,
					IntPtr.Zero,
					SHGFP_TYPE_CURRENT,
					path);

				// cut the string at the first '\0'
				int pos = path.IndexOf('\0');
				if (pos >= 0)
					path = path.Substring(0, pos);

				if (hresult == S_OK)
					return path;
				else
					System.Diagnostics.Debug.Write("SHGetFolderPath error " + hresult.ToString());
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.Fail(ex.ToString());
			}
			return null;
		}

		
		//***********************************
		[DllImport("Shell32.DLL",
			 //EntryPoint="SHGetFolderPathW",
			 SetLastError=false,
			 CharSet=CharSet.Unicode,
//			 ExactSpelling=true,
			 CallingConvention=CallingConvention.StdCall
			 )]
		protected static extern UInt32 SHGetFolderPath(
			IntPtr hwndOwner,		// HWND hwndOwner
			Int32  nFolder,			// int nFolder
			IntPtr hToken,			// HANDLE hToken
			UInt32 dwFlags,			// DWORD dwFlags
			String pszPath);		// LPTSTR pszPath




		public const int CSIDL_FLAG_CREATE = 0x8000;
		public const int CSIDL_ADMINTOOLS = 0x0030;
		public const int CSIDL_ALTSTARTUP = 0x001d;
		public const int CSIDL_APPDATA = 0x001a;
		public const int CSIDL_BITBUCKET = 0x000a;
		public const int CSIDL_CDBURN_AREA = 0x003b;
		public const int CSIDL_COMMON_ADMINTOOLS = 0x002f;
		public const int CSIDL_COMMON_ALTSTARTUP = 0x001e;
		public const int CSIDL_COMMON_APPDATA = 0x0023;
		public const int CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019;
		public const int CSIDL_COMMON_DOCUMENTS = 0x002e;
		public const int CSIDL_COMMON_FAVORITES = 0x001f;
		public const int CSIDL_COMMON_MUSIC = 0x0035;
		public const int CSIDL_COMMON_PICTURES = 0x0036;
		public const int CSIDL_COMMON_PROGRAMS = 0x0017;
		public const int CSIDL_COMMON_STARTMENU = 0x0016;
		public const int CSIDL_COMMON_STARTUP = 0x0018;
		public const int CSIDL_COMMON_TEMPLATES = 0x002d;
		public const int CSIDL_COMMON_VIDEO = 0x0037;
		public const int CSIDL_CONTROLS = 0x0003;
		public const int CSIDL_COOKIES = 0x0021;
		public const int CSIDL_DESKTOP = 0x0000;
		public const int CSIDL_DESKTOPDIRECTORY = 0x0010;
		public const int CSIDL_DRIVES = 0x0011;
		public const int CSIDL_FAVORITES = 0x0006;
		public const int CSIDL_FONTS = 0x0014;
		public const int CSIDL_HISTORY = 0x0022;
		public const int CSIDL_INTERNET = 0x0001;
		public const int CSIDL_INTERNET_CACHE = 0x0020;
		public const int CSIDL_LOCAL_APPDATA = 0x001c;
		public const int CSIDL_MYDOCUMENTS = 0x000c;
		public const int CSIDL_MYMUSIC = 0x000d;
		public const int CSIDL_MYPICTURES = 0x0027;
		public const int CSIDL_MYVIDEO = 0x000e;
		public const int CSIDL_NETHOOD = 0x0013;
		public const int CSIDL_NETWORK = 0x0012;
		public const int CSIDL_PERSONAL = 0x0005;
		public const int CSIDL_PRINTERS = 0x0004;
		public const int CSIDL_PRINTHOOD = 0x001b;
		public const int CSIDL_PROFILE = 0x0028;
		public const int CSIDL_PROFILES = 0x003e;
		public const int CSIDL_PROGRAM_FILES = 0x0026;
		public const int CSIDL_PROGRAM_FILES_COMMON = 0x002b;
		public const int CSIDL_PROGRAMS = 0x0002;
		public const int CSIDL_RECENT = 0x0008;
		public const int CSIDL_SENDTO = 0x0009;
		public const int CSIDL_STARTMENU = 0x000b;
		public const int CSIDL_STARTUP = 0x0007;
		public const int CSIDL_SYSTEM = 0x0025;
		public const int CSIDL_TEMPLATES = 0x0015;
		public const int CSIDL_WINDOWS = 0x0024;

		public const uint SHGFP_TYPE_CURRENT  = 0;   // current value for user, verify it exists
		public const uint SHGFP_TYPE_DEFAULT  = 1;   // default value, may not exist

		public const uint S_OK		= (uint)0x00000000L;
		public const uint S_FALSE	= (uint)0x00000001L;
		public const uint E_FAIL	= unchecked((uint)0x80004005L);

		// ------- private methods ------- 

		// ------- private properties ------- 
	} // class ShellAccess
} // namespace Alfray.ComicsSharp

//*******************************************************************
/*
 *	$Log: ShellAccess.cs,v $
 *	Revision 1.1  2003/08/01 00:51:58  ralf
 *	Added ComicsBook, ComicsBookCollection, Edit form and main form.
 *	
 * 
 */
//*******************************************************************

