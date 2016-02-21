using UnityEngine;

public class SnowEmitter : MonoBehaviour {
	
	public GameObject Snowflake;
	
	public float EmitPerSec;
	private float EmitDuration;
	
	public float Duration;
	public Vector3 Rotation;
	
	public Vector2 StartPoint;
	public Vector2 EndPoint;
	
	System.Random ran = new System.Random();
	
	public float Timer = 0;
	
	void Start()
	{
		EmitDuration = 1 / EmitPerSec;
	}

	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		while(Timer >= EmitDuration)
		{
			Timer = 0;
			
			GameObject go = Instantiate(Snowflake);
		
			SnowAnimationController snowCtrl = go.AddComponent<SnowAnimationController>();
			float x = (float)ran.NextDouble() * 6 - 3;
			snowCtrl.Duration = Duration;
			snowCtrl.Rotation = Rotation;
			snowCtrl.StartPoint = new Vector2(x, StartPoint.y);
			snowCtrl.EndPoint = new Vector2(x, EndPoint.y);
		}
	}
}
