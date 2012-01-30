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
		start,joining,countdown,playing,caught,status
	}
	
	public SmartFoxListener smartFoxListener;
	public SoundManager sm;
	public GameState State;
	
	public int oneScore;
	public int twoScore;
	
	public string myPlayerType;
	
	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		iPhoneSettings.screenCanDarken = false;
		Screen.sleepTimeout = 0.0f;
		
		bool debug = true;
		
		smartFoxListener = FindObjectOfType(typeof(SmartFoxListener)) as SmartFoxListener;
		sm = FindObjectOfType(typeof(SoundManager)) as SoundManager;
		State = GameState.start;
	}
	
	public float countdownStart;
	public float countDownTime = 10f;
	
	// Update is called once per frame
	void Update ()
	{
		if(State == GameLogic.GameState.countdown)
		{
			if(Time.time-countdownStart > countDownTime)
			{
				sm.PlaySound();
				iPhoneUtils.Vibrate();
				State = GameLogic.GameState.playing;
			}
		}
	}
	
	public void OnReadyToJoin()
	{
		if(smartFoxListener != null)
			smartFoxListener.Connect();
	}
	
	public void OnJoinedRoom()
	{
		State = GameLogic.GameState.joining;
		Debug.Log("Room was joined");
		Debug.Log("Is SFS connected: " + smartFoxListener.smartFox.IsConnected.ToString());
	}
		
	public void SetPlayerType(string playerType)
	{
		myPlayerType = playerType;
		Debug.Log("Setting sound to: " + playerType);
		sm.SetSound(myPlayerType);
		Debug.Log("Got player type: " + myPlayerType);
		SendReadySignal();
	}
	
	public void SendReadySignal()
	{
		ISFSObject dataObject = new SFSObject();
		dataObject.PutNull("");
		smartFoxListener.smartFox.Send(new ExtensionRequest("readytoPlay",dataObject));
	}
	
	public void OnStartPlayingGame()
	{
		countdownStart = Time.time;
		State = GameLogic.GameState.countdown;
	}
	
	public void RequestScore()
	{
		ISFSObject dataObject = new SFSObject();
		dataObject.PutNull("");
		smartFoxListener.smartFox.Send(new ExtensionRequest("requestScore",dataObject));
	}
	
	public void SendCaughtSignal()
	{
		sm.StopSound();
		ISFSObject dataObject = new SFSObject();
		dataObject.PutNull("");
		smartFoxListener.smartFox.Send(new ExtensionRequest("playerWasCaught",dataObject));
		State = GameLogic.GameState.caught;
	}
	
	void OnApplicationQuit()
	{
		
	}
	
	public void OnExtensionResponse(BaseEvent evt)
	{
		Debug.Log("In extension response");
		string cmd = (string)evt.Params["cmd"];
		SFSObject dataObject = (SFSObject)evt.Params["params"];
		switch(cmd)
		{
		case "playerTypeRequest":
			SetPlayerType(dataObject.GetUtfString("playerTypeRequest"));
			break;
		case "gameStart":
			Debug.Log("Got gameStart from server");
			OnStartPlayingGame();
			break;
		case "gameEnd":
			oneScore = dataObject.GetInt("oneScore");
			twoScore = dataObject.GetInt("twoScore");
			State = GameLogic.GameState.status;
			break;
		default:
			break;
		}
		
	}
}
