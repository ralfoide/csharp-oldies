;
;	PrintDir XP Installer
;
;
;	Log:
;	20020901	- RM - Created
;
;
;	2002 (c) Alfray Inc.
;
;
;
;	This script compiles with NSIS v1.99
;	NSIS can be downloaded here:
;		http://www.nullsoft.com/free/nsis/
;
;	Documentation regarding the syntax of this script is found in the doc
;	once the installer has been installed. It can also be viewed here:
;		http://www.nullsoft.com/free/nsis/makensis.htm
;
;
;-------------------------------------------------------------------------------------------------

;
; Initializes paths, names, locations, etc.
;

!include PrintDir-Defines.inc

!include "${RM_INST_PATH}\french.nsh"

;
; Name of the Installer
;

Name "PrintDir XP v1.0"


;
; Name of the Installer file being created
;

OutFile "${RM_OUT_PATH}\PrintDir_XP_Installer.exe"


;
;
;-------------------------------------------------------------------------------------------------
;-------------------------------------------------------------------------------------------------
;
;


;
; Where the program is installed
; The user can choose the location, this is the default
; The installer will reuse the previous location if any
;

InstallDir "$PROGRAMFILES\${RM_PROGRAM_FILE_EPP}"
InstallDirRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Alfray\${RM_INST_REGKEY_EPP}" ""

;
; Let the user change the installation directory
;

DirShow show ; (make this hide to not let the user change it)
; DirText "Select the directory to install PrintDir XP in:"
DirText "Choisissez l'emplacement où installer PrintDir XP :"


;
; Some default compiler settings (uncomment and change at will)
;
; SetCompress auto ; (can be off or force)
; SetDatablockOptimize on ; (can be off)
; AutoCloseWindow false ; (can be true for the window go away automatically at end)
; ShowInstDetails hide ; (can be show to have them shown, or nevershow to disable)
; SetDateSave off ; (can be on to have files restored to their orginal date)


;
; CRC check
; This is usually turned ON, but when the Content Browser is included
; in the installer, that's 20 MB of stuff that takes almost half a minute
; to be crc-checked, so in this case we want it off
;

CRCCheck off


;
; custom options:
; blue gradient background, normal non-lama icons
;

BGGradient
Icon			${RM_INST_PATH}\normal-install.ico
UninstallIcon	${RM_INST_PATH}\normal-uninstall.ico
EnabledBitmap	${RM_INST_PATH}\two-check.bmp
DisabledBitmap	${RM_INST_PATH}\two-nocheck.bmp

;
; The license text to be displayed first when the installer is launched
;

; LicenseText		"PrintDir XP License"
LicenseText		"License de PrintDir XP"
LicenseData		${RM_INST_PATH}\license_fr.txt


;
; Name of the component page
; (this also enables it so the user can turn off sections as needed)
;

; RM 20020902 nothing to choose from at all
; ComponentText "Please select the options you want to install"


;
;
;-------------------------------------------------------------------------------------------------
; Utility Routines
;-------------------------------------------------------------------------------------------------
;
;


;------------------------------------------------------------------------------
; GetParent
; input, top of stack  (i.e. C:\Program Files\Poop)
; output, top of stack (replaces, with i.e. C:\Program Files)
; modifies no other variables.
;
; Usage:
;   Push "C:\Program Files\Directory\Whatever"
;   Call GetParent
;   Pop $0
;   ; at this point $0 will equal "C:\Program Files\Directory"


Function GetParent 
	Exch $0 ; old $0 is on top of stack
	Push $1
	Push $2
	StrCpy $1 -1

	; RM 20020412 fix: if left char is already \, don't exit right away
	StrCpy $2 $0 1 $1
	StrCmp $2 "" exit
	IntOp $1 $1 - 1

loop:
	StrCpy $2 $0 1 $1
	StrCmp $2 "" exit
	StrCmp $2 "\" exit
	IntOp $1 $1 - 1
	Goto loop
	
exit:
	StrCpy $0 $0 $1
	Pop $2
	Pop $1
	Exch $0 ; put $0 on top of stack, restore $0 to original value
FunctionEnd


