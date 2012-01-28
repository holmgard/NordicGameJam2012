package org.nordicgamejam.nake;

import com.smartfoxserver.v2.extensions.SFSExtension;

public class NakeExtension extends SFSExtension {

	public void init()
	{
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
