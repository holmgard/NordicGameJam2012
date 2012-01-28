using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;

public class GameLogic : MonoBehaviour {
	
	private SmartFox smartFox;
	
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
		
		smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnApplicationQuit()
	{
		
	}
	
	public void OnExtensionResponse(BaseEvent evt)
	{
		
	}
}
