using UnityEngine;
using System.Collections;

public class missilelogic : MonoBehaviour {
	
	public GameObject homingtarget;
	float timer;

	// Use this for initialization
	void Start () {
		
		timer = 0;
		rigidbody.AddForce(new Vector3(Random.Range(5.0f, 0.0f), Random.Range(-5.0f, 5.0f), 0) * 10, ForceMode.Impulse);
	
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
				transform.position += transform.forward*15*Time.deltaTime;
				//rigidbody.AddForce(transform.forward * 0.2f, ForceMode.Impulse);
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
