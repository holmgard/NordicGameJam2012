package org.nordicgamejam.nake;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;

public class ReadyToPlayHandler extends SnakeHandler {

	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		trace("Got ready to play from user: " + sender.getName());
		getGameForPlayer(sender).readyPlayer(sender);
	}
}