;
;
;-------------------------------------------------------------------------------------------------
; Windows 95 Test -- Refuse to install
;-------------------------------------------------------------------------------------------------
;
;


;---------------
Function .onInit
;---------------

	; This check must be performed before the license is displayed

	Call bwValidatePreviousInstall


	; moved other validations to .onNextPage callback -- RM 20020722
	; this way the license is displayed first


	; init variable to count page number
	StrCpy $9 0 ; we start on page 0


FunctionEnd



;--------------------
Function .onUserAbort
;--------------------

	; MessageBox MB_YESNO "Abort install?" IDYES NoCancelAbort
	MessageBox MB_YESNO "Arréter l'installation ?" IDYES NoCancelAbort
		Abort ; causes installer to not quit.
	NoCancelAbort:

FunctionEnd



;-------------------
Function .onNextPage
;-------------------

	; at this point:
	; page 0 is the first empty page
	; page 1 is the license page
	; page 2 is the options, etc.

	StrCmp $9 1 0 onpIncPageCount

	;
	; Validate OS, Administrator mode and IIS
	;
	; This let the user see the license first, this is checked when
	; "I Agree" is selected.
	;


	Call bwValidateOS
	Call bwValidateAdmin


	;
	; Increment Page Count
	;

	onpIncPageCount:
		IntOp $9 $9 + 1


FunctionEnd


;
;
;-------------------------------------------------------------------------------------------------
; Installation Validity Test -- Administrator rights, FrontPage present, product installed
;-------------------------------------------------------------------------------------------------
;
;


;---------------------------------
Function bwValidatePreviousInstall
;---------------------------------
; Check the the software has already been installed by checking the App Path exe registry
; key. This does not check that the exe is physically present. The user is asked if she
; wants to continue or quit.


	; save $1, which will be modified later
	Push $1

	;
	; Check the key that tells Windows where the app has been installed
	;


	ReadRegStr $1 HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\${RM_INST_EXE_NAME}" ""

	; ReadRegStr may set an error if the key is not present ($1 will be empty too)
	; Cler these errors
	ClearErrors

	StrCmp $1 "" NotInstalledYet

		; MessageBox MB_YESNO|MB_ICONQUESTION "PrintDir XP has already been installed on this computer. Do you want to re-install it right now?" IDYES NotInstalledYet
		MessageBox MB_YESNO|MB_ICONQUESTION "PrintDir XP a déjà été installé sur cet ordinateur. Désirez vous le réinstaller à nouveau ?" IDYES NotInstalledYet
		Quit ; causes installer to quit immediatly if users does not select IDYES


	NotInstalledYet:

	; restore $1
	Pop $1

FunctionEnd



;
;
;-------------------------------------------------------------------------------------------------
;-------------------------------------------------------------------------------------------------
;
;



;--------------------
Function bwValidateOS
;--------------------

	; save $1, which will be modified later
	Push $1


	; ReadRegStr $1 HKEY_LOCAL_MACHINE "Software\Microsoft\Windows\CurrentVersion" "Version"
	; StrCmp $1 "Windows 95" 0 OsIsNot95
	; 	MessageBox MB_OK|MB_ICONSTOP "PrintDir XP does not support Windows 95. Only Windows 98, Me, NT4, 2000 and XP are currently supported."		
	; 	Abort ; causes installer to quit immediatly
	; OsIsNot95:


	ReadRegStr $1 HKEY_LOCAL_MACHINE "Software\Microsoft\Windows NT\CurrentVersion" "CurrentVersion"

	; CurrentVersion is:
	; 5.0 : Windows 2000
	; 5.1 : Windows XP Pro
	; empty if not WinNT

	StrCmp $1 "5.0" OsSuported
	StrCmp $1 "5.1" OsSuported

		MessageBox MB_OK|MB_ICONSTOP "PrintDir XP requier Windows 2000, XP Home ou XP Professional minimum."
		; MessageBox MB_OK|MB_ICONSTOP "PrintDir XP requires Windows 2000, XP Home or XP Professional minimum."
		Quit ; causes installer to quit immediatly

	OsSuported:

	; restore $1
	Pop $1

