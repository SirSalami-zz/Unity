using UnityEngine;
using System.Collections;

public class starlogic : MonoBehaviour {
	
	public GameObject planet; // set in editor
	public GameObject player;
	private planetlogic planetlogicscript;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		
		
		//transform.position = new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);
		transform.localScale *= Random.Range(1.0f, 2.5f);
		
		planetlogicscript = planet.GetComponent<planetlogic>();
		planetlogicscript.target = gameObject;
		GameObject planetclone = Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x/2*Random.Range(2f, 8f), 0), transform.rotation) as GameObject;
		planetclone.GetComponent<planetlogic>().player = player;
	
		
		planetclone = Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x/2*Random.Range(2f, 8f), 0), transform.rotation) as GameObject;
		planetclone.GetComponent<planetlogic>().player = player;
		
		planetclone = Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x/2*Random.Range(2f, 8f), 0), transform.rotation) as GameObject;
		planetclone.GetComponent<planetlogic>().player = player;
		
		
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x*2+2, layermask);
		if (hitColliders.Length > 0)
		{
			foreach(Collider gravitytarget in hitColliders)
			{
				if (gravitytarget.rigidbody)
				{
					Debug.DrawLine(transform.position, gravitytarget.transform.position, Color.cyan);
					Vector3 gravityforce = transform.position - gravitytarget.transform.position;
					gravitytarget.rigidbody.AddForce(gravityforce.normalized * (transform.localScale.x*0.5f));
				}
			}
		}
	}
	
}
