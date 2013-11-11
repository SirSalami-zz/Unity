using UnityEngine;
using System.Collections;

public class time : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyUp (KeyCode.Alpha0))
		{
			Time.timeScale = 1.0f;
			print ("time: 100%");
		}
		if (Input.GetKeyUp (KeyCode.Alpha9))
		{
			Time.timeScale = 0.90f;
		}
		if (Input.GetKeyUp (KeyCode.Alpha8))
		{
			Time.timeScale = 0.80f;
		}
		if (Input.GetKeyUp (KeyCode.Alpha7))
		{
			Time.timeScale = 0.70f;
		}
		if (Input.GetKeyUp (KeyCode.Alpha6))
		{
			Time.timeScale = 0.60f;
		}
		if (Input.GetKeyUp (KeyCode.Alpha5))
		{
			Time.timeScale = 0.50f;
		}
		if (Input.GetKeyUp (KeyCode.Alpha4))
		{
			Time.timeScale = 0.40f;
		}
		if (Input.GetKeyUp (KeyCode.Alpha3))
		{
			Time.timeScale = 0.30f;
		}
		if (Input.GetKeyUp (KeyCode.Alpha2))
		{
			Time.timeScale = 0.20f;
		}
		if (Input.GetKeyUp (KeyCode.Alpha1))
		{
			Time.timeScale = 0.10f;
		}
		
	}
}
