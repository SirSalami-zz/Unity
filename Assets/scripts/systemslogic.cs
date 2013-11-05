using UnityEngine;
using System.Collections;

public class systemslogic : MonoBehaviour {
	
	public GameObject playerprefab;
	public GameObject starprefab;

	// Use this for initialization
	void Start () {
		
		GameObject player = Instantiate(playerprefab, transform.position, transform.rotation) as GameObject;
		
		Camera.main.GetComponent<cameralogic>().player = player;
		
		Vector3 starposition = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		GameObject starclone = Instantiate(starprefab, starposition, transform.rotation) as GameObject;
		starclone.GetComponent<starlogic>().player = player;
		
		
		
		starposition = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		starclone = Instantiate(starprefab, starposition, transform.rotation) as GameObject;
		starclone.GetComponent<starlogic>().player = player;
		
		starposition = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		starclone = Instantiate(starprefab, starposition, transform.rotation) as GameObject;
		starclone.GetComponent<starlogic>().player = player;
		
		starposition = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		starclone = Instantiate(starprefab, starposition, transform.rotation) as GameObject;
		starclone.GetComponent<starlogic>().player = player;
		
		
	
	}
	
	// Update is called once per frame
	void Update () {
		

	
	}
}
