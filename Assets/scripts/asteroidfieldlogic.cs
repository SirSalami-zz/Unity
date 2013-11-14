using UnityEngine;
using System.Collections;

public class asteroidfieldlogic : MonoBehaviour {
	
	public GameObject asteroidprefab;
	public float perlinscale;
	public float griddistance;
	public float size;
	GameObject[] stars;
	
	// Use this for initialization
	void Start () {
		
		stars = GameObject.FindGameObjectsWithTag("star");
		
		for(float x=0; x <= size; x+=griddistance)
		{
			for(float y=0; y <= size; y+=griddistance)
			{
				float perlin =  Mathf.PerlinNoise(x*0.01f,y*0.01f) * perlinscale;
				if (perlin >= 1.0f)
				{
					Vector3 myposition = transform.position;
					float closeststardistance = Mathf.Infinity;
					foreach(GameObject star in stars)
					{
						if (Vector3.Distance(star.transform.position, new Vector3(myposition.x+x, myposition.y+y, 0.0f)) < closeststardistance)
						{
							closeststardistance = Vector3.Distance(star.transform.position, new Vector3(myposition.x+x, myposition.y+y, 0.0f));
						}
					}
					if (closeststardistance > 100.0f)
					{
						GameObject asteroidclone = Instantiate(asteroidprefab, new Vector3(myposition.x+x+Random.Range(-griddistance, griddistance), myposition.y+y+Random.Range(-griddistance, griddistance), 0.0f), Random.rotation) as GameObject;
						asteroidclone.transform.parent = transform;
						asteroidclone.GetComponent<asteroidlogic>().size = perlin * 2;
					}
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
