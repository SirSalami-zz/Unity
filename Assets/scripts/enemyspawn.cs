using UnityEngine;
using System.Collections;

public class enemyspawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject player;
	public GameObject enemy;
	
	// Update is called once per frame
	void Update () {
		
		//print (timer);
		if (Time.frameCount%100 == 0)
		{
			GameObject enemyclone = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation) as GameObject;
		}
	
	}
}
