using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using userInfo;
using System;

public class GetProfileButtonAction : MonoBehaviour, IMSHealthJSONResponder {

    public Text textCalorie;
    public Text textSleep;
    public Text textRun;
    public Text textName;
    private int start = -335;
    private int end = -178;
    private bool flag = false;
    PixelFit.Person user = new PixelFit.Person();

    public void GetProfile()
	{//-335
        MSHealthInterface.PerformRequest(this, MSHealthInterface.BaseHealthUri, "Profile");
        Debug.Log("hung");
    }
    public void GetActivities()
    {
        string startTime = Uri.EscapeDataString(DateTime.UtcNow.AddDays(start).ToString("O"));
        string endtime = Uri.EscapeDataString(DateTime.UtcNow.AddDays(end).ToString("O"));
        MSHealthInterface.PerformRequest(this, MSHealthInterface.BaseHealthUri, "Activities", string.Format("startTime={0}&endTime={1}", startTime, endtime));
        flag = true;
    }

    public bool RespondToJSONData(Dictionary<string, object> dict)
	{
        if (!flag)
        {
            user.parseProfile(dict);
            textName.text = "Name: " + user.name + " - Age: " + user.age;
            GetActivities();
        }
        else 
        {
            user.setCalorie(dict);
            user.setFrequency(start, end);
		PersonDataAdapter.WellSleptReflection = (float)Math.Round(user.sleepRatio(), 2);
		PersonDataAdapter.BodySizePercentage = ((float)user.BMI() -12)/36;
		//PersonDataAdapter.BodySizePercentage = (40f -12)/36;
		PersonDataAdapter.LegLength = (float)Math.Round(user.runRatio(start, end), 2);
		PersonDataAdapter.BodyHeight = (float)user.height;
		PersonDataAdapter.SunSnowLerpValue = (float)user.calorieRatio();
		//PersonDataAdapter.BodyHeight = 200;
		print(user.BMI());
		
            textCalorie.text = "Calorie Ratio: " + Math.Round(user.calorieRatio(), 2).ToString();
            textSleep.text = "Sleep Ratio: " + Math.Round(user.sleepRatio(), 2).ToString();
            textRun.text = "Run Ratio: " + Math.Round(user.runRatio(start, end), 2).ToString();
        }
        return true;
	}
	
	private float CircularLerp(float t)
	{
		return Mathf.Sqrt(1 - t*t);
	}
}
