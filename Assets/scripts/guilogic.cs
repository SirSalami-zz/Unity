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
			if  (GUI.Button(new Rect(0, 0 , 50, 30), "Quit"))
			{
				Application.Quit();
			}
			

			GUI.Toggle(new Rect(Screen.width/2, 10, 100, 30), toggleTxt, "A Toggle text");
			
			if (toggleTxt)
			{
        		playerlogicscript.acceleration = GUI.HorizontalSlider(new Rect(Screen.width/2, Screen.height/2, 100, 30), playerlogicscript.acceleration, 0.0F, 100.0F);
			}
			
			GUI.Box(new Rect(Screen.width-100, 0, 100, 20), "Level ");
		}
	
	}
}
