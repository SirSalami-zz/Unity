using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerlogic : MonoBehaviour {
	
	public GameObject guisystem;
	float mousetimer;
	public Vector3 mousepos;
	public Vector3 mouseinputposition;
	public float acceleration;
	public float maxspeed;
    public float smooth;
	public float targetangle;
	public float movementtargetangle;
	public GameObject bulletclone;
	public bool charging = false;
	bool firing = false;
	public float chargetime;
	public float rapidfiredelay;
	public float shotspeed;
	public float kickback;
	Vector3 kickbackvelocity;
	float shottimer;
	public int maxrapidfireshots;
	int shotstofire;
	int shotsfired;
	bool shotbuffer;
	public bool dead;
	public GameObject shield;
	public float score;
	public GameObject solarenergy;
	public GameObject charge;
	public GameObject missile;
	public GameObject homingtarget;
	public List<GameObject> homingtargets = new List<GameObject>();
	public bool pause;
	float zoomlerp;
	public float energy;
	public bool refueling;
	public GameObject closeststar;
	public GameObject missiletargetingline;
	public bool moving;
	public float brakes;

	
	//temp
	GameObject star;
	
	void Start()
	{
		rigidbody.freezeRotation = true;
		star = GameObject.Find("star");
		guisystem = GameObject.Find("guisystem");
		guisystem.GetComponent<guilogic>().player = gameObject;
		
		//stats
		energy = 500.0f;
		maxrapidfireshots = 3;
		brakes = 0.99f;
		kickback = 2;
		shotspeed = 1;
		rapidfiredelay = 0.1f;
		smooth = 5f;
		acceleration = 25.0f;
		maxspeed = 10.0f;
		shotspeed = 5000.0f;
	}

	
	void Update()
	{
		//zoomout and pause
		if (Input.GetAxis("Mouse ScrollWheel") < 0 && !pause)
		{

			pause = true;

		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && pause)
		{
			pause = false;
		}
		//handle pinch
		if (Input.touchCount >= 2)
		{
		    if (pause)
			{
				pause = false;
			}
			else
			{
				pause = true;
			}
		}
		if (pause && zoomlerp < 150.0f)
		{
			Camera.main.orthographic = true;
			Camera.main.farClipPlane = 1000.0f;
			if (zoomlerp < 1)
			{
				zoomlerp += 0.05f;
				Camera.main.orthographicSize = Mathf.Lerp (20.0f, 150.0f, zoomlerp);
			}
			Time.timeScale = 0.0f;
		}
		else
		{
			Camera.main.orthographic = false;
			Camera.main.farClipPlane = 1000.0f;
			zoomlerp = 0;
			Time.timeScale = 1;
		}
		
		//energy
		
		//find closest star
		GameObject[] stars;
		stars = GameObject.FindGameObjectsWithTag("star");
		float stardistance = Mathf.Infinity;
		foreach(GameObject star in stars)
		{
			float stardistancecheck = Vector3.Distance(transform.position, star.transform.position);
			if (stardistancecheck < stardistance)
			{
				closeststar = star;
				stardistance = stardistancecheck;
			}
		}
		
		//set refueling state of player if close proximity
		if (closeststar && stardistance < closeststar.transform.localScale.x+2)
		{
			refueling = true;
		}
		else
		{
			refueling = false;
		}
		
		//constanly deplete energy		
		if (energy > 0)
		{
			energy -= Time.deltaTime;
			if (moving)
			{
				energy -= Time.deltaTime*5;
			}
			if (firing)
			{
				energy -= Time.deltaTime*10;
			}
			if (charging && chargetime < 3.0f)
			{
				energy -= Time.deltaTime*25;
			}
		}
		
		//sets target angles based on mouse screen position when clicked or dragged
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
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
	        if (!Physics.Raycast(ray, out hit, 500, 1 << 11))
			{
				targetangle = Vector3.Angle(Vector3.right, mousepos);
				if ((Input.mousePosition.y - Screen.height/2) < 0)
				{
		        	targetangle *=-1;
				}
				//only set movement target angle when draged a certain distance
				if (Vector3.Distance(mousepos, mouseinputposition) > 80.0f || mousetimer > 0.35)
				{
					if (!charging)
					{
						moving = true;
						movementtargetangle = targetangle;
					}
				}
			}
		}
		
		if (!dead)
		{
			//temp: increase score when in close proximity to star. show particles (based on star scale)
			if (refueling)
			{
				solarenergy.particleSystem.emissionRate = 10;
				score += 100*Time.deltaTime;
				energy += 25*Time.deltaTime;
				//shield.GetComponent<shieldlogic>().shieldhealth -= Time.deltaTime*0.5f;
			}
			else
			{
				solarenergy.particleSystem.emissionRate = 0;
			}
			
			//temp: missile targeting
			if (mousetimer > 1.0f && !charging)
			{
				missiletargetingline.GetComponent<LineRenderer>().SetPosition(1, new Vector3(25, 0, 0));
				
				Ray ray = new Ray(transform.position, transform.right);
				
				//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		        RaycastHit[] hits;
				hits = Physics.RaycastAll(transform.position, transform.right, 25, 1 << 9);
				int i = 0;
				while (i < hits.Length)
				{
					if (!homingtargets.Contains(hits[i].transform.gameObject))
					{
						homingtargets.Add(hits[i].transform.gameObject);
					}
					i++;
				}
				/*
		        if (Physics.RaycastAll(ray, out hit, 25, 1 << 9))
				{
					//homingtarget = hit.transform.gameObject;
					if (!homingtargets.Contains(hit.transform.gameObject))
					{
						homingtargets.Add(hit.transform.gameObject);
					}
				}
				else
				{
				}
				*/
			}
			else
			{
				missiletargetingline.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
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
			        if (Physics.Raycast(ray, out hit, 500, 1 << 11))
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
				if (!charge.audio.isPlaying)
				{
					charge.audio.Play();
				}
			}
			else
			{
				TrailRenderer trail = gameObject.GetComponent<TrailRenderer>();
				trail.time = 0.5f;
				charge.particleSystem.emissionRate = 0;
				charge.audio.Stop();
			}
			
			//set firing mode on mouse release or if there is a shot in the buffer
			if (Input.GetKeyUp (KeyCode.Mouse0) || shotbuffer)
			{
				moving = false;
				if (homingtargets.Count > 0)
				{
					int i = 0;
					foreach (GameObject homingtarget in homingtargets)
					{
						missilelogic missilelogicscript = missile.GetComponent<missilelogic>();
						missilelogicscript.homingtarget = homingtarget;
						GameObject missileclone = Instantiate(missile, transform.position,  transform.rotation) as GameObject;
						missileclone.rigidbody.velocity = rigidbody.velocity;
						energy -= 5;
						i++;
					}
					homingtargets.Clear();
				}
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
							shotstofire = maxrapidfireshots;
							chargetime = 0.2f;
							firing = true;
							shottimer = Time.time;
						}
						else
						{
							//fire if proximity check
							if (Vector3.Distance(mousepos, mouseinputposition) > 100.0f)
							{
								shotstofire = 1;
								firing = true;
								shottimer = Time.time;
							}
							else
							{
								shieldlogic shieldlogicscript = shield.GetComponent<shieldlogic>();
								if (shieldlogicscript.shieldhealth < 3)
								{
									shieldlogicscript.shieldhealth += chargetime /3;
								}
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
					newbullet.rigidbody.AddForce(newbullet.transform.right * ((chargetime)*shotspeed));
					newbullet.rigidbody.freezeRotation = true;
					newbullet.GetComponent<bulletlogic>().chargetime = chargetime;
					Destroy(newbullet, 3);
					shotsfired++;
					shottimer = Time.time + rapidfiredelay;
					//handle kickback and bullet transforms
					if (chargetime > 0.2f)
					{
						newbullet.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f)*chargetime;
						newbullet.rigidbody.mass*=10.0f;
						kickbackvelocity += newbullet.transform.right * (chargetime*kickback) * -1;
					}
					else
					{
						kickbackvelocity += newbullet.transform.right * (chargetime*kickback) * -1;
						rigidbody.AddForce(newbullet.transform.right * (chargetime*kickback) * -2, ForceMode.Impulse);
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
			if (moving && !charging)
			{
				if (rigidbody.velocity.magnitude < maxspeed)
				{
					rigidbody.AddForce(transform.right * acceleration);
				}
				else
				{
					rigidbody.AddForce(rigidbody.velocity * -1);
				}
			}
			else
			{
				rigidbody.velocity *=  brakes;
			}
			
			//handle kickback
			rigidbody.AddForce(kickbackvelocity, ForceMode.Impulse);
			kickbackvelocity *= 0.0f;
		}
    }
	

	// bad collision event
	public GameObject explosion;
	
    void OnCollisionEnter(Collision collision) {
		if (shield.GetComponent<shieldlogic>().shieldhealth <= 0.0f)
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
