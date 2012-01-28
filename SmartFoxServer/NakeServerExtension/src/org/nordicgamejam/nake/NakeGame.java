package org.nordicgamejam.nake;

import java.util.ArrayList;

import com.smartfoxserver.v2.entities.User;

public class NakeGame {
	public int currentPlayers = 0;
	public int maxPlayers = 10;
	public int readyPlayers = 0;
	
	public ArrayList<User> playersInGame;
	
	public void addPlayer(User player)
	{
		if(playersInGame == null)
		{
			playersInGame = new ArrayList<User>();
		}
		playersInGame.add(player);
		currentPlayers++;
	}
	
	public boolean removePlayer(User player)
	{
		if(playersInGame != null && playersInGame.indexOf(player) != -1)
		{
			playersInGame.remove(player);
			currentPlayers--;
			return true;
		} else {
			return false;
		}
		
	}
	
	public boolean readytoPlay()
	{
		if(currentPlayers == readyPlayers)
		{
			return true;
		} else {
			return false;
		}
	}
}
