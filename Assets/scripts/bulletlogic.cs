using UnityEngine;
using System.Collections;

public class bulletlogic : MonoBehaviour {
	
	public float chargetime;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void OnCollisionEnter(Collision collision)
	{
		if (chargetime <= 0.2f)
		{
			Destroy(gameObject);
		}
	}
}
