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
		transform.localScale *= Random.Range(2.0f, 5.5f);
		
		planetlogicscript = planet.GetComponent<planetlogic>();
		planetlogicscript.target = gameObject;
		GameObject planetclone = Instantiate(planet, transform.position + new Vector3(0, transform.localScale.x*Random.Range(1f, 2.5f), 0), transform.rotation) as GameObject;
		planetclone.GetComponent<planetlogic>().player = player;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		
		//print (Vector3.Distance(player.transform.position, transform.position) + " / " + transform.localScale.x);
		
		
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
}
