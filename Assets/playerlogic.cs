using UnityEngine;
using System.Collections;

public class playerlogic : MonoBehaviour {
	
	public float movementspeed = 10f;
	public float maxspeed = 25f;
    public float smooth = 4.5f;
	public float targetangle;
	public GameObject bulletclone;
	bool charging = false;
	bool firing = false;
	float chargetime;
	public float rapidfiredelay = 0.1f;
	float shottimer;
	int shotstofire;
	int shotsfired;
	public bool dead;
	public GameObject shield;
	public int score;
	public GameObject solarenergy;
	int activeenemies;
	public GameObject missle;
	public GameObject homingtarget;
	
	//temp
	GameObject star;
	
	void OnGUI()
	{
		GUI.Box(new Rect(Screen.width-100,Screen.height-20,100,20), "Score: " + score);
		activeenemies = GameObject.FindGameObjectsWithTag("enemy").Length;
		if (activeenemies > 0)
		{
			GUI.Box(new Rect(Screen.width/2-100,Screen.height-20,200,20), "ALERT! Threats Detected: " + activeenemies);
		}
		if (homingtarget)
		{
			GUI.Box(new Rect(Screen.width/2 - 50, 10, 100, 20), "Target Aquired");
		}
	}
	
 
    void Update ()
    {
		
		//targeting temp
		
		if (Input.GetKey(KeyCode.Mouse1))
		{
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        RaycastHit hit;
	        if (Physics.Raycast(ray, out hit, 50, 1 << 9))
			{
				homingtarget = hit.transform.gameObject;
				print ("target aquired");
			}
			else
			{
				print ("");
			}
		}
		if (Input.GetKeyUp(KeyCode.Mouse1) && homingtarget)
		{
			misslelogic misslelogicscript = missle.GetComponent<misslelogic>();
			misslelogicscript.homingtarget = homingtarget;
			GameObject missleclone = Instantiate(missle, transform.position,  transform.rotation) as GameObject;
			missleclone.rigidbody.velocity = rigidbody.velocity;
			homingtarget = null;
		}
		
		///targeting temp
		
		if (!dead)
		{
			//sets target angle based on mouse screen position when clicked
			Vector3 mousepos = new Vector3(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2, 0);
			if (Input.GetKey(KeyCode.Mouse0))
			{
				targetangle = Vector3.Angle(Vector3.right, mousepos);
				if ((Input.mousePosition.y - Screen.height/2) < 0)
				{
		        	targetangle *=-1;
				}

			}
			
			//smoothly rotates object in relation to target position (wtf is a quaternion) ;)
	        Quaternion target = Quaternion.Euler(0, 0, targetangle);
	        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * smooth);
			
			//debugging rays
			Debug.DrawRay(transform.position, transform.right*100, Color.red, 0);
			star = GameObject.Find("star");
			Debug.DrawRay(transform.position, star.transform.position - transform.position, Color.yellow, 0, false);
			if (Input.GetKey (KeyCode.Mouse0))
			{
				Debug.DrawRay(transform.position, mousepos, Color.green, 0);
			}
			
			//temp: increase score when in close proximity to star. show particles (based on star scale)
			if (Vector3.Distance(transform.position, star.transform.position+new Vector3(0,0,-60)) < star.transform.localScale.x )
			{
				solarenergy.particleSystem.emissionRate = 10;
				score++;
			}
			else
			{
				solarenergy.particleSystem.emissionRate = 0;
			}
			
			//move in direction of target mouse position constantly. checks currently velocity and reduces if necessary
			if (rigidbody.velocity.magnitude < maxspeed)
			{
				rigidbody.AddForce(transform.right * movementspeed);
			}
			else
			{
				rigidbody.AddForce(rigidbody.velocity * -1);
			}
	
			//increase charge state when mouse1 is held
			if (Input.GetKeyDown (KeyCode.Mouse0))
			{
				charging = true;
			}
			if (charging)
			{
	   			chargetime += Time.deltaTime;
				chargetime = Mathf.Clamp(chargetime, 0, 3);
			}
			
			//set firing mode forwardon mouse release
			if (Input.GetKeyUp (KeyCode.Mouse0) && !firing)
			{
				firing = true;
				charging = false;
				shottimer = Time.time + rapidfiredelay;
			}
			
			//firing mechanism
			if (firing == true)
			{
				//set to rapidfire settings if very little chargetime
				if (chargetime <= 0.5f)
				{
					shotstofire = 3;
					chargetime = 0.2f;
				}
				else
				{
					shotstofire = 1;
				}
				//instantiate shot, grows when charged, sets bullet to self destruct, adds reverse force to player
				Vector3 firepos = transform.position;
				if(Time.time > shottimer)
				{
					GameObject newbullet = Instantiate(bulletclone, firepos,  transform.rotation) as GameObject;
					newbullet.transform.localScale = newbullet.transform.localScale*chargetime;
					newbullet.rigidbody.mass = chargetime * 10;
					newbullet.rigidbody.velocity = rigidbody.velocity*0.5f;
					newbullet.rigidbody.AddForce(newbullet.transform.right * (chargetime*10000));
					Destroy(newbullet, 3);
					shotsfired++;
					shottimer = Time.time + rapidfiredelay;
					if (chargetime > 0.2f)
					{
						rigidbody.AddForce(transform.right * (chargetime*10f) * -1, ForceMode.Impulse);
					}
					else
					{
						rigidbody.AddForce(transform.right * (chargetime*5f) * -1, ForceMode.Impulse);
					}
				}
				//handing rapidfire
				if (shotsfired >= shotstofire)
				{
					firing = false;
					shottimer = 0;
					shotsfired = 0;
					chargetime = 0;
				}
			}
		}
    }
	
	// bad collision event
	public GameObject explosion;
	
    void OnCollisionEnter(Collision collision) {
		if (!shield)
		{
			GameObject explosionclone = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
			Destroy(explosionclone, 5.0f);
			rigidbody.velocity = new Vector3(0, 0, 0);
			rigidbody.detectCollisions = false;
			dead = true;
			//Destroy(gameObject, 3.0f);
		}
	}
}
