using UnityEngine;
using System.Collections;

public class shieldlogic : MonoBehaviour {
	
	public GameObject player;
	public float shieldhealth;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		shieldhealth = 5.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	
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
