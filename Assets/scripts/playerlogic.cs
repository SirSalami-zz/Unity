using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerlogic : MonoBehaviour {
	
	public GameObject guisystem;
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
	public float shotspeed = 1;
	public float kickback = 1;
	float shottimer;
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
		energy = 500.0f;
		guisystem = GameObject.Find("guisystem");
		guisystem.GetComponent<guilogic>().player = gameObject;
		brakes = 0.99f;
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
		if (pause)
		{
			Time.timeScale = 0;
			Camera.main.orthographic = true;
			Camera.main.farClipPlane = 100.0f;
			if (zoomlerp < 1)
			{
				zoomlerp += 0.05f;
				Camera.main.orthographicSize = Mathf.Lerp (20.0f, 250.0f, zoomlerp);
			}
		}
		else
		{
			Time.timeScale = 1;
			Camera.main.orthographic = false;
			Camera.main.farClipPlane = 350.0f;
			zoomlerp = 0;
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
		if (closeststar && stardistance < closeststar.transform.localScale.x)
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
			if (firing)
			{
				energy -= Time.deltaTime*10;
			}
			if (charging && chargetime < 3.0f)
			{
				energy -= 0.2f;
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
				score += score+100*Time.deltaTime;
				energy+=+25*Time.deltaTime;
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
		        RaycastHit hit;
		        if (Physics.Raycast(ray, out hit, 25, 1 << 9))
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
							shotstofire = 3;
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
									shieldlogicscript.shieldhealth += chargetime;
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
					newbullet.rigidbody.AddForce(newbullet.transform.right * ((chargetime*10000)*shotspeed));
					newbullet.rigidbody.freezeRotation = true;
					Destroy(newbullet, 3);
					shotsfired++;
					shottimer = Time.time + rapidfiredelay;
					//handle kickback and bullet transforms
					if (chargetime > 0.2f)
					{
						newbullet.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f)*chargetime;
						rigidbody.AddForce(newbullet.transform.right * ((chargetime*movementspeed)*kickback) * -1, ForceMode.Impulse);
					}
					else
					{
						rigidbody.AddForce(newbullet.transform.right * ((chargetime*movementspeed*0.2f)*kickback) * -1, ForceMode.Impulse);
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
					rigidbody.AddForce(transform.right * movementspeed);
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
