//COMMAND	reset
//AUTHOR	TheBlackParrot	18701
//INFO	Resets the minigame the player is in, also applies to default minigames
//LIMIT	super admin
//enabled

function serverCmdReset(%this) {
	if(!%this.isCSCSAllowed($Pref::CSCS::AllowResetCommand)) {
		%this.logCSCSCommand("reset","",0);
		return;
	}
	if(!%this.checkCSCSCommandSpam()) {
		return;
	}

	if(!isObject(%this.minigame)) {
		%this.CSCSError("You are not in a minigame!");
		return;
	}

	%this.logCSCSCommand("reset","",1);
	
	%this.minigame.reset((%this.minigame.owner | 0));
	%this.minigame.messageAll('',"\c3" @ %this.name SPC "\c6reset the minigame");
}