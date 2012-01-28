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
	}
}
