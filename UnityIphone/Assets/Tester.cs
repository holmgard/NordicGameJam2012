using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {
	
	public SoundManager sm;
	
	// Use this for initialization
	void Start () {
		sm = FindObjectOfType(typeof(SoundManager)) as SoundManager;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(10,100,100,100), "CONGA!"))
		{
			if(sm != null)
			{
				sm.PlaySound("sheep");
			}
		}
		GUI.Box(new Rect(10,200,100,50), Input.acceleration.x.ToString());
		GUI.Box(new Rect(10,250,100,50), Input.acceleration.x.ToString());
	}
}
