package org.nordicgamejam.nake;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;

public class PlayerCaughtHandler extends SnakeHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		trace("In player caught handler");
		getGameForPlayer(arg0).catchPlayer(arg0);

	}

}
