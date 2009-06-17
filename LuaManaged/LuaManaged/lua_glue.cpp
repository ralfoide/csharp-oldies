/**************************************************************************

	Project:	LuaManaged
	File:		lua_glue.cpp

**************************************************************************/

// Functions extracted from lua/src/lua/lua.c

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

extern "C"
{
	#include "lua.h"
	#include "lauxlib.h"
	#include "lualib.h"
}

#define PROGNAME	"LuaManaged"

#ifndef lua_userinit
#define lua_userinit(L)		openstdlibs(L)
#endif


#ifndef LUA_EXTRALIBS
#define LUA_EXTRALIBS	/* empty */
#endif

const luaL_reg lualibs[] =
{
	{"base", luaopen_base},
	{"table", luaopen_table},
	{"io", luaopen_io},
	{"string", luaopen_string},
	{"math", luaopen_math},
	{"debug", luaopen_debug},
	{"loadlib", luaopen_loadlib},
	/* add your libraries here */
	LUA_EXTRALIBS
	{NULL, NULL}
};



void lstop (lua_State *l, lua_Debug *ar)
{
	//(void)ar;  /* unused arg. */
	//lua_sethook(l, NULL, 0, 0);
	//luaL_error(l, "interrupted!");
}


void laction (int i)
{
	//signal(i, SIG_DFL); /* if another SIGINT happens before lstop,
	//					terminate process (default action) */
	//lua_sethook(L, lstop, LUA_MASKCALL | LUA_MASKRET | LUA_MASKCOUNT, 1);
}

void l_message(const char *pname, const char *msg)
{
	//if (pname)
	//	fprintf(stderr, "%s: ", pname);
	//fprintf(stderr, "%s\n", msg);
}

int report(lua_State *L, int status)
{
	const char *msg;
	if (status)
	{
		msg = lua_tostring(L, -1);
		if (msg == NULL) 
			msg = "(error with no message)";
		l_message(PROGNAME, msg);
		lua_pop(L, 1);
	}
	return status;
}

int lcall(lua_State *L, int narg, int clear)
{
	int status;
	int base = lua_gettop(L) - narg;  /* function index */
	lua_pushliteral(L, "_TRACEBACK");
	lua_rawget(L, LUA_GLOBALSINDEX);  /* get traceback function */
	lua_insert(L, base);  /* put it under chunk and args */
//	signal(SIGINT, laction);
	status = lua_pcall(L, narg, (clear ? 0 : LUA_MULTRET), base);
//	signal(SIGINT, SIG_DFL);
	lua_remove(L, base);  /* remove traceback function */
	return status;
}

int docall(lua_State *L, int status)
{
	if (status == 0)
		status = lcall(L, 0, 1);
	return report(L, status);
}

int dostring(lua_State *L, const char *s, const char *name)
{
	return docall(L, luaL_loadbuffer(L, s, strlen(s), name));
}


void openstdlibs(lua_State *l)
{
	const luaL_reg *lib = lualibs;
	for (; lib->func; lib++)
	{
		lib->func(l);  /* open library */
		lua_settop(l, 0);  /* discard any results */
	}
}


int handle_luainit(void)
{
	// const char *init = getenv("LUA_INIT");
	// if (init == NULL) return 0;  /* status OK */
	// else if (init[0] == '@')
	// 	return file_input(init+1);
	// else
	// 	return dostring(init, "=LUA_INIT");

	return 0;
}


struct lua_glue_Smain
{
	const char *str;
	int status;
};


int pmain(lua_State *l)
{
	struct lua_glue_Smain *s = (struct lua_glue_Smain *)lua_touserdata(l, 1);
	int status;

	lua_userinit(l);  /* open libraries */

	status = dostring(l, s->str, "=<exec_string>");

	s->status = status;
	return 0;
}


int lua_glue_exec_string(const char *str)
{
	int status = -1;
	lua_State *l = lua_open();  /* create state */
	if (l != NULL)
	{
		struct lua_glue_Smain s;
		s.str = str;
		status = lua_cpcall(l, &pmain, &s);

		report(l, status);
		lua_close(l);
	}

	return status;
}

// end
/**************************************************************************
	$Log: lua_glue.cpp,v $
	Revision 1.1.1.1  2004/01/21 08:10:21  ralf
	Lua 5.0 as a mixed/managed .Net assembly.
	
**************************************************************************/
