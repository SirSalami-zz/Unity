using UnityEngine;
using System.Collections;

public class asteroidlogic : MonoBehaviour {
	
	public float size;
	float health;
	public GameObject asteroidprefab;
	public GameObject explosionprefab;

	// Use this for initialization
	void Start () {
		
		health = 1.5f;
		transform.localScale = new Vector3(size, size, size);
		gameObject.rigidbody.mass *= size*2;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
	
	}
	
    void OnCollisionEnter(Collision collision) {
		
		if (health < 0)
		{
			
			if (size > 2)
			{
				GameObject asteroidclone = Instantiate(asteroidprefab, transform.position, Random.rotation) as GameObject;
				asteroidclone.GetComponent<asteroidlogic>().size = size*0.5f;
				Vector3 randomforce = new Vector3(Random.Range(-55.0f*size, 55.0f*size), Random.Range(-55.0f*size, 55.0f*size), 0.0f);
				asteroidclone.rigidbody.AddForce(randomforce, ForceMode.Impulse);
				asteroidclone = Instantiate(asteroidprefab, transform.position, Random.rotation) as GameObject;
				asteroidclone.GetComponent<asteroidlogic>().size = size*0.5f;
				asteroidclone.rigidbody.AddForce(randomforce*-1, ForceMode.Impulse);
			}
			
			GameObject explosionclone = Instantiate(explosionprefab, transform.position, transform.rotation) as GameObject;
			
			
			Destroy(gameObject, 0.05f);	
			Destroy(explosionclone, 3.0f);
		}
		else
		{
			health -= 1;
		}

	}
	
}
