using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerlogic : MonoBehaviour {
	
	public GameObject guisystem;
	float mousetimer;
	public Vector3 mousepos;
	public Vector3 mouseinputposition;
	public Vector3 movementtargetvector;
	public float mouseinputtime;
	bool doubleclick;
	public float acceleration;
	public float maxspeed;
	public float throttle;
    public float smooth;
	public float targetangle;
	public float movementtargetangle;
	public GameObject bulletclone;
	public bool charging;
	bool firing;
	public float chargetime;
	public float rapidfiredelay;
	public float shotspeed;
	public float kickback;
	Vector3 kickbackvelocity;
	float shottimer;
	public int maxrapidfireshots;
	int shotstofire;
	public float shotlife;
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
	public float missiletargetinglength;
	public bool moving;
	public float brakes;
	bool stopping;
	public GameObject movementline;
	public float playerinputsize = 40f;
	
	//simplification / optimization
	Vector3 myposition;
	Camera mycamera;
	Rigidbody mybody;
	Vector3 myvelocity;
	float myvelocitymagnitude;
	
	
	//temp
	GameObject star;
	
	void Start()
	{
		rigidbody.freezeRotation = true;
		star = GameObject.Find("star");
		guisystem = GameObject.Find("guisystem");
		guisystem.GetComponent<guilogic>().player = gameObject;
		
		mycamera = Camera.main;
		
		//stats
		energy = 500.0f;
		maxrapidfireshots = 3;
		brakes = 0.95f;
		kickback = 2;
		shotspeed = 1;
		rapidfiredelay = 0.1f;
		smooth = 2f;
		acceleration = 1000.0f;
		maxspeed = 15.0f;
		shotspeed = 7500.0f;
		shotlife = 1.5f;
		missiletargetinglength = 35f;
	}

	
	void Update()
	{
		//print (chargetime);
		
		myposition = transform.position;
		mybody = rigidbody;
		myvelocity = rigidbody.velocity;
		myvelocitymagnitude = rigidbody.velocity.magnitude;
		
		mousepos = new Vector3(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2, 0);
		//print (mousepos);
		
		
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
			mycamera.orthographic = true;
			mycamera.farClipPlane = 1000.0f;
			if (zoomlerp < 1)
			{
				zoomlerp += 0.05f;
				mycamera.orthographicSize = Mathf.Lerp (20.0f, 150.0f, zoomlerp);
			}
			Time.timeScale = 0.0f;
		}
		else
		{
			mycamera.orthographic = false;
			mycamera.farClipPlane = 1000.0f;
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
			float stardistancecheck = Vector3.Distance(myposition, star.transform.position);
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
			energy -= Time.deltaTime*2*Mathf.Clamp(throttle, 0.25f, Mathf.Infinity);
			if (throttle == 2f)
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
		
		//mouseclick logic, handles double-click recognition, sets last known click posistion
		
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 previousinputposition = mouseinputposition;
			mouseinputposition = mousepos;
			if (Time.time - mouseinputtime < 0.5f && Time.time - mouseinputtime > 0.05f && mouseinputposition.magnitude < playerinputsize && previousinputposition.magnitude < playerinputsize)
			{
				doubleclick = true;
			}
			else
			{
				doubleclick = false;
			}
			mouseinputtime = Time.time;
		}
		
		//print (doubleclick);
		
		//sets target angles based on mouse screen position when clicked or dragged
		//zfight
		//transform.position = new Vector3(transform.position.x,myposition.y, 0);
		
		Debug.DrawRay(myposition, mousepos, Color.gray);
		
		if (Input.GetKey(KeyCode.Mouse0))
		{
			//always set targetangle
			targetangle = Vector3.Angle(Vector3.right, mousepos);
			if ((Input.mousePosition.y - Screen.height/2) < 0)
			{
	        	targetangle *=-1;
			}
			//only set movement target angle when initial click is near player and a little time has passed OR user drags input to show intent
			if (mouseinputposition.magnitude < playerinputsize)
			{
				if (!charging && mousetimer > 0.33f || Vector3.Distance(mouseinputposition, mousepos) > 80f)
				{
					moving = true;
					movementtargetvector = mousepos;
					throttle = Vector3.Distance(mycamera.WorldToScreenPoint(myposition), Input.mousePosition)-90;
					throttle = Mathf.Clamp(throttle/(Screen.height/8.5f), 0f, 2f);
					if (throttle > 1.0f)
					{
						throttle = Mathf.Floor(throttle);
					}
					//print (throttle);
					movementtargetangle = targetangle;
				}
			}
			
		}
		
		//gameObject.GetComponent<LineRenderer>().SetPosition(0, new Vector3(15 * throttle, 0, 0));
		movementline.GetComponent<LineRenderer>().SetPosition(0,myposition);
		movementline.GetComponent<LineRenderer>().SetPosition(1,myposition + (movementtargetvector.normalized * (15*throttle)));
		
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
			if (mousetimer > 0.50f && !charging && !moving)
			{
				if (missiletargetingline.GetComponent<LineRenderer>().enabled == false)
				{
					missiletargetingline.GetComponent<LineRenderer>().enabled = true;
				}
				Ray ray = new Ray (myposition, mousepos);
				missiletargetingline.GetComponent<LineRenderer>().SetPosition(0, myposition);
				missiletargetingline.GetComponent<LineRenderer>().SetPosition(1, myposition + ray.direction*missiletargetinglength	);

		        RaycastHit[] hits;
				hits = Physics.RaycastAll(transform.position, mousepos, missiletargetinglength, 1 << 9);
				int i = 0;
				while (i < hits.Length)
				{
					if (!homingtargets.Contains(hits[i].transform.gameObject))
					{
						homingtargets.Add(hits[i].transform.gameObject);
					}
					i++;
				}
			}

			///targeting temp
			
			//increase charge state when mouse1 is held on player
			if (Input.GetKey(KeyCode.Mouse0) && !firing && mousetimer < 0.5f)
			{
				mousetimer += Time.deltaTime;
				//check if mouse was just press and is within chargebubble


					//print (mousepos.magnitude);
					if (mousepos.magnitude < playerinputsize)
					{
						if (doubleclick)
						{
							charging = true;
							charge.particleSystem.Play();
						}
						else if (mousetimer > 0.5f)
						{
							stopping = true;
						}
					}

			}
			
			if (charging)
			{
				//throttle *= 0f;
				//TrailRenderer trail = gameObject.GetComponent<TrailRenderer>();
				//trail.time = 0;
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
				//TrailRenderer trail = gameObject.GetComponent<TrailRenderer>();
				//trail.time = 1.0f;
				charge.particleSystem.emissionRate = 0;
				charge.audio.Stop();
			}
			
			//set firing mode on mouse release or if there is a shot in the buffer
			if (Input.GetKeyUp(KeyCode.Mouse0) && mouseinputposition.magnitude > playerinputsize && mousetimer < 0.5f || shotbuffer)
			{
				if (!charging)
				{
					//remove shot from buffer
					if (shotbuffer)
					{
						shotbuffer = false;
					}
					
					if (!firing)
					{
						shotstofire = maxrapidfireshots;
						chargetime = 0.2f;
						firing = true;
						shottimer = Time.time+rapidfiredelay;

					}
					else
					{
						if (shotstofire < 3)
						{
							shotstofire += 3;
						}
					}

				}
			}
			
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				//fire if outside of player proximity
				if (chargetime > 0.5f)
				{
					if (mousepos.magnitude > playerinputsize)
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
							shieldlogicscript.shieldhealth += chargetime;
						}
					}
				}
				
				//fire missiles if targets exist
				if (homingtargets.Count > 0)
				{
					int i = 0;
					foreach (GameObject homingtarget in homingtargets)
					{
						missile.GetComponent<missilelogic>().homingtarget = homingtarget;
						GameObject missileclone = Instantiate(missile,myposition,  transform.rotation) as GameObject;
						missileclone.rigidbody.velocity = myvelocity;
						energy -= 5;
						i++;
					}
					homingtargets.Clear();
				}
				
				if (throttle == 2f)
				{
					throttle = 1f;
				}
				
				//clear some settings
				mousetimer = 0;
				stopping = false;
				charging = false;
				moving = false;
				missiletargetingline.GetComponent<LineRenderer>().enabled = false;
			}
			

			
			//firing mechanism
			if (firing == true)
			{
				//instantiate shot, grows when charged, sets bullet to self destruct, adds reverse force to player
				if(Time.time > shottimer)
				{
					GameObject newbullet = Instantiate(bulletclone, myposition,  Quaternion.Euler(0, 0, targetangle)) as GameObject;
					newbullet.transform.localScale = newbullet.transform.localScale*chargetime;
					Rigidbody bulletbody = newbullet.rigidbody;
					bulletbody.mass = chargetime * 10;
					bulletbody.velocity = rigidbody.velocity*1f;
					bulletbody.AddForce(newbullet.transform.right * ((chargetime)*shotspeed));
					bulletbody.freezeRotation = true;
					newbullet.GetComponent<bulletlogic>().chargetime = chargetime;
					Destroy(newbullet,shotlife);
					shotstofire--;
					shottimer = Time.time + rapidfiredelay;
					//handle kickback and bullet transforms
					if (chargetime > 0.2f)
					{
						newbullet.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f)*chargetime;
						bulletbody.mass*=chargetime*3f;
						kickbackvelocity += newbullet.transform.right * (chargetime*kickback) * -1;
					}
					else
					{
						kickbackvelocity += newbullet.transform.right * (chargetime*kickback) * -1;
						mybody.AddForce(newbullet.transform.right * (chargetime*kickback) * -2, ForceMode.Impulse);
					}
				}
				//handing rapidfire
				//print (shotstofire);
				if (shotstofire <= 0)
				{
					firing = false;
					shottimer = rapidfiredelay;
					chargetime = 0;
				}
			}
		}
	}
	
	float finalacceleration;
	float finalmaxspeed;
	float finalsmooth;
 
    void FixedUpdate ()
    {
		mybody = rigidbody;
		
		if (!dead)
		{
			//smoothly rotates object in relation to target position (wtf is a quaternion) ;)
			
			Quaternion target = Quaternion.Euler(0, 0, movementtargetangle);


	        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * finalsmooth);
			
			//debugging rays
			Debug.DrawRay(myposition, transform.right*100, Color.red, 0);
			//Debug.DrawRay(transform.position, star.transform.position -myposition, Color.yellow, 0, false);
			if (Input.GetKey (KeyCode.Mouse0))
			{
				Debug.DrawRay(myposition, mousepos, Color.green, 0);
			}
			
			//move in direction of target mouse position constantly. checks currently velocity and reduces if necessary

				
			if (throttle == 2f)
			{
				finalacceleration = acceleration*2f;
				finalmaxspeed = maxspeed*2f;
				finalsmooth = smooth*1f;
			}
			else
			{
				finalacceleration = acceleration;
				//finalmaxspeed = maxspeed;
				finalmaxspeed = maxspeed;
				finalsmooth = smooth;
			}
			//print (finalmaxspeed);
			if (myvelocitymagnitude < finalmaxspeed)
			{
				mybody.AddForce((transform.right * Mathf.Clamp((finalacceleration*throttle), 0f, finalacceleration*(finalmaxspeed/myvelocitymagnitude)))*Time.deltaTime);
			}
			else
			{
				mybody.AddForce((myvelocity *- Mathf.Clamp((finalacceleration), 0f, finalacceleration*(finalmaxspeed/myvelocitymagnitude)))/5*Time.deltaTime);
			}

			if (stopping || throttle <= 0f || myvelocity.magnitude > finalmaxspeed && !firing && !charging)
			{
				myvelocity *= brakes;
				rigidbody.velocity = myvelocity;
				//throttle *= brakes;
				//throttle *= 0f;
			}
			
			
			//handle kickback
			if (myvelocitymagnitude < maxspeed*0.95f)
			{
				mybody.AddForce(kickbackvelocity, ForceMode.Impulse);
			}
			kickbackvelocity *= 0f;
			
			//gravitywell
			/*
			if (charging)
			{
				RaycastHit hit;
				int layermask = 1<<12;
		        Collider[] hitColliders = Physics.OverlapSphere(myposition, 50f, layermask);
				if (hitColliders.Length > 0)
				{
					foreach(Collider gravitytarget in hitColliders)
					{
						if (gravitytarget.rigidbody)
						{
							Debug.DrawLine(myposition, gravitytarget.transform.position, Color.cyan);
							Vector3 gravityforce = myposition - gravitytarget.transform.position;
							//gravitytarget.rigidbody.AddForce(gravityforce.normalized * Vector3.Distance(transform.position, gravitytarget.transform.position)*0.20f);
							gravitytarget.rigidbody.AddForce((gravityforce.normalized * Vector3.Distance(myposition, gravitytarget.transform.position)*450f) * Time.deltaTime);
						}
					}
				}
			}
			*/
		}
    }
	

	// bad collision event
	public GameObject explosion;
	
    void OnCollisionEnter(Collision collision) {
		if (shield.GetComponent<shieldlogic>().shieldhealth <= 0.0f)
		{
			GameObject explosionclone = Instantiate(explosion,myposition, transform.rotation) as GameObject;
			Destroy(explosionclone, 5.0f);
			myvelocity = new Vector3(0, 0, 0);
			mybody.detectCollisions = false;
			dead = true;
			//Destroy(gameObject, 3.0f);
		}
	}
}
