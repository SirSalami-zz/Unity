using UnityEngine;
using System.Collections;

public class starlinelogic : MonoBehaviour {
	
	public GameObject star; //set in editor
	public GameObject player; //set in editor

	// Use this for initialization
	void Start () {
		
		player = GameObject.Find("player");
	
	}
	
	// Update is called once per frame
	void Update () {
		
		LineRenderer starline = gameObject.GetComponent<LineRenderer>();
		//starline.SetPosition(0, player.transform.position);
		//starline.SetPosition(1, star.transform.position);
	
	}
}