FunctionEnd



;
;
;-------------------------------------------------------------------------------------------------
;-------------------------------------------------------------------------------------------------
;
;





;--------------------
!macro bwCheckIsAdmin
;--------------------

	; Exchange current $0 with top of the stack
	; I.e. we save old $0 and we get in $0 the verb string

	Exch $0

	; save $1, which will be modified later
	Push $1

	; --- Check that the current user has Administrator priviledges
	; Use the isadmindll.dll for this purpose
	; Usage:
	;	call the DLL, then pop $1. If $1 == "1" the user has administrative priviledge.
	
	SetOutPath "$TEMP"
	
	;
	; Need the isadmindll.dll to perform the test
	;
	
	
	File "${RM_INST_PATH}\isadmindll.dll"
	
	
	;
	; Now test
	;
	
	CallInstDLL "$TEMP\isadmindll.dll" checkIsAdmin
	
	Pop $1
	; ($1 would be "0" (not admin) or "1" (admin) or some other value on error.

	; DEBUG
	; MessageBox MB_OK|MB_ICONSTOP $1
	

	;
	; Delete the temporary file
	;
	
	Delete /REBOOTOK "$TEMP\isadmindll.dll"
	
	
	;
	; Use the result from the admin test
	;
	
	StrCmp $1 "1" bwAdminContinue_${LABEL}	; if is admin, continue
	StrCmp $1 "0" bwNotAdmin_${LABEL}		; not admin, notify and abort
	
	
	; bwAdminError:
	
		MessageBox MB_OK|MB_ICONSTOP "L'installeur ne peut déterminer si vous avez le privilége d'administrateur (erreur $1). Afin de pouvoir $0 PrintDir XP, veuillez exécuter l'installeur en vous logguant depuis un compte administrateur."
		; MessageBox MB_OK|MB_ICONSTOP "The installer could not determine if you have administror priviledges (error $1). In order to $0 PrintDir XP, please run the installer again after logging as Administrator."
		Quit ; causes installer to quit immediatly


	bwNotAdmin_${LABEL}:

		MessageBox MB_OK|MB_ICONSTOP "Vous devez avoir le privilége d'administrateur pour $0 PrintDir XP. Merci de re-$0 apres vous être loggué en tant qu'Administrateur."
		; MessageBox MB_OK|MB_ICONSTOP "You must have administror priviledges to $0 PrintDir XP. Please run the $0er again after logging as Administrator."
		Quit ; causes installer to quit immediatly


	bwAdminContinue_${LABEL}:

	; --------------------------------

	;
	; Finaly revert the output path
	;

	SetOutPath "$INSTDIR"


	; restore $0 and $1
	Pop $1
	Pop $0

	!undef LABEL

!macroend


;-----------------------
Function bwValidateAdmin
;-----------------------

	Push "installer"
	; Push "install"
	!define LABEL install
	!insertmacro bwCheckIsAdmin

FunctionEnd


;--------------------------
Function un.bwValidateAdmin
;--------------------------

	Push "désinstaller"
	; Push "uninstall"
	!define LABEL install
	!insertmacro bwCheckIsAdmin

FunctionEnd



;
;
;-------------------------------------------------------------------------------------------------
;-------------------------------------------------------------------------------------------------
;
;

;-------------------------------------------------------------------------------------------------
Section ""	; section 0 (default section -- mandatory)
			; RM 20020307: removed name so it does not appear in the component list
;-------------------------------------------------------------------------------------------------
	
	SetOutPath "$INSTDIR"
	
	
	;
	; Tell the installer we want to use the All Users\Start Menu\Programs
	; rather than the Current User\Start Menu\Programs.
	;
	
	SetShellVarContext all
	
	
	
	;
	; *** Files for PrintDir XP Application ***
	;
	
	; Get WNT builds
	; Depending on the running platform, keep the correct one
	
	File /oname=${RM_INST_EXE_NAME} "${RM_BUILD_PATH}\${RM_WNT_EXE_NAME}"
	
	
	; Use everything from the installed_material, as is
	; RM 20020320 -- use everything inside this folder

; RM 20020902 nothing for PrintDirXP
;	File /r "${RM_MAT_PATH}\*"
	
	
	;
	; *** DLLs for Windows System ***
	;
	
	; RM 20020320 -- use everything inside this folder
	
; RM 20020902 nothing for PrintDirXP
;	File /r "${RM_DLL_PATH}\*"
	
	
	
	;
	; *** Fonts for Windows ***
	;
	
	; They have been copied by the recursive copy of the folder above.
	; Now install them but only if it's not already present on the system

; RM 20020902	
;	IfFileExists "$WINDIR\Fonts\tahoma.ttf" TahomaExists
;		CopyFiles /FilesOnly "$INSTDIR\tahoma.ttf" "$WINDIR\Fonts\tahoma.ttf" 252
;TahomaExists:
;	
;	IfFileExists "$WINDIR\Fonts\tahomabd.ttf" TahomaBoldExists
;		CopyFiles /FilesOnly "$INSTDIR\tahomabd.ttf" "$WINDIR\Fonts\tahomabd.ttf" 252
;TahomaBoldExists:
	
	
	;
	; *** Write registry entry for EPP file type ***
	;
	; use icon index 1 for the PrintDir XP File icon

; RM 20020902	
;	WriteRegStr HKEY_CLASSES_ROOT ".ep"											 "" "ePicturePro3.Application"
;	WriteRegStr HKEY_CLASSES_ROOT "ePicturePro3.Application"					 "" "PrintDir XP File"
;	WriteRegStr HKEY_CLASSES_ROOT "ePicturePro3.Application\DefaultIcon"		 "" "$INSTDIR\${RM_INST_EXE_NAME},1"
;	WriteRegStr HKEY_CLASSES_ROOT "ePicturePro3.Application\shell\open\command"	 "" '"$INSTDIR\${RM_INST_EXE_NAME}" "%1"'
	
	
	;
	; *** Setup the default template to create new EPP files ***
	;
	; RM 20020320 -- not used for ePP, only SF currently
	;WriteRegStr HKEY_CLASSES_ROOT ".sff\ShellNew" "FileName" "$INSTDIR\Template.sff"
	;
	
	;
	; *** Tell Windows where the app has been installed ***
	;
	
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\${RM_INST_EXE_NAME}" "" "$INSTDIR\${RM_INST_EXE_NAME}"
	
	
	;
	; *** Stuff for the installer/unsinstaller itself ***
	;
	
	WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Alfray\${RM_INST_REGKEY_EPP}" "" "$INSTDIR"
	; WriteRegStr HKEY_LOCAL_MACHINE "Software\Microsoft\Windows\CurrentVersion\Uninstall\${RM_INST_REGKEY_EPP}" "DisplayName" "PrintDir XP Uninstall"
	WriteRegStr HKEY_LOCAL_MACHINE "Software\Microsoft\Windows\CurrentVersion\Uninstall\${RM_INST_REGKEY_EPP}" "DisplayName" "Désinstaller PrintDir XP"
	WriteRegStr HKEY_LOCAL_MACHINE "Software\Microsoft\Windows\CurrentVersion\Uninstall\${RM_INST_REGKEY_EPP}" "UninstallString" '"$INSTDIR\${RM_UNINST_EXE_EPP}"'
	
	
	;
	; *** Stuff for the Shortcut Menu ***
	;

; RM 20020902 use Program Files directly since there's only one entry
	CreateShortCut "$SMPROGRAMS\PrintDir XP.lnk"		"$INSTDIR\${RM_INST_EXE_NAME}"							"" "$INSTDIR\${RM_INST_EXE_NAME}"							0
	
;	CreateDirectory "$SMPROGRAMS\${RM_PROGRAM_MENU_EPP}"
;	ExecShell open "$SMPROGRAMS\${RM_PROGRAM_MENU_EPP}"
;	CreateShortCut "$SMPROGRAMS\${RM_PROGRAM_MENU_EPP}\PrintDir XP.lnk"		"$INSTDIR\${RM_INST_EXE_NAME}"							"" "$INSTDIR\${RM_INST_EXE_NAME}"							0
;	CreateShortCut "$SMPROGRAMS\${RM_PROGRAM_MENU_EPP}\Uninstall PrintDir XP.lnk"	"$INSTDIR\${RM_UNINST_EXE_EPP}"							"" "$INSTDIR\${RM_UNINST_EXE_EPP}"							0
	
	;
	; *** Stuff to place a shortcut on the Desktop ***
	;
	
	MessageBox MB_YESNO|MB_ICONQUESTION "Désirez vous ajouter un lien vers PrintDir XP sur le bureau ?" IDNO NoDesktopShortcut
	; MessageBox MB_YESNO|MB_ICONQUESTION "Do you want a PrintDir XP shortcut on your desktop?" IDNO NoDesktopShortcut
		CreateShortCut "$DESKTOP\PrintDir XP.lnk" "$INSTDIR\${RM_INST_EXE_NAME}" "" "$INSTDIR\${RM_INST_EXE_NAME}" 0
	NoDesktopShortcut:
	
	
	; write out uninstaller
	
	WriteUninstaller "$INSTDIR\${RM_UNINST_EXE_EPP}"
	
	;
	; OK to reboot now
	;
	
	IfRebootFlag 0 noReboot
		Reboot
noReboot:


SectionEnd ; end of default section







;
;
;-------------------------------------------------------------------------------------------------
;-------------------------------------------------------------------------------------------------
;
;


;-------------------------------------------------------------------------------------------------
; uninstall settings/section
;-------------------------------------------------------------------------------------------------

; UninstallText "This will uninstall PrintDir XP from your system"
UninstallText "Ceci désinstallera PrintDir XP de votre ordinateur"



;-------------------------------------------------------------------------------------------------
Section Uninstall
;-------------------------------------------------------------------------------------------------



	;
	; Validate the user is Administrator
	; If the user is not, the IIS uninstall won't work
	;
	
	Call un.bwValidateAdmin


	;
	; Tell the Uninstaller we want to use the All Users\Start Menu\Programs
	; rather than the Current User\Start Menu\Programs.
	;
	
	SetShellVarContext all



	;
	; Uninstall PrintDir XP
	;
	
	
	Delete		 "$INSTDIR\${RM_UNINST_EXE_EPP}"
	DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Alfray\${RM_INST_REGKEY_EPP}"
	DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${RM_INST_REGKEY_EPP}"
	
	DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\${RM_INST_EXE_NAME}"
	
	; Clean up the content
	
	RMDir /r "$INSTDIR"
	
	; Cleanup Shortcuts menu and Desktop shortcut
	
	RMDir /r "$SMPROGRAMS\${RM_PROGRAM_MENU_EPP}"
	
	Delete "$DESKTOP\PrintDir XP.lnk"

SectionEnd ; end of uninstall section



;-------------------------------------------------------------------------------------------------
; eof
;-------------------------------------------------------------------------------------------------



;
; $Log: PrintDir.nsi,v $
; Revision 1.1.1.1  2003/06/08 18:49:07  ralf
; Working revision 1.0
;
; Revision 1.9  2002/08/31 01:46:36  ralf
; Fixes for Certification
;
; Revision 1.8  2002/08/27 20:31:58  ralf
; Upgraded to WMV 7.1.1
;
; Revision 1.7  2002/08/26 03:51:51  marc
; Disabled Ansi build for W98
;
; Revision 1.6  2002/08/22 20:57:53  ralf
; Support for All Users
;
; Revision 1.5  2002/08/22 02:35:17  ralf
; Installer fixes for certification.
; Added WMF 7.1, W2k/W.XP minimum, etc.
;
; Revision 1.4  2002/05/10 00:40:16  ralf
; Changed GIF to GIF Image and Still GIF (vs Animated)
;
; Revision 1.3  2002/05/06 18:42:02  ralf
; - merged FrontPage option in main installer
; - installing either W98/Ansi or WNT/Unicode
; - MSVCRT test.
;
; Revision 1.2  2002/03/21 02:52:41  ralf
; Missing NT4
;
; Revision 1.1  2002/03/20 23:36:20  ralf
; ePP3 installer
;
;