using UnityEngine;
using System.Collections;

public class perlintexture : MonoBehaviour {
	
	public float perlinscale;
	float debugtimer;
	float red = Random.Range(0f, 1f);
	float green = Random.Range(0f, 1f);
	float blue = Random.Range(0f, 1f);

	void buildtexture(int sizex, int sizey)
	{
		Texture2D texture = new Texture2D(sizex, sizey);		
		
		for(int x = 0; x < sizex; x++)
		{
			
			for(int y = 0; y < sizey; y++)
			{
				float perlin =  Mathf.PerlinNoise(x*0.1f,y*0.1f)*perlinscale;
				texture.SetPixel(x, y, new Color(perlin*red, perlin*green, perlin*blue));
				//texture.SetPixel(x, y, new Color(Random.Range(0f, perlinscale), Random.Range(0, perlinscale), Random.Range(0, perlinscale)));
			}
			
		}
		
		texture.filterMode = FilterMode.Point;
		
		texture.Apply();
		
		MeshRenderer mrenderer = GetComponent<MeshRenderer>();
		mrenderer.material.mainTexture = texture;
		
	}
	
	void Update()
	{
		if (debugtimer > 1.0f)
		{
			debugtimer = 0;
			buildtexture(64, 64);
		}
		else
		{
			debugtimer += Time.deltaTime;
		}
	}
	
}