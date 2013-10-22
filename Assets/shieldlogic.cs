using UnityEngine;
using System.Collections;

public class shieldlogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject player;
	
	// Update is called once per frame
	void Update () {
		
		transform.position = player.transform.position;
	
	}
	
    void OnCollisionEnter(Collision collision) {
		Destroy(gameObject, 0.25f);	
		//print ("ping");
	}
}
