using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject player;
	
	// Update is called once per frame
	void Update () {
		
		//transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -260f);
		//transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -260f), 0.25f);
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -50f);
		
	}
}
