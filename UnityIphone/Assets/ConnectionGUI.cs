using UnityEngine;
using System;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Logging;

public class ConnectionGUI : MonoBehaviour
{
	//----------------------------------------------------------
	// Setup variables
	//----------------------------------------------------------
	public string ip = "192.168.1.110";
	public int port = 9933;
	private string statusMessage = "";
	private static SmartFox smartFox;

	//----------------------------------------------------------
	// Called when program starts
	//----------------------------------------------------------
	void Start()
	{
		smartFox = new SmartFox();
		smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		smartFox.AddLogListener(LogLevel.DEBUG, OnDebugMessage);
		smartFox.Connect(ip, port);
	}
	
	//----------------------------------------------------------
	// As Unity is not thread safe, we process the queued up callbacks every physics tick
	//----------------------------------------------------------
	void FixedUpdate() {
		smartFox.ProcessEvents();
	}
	
	//----------------------------------------------------------
	// Draw GUI every frame
	//----------------------------------------------------------
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 500, 100), "Status: " + statusMessage);
	}

	//----------------------------------------------------------
	// Handle connection response from server
	//----------------------------------------------------------
	public void OnConnection(BaseEvent evt)
	{
		bool success = (bool)evt.Params["success"];
		if (success)
		{
			statusMessage = "Connection succesfull!";
		} else
		{
			statusMessage = "Can't connect!";			
		}
	}

	public void OnDebugMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		Debug.Log("[SFS DEBUG] " + message);
	}
	
	//----------------------------------------------------------
	// Disconnect from the socket when shutting down the game
	//----------------------------------------------------------
	public void OnApplicationQuit()
	{
		if(smartFox.IsConnected)
		{
            smartFox.Disconnect();
        }
	}
}