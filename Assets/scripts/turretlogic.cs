using UnityEngine;
using System.Collections;

public class turretlogic : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject player;
	float smooth = 15.0f;
	
		
	void Update () 
	
	{ 
	
		//smoothly rotates object in relation to target position (wtf is a quaternion) ;)
        Quaternion target = Quaternion.Euler(0, 0, player.GetComponent<playerlogic>().targetangle);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * smooth);
	
	}
			

}
