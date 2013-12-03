using UnityEngine;
using System.Collections;

public class enemylogic1 : MonoBehaviour {
	
	Vector3 star;
	public GameObject player;
	float pathfindingtimer;
	public float evadedistance;
	bool evade;
	GameObject evadetarget;
	float spawntimer;
	public int hitpoints = 1;
	public GameObject explosion;
	Vector3 myposition;
	Rigidbody mybody;
	
	// Use this for initialization
	void Start () {
		
		evadedistance = 10f;
		
		spawntimer += Time.deltaTime;
		star = GameObject.FindGameObjectWithTag("star").transform.position;
		player = GameObject.FindGameObjectWithTag("Player");
		transform.LookAt(star);
		//rigidbody.AddForce(transform.forward * 100.0f, ForceMode.Impulse);
	
	}
	
	void Update () {
		
		star = GameObject.FindGameObjectWithTag("star").transform.position;
		player = GameObject.FindGameObjectWithTag("Player");
		
		myposition = transform.position;
		
		if (collider.enabled == false)
		{
			spawntimer += Time.deltaTime;
			if (spawntimer > 1.0f)
			{
				collider.enabled = true;
			}
		}
		
		Debug.DrawRay(myposition, transform.forward * evadedistance, Color.white, 0.1f);
		
		//transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		if (!evade)
		{
	    	transform.LookAt(star);
		}

		//evasion state		
		pathfindingtimer += Time.deltaTime;
		if (pathfindingtimer > 0.33f)
		{
			Debug.DrawRay(myposition, transform.forward * evadedistance, Color.red, 0.1f);
			Debug.DrawRay(myposition, (transform.forward*-1) * evadedistance, Color.red, 0.1f);
			Debug.DrawRay(myposition, transform.up * evadedistance, Color.red, 0.1f);
			Debug.DrawRay(myposition, (transform.up*-1) * evadedistance, Color.red, 0.1f);
			int layermask = 1<<12 | 1<<8 | 1<<13;
			RaycastHit hit;
	        Collider[] hitColliders = Physics.OverlapSphere(myposition, evadedistance, layermask);
			if (hitColliders.Length > 0)
			{
					
				evade = true;
				float nearestobstacledistance = Mathf.Infinity;
				foreach(Collider obstacle in hitColliders)
				{
					if (Vector3.Distance(myposition, obstacle.transform.position) < nearestobstacledistance)
					{
						nearestobstacledistance = Vector3.Distance(myposition, obstacle.transform.position);
						evadetarget = obstacle.gameObject;
						Vector3 direction = myposition - evadetarget.transform.position;
						//print (Vector3.Angle(transform.position, evadetarget.transform.position));
						Debug.DrawLine(myposition, evadetarget.transform.position, Color.yellow, 0.33f);
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
	
	public float movementspeed = 0.1f;
	
	void FixedUpdate() {
		mybody = rigidbody;

		if (mybody.velocity.magnitude > movementspeed)
		{
			mybody.AddForce(mybody.velocity * -movementspeed);
			//print (rigidbody.velocity.magnitude);
		}
		else
		{
			//handle evasion velocity, or just move toward star
			if (evade && evadetarget)
			{
					mybody.AddForce(transform.forward * (movementspeed*0.5f), ForceMode.Impulse);
				
			}
			else
			{
				mybody.AddForce(transform.forward * movementspeed, ForceMode.Impulse);
			}

		}
	}
	
    void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal*-5, Color.white, 1.0f);
			hitpoints--;
        }
		if (hitpoints <= 0)
		{
			//print (controlscript.score);
			player.GetComponent<playerlogic>().score += 250;
			GameObject explosionclone = Instantiate(explosion, myposition, transform.rotation) as GameObject;
			Destroy(explosionclone, 2.0f);
			Destroy(gameObject);
		}
		//print ("ping");
    }
}
