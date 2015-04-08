exec("./support.cs");
$CSCS::Version = "v0.1-1";

datablock AudioProfile(CSCS_slap)
{
	filename = "./sounds/slap.wav";
	description = AudioClosest3d;
	preload = true;
};

if(!isFile("config/server/CSCS")) {
	$Pref::CSCS::CommandCooldownPeriod = 1000;
	$Pref::CSCS::LogCommands = 1;
	$Pref::CSCS::LogDir = "config/server/CSCS/logs";
	$Pref::CSCS::Root = "Add-Ons/Server_CustomServerCommandSystem";
	export("$Pref::CSCS::*","config/server/CSCS/prefs.cs");
}
exec("config/server/CSCS/prefs.cs");

function initCommandDB() {
	// needed to refresh the custom directories
	setModPaths(getModPaths());
	deleteVariables("$CSCS::Command*");
	$CSCS::CommandCount = 0;

	%filename = findFirstFile($Pref::CSCS::Root @ "/cmds/*.cs");
	while(%filename !$= "") {
		%error = 0;

		%file = new FileObject();
		%file.openForRead(%filename);

		for(%i=0;%i<4;%i++) {
			%line = %file.readLine();
			%line = strReplace(%line,"//","");
			if(parseCommandData(getField(%line,0),getField(%line,1),getField(%line,2),%filename) == -1) {
				%error = 1;
			}
		}

		%line = %file.readLine();
		if(%line $= "//enabled" && !%error) {
			exec(%filename);
			$CSCS::CommandCount++;
		} else {
			warn("Skipping" SPC filebase(%filename));
		}

		if(!%checked_default) {
			%filename = findNextFile($Pref::CSCS::Root @ "/cmds/*.cs");
		} else {
			%filename = findNextFile("config/server/CSCS/cmds/*.cs");
		}

		if(%filename $= "" && !%checked_default) {
			%checked_default = 1;
			%filename = findFirstFile("config/server/CSCS/cmds/*.cs");
		}
	}
}
initCommandDB();

function parseCommandData(%which,%data1,%data2,%fn) {
	switch$(%which) {
		case "COMMAND":
			$CSCS::Command[$CSCS::CommandCount,name] = %data1;
			$CSCS::Command[$CSCS::CommandCount,args] = %data2;
		case "AUTHOR":
			$CSCS::Command[$CSCS::CommandCount,authorname] = %data1;
			if(%data2 $= "") {
				%data2 = "???";
			}
			$CSCS::Command[$CSCS::CommandCount,authorid] = %data2;
		case "INFO":
			$CSCS::Command[$CSCS::CommandCount,desc] = %data1;
		case "LIMIT":
			// i feel like this is needed
			switch$(%data1) {
				case "anyone" or "all" or "none":
					%val = 0;
				case "admin" or "RA" or "A":
					%val = 1;
				case "super admin" or "superadmin" or "sadmin" or "SA" or "":
					%val = 2;
				case "host" or "master" or "owner" or "leader" or "H":
					%val = 3;
			}
			if($CSCS::Command[$CSCS::CommandCount,name] !$= "") {
				%name = $CSCS::Command[$CSCS::CommandCount,name] @ "Command";
				$Pref::CSCS::Allow[%name] = %val;
			} else {
				warn("Command name isn't defined for" SPC %fn SPC "before LIMIT, skipping...");
				return -1;
			}
	}
}

package CSCSCorePackage {
	function onServerDestroyed() {
		export("$Pref::CSCS::*","config/server/CSCS/prefs.cs");
		echo("CSCS prefs exported, any changes made to the CSCS prefs.cs file while the server was running have been overwritten.");
		return parent::onServerDestroyed();
	}
};
activatePackage(CSCSCorePackage);