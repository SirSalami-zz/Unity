using UnityEngine;
using System.Collections;

public class playerlogic : MonoBehaviour {
	
	float mousetimer;
	public Vector3 mousepos;
	public Vector3 mouseinputposition;
	public float movementspeed = 10f;
	public float maxspeed = 25f;
    public float smooth = 4.5f;
	public float targetangle;
	public float movementtargetangle;
	public GameObject bulletclone;
	public bool charging = false;
	bool firing = false;
	public float chargetime;
	public float rapidfiredelay = 0.1f;
	float shottimer;
	int shotstofire;
	int shotsfired;
	bool shotbuffer;
	public bool dead;
	public GameObject shield;
	public int score;
	public GameObject solarenergy;
	public GameObject charge;
	int activeenemies;
	public GameObject missile;
	public GameObject homingtarget;
	
	//temp
	GameObject star;
	
	void Start()
	{
		rigidbody.freezeRotation = true;
		star = GameObject.Find("star");
	}
	
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
	

	
	void Update()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		//sets target angles based on mouse screen position when clicked or dragged
		Vector3 mousepos = new Vector3(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2, 0);
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			mouseinputposition = mousepos;
		}
		
		if (Input.GetKey(KeyCode.Mouse0))
		{
			//check if mouse is in chargebubble (2nd similar call (charging), need to consolidate)
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        RaycastHit hit;
	        if (!Physics.Raycast(ray, out hit, 50, 1 << 11))
			{
				targetangle = Vector3.Angle(Vector3.right, mousepos);
				if ((Input.mousePosition.y - Screen.height/2) < 0)
				{
		        	targetangle *=-1;
				}
				//only set movement target angle when draged a certain distance
				if (Vector3.Distance(mousepos, mouseinputposition) > 250.0f || mousetimer > 0.35)
				{
					if (!charging)
					{
						movementtargetangle = targetangle;
					}
				}
			}
		}
		
		if (!dead)
		{
			//temp: increase score when in close proximity to star. show particles (based on star scale)
			if (Vector3.Distance(transform.position, star.transform.position) < star.transform.localScale.x )
			{
				solarenergy.particleSystem.emissionRate = 10;
				score++;
			}
			else
			{
				solarenergy.particleSystem.emissionRate = 0;
			}
			
			//temp: missile targeting
			if (Input.GetKey(KeyCode.Mouse1))
			{
		        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		        RaycastHit hit;
		        if (Physics.Raycast(ray, out hit, 50, 1 << 9))
				{
					homingtarget = hit.transform.gameObject;
				}
				else
				{
				}
			}
			if (Input.GetKeyUp(KeyCode.Mouse1) && homingtarget)
			{
				missilelogic missilelogicscript = missile.GetComponent<missilelogic>();
				missilelogicscript.homingtarget = homingtarget;
				GameObject missileclone = Instantiate(missile, transform.position,  transform.rotation) as GameObject;
				missileclone.rigidbody.velocity = rigidbody.velocity;
				homingtarget = null;
			}
			///targeting temp
			
			//increase charge state when mouse1 is held on player
			if (Input.GetKey(KeyCode.Mouse0) && !firing)
			{
				mousetimer += Time.deltaTime;
				//check if mouse was just press and is within chargebubble
				if (mousetimer < 0.1f)
				{
			        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			        RaycastHit hit;
			        if (Physics.Raycast(ray, out hit, 50, 1 << 11))
					{
						charging = true;
						charge.particleSystem.Play();
					}
				}
			}
			
			if (charging)
			{
				TrailRenderer trail = gameObject.GetComponent<TrailRenderer>();
				trail.time = 0;
				charge.particleSystem.emissionRate = 50;
	   			chargetime += Time.deltaTime;
				chargetime = Mathf.Clamp(chargetime, 0, 3);
			}
			else
			{
				TrailRenderer trail = gameObject.GetComponent<TrailRenderer>();
				trail.time = 0.5f;
				charge.particleSystem.emissionRate = 0;
			}
			
			//set firing mode on mouse release or if there is a shot in the buffer
			if (Input.GetKeyUp (KeyCode.Mouse0) || shotbuffer)
			{
				if (mousetimer < 0.35f || chargetime >= 0.35f)
				{
					//remove shot from buffer
					if (shotbuffer)
					{
						shotbuffer = false;
					}
					
					if (!firing)
					{
						//set to rapidfire settings if very little chargetime
						if (chargetime <= 0.5f)
						{
							shotstofire = 3;
							chargetime = 0.2f;
							firing = true;
							shottimer = Time.time;
						}
						else
						{
							if (Vector3.Distance(mousepos, mouseinputposition) > 100.0f)
							{
								shotstofire = 1;
								firing = true;
								shottimer = Time.time;
							}
							else
							{
								chargetime = 0.0f;
							}
						}
					}
					else if (shotstofire < 6 && !shotbuffer)
					{
						shotbuffer = true;
					}
				}
				mousetimer = 0;
				charging = false;
			}
			
			//firing mechanism
			if (firing == true)
			{
				if (shotbuffer)
				{
					shotstofire += 3;
				}
				//instantiate shot, grows when charged, sets bullet to self destruct, adds reverse force to player
				Vector3 firepos = transform.position;
				if(Time.time > shottimer)
				{
					GameObject newbullet = Instantiate(bulletclone, firepos,  Quaternion.Euler(0, 0, targetangle)) as GameObject;
					newbullet.transform.localScale = newbullet.transform.localScale*chargetime;
					newbullet.rigidbody.mass = chargetime * 10;
					newbullet.rigidbody.velocity = rigidbody.velocity*0.5f;
					newbullet.rigidbody.AddForce(newbullet.transform.right * (chargetime*10000));
					newbullet.rigidbody.freezeRotation = true;
					Destroy(newbullet, 3);
					shotsfired++;
					shottimer = Time.time + rapidfiredelay;
					//handle kickback
					if (chargetime > 0.2f)
					{
						rigidbody.AddForce(newbullet.transform.right * (chargetime*movementspeed) * -1, ForceMode.Impulse);
					}
					else
					{
						rigidbody.AddForce(newbullet.transform.right * (chargetime*movementspeed*0.2f) * -1, ForceMode.Impulse);
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
	
 
    void FixedUpdate ()
    {
		if (!dead)
		{
			//smoothly rotates object in relation to target position (wtf is a quaternion) ;)
	        Quaternion target = Quaternion.Euler(0, 0, movementtargetangle);
	        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * smooth);
			
			//debugging rays
			Debug.DrawRay(transform.position, transform.right*100, Color.red, 0);
			//Debug.DrawRay(transform.position, star.transform.position - transform.position, Color.yellow, 0, false);
			if (Input.GetKey (KeyCode.Mouse0))
			{
				Debug.DrawRay(transform.position, mousepos, Color.green, 0);
			}
			
			//move in direction of target mouse position constantly. checks currently velocity and reduces if necessary
			if (!charging)
			{
				if (rigidbody.velocity.magnitude < maxspeed)
				{
					rigidbody.AddForce(transform.right * movementspeed);
				}
				else
				{
					rigidbody.AddForce(rigidbody.velocity * -1);
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
