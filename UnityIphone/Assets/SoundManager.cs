using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void PlaySound(string soundToPlay)
	{
		GameObject go = GameObject.Find(soundToPlay);
		if(go != null)
		{
			AudioSource audioSource = go.GetComponent<AudioSource>();
			if(audioSource != null && !audioSource.audio.isPlaying)
			{
				audioSource.audio.Play();
			}
		}
	}
}
