using UnityEngine;
using System.Collections;

public class planetlogic : MonoBehaviour {
	
	public GameObject systems;
	public GameObject target; //target to orbit around
	public GameObject player;
	public GameObject enemy; //is set in editor
	public GameObject asteroidprefab;
	bool trigger;
	public float rotationspeed;
	public float orbitspeed;
	public float mydistance;
	public float maxhealth;
	public float health;
	public GameObject eruption;
	public float spawntimer;
	float spawntimertrigger;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		
		transform.localScale *= Random.Range(2.0f, 4.5f);
		
		rotationspeed = Random.Range(20.0f, 35.0f);
		//rotationspeed = 0.0f;
		
		orbitspeed = target.transform.localScale.x*Random.Range(0.2f, 0.6f);
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
		spawntimer = 3.0f;
		
		eruption.transform.localScale = transform.localScale;
	}
	
	void Update() {
		
		// z-fight
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);

	    // rotation
	    transform.Rotate(Vector3.forward * rotationspeed * Time.deltaTime);
	 
	    // orbit
	    transform.RotateAround (target.transform.position, Vector3.forward, orbitspeed * Time.deltaTime);
		
		//check if player is in range of sun to spawn. move this to sun at somepoint so it's called less
		//print (Vector3.Distance(target.transform.position+new Vector3(0,0,-60), player.transform.position));
		float playerdistance = Vector3.Distance(target.transform.position, player.transform.position);

		if (renderer.isVisible || playerdistance < mydistance)
		{
			if (Time.timeScale == 1)
			{
				trigger = true;
			}
		}	
		else
		{
			trigger = false;
		}
		
		//spawn enemies
		print (spawntimertrigger);
		if (spawntimertrigger > spawntimer && trigger && Time.timeScale == 1.0f)
		{
			GameObject enemyclone = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation) as GameObject;
			enemyclone.GetComponent<enemylogic>().player = player;
			enemyclone.GetComponent<collision>().player = player;
			spawntimertrigger = 0.0f;
		}
		spawntimertrigger+=Time.deltaTime;
		
		//health and effects
		
		if (health < maxhealth)
		{
			health += Time.deltaTime * 0.5f	;
		}
		
		eruption.particleSystem.emissionRate = maxhealth - health;
		
	}
	
	void FixedUpdate()
	{
		RaycastHit hit;
		int layermask = 1<<0 | 1<<8;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x*2+2, layermask);
		if (hitColliders.Length > 0)
		{
			foreach(Collider gravitytarget in hitColliders)
			{
				if (gravitytarget.rigidbody)
				{
					Debug.DrawLine(transform.position, gravitytarget.transform.position, Color.cyan);
					Vector3 gravityforce = transform.position - gravitytarget.transform.position;
					gravitytarget.rigidbody.AddForce(gravityforce.normalized * (transform.localScale.x*0.8f));
				}
			}
		}
	}
	
    void OnCollisionEnter(Collision collision) {
		
		if (health < 0)
		{
			GameObject asteroidclone = Instantiate(asteroidprefab, transform.position, Random.rotation) as GameObject;
			asteroidclone.GetComponent<asteroidlogic>().size = transform.localScale.x*0.5f;
			Vector3 randomforce = new Vector3(Random.Range(-25.0f*transform.localScale.x, 25.0f*transform.localScale.x), Random.Range(-25.0f*transform.localScale.x, 25.0f*transform.localScale.x), 0.0f);
			asteroidclone.rigidbody.AddForce(randomforce, ForceMode.Impulse);
			asteroidclone = Instantiate(asteroidprefab, transform.position, Random.rotation) as GameObject;
			asteroidclone.GetComponent<asteroidlogic>().size = transform.localScale.x*0.5f;
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
