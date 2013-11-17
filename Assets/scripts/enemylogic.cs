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
		
		if (collider.enabled == false)
		{
			spawntimer += Time.deltaTime;
			if (spawntimer > 1.0f)
			{
				collider.enabled = true;
			}
		}
		
		Debug.DrawRay(transform.position, transform.forward * evadedistance, Color.white, 0.1f);
		
		//transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		if (player && !evade)
		{
	    	transform.LookAt(player.transform.position + player.rigidbody.velocity);
		}

		//evasion state		
		pathfindingtimer += Time.deltaTime;
		if (pathfindingtimer > 0.33f)
		{
			Debug.DrawRay(transform.position, rigidbody.velocity.normalized * evadedistance, Color.red, 0.1f);
			int layermask = 1<<12 | 1<<8;
			RaycastHit hit;
	        Collider[] hitColliders = Physics.OverlapSphere(transform.position, evadedistance, layermask);

			if (hitColliders.Length > 0)
			{
					
				evade = true;
				float nearestobstacledistance = Mathf.Infinity;
				foreach(Collider obstacle in hitColliders)
				{
					if (Vector3.Distance(transform.position, obstacle.transform.position) < nearestobstacledistance)
					{
						nearestobstacledistance = Vector3.Distance(transform.position, obstacle.transform.position);
						evadetarget = obstacle.gameObject;
						Vector3 direction = transform.position - evadetarget.transform.position;
						//print (Vector3.Angle(transform.position, evadetarget.transform.position));
						Debug.DrawLine(transform.position, evadetarget.transform.position, Color.green);
						transform.rotation = Quaternion.LookRotation(direction);
					}
				}

			}
			else if (evade)
			{
				evade = false;
				evadetarget = null;
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
			//handle evasion velocity, or just move toward player
			if (evade && evadetarget)
			{
					rigidbody.AddForce(transform.forward * (movementspeed*0.5f), ForceMode.Impulse);
				
			}
			else
			{
				rigidbody.AddForce(transform.forward * movementspeed, ForceMode.Impulse);
			}

		}
	}
}
