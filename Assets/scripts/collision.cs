using UnityEngine;
using System.Collections;

public class collision : MonoBehaviour {
	
	public GameObject player;
	private playerlogic playerlogicscript;
	

	// Use this for initialization
	void Start () {
		
		playerlogicscript = player.GetComponent<playerlogic>();
		
	}
	
	public int hitpoints = 1;
	public GameObject explosion;
	
	// Update is called once per frame
	void Update () {
		
		if (hitpoints <= 0)
		{
			//print (controlscript.score);
			playerlogicscript.score = playerlogicscript.score+100;
			GameObject explosionclone = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
			Destroy(explosionclone, 2.0f);
			Destroy(gameObject);
		}
	
	}
	
    void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal*-5, Color.white, 1.0f);
			hitpoints--;
        }
		//print ("ping");
    }
		
}
