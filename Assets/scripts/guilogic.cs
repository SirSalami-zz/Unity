using UnityEngine;
using System.Collections;

public class guilogic : MonoBehaviour {
	
	public GameObject player;
	public playerlogic playerlogicscript;
	public GameObject systems;
	public systemslogic systemslogicscript;
	bool toggleTxt;
	
	// Use this for initialization
	void Start () {
			toggleTxt = false;
	}
	
	// Update is called once per frame
	void OnGUI() {
		
		if (player && playerlogicscript)
		{
			//score
			GUI.Box(new Rect(Screen.width-100,Screen.height-30,100,20), "Score: " + Mathf.Round(playerlogicscript.score));
			
			//threat detection
			int activeenemies = GameObject.FindGameObjectsWithTag("enemy").Length;
			if (activeenemies > 0)
			{
				GUI.Box(new Rect(Screen.width/2-100,Screen.height-20,200,20), "ALERT! Threats Detected: " + activeenemies);
			}
			
			//missile targeting
			if (playerlogicscript.homingtargets.Count > 0)
			{
				GUI.Box(new Rect(Screen.width/2 - 75, 10, 170, 20), "Targets Aquired: " + playerlogicscript.homingtargets.Count);
			}
			
			//shield
			
			GUI.Box(new Rect(0, Screen.height-50, 100, 25), "Shield: " + Mathf.Round(playerlogicscript.shield.GetComponent<shieldlogic>().shieldhealth) + "/ 3");
			
			
			//energy
			
			GUI.Box(new Rect(0, Screen.height-30, 150, 25), "Energy: " + Mathf.Round(playerlogicscript.energy) + " / 1000");

			
		}
		else if (!player)
		{

		}
		else if (!playerlogicscript)
		{
			playerlogicscript = player.GetComponent<playerlogic>();
		}
		
		if (playerlogicscript.energy > 1000 && !playerlogicscript.dead)
		{
	        if (GUI.Button(new Rect(Screen.width/2 -25, Screen.height/2 + 30, 50, 30), "Warp"))
			{
				Application.LoadLevel (0);
			}
		}
		
		if (playerlogicscript.dead)
		{
	        if (GUI.Button(new Rect(Screen.width/2 -45, Screen.height/2 + 30, 90, 30), "Game Over"))
			{
				Application.LoadLevel (0);
			}
		}
	
        if (playerlogicscript.pause)
		{
			if  (GUI.Button(new Rect(Screen.width-50, 0 , 50, 30), "Quit"))
			{
				Application.Quit();
			}
			

			toggleTxt = GUI.Toggle(new Rect(75, 5, 100, 30), toggleTxt, "Stats");
			
			if (toggleTxt)
			{
				
				GUI.Box(new Rect(0, 25, 200, 25), "Acceleration: " + playerlogicscript.acceleration);
				if (GUI.Button(new Rect(200, 25, 25, 25), "-"))
				{
					playerlogicscript.acceleration-=25.0f;
				}
				if (GUI.Button(new Rect(225, 25, 25, 25), "+"))
				{
					playerlogicscript.acceleration+=25.0f;
				}
				
				
				GUI.Box(new Rect(0, 50, 200, 25), "Max Speed: " + playerlogicscript.maxspeed);
				if (GUI.Button(new Rect(200, 50, 25, 25), "-"))
				{
					playerlogicscript.maxspeed-=1.0f;
				}
				if (GUI.Button(new Rect(225, 50, 25, 25), "+"))
				{
					playerlogicscript.maxspeed+=1.0f;
				}
				

				GUI.Box(new Rect(0, 75, 200, 25), "Turn Speed: " + playerlogicscript.smooth);
				if (GUI.Button(new Rect(200, 75, 25, 25), "-"))
				{
					playerlogicscript.smooth-=0.1f;
				}
				if (GUI.Button(new Rect(225, 75, 25, 25), "+"))
				{
					playerlogicscript.smooth+=0.1f;
				}
				

				GUI.Box(new Rect(0, 75, 200, 25), "Turn Speed: " + playerlogicscript.smooth);
				if (GUI.Button(new Rect(200, 75, 25, 25), "-"))
				{
					playerlogicscript.smooth-=0.1f;
				}
				if (GUI.Button(new Rect(225, 75, 25, 25), "+"))
				{
					playerlogicscript.smooth+=0.1f;
				}
				
				
				GUI.Box(new Rect(0, 100, 200, 25), "Decelerator: " + (Mathf.Round(100f-(playerlogicscript.brakes*100))));
				if (GUI.Button(new Rect(200, 100, 25, 25), "-"))
				{
					playerlogicscript.brakes+=0.01f;
				}
				if (GUI.Button(new Rect(225, 100, 25, 25), "+"))
				{
					playerlogicscript.brakes-=0.01f;
				}
				
				
				GUI.Box(new Rect(0, 125, 200, 25), "Mass: " + player.rigidbody.mass);
				if (GUI.Button(new Rect(200, 125, 25, 25), "-"))
				{
					player.rigidbody.mass-=0.5f;
				}
				if (GUI.Button(new Rect(225, 125, 25, 25), "+"))
				{
					player.rigidbody.mass+=0.5f;
				}
				
				
				GUI.Box(new Rect(0, 175, 200, 25), "Shots fired: " + playerlogicscript.maxrapidfireshots);
				if (GUI.Button(new Rect(200, 175, 25, 25), "-"))
				{
					playerlogicscript.maxrapidfireshots-=1;
				}
				if (GUI.Button(new Rect(225, 175, 25, 25), "+"))
				{
					playerlogicscript.maxrapidfireshots+=1;
				}
				
				
				GUI.Box(new Rect(0, 200, 200, 25), "Shot Speed: " + playerlogicscript.shotspeed);
				if (GUI.Button(new Rect(200, 200, 25, 25), "-"))
				{
					playerlogicscript.shotspeed-=100f;
				}
				if (GUI.Button(new Rect(225, 200, 25, 25), "+"))
				{
					playerlogicscript.shotspeed+=100f;
				}
				
				
				GUI.Box(new Rect(0, 225, 200, 25), "Shot Rate: " + playerlogicscript.rapidfiredelay);
				if (GUI.Button(new Rect(200, 225, 25, 25), "-"))
				{
					playerlogicscript.rapidfiredelay-=0.01f;
				}
				if (GUI.Button(new Rect(225, 225, 25, 25), "+"))
				{
					playerlogicscript.rapidfiredelay+=0.01f;
				}
				
				
				GUI.Box(new Rect(0, 250, 200, 25), "Kick-back: " + playerlogicscript.kickback);
				if (GUI.Button(new Rect(200, 250, 25, 25), "-"))
				{
					playerlogicscript.kickback-=0.5f;
				}
				if (GUI.Button(new Rect(225, 250, 25, 25), "+"))
				{
					playerlogicscript.kickback+=0.5f;
				}
				
				
				
			}
			
		}
	
	}
}