using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class VectorCircleLine : MonoBehaviour {

	public int CircleVertexCount;
	public Material LineMaterial;
	public float CircleRadius;
	public float LineWidth;
	public Color LineColor = Color.white;
	
	void Start()
	{
		if(GetComponent<LineRenderer>() == null)
			DrawCircle();
	}
	
	void DrawCircle()
	{
		LineRenderer CircleLine = gameObject.AddComponent<LineRenderer> ();
		CircleLine.useWorldSpace = false;
		CircleLine.material = LineMaterial;
		CircleLine.SetVertexCount (CircleVertexCount + 1);
		for (int count = 0; count < CircleVertexCount; count++) {
			CircleLine.SetPosition (count, new Vector3 (CircleRadius * Mathf.Cos (2 * Mathf.PI * count / CircleVertexCount), CircleRadius * Mathf.Sin (2 * Mathf.PI * count / CircleVertexCount)));
		}
		CircleLine.SetPosition (CircleVertexCount, new Vector3 (CircleRadius * Mathf.Cos (0), CircleRadius * Mathf.Sin (0)));
		CircleLine.SetColors(LineColor, LineColor);
		CircleLine.useLightProbes = false;
		CircleLine.enabled = true;
		CircleLine.SetWidth (LineWidth, LineWidth);
	}
}
