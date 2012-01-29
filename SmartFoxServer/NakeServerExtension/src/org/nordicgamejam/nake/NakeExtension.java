package org.nordicgamejam.nake;

//import java.util.Stack;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class NakeExtension extends SFSExtension {

//	public static Stack<NakeGame> nakeGames;
	
	public static NakeGame nakeGame;
	
	public static final String TEAM_REQUEST = "teamRequest";
	public static final String READY_PLAYER = "readytoPlay";
	public static final String REQUEST_SCORE = "requestScore";
	public static final String PLAYER_CAUGHT = "playerWasCaught";
	public static final String PLAYER_TYPE = "playerTypeRequest";
	public static final String GAME_START = "gameStart";
	public static final String GAME_END = "gameEnd";

	public void init() {
		trace("alfa");
		nakeGame = new NakeGame();
		trace("nakeGame: " + nakeGame.toString());
		nakeGame.setParentExtension(this);
//		nakeGames = new Stack<NakeGame>();
		trace("beta");
		addEventHandler(SFSEventType.USER_JOIN_ZONE, JoinZoneEventHandler.class);

		trace("-== Nake server v0.1 loaded... ==-");
		// addRequestHandler(TEAM_REQUEST, TeamRequestHandler.class);
		addRequestHandler(READY_PLAYER, ReadyToPlayHandler.class);
		addRequestHandler(REQUEST_SCORE, ScoreRequestHandler.class);
		addRequestHandler(PLAYER_CAUGHT, PlayerCaughtHandler.class);
		// addRequestHandler(PLAYER_TYPE, PlayerTypeRequestHandler.class);
	}

	@Override
	public void destroy() {
		super.destroy();
	}
}
