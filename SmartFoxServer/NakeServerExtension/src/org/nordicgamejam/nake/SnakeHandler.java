package org.nordicgamejam.nake;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public abstract class SnakeHandler extends BaseClientRequestHandler {

	protected static NakeGame getGameForPlayer(User arg0) {
//		for (NakeGame g : NakeExtension.nakeGames)
//			if (g.hasPlayer(arg0))
//				return g;
//
//		return null;
		return NakeExtension.nakeGame;
	}

	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		
	}

}
