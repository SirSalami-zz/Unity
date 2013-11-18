using UnityEngine;
using System.Collections;

public class starlogic : MonoBehaviour {
	
	public GameObject planet; // set in editor
	public GameObject player;
	private planetlogic planetlogicscript;
	
	void makeplanet(float orbit)
	{
		planetlogicscript = planet.GetComponent<planetlogic>();
		planetlogicscript.target = gameObject;
		GameObject planetclone = Instantiate(planet, transform.position + new Vector3(0, ((transform.localScale.x*0.9f)+orbit*10+Random.Range(orbit, orbit*5)), 0), transform.rotation) as GameObject;
		planetclone.GetComponent<planetlogic>().player = player;
	}
	
	
		int planetstospawn = 5;
		int planetcount;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		
		
		//transform.position = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		transform.localScale *= Random.Range(2.0f, 4.0f);
		
		while (planetcount <= planetstospawn)
		{
			makeplanet(planetcount);
			planetcount++;
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
					gravitytarget.rigidbody.AddForce(gravityforce.normalized * Vector3.Distance(transform.position, gravitytarget.transform.position)*0.20f);
				}
			}
		}
	}
	
}
