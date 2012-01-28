using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;

public class SmartFoxListener : MonoBehaviour {
	public string serverName = "127.0.0.1";
	public int serverPort = 9933;

	public SmartFox smartFox;
	public GameLogic gl;
	
	private string zone = "SimpleChat";
	private string username = "";
	private string password = "";
	private string loginErrorMessage = "";
	private bool isLoggedIn;
	private bool isJoining = false;
	
	private string newMessage = "";
	private ArrayList messages = new ArrayList();
	// Locker to use for messages collection to ensure its cross-thread safety
	private System.Object messagesLocker = new System.Object();
	
	private Vector2 chatScrollPosition, userScrollPosition;

	private int roomSelection = 0;
	private string [] roomStrings;
	
	public GUISkin gSkin;
	
	private Room currentActiveRoom;
	
	
	// Use this for initialization
	void Start () {
		
		username = "user" + UnityEngine.Random.Range(1,100000).ToString();
		
		bool debug = true;
		if (SmartFoxConnection.IsInitialized)
		{
			smartFox = SmartFoxConnection.Connection;
		}
		else
		{
			smartFox = new SmartFox(debug);
		}
		
		smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		smartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);
		smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		smartFox.AddEventListener(SFSEvent.LOGOUT, OnLogout);
		smartFox.AddEventListener(SFSEvent.ROOM_JOIN, OnJoinRoom);
		smartFox.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
		smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

		smartFox.AddLogListener(LogLevel.DEBUG, OnDebugMessage);
		
		//smartFox.Connect(serverName, serverPort);
		
		gl = GameObject.FindObjectOfType(typeof(GameLogic)) as GameLogic;
		
		Debug.Log(Application.platform.ToString());
	}
	
	void FixedUpdate()
	{
		smartFox.ProcessEvents();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void Connect()
	{
		Debug.Log("SmartFoxListener going to connect");
		smartFox.Connect(serverName,serverPort);
	}
	
	private void UnregisterSFSSceneCallbacks() {
		// This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
		smartFox.RemoveAllEventListeners();
	}
	
	public void OnConnection(BaseEvent evt) {
		bool success = (bool)evt.Params["success"];
		string error = (string)evt.Params["errorMessage"];
		
		Debug.Log("On Connection callback got: " + success + " (error : <" + error + ">)");

		if (success)
		{
			SmartFoxConnection.Connection = smartFox;
			DoLogin();
		}
	}
	
	public void DoLogin()
	{
		Debug.Log("Sending login request");
		smartFox.Send(new LoginRequest(username, "password", "SnakeClub"));
	}
	
	public void OnConnectionLost(BaseEvent evt) {
		Debug.Log("OnConnectionLost");
		isLoggedIn = false;
		isJoining = false;
		currentActiveRoom = null;
		UnregisterSFSSceneCallbacks();
	}
	
	// Various SFS callbacks
	public void OnLogin(BaseEvent evt) {
		try {
			bool success = true;
			if (evt.Params.ContainsKey("success") && !(bool)evt.Params["success"]) {
				loginErrorMessage = (string)evt.Params["errorMessage"];
				Debug.Log("Login error: "+loginErrorMessage);
			}
			else {
				isLoggedIn = true;
				Debug.Log("Logged in successfully");
				ReadRoomListAndJoin();
			}
		}
		catch (Exception ex) {
			Debug.Log("Exception handling login request: "+ex.Message+" "+ex.StackTrace);
		}
	}

	public void OnLoginError(BaseEvent evt) {
		Debug.Log("Login error: "+(string)evt.Params["errorMessage"]);
	}
	
	void OnLogout(BaseEvent evt) {
		Debug.Log("OnLogout");
		isLoggedIn = false;
		isJoining = false;
		currentActiveRoom = null;
	}

	public void OnDebugMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		Debug.Log("[SFS DEBUG] " + message);
	}
	
	private void ReadRoomListAndJoin() {
		Debug.Log("Room list: ");
						
		List<Room> roomList = smartFox.RoomManager.GetRoomList();
		List<string> roomNames = new List<string>();
		foreach (Room room in roomList) {
			if (room.IsHidden || room.IsPasswordProtected) {
				continue;
			}	
			
			roomNames.Add(room.Name);
			Debug.Log("Room id: " + room.Id + " has name: " + room.Name);
				
		}
		
		roomStrings = roomNames.ToArray();
		
		if (smartFox.LastJoinedRoom==null) {
			JoinRoom("Game1");
		}
	}

		
	void JoinRoom(string roomName) {
		if (isJoining) return;
		
		isJoining = true;
		currentActiveRoom = null;
		Debug.Log("Joining room: "+roomName);
		
		// Need to leave current room, if we are joined one
		if (smartFox.LastJoinedRoom==null)
			smartFox.Send(new JoinRoomRequest(roomName));
		else	
			smartFox.Send(new JoinRoomRequest(roomName, "", smartFox.LastJoinedRoom.Id));
	}

	void OnJoinRoom(BaseEvent evt) {
		Room room = (Room)evt.Params["room"];
		Debug.Log("Room " + room.Name + " joined successfully");
		
		lock (messagesLocker) {
			messages.Clear();
		}
		
		currentActiveRoom = room;
		isJoining = false;
		
		gl.OnJoinedRoom();
	}

	void OnPublicMessage(BaseEvent evt) {
		try {
			string message = (string)evt.Params["message"];
			User sender = (User)evt.Params["sender"];
	
			// We use lock here to ensure cross-thread safety on the messages collection 
			lock (messagesLocker) {
				messages.Add(sender.Name + " said " + message);
			}
			
			chatScrollPosition.y = Mathf.Infinity;
			Debug.Log("User " + sender.Name + " said: " + message);
		}
		catch (Exception ex) {
			Debug.Log("Exception handling public message: "+ex.Message+ex.StackTrace);
		}
	}
	
	public void OnExtensionResponse(BaseEvent evt)
	{
		gl.OnExtensionResponse(evt);
	}
	
	public void OnApplicationQuit()
	{
		smartFox.Disconnect();
		smartFox.RemoveAllEventListeners();	
	}
}
