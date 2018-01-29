require"FairyGUI";
WindowBase = fgui.window_class()

--构建函数
function WindowBase:ctor()	
end

function WindowBase:Close(param)
	-- body
	UIManager .GetInstance():Close(param)
end
