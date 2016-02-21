using UnityEngine;
using System.Collections;

public class PersonController : MonoBehaviour {

	[Range(0.25f, 0.9f)]
	public float BodySizePercentage;
	[Range(1f, 2.5f)]
	public float BodyHeight;
	public float HeadRadius = 0.5f;
	[Range(1f, 3f)]
	public float LegLength;
	
	public Transform Body;
	public Transform Head;
	public Transform LeftLeg;
	public Transform RightLeg;
	public Transform LeftArm;
	public Transform RightArm;
	public Transform FacialExpression;
	
	void Awake()
	{
		PersonDataAdapter.personCtrl = this;
	}
	
	void Start()
	{
		RefreshBody();
	}
	
	public void ChangeLeftArmAngle(float z)
	{
		Vector3 a = LeftArm.localEulerAngles;
		a.z = z;
		LeftArm.localEulerAngles = a;
	}
	
	public void ChangeRightArmAngle(float z)
	{
		Vector3 a = RightArm.localEulerAngles;
		a.z = z;
		RightArm.localEulerAngles = a;
	}
	
	//ranges from -100% to 100%
	public void ChangeFaceExpressionWithPercentage(float percentage)
	{
		Vector3 n = FacialExpression.localScale;
		n.y = percentage;
		FacialExpression.localScale = n;
	}
	
	//ranges from 0% to 100%
	public void RefreshBody()
	{
		float yPosMinimum = BodyHeight - HeadRadius;
		float yPosMaximum = BodyHeight;
		float xScaleMaximum = HeadRadius;
		float xScaleMinimum = 0;
		
		float xScaleLerp = Mathf.Lerp(xScaleMinimum, xScaleMaximum, BodySizePercentage);
		float yScaleLerp = Mathf.Lerp(yPosMinimum, yPosMaximum, 1-CircularLerp(BodySizePercentage));
		
		Vector3 s = Body.localScale;
		s.x = xScaleLerp;
		s.y = yScaleLerp;
		Body.localScale = s;
		
		Vector3 p = Body.localPosition;
		p.y = -BodyHeight;
		Body.localPosition = p;
		
		Vector3 las = LeftArm.localPosition;
		las.x = -xScaleLerp;
		LeftArm.localPosition = las;
		
		Vector3 ras = RightArm.localPosition;
		ras.x = xScaleLerp;
		RightArm.localPosition = ras;
		
		Vector3 llp = LeftLeg.localPosition;
		llp.y = -BodyHeight;
		LeftLeg.localPosition = llp;
		
		Vector3 rlp = RightLeg.localPosition;
		rlp.y = -BodyHeight;
		RightLeg.localPosition = rlp;
		
		Vector3 lls = LeftLeg.localScale;
		lls.y = LegLength;
		LeftLeg.localScale = lls;
		
		Vector3 rls = RightLeg.localScale;
		rls.y = LegLength;
		RightLeg.localScale = rls;
	}
	
	private float CircularLerp(float t)
	{
		return Mathf.Sqrt(1 - t*t);
	}
}
