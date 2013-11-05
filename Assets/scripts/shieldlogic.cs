using UnityEngine;
using System.Collections;

public class shieldlogic : MonoBehaviour {
	
	public GameObject player;
	public float shieldhealth;
	public float shieldhealthmax;

	// Use this for initialization
	void Start () {
		
		shieldhealth = 3.0f;
		shieldhealthmax = 3.0f;
		rigidbody.freezeRotation = true;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (shieldhealth > shieldhealthmax)
		{
			shieldhealth = shieldhealthmax;
		}
		
		if (shieldhealth <= 0)
		{
			shieldhealth = 0.0f;
			renderer.enabled = false;
			rigidbody.detectCollisions = false;
			//Destroy(gameObject, 0.25f);	
		}
		else
		{
			renderer.enabled = true;
			rigidbody.detectCollisions = true;
		}
	
	}
	
    void OnCollisionEnter(Collision collision) {
		
		shieldhealth -= 1;

	}
}
