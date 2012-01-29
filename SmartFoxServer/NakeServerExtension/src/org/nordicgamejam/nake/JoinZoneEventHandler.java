package org.nordicgamejam.nake;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class JoinZoneEventHandler extends BaseServerEventHandler {
	
	public void handleServerEvent(ISFSEvent event) throws SFSException {
		trace("User joined Zone");
		User user = (User) event.getParameter(SFSEventParam.USER);

		/*if(NakeExtension.game == null)
			trace("1a");
			NakeExtension.game = new NakeGame((NakeExtension) getParentExtension());*/
		NakeGame g = NakeExtension.nakeGame;

//		if (NakeExtension.nakeGames.size() < 1) {
//			NakeExtension.nakeGames.add(new NakeGame(
//					(NakeExtension) getParentExtension()));
//			g = NakeExtension.nakeGames.get(0);
//			g.addPlayer(user, g.getNextTeamId());
//		} else if (NakeExtension.nakeGames.size() >= 1) {
//			if (NakeExtension.nakeGames.get(NakeExtension.nakeGames.size() - 1)
//					.isFull()) {
//				NakeExtension.nakeGames.add(new NakeGame(
//						(NakeExtension) getParentExtension()));
//				g = NakeExtension.nakeGames
//						.get(NakeExtension.nakeGames.size() - 1);
//				g.addPlayer(user, g.getNextTeamId());
//			}
//		}

		g.addPlayer(user, g.getNextTeamId());
		String playerType = g.getPlayerType(user);
		
		trace("PlayerType is: " + playerType);
		SFSObject ret = new SFSObject();
		ret.putUtfString(NakeExtension.PLAYER_TYPE, playerType);
		this.send(NakeExtension.PLAYER_TYPE, ret, user);
		trace("Sent message to new client");
	}
}
