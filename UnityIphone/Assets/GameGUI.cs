using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {
	
	public GameLogic gl;
	
	// Use this for initialization
	void Start () {
		gl = GameObject.FindObjectOfType(typeof(GameLogic)) as GameLogic;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnGUI()
	{
		if(gl != null)
		{
			switch(gl.State)
			{
			case GameLogic.GameState.start:
				start();
				break;
				
			case GameLogic.GameState.joining:
				joining();
				break;
				
			case GameLogic.GameState.admin:
				admin();
				break;
						
			case GameLogic.GameState.player:
				player();
				break;
				
			case GameLogic.GameState.playing:
				playing();
				break;
				
			case GameLogic.GameState.caught:
				caught();
				break;
				
			case GameLogic.GameState.status:
				status();
				break;
				
			default:
				break;
			}
		}
	}
	
	private void start()
	{
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.Box("Are you ready to play?");
		if(GUILayout.Button("READY!"))
		{
			gl.OnReadyToJoin();
		}
		GUILayout.EndArea();
	}
		
	private void joining()
	{
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.Box("Joining game...");
		GUILayout.EndArea();
	}
	
	private void admin(){
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.Box("You are the admin. Press the button, when every one is ready to start");
		if(GUILayout.Button("Everyone's ready!"))
		{
			
		}
		GUILayout.EndArea();
	}
	
	private void player(){
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.Box("Waiting for the admin to start the game\n.Now would be good time to put your phone in your pocket.");
		GUILayout.EndArea();
	}
	
	private void playing(){
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.EndArea();
	}
	
	private void caught(){
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.Box("And you're out!");
		GUILayout.EndArea();
	}
	
	private void status(){
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.Box("Here will be the score board");
		GUILayout.EndArea();
	}
}