//COMMAND	dfg
//AUTHOR	TheBlackParrot	18701
//INFO	Randomizes all of your bricks' color
//LIMIT	none
//enabled

$Pref::CSCS::DFGLimit = 10000; // extra prefs CAN be defined and exported, the CSCS core takes care of exporting

function serverCmdDFG(%this) {
	if(!%this.isCSCSAllowed($Pref::CSCS::AllowDFGCommand)) {
		// this if statement will check to see if the player is allowed to use the command
		// $Pref::CSCS::Allow[command]Command is the syntax
		%this.logCSCSCommand("dfg","",0);
		return;
	}
	if(!%this.checkCSCSCommandSpam()) {
		// built in spam checker, this applies to all commands that have this if statement
		return;
	}

	// logs that the user could use the command
	%this.logCSCSCommand("dfg","",1);

	%group = "BrickGroup_" @ %this.bl_id;
	if(%this.isAdmin || %group.getCount < $Pref::CSCS::DFGLimit) {
		%count = %group.getCount();
	} else {
		%count = $Pref::CSCS::DFGLimit;
	}

	for(%i=0;%i<%count;%i++) {
		%brick = %group.getObject(%i);
		if(%brick.originColor $= "") {
			%brick.originColor = %brick.colorID;
		}
		%brick.setColor(getRandom(0,4));
	}
}