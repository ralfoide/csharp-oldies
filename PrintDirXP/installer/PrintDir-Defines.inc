;
;	PrintDir XP DEFINES
;
;
;	Created:	2002/09/02
;	By:			RM
;
;	2002 (c) Alfray Inc
;
;
;	This file is included by PrintDir.nsi
;	See comments in these files.
;
;-------------------------------------------------------------------------------------------------

;
; Location of the Installer files being created
;

!define RM_OUT_PATH .


;
; Path where the Executable is located
; Can be relative to the NSI or absolute
;

!define RM_BUILD_PATH ..\PrintDirXP\bin\Release

;
; Name of the exe file to be used by the installer
; and
; Name of the exe file once installed
;

!define RM_W98_EXE_NAME   PrintDirXP.exe
!define RM_WNT_EXE_NAME   PrintDirXP.exe
!define RM_INST_EXE_NAME  PrintDirXP.exe

;
; Path where the installed material (doc, tips, etc.) is located
; Can be relative to the NSI or absolute
;

!define RM_MAT_PATH .\installed_material
;
; Path where the installer components are located
; The current components are the installer's text license and the icons to be used 
;

!define RM_INST_PATH .\installer_components

;
; Path where the Windows DLLs to be used are located
;

!define RM_DLL_PATH .\windows_dlls

;
; Name of the Uninstallers
;

!define RM_UNINST_EXE_EPP		"Uninstall-PrintDirXP.exe"
!define RM_INST_REGKEY_EPP		"PrintDir XP"
!define RM_PROGRAM_FILE_EPP		"PrintDir XP"
!define RM_PROGRAM_MENU_EPP		"PrintDir XP"




;-------------------------------------------------------------------------------------------------
; eof
;-------------------------------------------------------------------------------------------------

;
; $Log: PrintDir-Defines.inc,v $
; Revision 1.1.1.1  2003/06/08 18:49:07  ralf
; Working revision 1.0
;
; Revision 1.6  2002/08/31 01:46:36  ralf
; Fixes for Certification
;
; Revision 1.5  2002/08/28 23:03:25  ralf
; Installer's material directory name changed
;
; Revision 1.4  2002/08/08 18:51:35  marc
; Upgrading new path on Godzilla.
;
; Revision 1.3  2002/05/14 00:01:05  ralf
; ePP3 trialware without limitations
;
; Revision 1.2  2002/05/06 18:42:02  ralf
; - merged FrontPage option in main installer
; - installing either W98/Ansi or WNT/Unicode
; - MSVCRT test.
;
; Revision 1.1  2002/03/20 23:36:20  ralf
; ePP3 installer
;
;