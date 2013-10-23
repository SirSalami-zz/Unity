using UnityEngine;
using System.Collections;

public class misslelogic : MonoBehaviour {
	
	public GameObject homingtarget;
	float timer;

	// Use this for initialization
	void Start () {
		
		timer = 0;
		rigidbody.AddForce(transform.right * 50, ForceMode.Impulse);
	
	}
	
	// Update is called once per frame
	
	void Update () {
		
		timer += Time.deltaTime;
		
		if (timer > 4.0f)
		{
			Destroy(gameObject, 0.25f);
		}
		else if (homingtarget)
		{
			if (timer > 0.5f)
			{
				transform.LookAt(homingtarget.transform.position);
				rigidbody.AddForce(transform.forward * 50);
			}
			else
			{
				rigidbody.velocity *= 0.8f;
			}
		}	
	}
	
    void OnCollisionEnter(Collision collision) {
		
		Destroy(gameObject);
		
	}
	
}
