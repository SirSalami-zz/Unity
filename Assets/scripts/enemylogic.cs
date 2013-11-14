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
	    	transform.LookAt(player.transform.position);
		}
		//move in direction of target constantly. checks currently velocity and reduces if necessary
		
		pathfindingtimer += Time.deltaTime;
		if (pathfindingtimer > 0.33f)
		{
			Debug.DrawRay(transform.position, rigidbody.velocity.normalized * evadedistance, Color.red, 0.1f);
			RaycastHit hit;
	        Collider[] hitColliders = Physics.OverlapSphere(transform.position, evadedistance, 1<<12);

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
			if (evade && evadetarget)
			{
				Vector3 direction = transform.position - evadetarget.transform.position;
				print (Vector3.Angle(transform.position, evadetarget.transform.position));

					Debug.DrawLine(transform.position, evadetarget.transform.position, Color.green);
					transform.rotation = Quaternion.LookRotation(direction);
					rigidbody.AddForce(transform.forward * movementspeed, ForceMode.Impulse);
				
			}
			else
			{
				rigidbody.AddForce(transform.forward * movementspeed, ForceMode.Impulse);
			}

		}
	}
}
