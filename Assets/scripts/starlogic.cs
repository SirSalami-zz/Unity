using UnityEngine;
using System.Collections;

public class starlogic : MonoBehaviour {
	
	public GameObject planet; // set in editor
	public GameObject player;
	public GameObject asteroid;
	private planetlogic planetlogicscript;
	Ray asteroidray;
	
	void makeplanet(float orbit)
	{
		planetlogicscript = planet.GetComponent<planetlogic>();
		planetlogicscript.target = gameObject;
		Ray vposition = new Ray(transform.position, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f));
		GameObject planetclone = Instantiate(planet, (vposition.origin + vposition.direction*((2+orbit)+ (transform.localScale.x*2))), transform.rotation) as GameObject;
		planetclone.GetComponent<planetlogic>().player = player;
	}
	
	void makeasteroid(float orbit)
	{
		Ray vposition = new Ray(transform.position, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f));
		GameObject asteroidclone = Instantiate(asteroid, (vposition.origin + vposition.direction*((2+orbit)+ (transform.localScale.x*2))), transform.rotation) as GameObject;
		asteroidclone.GetComponent<asteroidlogic>().size = Random.Range(1f, 3f);
		asteroidclone.GetComponent<asteroidlogic>().star = gameObject;
		//asteroidclone.GetComponent<planetlogic>().player = player;
	}
	
	
		int planetstospawn = 1;
		int planetcount;
		int asteroidstospawn = 15;
		int asteroids;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		
		
		//transform.position = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		transform.localScale *= Random.Range(25.0f, 25.0f);
		
		planetcount=1;
		while (planetcount <= planetstospawn)
		{
			makeplanet(planetcount);
			makeplanet(5);
			makeplanet(9);
			planetcount++;
		}
		
		asteroids=1;
		while (asteroids <= asteroidstospawn)
		{
			makeasteroid(3);
			asteroids++;
		}
		
		asteroids=1;
		while (asteroids <= asteroidstospawn)
		{
			makeasteroid(7);
			asteroids++;
		}
		

	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);

		//set refueling state of player if close proximity
		if (Vector3.Distance(player.transform.position, transform.position) < transform.localScale.x)
		{
			player.GetComponent<playerlogic>().refueling = true;
		}
		else
		{
			player.GetComponent<playerlogic>().refueling = false;
		}
		
		//Debug.DrawRay(transform.position, transform.right*50, Color.white);
		asteroidray = new Ray(transform.position, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
		Debug.DrawRay(asteroidray.origin, asteroidray.direction*50, Color.white);
	
	}
	
	
	void FixedUpdate()
	{
		RaycastHit hit;
		int layermask = 1<<0 | 1<<8;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, (2+transform.localScale.x)*2, layermask);
		if (hitColliders.Length > 0)
		{
			foreach(Collider gravitytarget in hitColliders)
			{
				if (gravitytarget.rigidbody)
				{
					Debug.DrawLine(transform.position, gravitytarget.transform.position, Color.cyan);
					Vector3 gravityforce = transform.position - gravitytarget.transform.position;
					gravitytarget.rigidbody.AddForce(gravityforce.normalized * 2f);
				}
			}
		}
	}
	
}
