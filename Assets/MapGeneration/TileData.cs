using UnityEngine;
using System.Collections;

public class TileData
{
	//private const int TYPE_MAX = 8;
	private const int HEIGHT_MAX = 100;



	private Vector3 type = new Vector3 ();
	private float height = 0;
	private Coords coordinates;

	public Coords Coordinates { get { return coordinates; } }

	public Vector3 Type
	{ 
		get { return type; }
		set
		{
			type = value;
		}
	}

	public float Height
	{ 
		get { return height; }
		set
		{
			//if ( value >= 0 && value <= HEIGHT_MAX )
			{
				height = value;
			}
		}
	}


	public TileData ( int x , int y )
    {
        coordinates = new Coords( x , y );
    }



    public class Coords
    {
        private Vector2 map; // position in the tileMap
        private Vector3 local; // position in local scale

        public Vector2 Map 
        { 
            get { return map; } 
        }
        public Vector3 Local
        { 
            get { return local; } 
        }

        public Coords( int x , int y )
        {
            map = new Vector2( x , y );
        }
    }
}
