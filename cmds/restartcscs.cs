//COMMAND	restartcscs
//AUTHOR	TheBlackParrot	18701
//INFO	Reinitializes CSCS commands, adding (or removing) custom commands in the CSCS cmds folder in config/server
//LIMIT	host
//enabled

function serverCmdRestartCSCS(%this) {
	if(!%this.isCSCSAllowed($Pref::CSCS::AllowDFGCommand)) {
		%this.logCSCSCommand("restartcscs","",0);
		return;
	}
	if(!%this.checkCSCSCommandSpam()) {
		return;
	}

	%this.logCSCSCommand("restartcscs","",1);

	messageAll('',"\c3" @ %this.name SPC "\c6reinitialized CSCS commands");
	initCommandDB();
}