using UnityEngine;
using System.Collections;

public class shieldlogic : MonoBehaviour {
	
	public GameObject player;
	public float shieldhealth;

	// Use this for initialization
	void Start () {
		
		shieldhealth = 5.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = player.transform.position;
	
	}
	
    void OnCollisionEnter(Collision collision) {
		
		if (shieldhealth < 0)
		{
			Destroy(gameObject, 0.25f);	
		}
		else
		{
			shieldhealth -= 1;
		}

	}
}
