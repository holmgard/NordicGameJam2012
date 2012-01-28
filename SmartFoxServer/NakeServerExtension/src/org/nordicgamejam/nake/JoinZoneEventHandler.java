package org.nordicgamejam.nake;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class JoinZoneEventHandler extends BaseServerEventHandler {

	@Override
	public void handleServerEvent(ISFSEvent event) throws SFSException {
		User user = (User) event.getParameter(SFSEventParam.USER);
        
		if(NakeExtension.nakeGames.size() < 1)
		{
			NakeExtension.nakeGames.add(new NakeGame());
			NakeExtension.nakeGames.get(0).addPlayer(user);
		} else if (NakeExtension.nakeGames.size() >= 1) {
			if(NakeExtension.nakeGames.get(NakeExtension.nakeGames.size()-1).isFull()){
				NakeExtension.nakeGames.add(new NakeGame());
				NakeExtension.nakeGames.get(NakeExtension.nakeGames.size()-1).addPlayer(user);
			} 
		}
		
	}

}
