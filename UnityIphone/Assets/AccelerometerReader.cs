using UnityEngine;
using System.Collections;

public class AccelerometerReader : MonoBehaviour {
	
	public float lastAccX;
	public float lastAccY;
	
	public float latestXDelta;
	public float latestYDelta;
	
	public float highestDeltaY = 0;
	public float highestDeltaX = 0;
	public float highestDeltaTotal = 0;
	
	// Update is called once per frame
	void Update () {
		latestXDelta = Mathf.Abs(lastAccX-Input.acceleration.x);
		latestYDelta = Mathf.Abs(lastAccY-Input.acceleration.y);
		
		lastAccX = Input.acceleration.x;
		lastAccY = Input.acceleration.y;
		
		if(latestXDelta > highestDeltaX)
			highestDeltaX = latestXDelta;
		if(latestYDelta > highestDeltaY)
			highestDeltaY = latestYDelta;
		if((latestXDelta + latestYDelta) > highestDeltaTotal)
			highestDeltaTotal = (latestXDelta + latestYDelta);
	}
	
	void OnGUI()
	{
		GUI.Box(new Rect(0,Screen.height-150,Screen.width,150),highestDeltaTotal.ToString());
	}
}
