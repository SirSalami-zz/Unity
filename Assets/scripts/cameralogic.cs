using UnityEngine;
using System.Collections;

public class cameralogic : MonoBehaviour {
	
	public GameObject player;
	playerlogic playerlogicscript;

	// Use this for initialization
	void Start () {
		
		if (player)
		{
			playerlogicscript = player.GetComponent<playerlogic>();
		}
	
	}
	
	float zoomlerp;
	
	// Update is called once per frame
	void Update () {
		
		zoomlerp=Time.deltaTime;
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 25.0f+player.rigidbody.velocity.magnitude*2.0f, zoomlerp);
		
		if (player)
		{
				
			if (playerlogicscript && playerlogicscript.charging)
			{
				float chargetime = playerlogicscript.chargetime * 0.5f;
				transform.position = new Vector3(player.transform.position.x + Random.Range(-0.1f, 0.1f)*chargetime, player.transform.position.y + Random.Range(-0.25f, 0.25f)*chargetime, transform.position.z);
			}
			else
			{
				transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
			}
		}
		else
		{
			player = GameObject.Find("player");
		}
		
	}
}
