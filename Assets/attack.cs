using UnityEngine;
using System.Collections;

public class attack : MonoBehaviour {
	
	public GameObject player; // drag the player here
	
	// Use this for initialization
	void Start () {
		
		player = GameObject.Find("player");
	
	}
	
	public float movementspeed = 10;
	
	void Update() {
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	    transform.LookAt(player.transform.position);
		//move in direction of target constantly. checks currently velocity and reduces if necessary
		if (rigidbody.velocity.magnitude > movementspeed)
		{
			rigidbody.AddForce(rigidbody.velocity * -movementspeed);
			//print (rigidbody.velocity.magnitude);
		}
		rigidbody.AddForce(transform.forward * movementspeed, ForceMode.Impulse);
	}
}
