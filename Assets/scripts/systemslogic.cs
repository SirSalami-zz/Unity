using UnityEngine;
using System.Collections;

public class systemslogic : MonoBehaviour {
	
	public GameObject playerprefab;
	public GameObject starprefab;
	public GameObject asteroidfieldprefab;
	public GameObject wormholeprefab;

	// Use this for initialization
	void Start () {
		
		GameObject player = Instantiate(playerprefab, new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0), transform.rotation) as GameObject;
		
		Camera.main.GetComponent<cameralogic>().player = player;
		
		
		Vector3 starposition = transform.position;
		GameObject starclone = Instantiate(starprefab, starposition, transform.rotation) as GameObject;
		starclone.GetComponent<starlogic>().player = player;
		
		
		/*
		starposition = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		starclone = Instantiate(starprefab, starposition, transform.rotation) as GameObject;
		starclone.GetComponent<starlogic>().player = player;
		
		starposition = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		starclone = Instantiate(starprefab, starposition, transform.rotation) as GameObject;
		starclone.GetComponent<starlogic>().player = player;
		*/
		
		
		Vector3 asteroidfieldposition = new Vector3(-250.0f, -250.0f, 0f);
		GameObject asteroidfieldclone = Instantiate(asteroidfieldprefab, asteroidfieldposition, transform.rotation) as GameObject;
		asteroidfieldclone.GetComponent<asteroidfieldlogic>().size = Mathf.Round(Random.Range(500.0f, 500.0f));
		
		Instantiate(wormholeprefab, new Vector3(Random.Range(-250f, 250f), Random.Range(-250f, 250f), 0f), transform.rotation);
		
	}
	
	float timer;
	int count = 1;
	int current = 0;
	
	// Update is called once per frame
	void Update () {
		
		if (timer > 60.0f)
		{
			while (current < count)
			{
				Instantiate(wormholeprefab, new Vector3(Random.Range(-250f, 250f), Random.Range(-250f, 250f), 0f), transform.rotation); 
				current++;
			}
			count++;
			current = 0;
			timer = 0;
		}
		timer+=Time.deltaTime;
	
	}
}
