//COMMAND	dfgoff
//AUTHOR	TheBlackParrot	18701
//INFO	Reverses the effects of /dfg
//LIMIT	none
//enabled

function serverCmdDFGOff(%this) {
	if(!%this.isCSCSAllowed($Pref::CSCS::AllowDFGOffCommand)) {
		%this.logCSCSCommand("dfgoff","",0);
		return;
	}
	if(!%this.checkCSCSCommandSpam()) {
		return;
	}

	%this.logCSCSCommand("dfgoff","",1);

	%group = "BrickGroup_" @ %this.bl_id;
	if(%this.isAdmin || %group.getCount < $Pref::CSCS::DFGLimit) {
		%count = %group.getCount();
	} else {
		%count = $Pref::CSCS::DFGLimit;
	}

	for(%i=0;%i<%count;%i++) {
		%brick = %group.getObject(%i);
		if(%brick.originColor !$= "") {
			%brick.setColor(%brick.originColor);
			%brick.originColor = "";
		}
	}
}