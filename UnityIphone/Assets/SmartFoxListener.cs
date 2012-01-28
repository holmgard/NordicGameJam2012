using UnityEngine;
using System.Collections;

public class SmartFoxListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
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

		smartFox.AddLogListener(LogLevel.DEBUG, OnDebugMessage);
		
		smartFox.Connect(serverName, serverPort);
		
		Debug.Log(Application.platform.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
