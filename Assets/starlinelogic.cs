using UnityEngine;
using System.Collections;

public class starlinelogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public GameObject star; //set in editor
	public GameObject player; //set in editor
	
	// Update is called once per frame
	void Update () {
		
		LineRenderer starline = gameObject.GetComponent<LineRenderer>();
		starline.SetPosition(0, player.transform.position);
		starline.SetPosition(1, star.transform.position);
	
	}
}
