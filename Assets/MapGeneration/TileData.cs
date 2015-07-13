using UnityEngine;
using System.Collections;

public class TileData
{
	private const int TYPE_MAX = 8;
	private const int HEIGHT_MAX = 100;



	private int type = -1;
	private int height = 0;
	private Coords coordinates;

	public Coords Coordinates { get { return coordinates; } }

	public int Type
	{ 
		get { return type; }
		set
		{
			if ( value >= 0 && value <= TYPE_MAX )
			{
				type = value;
			}
		}
	}

	public int Height
	{ 
		get { return height; }
		set
		{
			if ( value >= 0 && value <= HEIGHT_MAX )
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
