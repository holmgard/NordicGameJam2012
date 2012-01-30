using UnityEngine;
using System.Collections;

public class FiveSecondShuffle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		latestTime = Time.time;
	}
	
	public string[] sounds = {"cat","cattle","dog","duck","goat"};
	
	public SoundManager soundMan;
	public string soundToPlay;
	
	public float latestTime;
	public float dongTime = 5;
	
	// Update is called once per frame
	void Update () {
		if((Time.time - latestTime) > dongTime)
		{
			latestTime = Time.time;
			soundMan.StopSound();
			soundMan.SetSound("bell");
			soundMan.PlaySound();
			soundToPlay = sounds[Random.Range(0,sounds.Length-1)];
			soundMan.SetSound(soundToPlay);
			//soundMan.PlaySound();
		} else 
		{
			soundMan.PlaySound();
		}
	}
}
