package org.nordicgamejam.nake;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.SFSException;
//import com.smartfoxserver.v2.util.IDisconnectionReason;

public class NakeGame extends Object {
	private class Score extends SFSObject {

		public Score() {
			putInt("oneScore", 0);
			putInt("twoScore", 0);
		}

	}

	// public int currentPlayers = 0;
	private static final int maxPlayers = 10;
	private static final int minPlayers = 2;
	// private final int readyPlayers = 0;

	// public ArrayList<User> playersInGame;
	private final Set<User> oneTeam;
	private final Set<User> twoTeam;
	private final Set<User> readyPlayers;
	private final List<User> caughtPlayers;

	private final Score score;
	private NakeExtension extension;

	private final String oneType;
	private final String twoType;
	
	public static String[] teamTypes = {"dog","cat","duck","goat","sheep","cattle"};
	
	public static String generateTeamType(String alreadyUsed) {
		String teamType = teamTypes[(int) Math.random()*teamTypes.length + 1];
		System.out.println("TeamType: " + teamType);
		/*while(teamType == alreadyUsed)
		{
			teamType = teamTypes[(int) Math.random()*teamTypes.length + 1];
		}*/
		//TODO make this work
		return teamType;
	}
	
	public NakeGame() {
		oneTeam = new HashSet<User>();
		twoTeam = new HashSet<User>();
		readyPlayers = new HashSet<User>();
		caughtPlayers = new ArrayList<User>();
		//extension = sfsExtension;
		this.score = new Score();
		oneType = generateTeamType("TODO");
		twoType = generateTeamType("TODO");
	}
	
	public void setParentExtension(NakeExtension ext)
	{
		extension = ext;
	}

	public int getNextTeamId() {
		return oneTeam.size() > twoTeam.size() ? 1 : 0;
	}

	public void addPlayer(User player, int team) throws SFSException {
		// if(playersInGame == null)
		// {
		// playersInGame = new ArrayList<User>();
		// }
		// playersInGame.add(player);
		// currentPlayers++;
		if (team == 0) {
			if (twoTeam.contains(player))
				throw new SFSException("player already in team one");
			oneTeam.add(player);
		} else {
			if (oneTeam.contains(player))
				throw new SFSException("player already in team one");
			oneTeam.add(player);
		}

	}

	public boolean removePlayer(User player) {
		return oneTeam.remove(player) || twoTeam.remove(player);
	}

	public int getNumberOfPlayers() {
		return oneTeam.size() + twoTeam.size();
	}

	public boolean isFull() {
		if (getNumberOfPlayers() >= maxPlayers)
			return true;
		else
			return false;
	}

	public void readyPlayer(User player) {
		readyPlayers.add(player);
		extension.trace("Readied player: " + player.getName());
		extension.trace("Ready to play: " + readytoPlay());
		extension.trace("Number of players: " + getNumberOfPlayers());
		if(readytoPlay() && getNumberOfPlayers() >= minPlayers)
		{
			extension.trace("Ready to play, sending signal to players, number: " + readyPlayers.size());
			ISFSObject dataObject = new SFSObject();
			dataObject.putNull(NakeExtension.GAME_START);
			extension.send(NakeExtension.GAME_START, dataObject, new ArrayList<User>(readyPlayers));
		}
	}

	public boolean readytoPlay() {
		if (getNumberOfPlayers() == readyPlayers.size()) {
			return true;
		} else {
			return false;
		}
	}

	public boolean hasPlayer(User arg0) {
		return oneTeam.contains(arg0) | twoTeam.contains(arg0);
	}

	public boolean catchPlayer(User arg0) {
		boolean newlyCaught = !caughtPlayers.contains(arg0);
		caughtPlayers.add(arg0);
		if(oneTeam.contains(arg0))
		{
			int oldScore = score.getInt("oneScore");
			score.putInt("oneScore", oldScore+1);
		}
		if(twoTeam.contains(arg0))
		{
			int oldScore = score.getInt("twoScore");
			score.putInt("twoScore", oldScore+1);
		}
		
		extension.trace("Player" + arg0.getName() + "was caught!");
		checkForGameEnd();

		return newlyCaught;

	}

	private void checkForGameEnd() {
		extension.trace("Checking whether game is over...");
		if (getNumberOfPlayers() == caughtPlayers.size()) {
			// ...
			extension.trace("Game Over!");
			extension.send(NakeExtension.GAME_END, score, caughtPlayers);
			// reset();
			/*for (User u : caughtPlayers)
				u.disconnect(new IDisconnectionReason() {

					@Override
					public int getValue() {
						return 0;
					}

					@Override
					public byte getByteValue() {

						return 0;
					}
				});*/
		}

	}

	public String getPlayerType(User user) {
		if (oneTeam.contains(user)) {
			return oneType;
		} else if (twoTeam.contains(user)) {
			return twoType;
		} else
			return null;
	}
}
