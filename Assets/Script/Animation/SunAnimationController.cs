using UnityEngine;
using System.Collections;

public class SunAnimationController : MonoBehaviour {
	
	public float Duration;
	public Vector3 Rotation;
	
	public Vector2 StartPoint;
	public Vector2 EndPoint;
	
	private float Timer = 0;
	
	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		while(Timer > Duration)
			Timer -= Duration;
		transform.localPosition = Vector2.Lerp(StartPoint, EndPoint, Timer / Duration);
		transform.Rotate(Rotation);
	}
	

}
