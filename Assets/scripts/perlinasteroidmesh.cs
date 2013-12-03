using UnityEngine;
using System.Collections;

public class perlinasteroidmesh : MonoBehaviour {
	
		MeshFilter mf;
		Vector3[] originalvertices;
		Vector3[] vertices;
		Vector3[] normals;
	

	// Use this for initialization
	void Start () {
		

			mf = GetComponent<MeshFilter>();
		originalvertices = mf.mesh.vertices;
		vertices = mf.mesh.vertices;
		normals = mf.mesh.normals;
		
		for (int i = 0; i < vertices.Length; i++) 
		{

			Ray vertexray = new Ray(transform.position, originalvertices[i]);
			
			Vector3 vertex = vertices[i];
			vertex.x = vertexray.direction.x * 1+(Random.Range(-0.15f, 0.15f));
			vertex.y = vertexray.direction.y * 1+(Random.Range(-0.15f, 0.15f));
			vertex.z = vertexray.direction.z * 1+(Random.Range(-0.15f, 0.15f));
			vertices[i] = vertex;
				
		}
		mf.mesh.vertices = vertices;
		mf.mesh.RecalculateBounds();
		mf.mesh.RecalculateNormals();
		
		
		
	
	}
	
	// Update is called once per frame
	void Update () {
		

		



	}
}
