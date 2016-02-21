using UnityEngine;
using System.Collections;

public class SunSnowSwitch : MonoBehaviour {
	
	public Transform Sun;
	public SnowEmitter Snow;
	
	public float LerpValue = 0.5f;
	
	void Awake()
	{
		Sun.gameObject.SetActive(false);
		Snow.gameObject.SetActive(false);
		PersonDataAdapter.sunSnowSwitch = this;
	}

	// Use this for initialization
	public void UpdateSky () {
		if(LerpValue < 0.20)
		{
			Snow.gameObject.SetActive(true);
			Snow.EmitPerSec  = Mathf.Lerp(0, 0.25f, LerpValue/0.2f);
			Sun.gameObject.SetActive(false);
		}
		else
		{
			Snow.gameObject.SetActive(false);
			Sun.gameObject.SetActive(true);
			Sun.position = new Vector2(0, 4 - Mathf.Lerp(0, 4, (LerpValue- 0.2f)/0.3f));
		}
		
	}
}
