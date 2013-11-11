using UnityEngine;
using System.Collections;

public class asteroidfieldlogic : MonoBehaviour {
	
	public GameObject asteroidprefab;
	public float size;
	
	// Use this for initialization
	void Start () {
		
		for(float x=0; x <= size; x+=20.0f)
		{
			for(float y=0; y <= size; y+=20.0f)
			{
				float perlin =  Mathf.PerlinNoise(x*0.01f,y*0.01f) * 1.5f;
				if (perlin >= 1.0f)
				{
					GameObject asteroidclone = Instantiate(asteroidprefab, new Vector3(transform.position.x+x, transform.position.y+y, 0.0f), Random.rotation) as GameObject;
					asteroidclone.transform.parent = transform;
					asteroidclone.GetComponent<asteroidlogic>().size = perlin * 5;
				}
				//asteroidclone.GetComponent<asteroidlogic>().size = Random.Range(3.0f,0.0f);
				//asteroidclone.GetComponent<asteroidlogic>().size = 1;
				
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
