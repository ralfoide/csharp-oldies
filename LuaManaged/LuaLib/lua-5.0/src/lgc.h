/*
** $Id: lgc.h,v 1.1.1.1 2004/01/21 08:10:20 ralf Exp $
** Garbage Collector
** See Copyright Notice in lua.h
*/

#ifndef lgc_h
#define lgc_h


#include "lobject.h"


#define luaC_checkGC(L) { lua_assert(!(L->ci->state & CI_CALLING)); \
	if (G(L)->nblocks >= G(L)->GCthreshold) luaC_collectgarbage(L); }


void luaC_separateudata (lua_State *L);
void luaC_callGCTM (lua_State *L);
void luaC_sweep (lua_State *L, int all);
void luaC_collectgarbage (lua_State *L);
void luaC_link (lua_State *L, GCObject *o, lu_byte tt);


#endif
