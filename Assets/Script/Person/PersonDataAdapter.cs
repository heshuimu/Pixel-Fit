using UnityEngine;
using System.Collections;

public static class PersonDataAdapter {

	public static PersonController personCtrl
	{
		get
		{
			return personCtrl_p;
		}
		set
		{
			personCtrl_p = value;
		}
	}
	private static PersonController personCtrl_p;
	
	public static SunSnowSwitch sunSnowSwitch
	{
		get
		{
			return sunSnowSwitch_p;
		}
		set
		{
			sunSnowSwitch_p = value;
		}
	}
	private static SunSnowSwitch sunSnowSwitch_p;
	
	public static float BodySizePercentage
	{
		set
		{
			personCtrl.BodySizePercentage = Mathf.Lerp(0.25f, 0.9f, value);
			personCtrl.RefreshBody();
		}
	}
	
	public static float BodyHeight
	{
		set
		{
			personCtrl.BodyHeight = value/100;
			personCtrl.RefreshBody();
		}
	}
	
	public static float LegLength
	{
		set
		{
			Debug.Log(value);
			personCtrl.LegLength = Mathf.Lerp(1, 3, value);
			personCtrl.RefreshBody();
		}
	}
	
	public static float WellSleptReflection
	{
		set
		{
			personCtrl.ChangeFaceExpressionWithPercentage(2*value -1);
		}
	}
	
	public static float SunSnowLerpValue
	{
		set
		{
			sunSnowSwitch.LerpValue = value;
			sunSnowSwitch.UpdateSky();
		}
	}
	
	
	
}
