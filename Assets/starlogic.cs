using UnityEngine;
using System.Collections;

public class starlogic : MonoBehaviour {
	
	public GameObject planet; // set in editor
	private planetlogic planetlogicscript;

	// Use this for initialization
	void Start () {
		
		planetlogicscript = planet.GetComponent<planetlogic>();
		Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x+Random.Range(0, 25), 0), transform.rotation);
		Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x+Random.Range(0, 25), 0), transform.rotation);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
