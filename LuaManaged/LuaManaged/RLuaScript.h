/**************************************************************************

	Project:	RLuaScript.h
	File:		AssemblyInfo.cpp

**************************************************************************/


#pragma once

#include "lua_glue.h"

#using <mscorlib.dll>

using namespace System;
using namespace System::Runtime::InteropServices;	// for String Marshalling


namespace Alfray
{
namespace LuaManaged
{
	public __gc class RLuaScript
	{
	public:
		RLuaScript()
		{
		}

		~RLuaScript()
		{
		}

		int ExecuteString(System::String __gc * in_str)
		{
			const char* c_str = getSystemStringAnsi(in_str);

			int status = -1;
			
			try
			{
				status = lua_glue_exec_string(c_str);
			}
			catch(System::Exception * ex)
			{
				System::Diagnostics::Debug::WriteLine(ex->Message);
			}
			catch(...)
			{
				System::Diagnostics::Debug::WriteLine("Unknown C++ exception");
			}

			freeString((void *)c_str);

			return status;
		}



	private:

		const char* getSystemStringAnsi(System::String __gc * in_str)
		{
			// To convert a System::String to a bwchar pointer, read this:
			// http://www.msdnaa.net/Resources/Display.aspx?ResID=1002
			// returns a copy of the string
			return (const char*) (Marshal::StringToHGlobalAnsi(in_str)).ToPointer();
		}


		const wchar_t* getSystemStringUnicode(System::String __gc * in_str)
		{
			// To convert a System::String to a bwchar pointer, read this:
			// http://www.msdnaa.net/Resources/Display.aspx?ResID=1002
			// returns a copy of the string
			return (const wchar_t*) (Marshal::StringToHGlobalUni(in_str)).ToPointer();
		}

		void freeString(void *in_str)
		{
			// free marshalled string
			Marshal::FreeHGlobal(System::IntPtr(in_str));
		}

	};
} // namespace LuaManaged
} // namespace Alfray

/**************************************************************************
	$Log: RLuaScript.h,v $
	Revision 1.2  2004/01/21 19:11:22  ralf
	Try/catch
	
	Revision 1.1.1.1  2004/01/21 08:10:21  ralf
	Lua 5.0 as a mixed/managed .Net assembly.
	
**************************************************************************/
