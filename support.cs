function GameConnection::isHost(%this) {
	if(%this.bl_id == getNumKeyID() || %this.bl_id == 999999) {
		return 1;
	}
	return 0;
}

function GameConnection::isCSCSAllowed(%this,%var,%silent) {
	switch(%var) {
		case 0:
			return 1;
		case 1:
			if(%this.isAdmin) {
				return 1;
			}
		case 2:
			if(%this.isSuperAdmin) {
				return 1;
			}
		case 3:
			if(%this.isHost()) {
				return 1;
			}
	}
	if(!%silent) {
		%this.CSCSError("You do not have permission to use this command.");
	}
	return 0;
}

function GameConnection::checkCSCSCommandSpam(%this) {
	if(getSimTime() - %this.lastCommandTime < $Pref::CSCS::CommandCooldownPeriod) {
		return 0;
	}
	%this.lastCommandTime = getSimTime();
	return 1;
}

function GameConnection::CSCSError(%this,%msg) {
	messageClient(%this,'',"\c0ERROR\c6:" SPC %msg);
	%this.play2D(errorSound);
}

function getDate(%format) {
	// switch maybe later?
	%str = getWord(getDateTime(),0);
	if(%format) {
		%str = strReplace(%str,"/","-");
	}

	return %str;
}

function GameConnection::logCSCSCommand(%this,%cmd,%args,%success) {
	if(!$Pref::CSCS::LogCommands) {
		return;
	}

	%file = new FileObject();
	%filename = $Pref::CSCS::LogDir @ "/cmds/" @ getDate(1) @ ".txt";
	if(!isFile(%filename)) {
		%nf = 1;
	}
	%file.openForAppend($Pref::CSCS::LogDir @ "/cmds/" @ getDate(1) @ ".txt");

	if(%success) {
		%success = "successful";
	} else {
		%success = "failed";
	}
	// CSV format
	if(%nf) {
		%file.writeLine("time,name,bl_id,IP,command,arguments,result");
	}
	%file.writeLine(getDateTime() @ "," @ %this.name @ "," @ %this.bl_id @ "," @ %this.getRawIP() @ "," @ %cmd @ "," @ %args @ "," @ %success);

	%file.close();
	%file.delete();
}