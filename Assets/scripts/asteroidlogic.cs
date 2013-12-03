using UnityEngine;
using System.Collections;

public class asteroidlogic : MonoBehaviour {
	
	public float size;
	float health;
	public GameObject asteroidprefab;
	public GameObject explosionprefab;
	float spawntimer;
	float timertrigger;
	Vector3 myposition;

	// Use this for initialization
	void Start () {
		
		health = size*1;
		float randomsize = Random.Range(1.0f, size/2);
		transform.localScale = new Vector3(randomsize, randomsize, randomsize);
		gameObject.rigidbody.mass *= size;
		//rigidbody.AddForce(Random.Range(-100.0f, 100.0f),Random.Range(-100.0f, 100.0f),0, ForceMode.Impulse);
		rigidbody.AddTorque(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f));
	}
	
	// Update is called once per frame
	void Update () {
		
		myposition = transform.position;
		
		Debug.DrawRay(myposition, rigidbody.velocity*rigidbody.velocity.magnitude, Color.grey);
		
		if (collider.enabled == false)
		{
			spawntimer += Time.deltaTime;
			if (spawntimer > 0.25f)
			{
				collider.enabled = true;
			}
		}
		
		timertrigger += Time.deltaTime;
		
		if (renderer.isVisible && timertrigger >= 1.0f)
		{
			if (myposition.z != 0)
			{
				myposition = new Vector3(myposition.x, myposition.y, 0);
			}
			timertrigger = 0;
		}
		
		
		//generate shadow with linerenderer
		/*
		gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
		Vector3 shadow = new Ray(transform.position, star.transform.position).direction;
		gameObject.GetComponent<LineRenderer>().SetPosition(1, shadow*0);
		gameObject.GetComponent<LineRenderer>().SetWidth(size/2, size*2);
		*/
		
	
	}
	
    void OnCollisionEnter(Collision collision) {
		
		if (health < 0)
		{
			print ("name: " + collision.gameObject.tag);
			if (collision.gameObject.tag == "bullet")
			{
				GameObject player = GameObject.FindGameObjectWithTag("Player");
				if (player)
				{
					player.GetComponent<playerlogic>().score+=10f;
				}
			}
			
			
			collider.enabled = false;
			
			if (size > 6)
			{
				Vector3 randomforce = new Vector3(Random.Range(75.0f, 150.0f), Random.Range(75.0f, 150.0f), 0.0f);
				Vector3 randomposition =  new Vector3(Random.Range(size/6, size/6), Random.Range(size/6, size/6), 0);
				GameObject asteroidclone = Instantiate(asteroidprefab, myposition + randomposition, Random.rotation) as GameObject;
				asteroidclone.GetComponent<asteroidlogic>().size = size*0.5f;
				asteroidclone.rigidbody.AddForce(randomforce, ForceMode.Impulse);
				asteroidclone = Instantiate(asteroidprefab, myposition - randomposition, Random.rotation) as GameObject;
				asteroidclone.GetComponent<asteroidlogic>().size = size*0.5f;
				asteroidclone.rigidbody.AddForce(randomforce*-1, ForceMode.Impulse);
			}
			
			Destroy(gameObject);
			
			GameObject explosionclone = Instantiate(explosionprefab, myposition, transform.rotation) as GameObject;
			Destroy(explosionclone, 3.0f);
			
		}
		else
		{
			health -= 1;
		}

	}
	
}
