using UnityEngine;
using System.Collections;

public class enemylogic : MonoBehaviour {
	
	public GameObject player; // drag the player here
	
	// Use this for initialization
	void Start () {
		
		//player = GameObject.Find("player");
	
	}
	
	void Update () {
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		if (player)
		{
	    	transform.LookAt(player.transform.position);
		}
		//move in direction of target constantly. checks currently velocity and reduces if necessary
		
	}
	
	public float movementspeed = 10;
	
	void FixedUpdate() {

		if (rigidbody.velocity.magnitude > movementspeed)
		{
			rigidbody.AddForce(rigidbody.velocity * -movementspeed);
			//print (rigidbody.velocity.magnitude);
		}
		else
		{
			rigidbody.AddForce(transform.forward * movementspeed, ForceMode.Impulse);
		}
	}
}
