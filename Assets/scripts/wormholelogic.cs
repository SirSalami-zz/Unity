using UnityEngine;
using System.Collections;

public class wormholelogic : MonoBehaviour {
	
	public GameObject enemyprefab;
	public GameObject player;
	float spawntimer;
	float spawntimertrigger;
	float lastcheck;
	
	// Use this for initialization
	void Start () {
		
		player = GameObject.FindGameObjectWithTag("Player");
		spawntimer = 3.0f;
		lastcheck = Time.time;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//spawn enemies
		//print (spawntimertrigger);
		if (spawntimertrigger > spawntimer && Time.timeScale == 1.0f)
		{
			Instantiate(enemyprefab, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
			//enemyclone.GetComponent<enemylogic>().player = player;
			spawntimertrigger = 0.0f;
		}
		spawntimertrigger+=Time.deltaTime;
		
		//reverse gratvity
		if (Time.time - lastcheck > 0.5)
		{
			lastcheck = Time.time;
			RaycastHit hit;
			int layermask = 1<<0 | 1<<12;
	        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20, layermask);
			if (hitColliders.Length > 0)
			{
				foreach(Collider gravitytarget in hitColliders)
				{
					if (gravitytarget.rigidbody)
					{
						//Debug.DrawLine(transform.position, gravitytarget.transform.position, Color.cyan);
						Vector3 gravityforce = transform.position - gravitytarget.transform.position;
						gravitytarget.rigidbody.AddForce((gravityforce.normalized * -2500f)*Time.deltaTime, ForceMode.Impulse);
					}
				}
			}
		}
	
	}
	
	void FixedUpdate() {
	
		transform.Rotate(new Vector3(0f, 0f, 50f)*Time.deltaTime);
		
	}
}
