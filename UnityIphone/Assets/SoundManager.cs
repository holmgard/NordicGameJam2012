using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	AudioSource audioSource;
	
	public void PlaySound(string soundToPlay)
	{
		GameObject go = GameObject.Find(soundToPlay);
		if(go != null)
		{
			audioSource = go.GetComponent<AudioSource>();
			if(audioSource != null && !audioSource.audio.isPlaying)
			{
				audioSource.audio.Play();
			}
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
