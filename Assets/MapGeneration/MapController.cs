using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
	public  int SUB_MESH_SIZE = 30;
	private MapData map;
	public int mapWidth = 10 , mapHeight = 10;
	public int tileWidth = 10 , tileHeight = 10;
	public int numTextures = 8;

	public GameObject meshPrefab;
	private GameObject[,] meshObjects;

	public Material mat;



	// Use this for initialiyation
	void Start ()
	{


		map = new MapData ( mapWidth , mapHeight );
		map.Generate ();
		BuildMesh ();
	}
    
	// Update is called once per frame
	void Update ()
	{
    
	}

	public void BuildMesh ()
	{


		GameObject[] names = GameObject.FindGameObjectsWithTag ( "Map" );
		
		foreach ( GameObject item in names )
		{
			DestroyImmediate ( item );
		}


		map = new MapData ( mapWidth , mapHeight );
		map.Generate ();

		int numTiles = mapWidth * mapHeight;
		List<Triangle> faces = new List<Triangle> ( numTiles );
      
//
//		int numVerts = numTiles * 3;
//
////        // Generate the mesh data
//		Vector3[] vertices = new Vector3[ numVerts ];
//		Vector2[] uv = new Vector2[numVerts];
//		Vector2[] normals = new Vector2[numVerts];
//		int[] triangles = new int[ numVerts ];
//       

		for ( int y=0 ; y < mapHeight ; y++ )
		{
			for ( int x=0 ; x < mapWidth ; x++ )
			{
				int tileIndex = ( y * mapHeight + x ) * 3;
				bool isUp = ( x + y ) % 2 == 0;

				float z = -1 - map.GetTileAt ( x , y ).Height;

				Triangle face = new Triangle ( x , y );
                
				if ( isUp )
				{
					face.vertices [ 0 ] = new Vector3 ( ( x + 1 ) * tileWidth , y * tileHeight , z );
					face.vertices [ 1 ] = new Vector3 ( ( x ) * tileWidth , ( y + 1 ) * tileHeight , z );
					face.vertices [ 2 ] = new Vector3 ( ( x + 2 ) * tileWidth , ( y + 1 ) * tileHeight , z );
                    
					face.uvs [ 0 ] = new Vector2 ( ( 0.5f + map.GetTileAt ( x , y ).Type ) / numTextures , 0 );
					face.uvs [ 1 ] = new Vector2 ( ( 0f + map.GetTileAt ( x , y ).Type ) / numTextures , 1 );
					face.uvs [ 2 ] = new Vector2 ( ( 1f + map.GetTileAt ( x , y ).Type ) / numTextures , 1 );
                    
                    
                    
				}
				else
				{
					face.vertices [ 0 ] = new Vector3 ( x * tileWidth , y * tileHeight , z );
					face.vertices [ 1 ] = new Vector3 ( ( x + 1 ) * tileWidth , ( y + 1 ) * tileHeight , z );
					face.vertices [ 2 ] = new Vector3 ( ( x + 2 ) * tileWidth , ( y ) * tileHeight , z );
                    
					face.uvs [ 0 ] = new Vector2 ( ( 1f + map.GetTileAt ( x , y ).Type ) / numTextures , 0 );
					face.uvs [ 1 ] = new Vector2 ( ( 0.5f + map.GetTileAt ( x , y ).Type ) / numTextures , 1 );
					face.uvs [ 2 ] = new Vector2 ( ( 0f + map.GetTileAt ( x , y ).Type ) / numTextures , 0 );
				}
                
				face.normals [ 2 ] = Vector3.forward;
				face.normals [ 2 ] = Vector3.forward;
				face.normals [ 2 ] = Vector3.forward;

				faces.Add( face );
			}
		}


		//smoothing
		//Debug.Log (" triangle count " + faces.Count );



		int numMeshX = (int) Mathf.Ceil ( (float) mapWidth / SUB_MESH_SIZE );
		int numMeshY = (int) Mathf.Ceil ( (float) mapHeight / SUB_MESH_SIZE );

		List<Triangle>[,] subMeshes = new List<Triangle>[ numMeshX , numMeshY ];

		for (int j = 0; j < numMeshY; j++) 
		{
			for (int i = 0; i < numMeshX; i++) 
			{
				subMeshes[ i , j ] = new List<Triangle>( SUB_MESH_SIZE * SUB_MESH_SIZE );
			}
		}
	


		for (int k = 0 , listSize = faces.Count ; k < listSize ; k++) 
		{
			Triangle face = faces[ k ];
			int i = (int) Mathf.Floor( face.mapCoords.x / SUB_MESH_SIZE );
			int j = (int) Mathf.Floor( face.mapCoords.y / SUB_MESH_SIZE );

			if ( i >= numMeshX || j >= numMeshY )
			{
				Debug.Log( i + " , " + j );
			}
			else
			{
				subMeshes[ i , j ].Add( face );
			}
			
		}


		for (int j = 0; j < numMeshY; j++) 
		{
			for (int i = 0; i < numMeshX; i++) 
			{
				List<Triangle> _faces = subMeshes[ i , j ];
				int numFaces = _faces.Count;
				//Debug.Log ("SubMesh face Count: " + numFaces );

				Vector3[] vertices = new Vector3[ numFaces * 3 ];
				int[] triangles = new int[ numFaces * 3 ];
				Vector2[] uvs = new Vector2[ numFaces * 3 ];
				Vector3[] normals = new Vector3[ numFaces * 3 ];

				for (int k = 0 ; k < numFaces ; k++) {
					Triangle face = _faces[ k ];
					int index = k * 3;


					vertices[ index + 0 ] = face.vertices[ 0 ];
					vertices[ index + 1 ] = face.vertices[ 1 ];
					vertices[ index + 2 ] = face.vertices[ 2 ];

					uvs[  index + 0 ] = face.uvs[ 0 ];
					uvs[  index + 1 ] = face.uvs[ 1 ];
					uvs[  index + 2 ] = face.uvs[ 2 ];

					normals[ index + 0 ] = face.normals[ 0 ];
					normals[ index + 1 ] = face.normals[ 1 ];
					normals[ index + 2 ] = face.normals[ 2 ];

					triangles[ index + 0 ] = index + 0;
					triangles[ index + 1 ] = index + 1;
					triangles[ index + 2 ] = index + 2;
					
				}

				GameObject meshObj = Instantiate( meshPrefab , new Vector3() , Quaternion.identity ) as GameObject;


				Mesh mesh = new Mesh();
				mesh.vertices = vertices;
				mesh.triangles = triangles;
				mesh.normals = normals;
				mesh.uv = uvs;


				MeshFilter meshFilter = meshObj.GetComponent<MeshFilter> ();
				MeshRenderer meshRenderer = meshObj.GetComponent<MeshRenderer> ();
				
				meshFilter.mesh = mesh;
				meshRenderer.material = mat;

			}
		}







//        // Create a new Mesh and populate with the data
		//Mesh mesh = new Mesh ();

//        
//        // Assign our mesh to our filter/renderer/collider

//        Debug.Log ("Done Mesh!");
//        
		//BuildTexture();
        
	}




	private class Triangle
	{
		public Vector3[] vertices = new Vector3[ 3 ];
		public Vector2[] uvs = new Vector2[ 3 ];
		public Vector2[] normals = new Vector2[ 3 ];

		public Vector2 mapCoords;

		public Triangle ( int x , int y )
		{
			mapCoords = new Vector2( x , y );
		}

		public Triangle ( Vector3 v1 , Vector3 v2 , Vector3 v3 , Vector2 u1 , Vector2 u2 , Vector2 u3 , Vector2 n1 , Vector2 n2 , Vector2 n3 )
		{
			vertices [ 0 ] = v1;
			vertices [ 1 ] = v2;
			vertices [ 2 ] = v3;
			uvs [ 0 ] = u1;
			uvs [ 1 ] = u2;
			uvs [ 2 ] = u3;
			normals [ 0 ] = n1;
			normals [ 1 ] = n2;
			normals [ 2 ] = n3;
		}
	}


}

