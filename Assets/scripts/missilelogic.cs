using UnityEngine;
using System.Collections;

public class missilelogic : MonoBehaviour {
	
	public GameObject homingtarget;
	float timer;
	Vector3 lastknownposition;

	// Use this for initialization
	void Start () {
		
		timer = 0;
		float random = Random.Range (0, 2);

			rigidbody.AddForce(new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), 0), ForceMode.Impulse);
			//rigidbody.AddTorque(new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f)));
	
	}
	
	// Update is called once per frame
	float accelerationlimit;
	
	void Update () {
		
		timer += Time.deltaTime;
		
		if (homingtarget)
		{
			lastknownposition = homingtarget.transform.position;
		}
		
		if (timer > 5.0f)
		{
			Destroy(gameObject, 0.25f);
		}
		else
		{
			if (timer > 0.25f)
			{
				if (gameObject.GetComponent<TrailRenderer>().time < 3f)
				{
					gameObject.GetComponent<TrailRenderer>().time = 3f;
				}
				transform.LookAt(lastknownposition);
				transform.position += (transform.forward*(Mathf.Clamp(accelerationlimit, 0, 25f)))*Time.deltaTime;
				accelerationlimit += Time.deltaTime*20;
				Mathf.Clamp(accelerationlimit, 0f, 250f);
				//rigidbody.AddForce(transform.forward * 0.2f, ForceMode.Impulse);
				/*
				if (Vector3.Distance(transform.position, homingtarget.transform.position) < 1.0f)
				{
					collider.enabled = true;
				}
				*/
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
