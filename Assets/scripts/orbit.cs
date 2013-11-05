using UnityEngine;
using System.Collections;

public class orbit : MonoBehaviour {
	
	public GameObject target;
	public float rotationspeed;
	public float orbitspeed;

	// Use this for initialization
	void Start () {
		
		rotationspeed = Random.Range(-200.0f, 200.0f);
		orbitspeed = Random.Range(-50.0f, 50.0f);
        //rigidbody.AddTorque(Vector3.forward * -100);

	}
 
	void Update() {
	    // rotation
	    transform.Rotate(Vector3.right * rotationspeed * Time.deltaTime);
	 
	    // orbit
	    transform.RotateAround (target.transform.position, Vector3.forward, orbitspeed * Time.deltaTime);
	}
}
