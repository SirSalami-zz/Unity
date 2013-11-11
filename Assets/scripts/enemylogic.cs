using UnityEngine;
using System.Collections;

public class enemylogic : MonoBehaviour {
	
	public GameObject player; // drag the player here
	float pathfindingtimer;
	public float evadedistance;
	bool evade;
	GameObject evadetarget;
	float spawntimer;
	
	// Use this for initialization
	void Start () {
		
		evadedistance = 5.0f;
		
		spawntimer += Time.deltaTime;
		//player = GameObject.Find("player");
	
	}
	
	void Update () {
		
		spawntimer += Time.deltaTime;
		if (spawntimer > 2.0f)
		{
			collider.enabled = true;
		}
		
		//transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		if (player)
		{
	    	transform.LookAt(player.transform.position);
		}
		//move in direction of target constantly. checks currently velocity and reduces if necessary
		
		pathfindingtimer += Time.deltaTime;
		if (pathfindingtimer > 0.33f)
		{
			Debug.DrawRay(transform.position, rigidbody.velocity.normalized * evadedistance, Color.red, 0.1f);
			RaycastHit hit;
	        Collider[] hitColliders = Physics.OverlapSphere(transform.position, evadedistance, 1 << 12);

			if (hitColliders.Length > 0)
			{
					
				evade = true;
				evadetarget = hitColliders[0].gameObject;

				print(hitColliders[0].transform.tag);
			}
			else if (evade)
			{
				evade = false;
				evadetarget = null;
				print("");
			}
			pathfindingtimer = 0.0f;
		}
		
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
			if (evade && evadetarget)
			{
				rigidbody.AddForce(evadetarget.transform.position.normalized * (movementspeed*-0.25f), ForceMode.Impulse);
			}
			else
			{
				rigidbody.AddForce(transform.forward * movementspeed, ForceMode.Impulse);
			}
		}
	}
}
