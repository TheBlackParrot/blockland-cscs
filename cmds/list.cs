//COMMAND	cmds
//AUTHOR	TheBlackParrot	18701
//INFO	Lists all available CSCS commands
//LIMIT	none
//enabled

function serverCmdCmds(%this) {
	if(!%this.isCSCSAllowed($Pref::CSCS::AllowResetCommand)) {
		%this.logCSCSCommand("cmds","",0);
		return;
	}
	if(!%this.checkCSCSCommandSpam()) {
		return;
	}

	%this.logCSCSCommand("cmds","",1);

	messageClient(%this,'',"\c4Command list:");
	for(%i=0;%i<$CSCS::CommandCount;%i++) {
		%args = "";
		%arg = "";
		%argstr = "";

		%name = $CSCS::Command[%i,name] @ "Command";
		%val = $Pref::CSCS::Allow[%name];

		if(%this.isCSCSAllowed(%val,1)) {
			%c++;

			%args = $CSCS::Command[%i,args];
			while(strLen(%args)) {
				if(stripos(%args,";") != -1) {
					%arg = getSubStr(%args,0,stripos(%args,";"));
				} else {
					%arg = %args;
				}

				if(%argstr $= "") {
					%argstr = "[" @ %arg @ "]";
				} else {
					%argstr = %argstr SPC "[" @ %arg @ "]";
				}

				if(stripos(%args,";") != -1) {
					%args = getSubStr(%args,stripos(%args,";")+1,strLen(%args));
				} else {
					%args = "";
				}
			}
			%argstr = "\c5" @ %argstr @ " ";

			switch(%val) {
				case 0:
					%limitstr = "";
				case 1:
					%limitstr = "<color:aaaaaa>(Admin)";
				case 2:
					%limitstr = "<color:ffbb00>(Super Admin)";
				case 3:
					%limitstr = "<color:00ffff>(Host)";
			}

			messageClient(%this,'',"\c2" @ %c @ ". \c3/" @ $CSCS::Command[%i,name] SPC %argstr @ "\c7--\c6" SPC $CSCS::Command[%i,desc] SPC %limitstr);
		}
	}
}
function serverCmdCommands(%this) { serverCmdCmds(%this); }
function serverCmdCSCS(%this) { serverCmdCmds(%this); }
function serverCmdCmdList(%this) { serverCmdCmds(%this); }