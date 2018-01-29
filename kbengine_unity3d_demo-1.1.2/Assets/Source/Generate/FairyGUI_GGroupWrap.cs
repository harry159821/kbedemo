﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_GGroupWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.GGroup), typeof(FairyGUI.GObject));
		L.RegFunction("SetBoundsChangedFlag", SetBoundsChangedFlag);
		L.RegFunction("EnsureBoundsCorrect", EnsureBoundsCorrect);
		L.RegFunction("Setup_BeforeAdd", Setup_BeforeAdd);
		L.RegFunction("Setup_AfterAdd", Setup_AfterAdd);
		L.RegFunction("New", _CreateFairyGUI_GGroup);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("layout", get_layout, set_layout);
		L.RegVar("lineGap", get_lineGap, set_lineGap);
		L.RegVar("columnGap", get_columnGap, set_columnGap);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_GGroup(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FairyGUI.GGroup obj = new FairyGUI.GGroup();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.GGroup.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetBoundsChangedFlag(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.GGroup obj = (FairyGUI.GGroup)ToLua.CheckObject<FairyGUI.GGroup>(L, 1);
				obj.SetBoundsChangedFlag();
				return 0;
			}
			else if (count == 2)
			{
				FairyGUI.GGroup obj = (FairyGUI.GGroup)ToLua.CheckObject<FairyGUI.GGroup>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				obj.SetBoundsChangedFlag(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GGroup.SetBoundsChangedFlag");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnsureBoundsCorrect(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)ToLua.CheckObject<FairyGUI.GGroup>(L, 1);
			obj.EnsureBoundsCorrect();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Setup_BeforeAdd(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)ToLua.CheckObject<FairyGUI.GGroup>(L, 1);
			FairyGUI.Utils.XML arg0 = (FairyGUI.Utils.XML)ToLua.CheckObject<FairyGUI.Utils.XML>(L, 2);
			obj.Setup_BeforeAdd(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Setup_AfterAdd(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)ToLua.CheckObject<FairyGUI.GGroup>(L, 1);
			FairyGUI.Utils.XML arg0 = (FairyGUI.Utils.XML)ToLua.CheckObject<FairyGUI.Utils.XML>(L, 2);
			obj.Setup_AfterAdd(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_layout(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)o;
			FairyGUI.GroupLayoutType ret = obj.layout;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index layout on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lineGap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)o;
			int ret = obj.lineGap;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index lineGap on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_columnGap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)o;
			int ret = obj.columnGap;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index columnGap on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_layout(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)o;
			FairyGUI.GroupLayoutType arg0 = (FairyGUI.GroupLayoutType)ToLua.CheckObject(L, 2, typeof(FairyGUI.GroupLayoutType));
			obj.layout = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index layout on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lineGap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.lineGap = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index lineGap on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_columnGap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGroup obj = (FairyGUI.GGroup)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.columnGap = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index columnGap on a nil value");
		}
	}
}

