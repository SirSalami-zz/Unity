using UnityEngine;
using System.Collections;

public class planetlogic : MonoBehaviour {
	
	public GameObject systems;
	public GameObject target; //target to orbit around
	public GameObject player;
	public GameObject asteroidprefab;
	public float rotationspeed;
	public float orbitspeed;
	public float mydistance;
	public float maxhealth;
	public float health;
	public GameObject eruption;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		
		transform.localScale *= Random.Range(5.0f, 10f);
		
		rotationspeed = Random.Range(20.0f, 35.0f);
		//rotationspeed = 0.0f;
		
		orbitspeed = Random.Range(12.5f, 15.0f);
        //rigidbody.AddTorque(Vector3.forward * -100);
		if (Random.Range(0, 2) < 1)
		{
			rotationspeed*=-1;
		}
		if (Random.Range(0, 2) < 1)
		{
			orbitspeed*=-1;
		}

		mydistance = Vector3.Distance(target.transform.position+new Vector3(0,0,0), transform.position);
		
		maxhealth = 50;
		health = 50;
		
		eruption.transform.localScale = transform.localScale;
		
		rigidbody.AddForce(Random.Range(-10.0f, 10.0f),Random.Range(-10.0f, 10.0f),0, ForceMode.Impulse);
	}
	
	void Update() {
		
		// z-fight
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);

	    // rotation
	    transform.Rotate(Vector3.forward * rotationspeed * Time.deltaTime);
	 
	    // orbit
	    transform.RotateAround (target.transform.position, Vector3.forward, orbitspeed * Time.deltaTime);

		//health and effects
		
		if (health < maxhealth)
		{
			health += Time.deltaTime * 0.5f	;
		}
		
		eruption.particleSystem.emissionRate = maxhealth - health;
		
	}
	
	void FixedUpdate()
	{
		//planet gravity
		/*
		RaycastHit hit;
		int layermask = 1<<0 | 1<<8;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, (2+transform.localScale.x)*100, layermask);
		if (hitColliders.Length > 0)
		{
			foreach(Collider gravitytarget in hitColliders)
			{
				if (gravitytarget.rigidbody)
				{
					//Debug.DrawLine(transform.position, gravitytarget.transform.position, Color.cyan);
					Vector3 gravityforce = transform.position - gravitytarget.transform.position;
					gravitytarget.rigidbody.AddForce(gravityforce.normalized * 1f);
				}
			}
		}
		*/
	}
	
    void OnCollisionEnter(Collision collision) {
		
		if (health < 0)
		{
			//asteroid 1
			GameObject asteroidclone = Instantiate(asteroidprefab, transform.position, Random.rotation) as GameObject;
			asteroidclone.GetComponent<asteroidlogic>().size = transform.localScale.x*0.5f;
			//asteroidclone.GetComponent<asteroidlogic>().star = target;
			Vector3 randomforce = new Vector3(Random.Range(-25.0f*transform.localScale.x, 25.0f*transform.localScale.x), Random.Range(-25.0f*transform.localScale.x, 25.0f*transform.localScale.x), 0.0f);
			asteroidclone.rigidbody.AddForce(randomforce, ForceMode.Impulse);
			//asteroid 2
			asteroidclone = Instantiate(asteroidprefab, transform.position, Random.rotation) as GameObject;
			asteroidclone.GetComponent<asteroidlogic>().size = transform.localScale.x*0.5f;
			//asteroidclone.GetComponent<asteroidlogic>().star = target;
			asteroidclone.rigidbody.AddForce(randomforce*-1, ForceMode.Impulse);
			
			player.GetComponent<playerlogic>().score += 1000;
			Destroy(gameObject, 0.25f);	
		}
		else
		{
			health -= collision.transform.localScale.x*2;
		}

	}
	
}
