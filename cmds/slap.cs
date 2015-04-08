//COMMAND	slap	player or id
//AUTHOR	TheBlackParrot	18701
//INFO	Moves players out of the way
//LIMIT	admin
//enabled

function serverCmdSlap(%this,%target) {
	if(!%this.isCSCSAllowed($Pref::CSCS::AllowSlapCommand)) {
		%this.logCSCSCommand("slap",%target,0);
		return;
	}
	if(!%this.checkCSCSCommandSpam()) {
		return;
	}

	%targetObject = findClientByName(%target);
	if(!isObject(%targetObject)) {
		%targetObject = findClientByBL_ID(%targetObject);
		if(!isObject(%targetObject)) {
			%this.CSCSError(%target SPC "does not exist!");
			return;
		}
	}

	if(!isObject(%targetObject.player)) {
		%this.CSCSError(%targetObject.name SPC "has not spawned!");
		return;
	}

	%this.logCSCSCommand("slap",%target,1);

	messageClient(%targetObject,'',"\c3" @ %this.name SPC "\c6slapped you!");
	%targetObject.play2D(CSCS_slap);
	messageClient(%this,'',"\c6You have slapped\c3" SPC %targetObject.name);

	%targetObject.player.addVelocity(getRandom(-20,20) SPC getRandom(-20,20) SPC getRandom(0,20));
}