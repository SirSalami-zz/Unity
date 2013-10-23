using UnityEngine;
using System.Collections;

public class planetlogic : MonoBehaviour {
	
	public GameObject target; //target to orbit around
	public GameObject player;
	public GameObject enemy; //is set in editor
	public bool trigger;
	public float rotationspeed;
	public float orbitspeed;

	// Use this for initialization
	void Start () {
		
		player = GameObject.Find("player");
		
		rotationspeed = Random.Range(20.0f, 200.0f);
		orbitspeed = Random.Range(20.0f, 50.0f);
        //rigidbody.AddTorque(Vector3.forward * -100);
		if (Random.Range(0, 2) < 1)
		{
			rotationspeed*=-1;
			orbitspeed*=-1;
		}

	}
	
	void Update() {
	    // rotation
	    transform.Rotate(Vector3.right * rotationspeed * Time.deltaTime);
	 
	    // orbit
	    transform.RotateAround (target.transform.position, Vector3.forward, orbitspeed * Time.deltaTime);
		
		//check if player is in range of sun to spawn. move this to sun at somepoint so it's called less
		//print (Vector3.Distance(target.transform.position+new Vector3(0,0,-60), player.transform.position));
		if (Vector3.Distance(target.transform.position+new Vector3(0,0,-60), player.transform.position) < 30)
		{
			trigger = true;
		}
		else
		{
			trigger = false;
		}
		
		//spawn enemies
		//print (timer);
		if (Time.frameCount%100 == 0 && trigger)
		{
			Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
		}
	}
}
