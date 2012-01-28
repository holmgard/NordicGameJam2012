package org.nordicgamejam.nake;

import java.util.ArrayList;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class NakeExtension extends SFSExtension {
	
	public static ArrayList<NakeGame> nakeGames;
	
	public void init()
	{
		nakeGames = new ArrayList<NakeGame>();
		
		addEventHandler(SFSEventType.USER_JOIN_ZONE, JoinZoneEventHandler.class);
		
		trace("-== Nake server v0.1 loaded... ==-");
		addRequestHandler("teamRequest",TeamRequestHandler.class);
		addRequestHandler("readytoPlay",ReadyToPlayHandler.class);
		addRequestHandler("requestScore",ScoreRequestHandler.class);
		addRequestHandler("playerWasCaught",PlayerCaughtHandler.class);
		addRequestHandler("playerTypeRequest",PlayerTypeRequestHandler.class);
	}
	
	public void destroy()
	{
		super.destroy();
	}
}
