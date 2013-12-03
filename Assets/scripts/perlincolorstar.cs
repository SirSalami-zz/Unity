using UnityEngine;
using System.Collections;

public class perlincolorstar : MonoBehaviour {
	
	public float intensity;

	void buildtexture(int sizex, int sizey)
	{
		Texture2D texture = new Texture2D(sizex, sizey);
		
		for(int x = 0; x < sizex; x++)
		{
			
			for(int y = 0; y < sizey; y++)
			{
				float perlin =  Mathf.PerlinNoise(x*0.1f,y*0.1f)*intensity;
				texture.SetPixel(x, y, new Color(perlin, perlin, perlin));
				//texture.SetPixel(x, y, new Color(Random.Range(0f, intensity), Random.Range(0, intensity), Random.Range(0, intensity)));
			}
			
		}
		
		//texture.filterMode = FilterMode.Point;
		//bumptexture.filterMode = FilterMode.Point;
		
		texture.Apply();
		
		MeshRenderer mrenderer = GetComponent<MeshRenderer>();
		mrenderer.material.mainTexture = texture;
		
	}
	
	void Start()
	{
		intensity = Random.Range(1.5f, 1.5f);
		buildtexture(128, 128);
	}
	
	void FixedUpdate()
	{
		//buildtexture (256, 256);
	}
	
/*
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
*/
	
}