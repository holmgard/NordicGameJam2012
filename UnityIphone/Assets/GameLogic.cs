using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Logging;

public class GameLogic : MonoBehaviour {
	
	public enum GameState
	{
		start,joining,admin,player,playing,caught,status
	}
	
	public SmartFoxListener smartFoxListener;
	public SoundManager sm;
	public GameState State;
	
	public string myTeam;
	
	// Use this for initialization
	void Start () {
		bool debug = true;
		
		smartFoxListener = FindObjectOfType(typeof(SmartFoxListener)) as SmartFoxListener;
		sm = FindObjectOfType(typeof(SoundManager)) as SoundManager;
		
		State = GameState.start;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void OnReadyToJoin()
	{
		if(smartFoxListener != null)
			smartFoxListener.Connect();
	}
	
	public void OnJoinedRoom()
	{
		Debug.Log("Room was joined, going to request player type");
		RequestPlayerType();
		Debug.Log("Is SFS connected: " + smartFoxListener.smartFox.IsConnected.ToString());
	}
	
	public void RequestPlayerType()
	{
		Debug.Log("Requesting player type");
		SFSObject dataObject = new SFSObject();
		dataObject.PutNull("");
		smartFoxListener.smartFox.Send(new ExtensionRequest("playerTypeRequest",dataObject));
	}
	
	public void RequestTeam()
	{
		Debug.Log("Requesting team");
		smartFoxListener.smartFox.Send(new ExtensionRequest("teamRequest",null));
	}
	
	public void SetTeam(string team)
	{
		myTeam = team;
		sm.PlaySound(team);
	}
	
	public void SendReadySignal()
	{
		ISFSObject dataObject = new SFSObject();
		dataObject.PutNull("");
		smartFoxListener.smartFox.Send(new ExtensionRequest("readytoPlay",dataObject));
	}
	
	public void RequestScore()
	{
		ISFSObject dataObject = new SFSObject();
		dataObject.PutNull("");
		smartFoxListener.smartFox.Send(new ExtensionRequest("requestScore",dataObject));
	}
	
	public void SendCaughtSignal()
	{
		ISFSObject dataObject = new SFSObject();
		dataObject.PutNull("");
		smartFoxListener.smartFox.Send(new ExtensionRequest("playerWasCaught",dataObject));
	}
	
	void OnApplicationQuit()
	{
		
	}
	
	public void OnExtensionResponse(BaseEvent evt)
	{
		string cmd = (string)evt.Params["cmd"];
		SFSObject dataObject = (SFSObject)evt.Params["params"];
		switch(cmd)
		{
		case "playerType":
			string type = dataObject.GetUtfString("type");
			if(type == "admin")
			{
				State = GameLogic.GameState.admin;
			} else if (type == "player")
			{
				State = GameLogic.GameState.player;
			}
			break;
		case "gameState":
			string gameState = dataObject.GetUtfString("gameState");
			if(gameState == "startGame")
			{
				RequestTeam();
			}
			if(gameState == "gameOver")
			{	
				RequestScore();
			}
			break;
		case "teamInfo":
			string playerTeam = dataObject.GetUtfString("playerTeam");
			SetTeam(playerTeam);
			break;
		default:
			break;
		}
		
	}
}
