using UnityEngine;
using System.Collections;

public class stargravity : MonoBehaviour {
	
	GameObject[] stars;
	Vector3[] starpositions;
	float closeststardistance;
	Vector3 closeststarposition;
	int i = 0;
	// Use this for initialization
	void Start () {
		
		stars = GameObject.FindGameObjectsWithTag("star");
		//for some reason, i have to specify the length of the starpositions array
		starpositions = new Vector3[stars.Length];
		closeststardistance = Mathf.Infinity;
		foreach(GameObject temp in stars)
		{
			starpositions[i] = temp.transform.position;
			i++;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
		closeststardistance = Mathf.Infinity;
		i=0;
		foreach(Vector3 temp in starpositions)
		{
			if (Vector3.Distance(temp, transform.position) < closeststardistance)
			{
				closeststarposition = stars[i].transform.position;
				closeststardistance = Vector3.Distance(temp, transform.position);
			}
			i++;
		}
	
	}
	
	void FixedUpdate() {
		
		transform.RotateAround (closeststarposition, Vector3.forward, (200/(Vector3.Distance(transform.position, closeststarposition))) * Time.deltaTime);
		//Vector3 gravityforce = closeststarposition - transform.position;
		//rigidbody.AddForce((gravityforce.normalized * 100f)*Time.deltaTime);
		
	}
}
