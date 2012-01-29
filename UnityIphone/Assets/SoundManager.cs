using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	public GameObject soundsObject;
	public AudioSource audioSource;
	public GameObject go;
	
	public void Awake()
	{
		soundsObject = GameObject.Find("Sounds");
	}
	
	public void SetSound(string soundToPlay)
	{
		Debug.Log("Looking for AudioSource: " + soundToPlay);
		for(int i = 0; i<soundsObject.gameObject.transform.GetChildCount(); i++)
		{
			Debug.Log("Testing if " + soundsObject.gameObject.transform.GetChild(i).gameObject.name + " is the right one");
			if(soundsObject.gameObject.transform.GetChild(i).gameObject.name == soundToPlay){
				audioSource = soundsObject.gameObject.transform.GetChild(i).gameObject.GetComponent<AudioSource>();
			}
		}
	}
	
	public void PlaySound()
	{
		if(audioSource != null && !audioSource.isPlaying)
		{
			audioSource.Play();
		}
	}
	
	public void StopSound()
	{
		if(audioSource != null && audioSource.isPlaying)
		{
			audioSource.Stop();
		}
	}
}
