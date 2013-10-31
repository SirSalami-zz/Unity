using UnityEngine;
using System.Collections;

public class cameralogic : MonoBehaviour {
	
	public GameObject player;
	playerlogic playerlogicscript;

	// Use this for initialization
	void Start () {
		
		playerlogicscript = player.GetComponent<playerlogic>();
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -260f);
		//transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -260f), 0.25f);
		if (playerlogicscript.charging)
		{
			float chargetime = playerlogicscript.chargetime * 0.5f;
			transform.position = new Vector3(player.transform.position.x + Random.Range(-0.1f, 0.1f)*chargetime, player.transform.position.y + Random.Range(-0.1f, 0.1f)*chargetime, -50f);
		}
		else
		{
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -50f);
		}
		
	}
}
