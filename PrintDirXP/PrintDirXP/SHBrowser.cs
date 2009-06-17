using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

namespace Microbion.SHBrowseFolders
{
	/// <summary>
	/// Class to call the Shell API function SHBrowseForFolder.
	/// </summary>
	/// <remarks>This class uses pointers and therefore is marked as unsafe. Compile with the /unsafe
	/// compiler option.
	/// <para>This class is copyright (c) Steve Pedler and Microbion Software, but may be freely used
	///  and distributed.</para>
	/// <para>For more details see the Microbion web site at http://www.microbion.co.uk
	/// </para></remarks>
	unsafe public class SHBrowser
	{
		// declarations for API functions
		[DllImport("shell32.dll")]
			static extern int SHBrowseForFolder(browseInfo* br);
		[DllImport("shell32.dll")]
			static extern bool SHGetPathFromIDList(int pidl, byte* pszPath);
		[DllImport("ole32.dll")]
			static extern void CoTaskMemFree(void* pv);

		// declares an API structure - element names are from the Windows SDK
		private struct browseInfo 
		{
			public int hwndOwner;
			public int pidlRoot;
			public byte* pszDisplayName;
			public byte* lpszTitle;
			public uint ulFlags;
			public int lpfn;
			public int lparam;
			public int imImage;
		}

		// class-level fields
		private browseInfo brinfo;
		private string brtitle;

		/// <summary>
		/// The constructor for the class.
		/// </summary>
		/// <remarks>The constructor initialises the required API structure with default values.</remarks>
		public SHBrowser()
		{
			brinfo.hwndOwner = 0;
			brinfo.pidlRoot = 0;
			brinfo.pszDisplayName = (byte*)0;
			brinfo.lpszTitle = (byte*)0;
			brinfo.ulFlags = 0;
			brinfo.lpfn = 0;
			brinfo.lparam = 0;
			brinfo.imImage = 0;
		}

		/// <summary>
		/// This method is called to browse for a folder (plus 3 overloads).
		/// </summary>
		/// <returns>Returns a string with a fully-qualified path name on success, or an empty 
		/// string on failure; an empty string is also returned if the user clicks the Cancel button.</returns>
		/// <remarks>Overloaded. This version uses default values; the dialog title is set to
		/// "Choose a folder:"</remarks>
		public string DoBrowse() {				// no parameters, so provide defaults
			brtitle = "Choose a folder:";
			return BrowseIt();
		}

		/// <summary>
		/// Overloaded method to browse for a folder.
		/// </summary>
		/// <param name="title">The title to be displayed in the dialog box.</param>
		/// <returns>Returns a string with a fully-qualified path name on success, or an empty 
		/// string on failure; an empty string is also returned if the user clicks the Cancel button.</returns>
		public string DoBrowse(string title) {	// title only
			brtitle = title;
			return BrowseIt();
		}

		/// <summary>
		/// Overloaded method to browse for a folder.
		/// </summary>
		/// <param name="title">The title to be displayed in the dialog box.</param>
		/// <param name="flags">A bit field giving the flags to be set which govern the behaviour
		/// of the dialog box. Set to zero by default and ignored in this version of the class.</param>
		/// <returns>Returns a string with a fully-qualified path name on success, or an empty 
		/// string on failure; an empty string is also returned if the user clicks the Cancel button.</returns>
		public string DoBrowse(string title, int flags) {	// owner handle and flags
			brtitle = title;
			// brinfo.ulFlags = (uint)flags;
			return BrowseIt();
		}

		/// <summary>
		/// Overloaded method to browse for a folder.
		/// </summary>
		/// <param name="title">The title to be displayed in the dialog box.</param>
		/// <param name="flags">A bit field giving the flags to be set which govern the behaviour
		/// of the dialog box. Set to zero by default and ignored in this version of the class.</param>
		/// <param name="handle">The handle of the window owning the dialog box. Set to zero by default.</param>
		/// <returns>Returns a string with a fully-qualified path name on success, or an empty 
		/// string on failure; an empty string is also returned if the user clicks the Cancel button.</returns>
		public string DoBrowse(string title, uint flags, IntPtr handle) {	// title, flags, and owner handle
			brtitle = title;
			// brinfo.ulFlags = (uint)flags;
			brinfo.hwndOwner = (int)handle;
			return BrowseIt();
		}

		/// <summary>
		/// Private method which actually calls the Shell API function.
		/// </summary>
		/// <returns>Returns a string with a fully-qualified path name on success, or an empty 
		/// string on failure; an empty string is also returned if the user clicks the Cancel button.</returns>
		private string BrowseIt() {
			string showPath;			// the fully-qualified path is returned in this string
			int pIDL;					// the PIDL structure returned from SHBrowseForFolders API call
			bool bSuccess = false;		// indicates if we successfully got a folder
			ASCIIEncoding enc = new ASCIIEncoding();	// converts Unicode to 8-bit ASCII and vice-versa
			byte[] dialogTitle;							// buffer for the dialog title
			byte[] pathName = new byte[256];			// buffer to hold the display name (folder name)
			byte[] pathBuffer = new byte[256];			// buffer to hold the fully-qualified path

			showPath = "";
			dialogTitle = enc.GetBytes(brtitle);		// convert .NET string to API buffer
			fixed (byte* pname = pathName)				// get a pointer to this buffer which we must supply
			{
				fixed(byte* pTitle = dialogTitle)		// and a pointer to the title buffer
				{
					try {
						// complete the browseInfo structure initialisation
						brinfo.pszDisplayName = pname;
						brinfo.lpszTitle = pTitle;

						fixed (browseInfo* brp = &brinfo)		// get a pointer to the browseInfo
							pIDL = SHBrowseForFolder(brp);		// call the shell and return a PIDL
						fixed (byte* pbuffer = pathBuffer) {		// get a pointer to the buffer for the path
							if (pIDL != 0) {
								bSuccess = SHGetPathFromIDList(pIDL, pbuffer);		// get the path name from the ID list
								CoTaskMemFree((void*)pIDL);				// free memory used by the shell
								showPath = enc.GetString(pathBuffer);	// convert to a string from the byte array
							}
						}
					}
					catch (System.Exception e) {
						MessageBox.Show("Exception in SHBrowser.cs: " + e.Message, "Browse for Folders exception",
							MessageBoxButtons.OK, MessageBoxIcon.Stop);
					}
				}
			}
			if (bSuccess)
				return showPath;
			else
				return "";
		}
	}
}
