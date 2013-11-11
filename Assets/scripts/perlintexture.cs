using UnityEngine;
using System.Collections;

public class perlintexture : MonoBehaviour {

	void buildtexture(int sizex, int sizey)
	{
		Texture2D texture = new Texture2D(sizex, sizey);
		print ("texture done!");
		
		
		for(int x = 0; x < sizex; x++)
		{
			
			for(int y = 0; y < sizey; y++)
			{
				float sample = Mathf.PerlinNoise(x, y);
				texture.SetPixel(x, y, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
			}
			
		}
		
		texture.filterMode = FilterMode.Point;
		
		texture.Apply();
		
		MeshRenderer mrenderer = GetComponent<MeshRenderer>();
		mrenderer.sharedMaterial.mainTexture = texture;
		
	}
	
	void Start()
	{
		buildtexture(16, 16);
	}
	
}