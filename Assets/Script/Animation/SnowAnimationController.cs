using UnityEngine;
using System.Collections;

public class SnowAnimationController : MonoBehaviour {

	// Use this for initialization
	public float Duration;
	public Vector3 Rotation;
	
	public Vector2 StartPoint;
	public Vector2 EndPoint;
	
	private float Timer = 0;
	
	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		if(Timer > Duration)
			Destroy(gameObject);
		transform.localPosition = Vector2.Lerp(StartPoint, EndPoint, Timer / Duration);
		transform.Rotate(Rotation);
	}
}
