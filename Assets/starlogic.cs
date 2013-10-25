using UnityEngine;
using System.Collections;

public class starlogic : MonoBehaviour {
	
	public GameObject planet; // set in editor
	private planetlogic planetlogicscript;

	// Use this for initialization
	void Start () {
		
		
		transform.position = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 60f);
		transform.localScale *= Random.Range(0.5f, 2.5f);
		
		planetlogicscript = planet.GetComponent<planetlogic>();
		planetlogicscript.target = gameObject;
		Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x+Random.Range(0, 25), 0), transform.rotation);
		Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x+Random.Range(0, 25), 0), transform.rotation);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
