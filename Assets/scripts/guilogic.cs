using UnityEngine;
using System.Collections;

public class guilogic : MonoBehaviour {
	
	public GameObject player;
	public playerlogic playerlogicscript;
	public GameObject systems;
	public systemslogic systemslogicscript;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI() {
		
		if (player && playerlogicscript)
		{
			//score
			GUI.Box(new Rect(Screen.width-100,Screen.height-30,100,20), "Score: " + playerlogicscript.score);
			
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
			GUI.Box(new Rect(Screen.width-100, 0, 100, 20), "Level ");
		}
	
	}
}
