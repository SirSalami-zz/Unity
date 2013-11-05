using UnityEngine;
using System.Collections;

public class planetlogic : MonoBehaviour {
	
	public GameObject systems;
	public GameObject target; //target to orbit around
	public GameObject player;
	public GameObject enemy; //is set in editor
	public bool trigger;
	public float rotationspeed;
	public float orbitspeed;
	public float mydistance;
	public float maxhealth;
	public float health;
	public GameObject eruption;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		
		transform.localScale *= Random.Range(1.0f, 3.5f);
		
		rotationspeed = Random.Range(20.0f, 75.0f);
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
			if (Time.timeScale > 0)
			{
				trigger = true;
			}
		}	
		else
		{
			trigger = false;
		}
		
		//spawn enemies
		//print (timer);
		if (Time.frameCount%100 == 0 && trigger && Time.timeScale > 0)
		{
			GameObject enemyclone = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation) as GameObject;
			enemyclone.GetComponent<enemylogic>().player = player;
			enemyclone.GetComponent<collision>().player = player;
		}
		
		//health and effects
		
		if (health < maxhealth)
		{
			health += Time.deltaTime * 0.5f	;
		}
		
		eruption.particleSystem.emissionRate = maxhealth - health;
		
	}
	
    void OnCollisionEnter(Collision collision) {
		
		if (health < 0)
		{
			Destroy(gameObject, 0.25f);	
		}
		else
		{
			health -= collision.transform.localScale.x*2;
		}

	}
	
}
