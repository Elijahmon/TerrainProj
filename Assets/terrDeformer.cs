using UnityEngine;
using System.Collections;

public class terrDeformer : MonoBehaviour {
	public Terrain terr;
	int hmWidth;
	int hmHeight;
	int posXInTerrain;
	int posYInTerrain;
	int size;
	public float deformFactor;
	float desiredHeight;
	float currHeight;
	float deformCap;
	bool deform;

	Vector3 tempCoord;
	Vector3 coord;
	float[,] heights;
	// Use this for initialization
	void Start () {
		//terr = Terrain.activeTerrain;
		hmWidth = terr.terrainData.heightmapWidth;
		hmHeight = terr.terrainData.heightmapHeight;
		if(this.GetComponent<MeshFilter>() != null)
			size = Mathf.CeilToInt(this.GetComponent<MeshFilter>().mesh.bounds.size.x);
		if(this.GetComponent<Collider>() != null){
			if(this.GetComponent<Collider>().bounds.size.x >= this.GetComponent<Collider>().bounds.size.z)
				size = Mathf.CeilToInt(this.GetComponent<Collider>().bounds.size.x);
			else
				size = Mathf.CeilToInt(this.GetComponent<Collider>().bounds.size.z);
		}
		desiredHeight = 50f;
		deformCap = 0;
		deform = true;

	}

	// Update is called once per frame
	void FixedUpdate(){
		tempCoord = (transform.position - terr.gameObject.transform.position);
		coord.x = tempCoord.x / terr.terrainData.size.x;
		coord.y = tempCoord.y / terr.terrainData.size.y;
		coord.z = tempCoord.z / terr.terrainData.size.z;
		
		// get the position of the terrain heightmap where this game object is
		posXInTerrain = (int) (coord.x * hmWidth); 
		posYInTerrain = (int) (coord.z * hmHeight);
		if(deform)
		{
		heights = terr.terrainData.GetHeights(posXInTerrain,posYInTerrain,size,size);
		// we set each sample of the terrain in the size to the desired height
		for (int i=0; i<size; i++){
			for (int j=0; j<size; j++){
				if(deformCap == 0){
					deformCap = heights[i,j] - deformFactor*.0001f;
				}
				
				if(heights[i,j] > deformCap){
					heights[i,j] = heights[i,j] - deformFactor*.0001f;
				}
				else{
					//deform = false;
				}
				
			}
		}
		
		// go raising the terrain slowly
		
		// set the new height
		terr.terrainData.SetHeights(posXInTerrain,posYInTerrain,heights);

		}
	}
	void Update () {
	
	}
	void OnTriggerStay(Collider coll){
		if(coll.gameObject.name == "terraincoll"){
			deform = true;
			// get the normalized position of this game object relative to the terrain
			/*Vector3 tempCoord = (transform.position - terr.gameObject.transform.position);
			Vector3 coord;
			coord.x = tempCoord.x / terr.terrainData.size.x;
			coord.y = tempCoord.y / terr.terrainData.size.y;
			coord.z = tempCoord.z / terr.terrainData.size.z;
			
			// get the position of the terrain heightmap where this game object is
			posXInTerrain = (int) (coord.x * hmWidth); 
			posYInTerrain = (int) (coord.z * hmHeight);
			*/
			// we set an offset so that all the raising terrain is under this game object
			//int offset = 0;//size / 2;
			
			// get the heights of the terrain under this game object

			

		}
	}
	/*void OnCollisionStay(Collision coll){
		if(deform){
			print ("bop");
			// get the normalized position of this game object relative to the terrain
			Vector3 tempCoord = (transform.position - terr.gameObject.transform.position);
			Vector3 coord;
			coord.x = tempCoord.x / terr.terrainData.size.x;
			coord.y = tempCoord.y / terr.terrainData.size.y;
			coord.z = tempCoord.z / terr.terrainData.size.z;
			
			// get the position of the terrain heightmap where this game object is
			posXInTerrain = (int) (coord.x * hmWidth); 
			posYInTerrain = (int) (coord.z * hmHeight);
			
			// we set an offset so that all the raising terrain is under this game object
			int offset = 0;//size / 2;
			
			// get the heights of the terrain under this game object
			float[,] heights = terr.terrainData.GetHeights(posXInTerrain-offset,posYInTerrain-offset,size,size);
			
			// we set each sample of the terrain in the size to the desired height
			for (int i=0; i<size; i++){
				for (int j=0; j<size; j++){
					if(deformCap == 0){
					deformCap = heights[i,j] - deformFactor*.0001f;
					}
					print (heights[i,j]);
					if(heights[i,j] > deformCap){
					heights[i,j] = heights[i,j] - deformFactor*.0001f;
					}
					else{
					//deform = false;
					}
					print (heights[i,j]);
				}
			}
			
			// go raising the terrain slowly
			
			// set the new height
			terr.terrainData.SetHeights(posXInTerrain-offset,posYInTerrain-offset,heights);
		}
	}*/
	void OnTriggerExit(Collider coll)
	{
		if(!deform)
		{
			deform = true;
		}
	}
}
