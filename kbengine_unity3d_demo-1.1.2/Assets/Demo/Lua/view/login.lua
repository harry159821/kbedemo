require"FairyGUI";
require"view/WindowBase";
login = fgui.window_class(WindowBase)

function login.Start( param )	
	login.param=param;
	param.win:ConnectLua(login)
	param.win:Show()	
end

function login:OnInit( param )
	
end