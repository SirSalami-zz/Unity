using UnityEngine;
using System.Collections;

public class starlogic : MonoBehaviour {
	
	public GameObject planet; // set in editor
	private planetlogic planetlogicscript;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		
		
		transform.position = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		transform.localScale *= Random.Range(1.0f, 3.5f);
		
		planetlogicscript = planet.GetComponent<planetlogic>();
		planetlogicscript.target = gameObject;
		Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x*Random.Range(1f, 2.5f), 0), transform.rotation);
		Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x*Random.Range(1f, 2.5f), 0), transform.rotation);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	
	}
}
