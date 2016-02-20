using UnityEngine;
using System.Collections;

public static class PixelTextureGenerationUtility {
	
	public enum PrimitiveShapes
	{
		Square,
		Ellipse,
		Triagle,
		SquareFrame,
		EllipseFrame,
		TriagleFrame
	}
	
	public static Texture2D GenerateTexture(int x, int y, PrimitiveShapes shape)
	{
		switch(shape)
		{
			case PrimitiveShapes.SquareFrame:
				return GenerateTextureWithBooleanMap(GetSquareFrameBooleanMap(x, y));
			case PrimitiveShapes.Square: 
				return GenerateTextureWithBooleanMap(FillFrame(GetSquareFrameBooleanMap(x, y)));
			default:
				return null;
		}
	}
	
	private static bool[][] GetSquareFrameBooleanMap(int x, int y)
	{
		bool[][] map = new bool[x][];
		for(int i = 0; i < x; i++)
		{
			map[i] = new bool[y];
			
			if(i == 0 || i == x - 1)
			{
				for(int j = 0; j < y; j++)
				{
					map[i][j] = true;
				}
			}
			else
			{
				map[i][0] = true;
				map[i][y-1] = true;
			}
		}
		return map;
	}
	
	private static bool[][] FillFrame(bool[][] map)
	{
		int x = map.Length;
		for(int i = 0; i < x; i++)
		{
			int y = map[i].Length;
			for(int j = 1; j < y - 1; j++)
			{
				if(map[i][j-1])
				{
					map[i][j] = true;
				}
			}
		}
		
		return map;
	}
	
	private static Texture2D GenerateTextureWithBooleanMap(bool[][] map)
	{
		Color ClearWhite = new Color(1, 1, 1, 0);
		
		int x = map.Length;
		int y = map[0].Length;
		
		Texture2D t = new Texture2D(x, y);
		t.wrapMode = TextureWrapMode.Clamp;
		t.filterMode = FilterMode.Point;
		
		for(int i = 0; i < x; i++)
		{
			for(int j = 0; j < y; j++)
			{
				if(map[i][j])
				{
					t.SetPixel(i, j, Color.white);
				}
				else
				{
					t.SetPixel(i, j, ClearWhite);
				}
			}
		}
		
		t.Apply();
		
		return t;
	}
}
