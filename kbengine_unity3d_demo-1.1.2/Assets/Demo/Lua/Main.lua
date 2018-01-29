--主入口函数。从这里开始lua逻辑
require "kbe/KBEngine"
function Main(gameobject)					
	print("logic start")
	KBEngineLua.InitEngine()
	
	local ui_manager=	UIManager.GetInstance()	
	ui_manager:OpenWindow("ui/login/login","bg","view/login",UIManager.ShowType.IntToEnum(1000),false,nil)
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end