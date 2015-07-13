using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileMap : MonoBehaviour
{
	
	public int size_x = 100;
	public int size_y = 50;

	public int tileHeight = 10;
	public int tileWidth = 10;

	public int texNum = 8;
	
	// Use this for initialization
	void Start ()
	{
		BuildMesh ();
	}
	
//	Color[][] ChopUpTiles() {
//		int numTilesPerRow = 4;
//		int numRows = 1;
//		
//		Color[][] tiles = new Color[numTilesPerRow*numRows][];
//		
//		for(int y=0; y<numRows; y++) {
//			for(int x=0; x<numTilesPerRow; x++) {
//				tiles[y*numTilesPerRow + x] = terrainTiles.GetPixels( x*tileWidth , y*tileHeight, tileWidth, tileHeight );
//			}
//		}
//
//		return tiles;
//	}
//	
//	void BuildTexture() {
//
//		DTileMap map = new DTileMap(size_x, size_z);
//
//		int texWidth = size_x * tileResolution;
//		int texHeight = size_z * tileResolution;
//		Texture2D texture = new Texture2D(texWidth, texHeight);
//		
//		Color[][] tiles = ChopUpTiles();
//		
//		for(int y=0; y < size_z; y++) {
//			for(int x=0; x < size_x; x++) {
//				Color[] p = tiles[ map.GetTileAt(x,y) ];
//				texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
//			}
//		}
//		
//		texture.filterMode = FilterMode.Point;
//		texture.wrapMode = TextureWrapMode.Clamp;
//		texture.Apply();
//		
//		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
//		//mesh_renderer.sharedMaterials[0].mainTexture = texture;
//		
//		Debug.Log ("Done Texture!");
//	}
	
	public void BuildMesh ()
	{

		DTileMap map = new DTileMap (size_x, size_y);

		int numTiles = size_x * size_y;
		int numVerts = numTiles * 3;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numVerts ];

		int x, z;
		for (z=0; z < size_y; z++) {
			for (x=0; x < size_x; x++) {
				int tileIndex = (z * size_x + x)*3;
				bool isUp = (x + z) % 2 == 0;

				if (isUp) {
					vertices [tileIndex] = new Vector3 ((x + 1) * tileWidth , z * tileHeight , -1 );
					vertices [tileIndex + 1] = new Vector3 ((x) * tileWidth, (z + 1) * tileHeight , -1 );
					vertices [tileIndex + 2] = new Vector3 ((x + 2) * tileWidth, (z + 1) * tileHeight , -1 );

					uv [tileIndex] = new Vector2 ((0.5f + map.GetTileAt (x, z)) / texNum, 0);
					uv [tileIndex + 1] = new Vector2 ((0f + map.GetTileAt (x, z)) / texNum, 1);
					uv [tileIndex + 2] = new Vector2 ((1f + map.GetTileAt (x, z)) / texNum, 1);



				} else {
					vertices [tileIndex] = new Vector3 (x * tileWidth, z * tileHeight , -1 );
					vertices [tileIndex + 1] = new Vector3 ((x + 1) * tileWidth, (z + 1) * tileHeight , -1 );
					vertices [tileIndex + 2] = new Vector3 ((x + 2) * tileWidth, (z) * tileHeight , -1 );

					uv [tileIndex + 0] = new Vector2 ((1f + map.GetTileAt (x, z)) / texNum, 0);
					uv [tileIndex + 1] = new Vector2 ((0.5f + map.GetTileAt (x, z)) / texNum, 1);
					uv [tileIndex + 2] = new Vector2 ((0f + map.GetTileAt (x, z)) / texNum, 0);
				}

				triangles [tileIndex] = tileIndex;
				triangles [tileIndex + 1] = tileIndex + 1;
				triangles [tileIndex + 2] = tileIndex + 2;

				normals [tileIndex] = Vector3.forward;
				normals [tileIndex + 1] = Vector3.forward;
				normals [tileIndex + 2] = Vector3.forward;


//				Debug.Log( "vert1: (" + vertices[tileIndex].x + " , " + vertices[tileIndex].z );
//				Debug.Log( "vert2: (" + vertices[tileIndex+1].x + " , " + vertices[tileIndex+1].z );
//				Debug.Log( "vert3: (" + vertices[tileIndex+2].x + " , " + vertices[tileIndex+2].z );
//
//				Debug.Log( "uv1: (" + uv[tileIndex].x + " , " + uv[tileIndex].y );
//				Debug.Log( "uv2: (" + uv[tileIndex+1].x + " , " + uv[tileIndex+1].y );
//				Debug.Log( "uv3: (" + uv[tileIndex+2].x + " , " + uv[tileIndex+2].y );



			}
		}
		Debug.Log ("Done Verts!");
//		
//		for(z=0; z < size_z; z++) {
//			for(x=0; x < size_x; x++) {
//
//				int squareIndex = z * size_x + x;
//				int triOffset = squareIndex * 6;
//				
//				if( z % 2 == 0 )
//				{
//					
//					triangles[triOffset + 0] = z * vsize_x + x + 		   0;
//					triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
//					triangles[triOffset + 2] = z * vsize_x + x + 1;
//					
//					triangles[triOffset + 3] = z * vsize_x + x + 		   1;
//					triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 0;
//					triangles[triOffset + 5] = z * vsize_x + x + vsize_x + 1;
//				}
//				else
//				{
//					triangles[triOffset + 0] = z * vsize_x + x + 		   0;
//					triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
//					triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;
//					
//					triangles[triOffset + 3] = z * vsize_x + x + 		   0;
//					triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
//					triangles[triOffset + 5] = z * vsize_x + x + 		   1;
//				}
//			}
//		}
		
		Debug.Log ("Done Triangles!");
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter> ();
		
		mesh_filter.mesh = mesh;
		Debug.Log ("Done Mesh!");
		
		//BuildTexture();
	}
	
	
}
