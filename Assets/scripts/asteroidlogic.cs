using UnityEngine;
using System.Collections;

public class asteroidlogic : MonoBehaviour {
	
	public float size;
	float health;
	public GameObject asteroidprefab;
	public GameObject explosionprefab;
	float spawntimer;
	float timertrigger;
	public GameObject star;

	// Use this for initialization
	void Start () {
		
		health = size*1;
		transform.localScale = new Vector3(size, size, size);
		//gameObject.rigidbody.mass = size;
		//rigidbody.AddForce(Random.Range(-100.0f, 100.0f),Random.Range(-100.0f, 100.0f),0, ForceMode.Impulse);
		rigidbody.AddTorque(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f));
	}
	
	// Update is called once per frame
	void Update () {
		
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
			if (transform.position.z != 0)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, 0);
			}
			timertrigger = 0;
		}
		
		//transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
	
	}
	
	void FixedUpdate() {
		
		if (star)
		{
			transform.RotateAround (star.transform.position, Vector3.forward, (200/(Vector3.Distance(transform.position, star.transform.position))) * Time.deltaTime);
		}
		
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
			
			if (size > 1)
			{
				GameObject asteroidclone = Instantiate(asteroidprefab, transform.position, Random.rotation) as GameObject;
				asteroidclone.GetComponent<asteroidlogic>().size = size*0.5f;
				Vector3 randomforce = new Vector3(Random.Range(5.0f*size, 15.0f*size), Random.Range(5.0f*size, 15.0f*size), 0.0f);
				asteroidclone.rigidbody.AddForce(randomforce, ForceMode.Impulse);
				asteroidclone = Instantiate(asteroidprefab, transform.position, Random.rotation) as GameObject;
				asteroidclone.GetComponent<asteroidlogic>().size = size*0.5f;
				asteroidclone.rigidbody.AddForce(randomforce*-1, ForceMode.Impulse);
			}
			
			Destroy(gameObject);
			
			GameObject explosionclone = Instantiate(explosionprefab, transform.position, transform.rotation) as GameObject;
			Destroy(explosionclone, 3.0f);
			
		}
		else
		{
			health -= 1;
		}

	}
	
}
