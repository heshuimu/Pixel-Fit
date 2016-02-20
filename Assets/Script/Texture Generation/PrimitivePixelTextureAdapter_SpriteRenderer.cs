using UnityEngine;
using System.Collections;

public class PrimitivePixelTextureAdapter_SpriteRenderer : MonoBehaviour {
	
	public SpriteRenderer Renderer;
	public PixelTextureGenerationUtility.PrimitiveShapes Shape;
	public Rect TextureSize;

	// Use this for initialization
	void Start () 
	{
		if(Renderer == null)
		{
			Destroy(this);
		}
		Renderer.sprite = Sprite.Create(
			PixelTextureGenerationUtility.GenerateTexture((int)TextureSize.width, (int)TextureSize.height, Shape), 
			TextureSize, 
			new Vector2(0.5f, 0.5f)
		);
	}
}
