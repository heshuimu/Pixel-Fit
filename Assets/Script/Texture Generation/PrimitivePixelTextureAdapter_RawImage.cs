using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrimitivePixelTextureAdapter_RawImage : MonoBehaviour {
	
	public RawImage Renderer;
	public PixelTextureGenerationUtility.PrimitiveShapes Shape;
	public Rect TextureSize;

	// Use this for initialization
	void Start () 
	{
		if(Renderer == null)
		{
			Destroy(this);
		}
		Renderer.texture = PixelTextureGenerationUtility.GenerateTexture((int)TextureSize.width, (int)TextureSize.height, Shape);
	}
	
}
