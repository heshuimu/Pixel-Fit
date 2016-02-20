using UnityEngine;
using System.Collections.Generic;

public class GetProfileButtonAction : MonoBehaviour, IMSHealthJSONResponder {
	
	public void GetProfile()
	{
		MSHealthInterface.PerformRequest(this, "Activities");
	}
	
	public bool RespondToJSONData(Dictionary<string, object> dic)
	{
		print("I got the thing!!");
		return true;
	}

}
