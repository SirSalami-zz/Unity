using UnityEngine;
using System.Collections;

public class createplanet : MonoBehaviour {
	
	public GameObject planet;
	private orbit orbitscript;

	// Use this for initialization
	void Start () {
		
		orbitscript = planet.GetComponent<orbit>();
		orbitscript.target = gameObject;
		Instantiate(planet, transform.position + new Vector3(0, 5+Random.Range(5, 25), 0), transform.rotation);
		Instantiate(planet, transform.position + new Vector3(0, 5+Random.Range(5, 25), 0), transform.rotation);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
