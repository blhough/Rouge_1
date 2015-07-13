using UnityEngine;
using System.Collections;


public class MapData
{
	public readonly int mapWidth = 10;
	public readonly int mapHeight = 10;
	protected TileData[,] tileMap;

	public MapData ()
	{
		Init ();
	}

	public MapData ( int mapWidth , int mapHeight )
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        Init ();
    }

    
    public TileData GetTileAt( int x , int y )
    {
        return tileMap[ x , y ];
    }



    public void Generate()
    {
        int numBiomes = Random.Range( 3 , 10 );
        Biome[] biomes = new Biome[ numBiomes ];

		GenerateBiomes( ref biomes , numBiomes );     
        ExpandBiomes( ref biomes , numBiomes );
		BlendBiomes();
		BlendBiomes();
		BlendBiomes();
		BlendBiomes();
		BlendBiomes();
		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();
//		BlendBiomes();


    }

    private void Init()
    {
        tileMap = new TileData[ mapWidth , mapHeight ];
        InitTileMap();
    }

    private void InitTileMap ()
    {
        for ( int y = 0 ; y < mapHeight ; y++ )
        {
            for ( int x = 0 ; x < mapWidth ; x++ )
            {
                tileMap [ x , y ] = new TileData ( x , y );
            }
        }
    }

	private void GenerateBiomes( ref Biome[] biomes , int numBiomes )
	{
		Vector3[] primaryTypes = new Vector3[ 3 ];

		primaryTypes[ 0 ] = new Vector3( 1f , 0f , 0f );
		primaryTypes[ 1 ] = new Vector3( 0f , 1f , 0f );
		primaryTypes[ 2 ] = new Vector3( 0f , 0f , 1f );
		
        for (int i = 0; i < numBiomes; i++) 
		{
	        biomes[ i ] = new Biome( 
	            Random.Range( 0 , mapWidth ),
	            Random.Range( 0 , mapHeight ),
	            primaryTypes[ Random.Range( 0 , 3 ) ] // the types for the biomes
	        );
		}
	}

    private void ExpandBiomes( ref Biome[] biomes , int numBiomes )
    {
        int offset;
        bool isMapFilled;


        do {
            isMapFilled = true;

            
            for ( int i = 0 ; i < numBiomes ; i++ )
            {
                biomes[ i ].Expand();
            }

            for ( int y = 0 ; y < mapHeight ; y++ )
            {
                for ( int x = 0 ; x < mapWidth ; x++ )
                {
                    TileData tile = tileMap[ x , y ]; 
                    //Debug.Log( tile.Type );
                    if ( tile.Type == new Vector3() )
                    {
                        isMapFilled = false;
                        offset = Random.Range( 0 , numBiomes );
                        for (int k = 0; k < numBiomes; k++) 
                        {
                            if (  biomes[ ( k + offset ) % numBiomes ].IsInRadius( x , y )  ) 
                            {
                                tile.Type = biomes[ ( k + offset ) % numBiomes ].Type;
                                break;
                            }
                        }
                    }
                }
            }
        } while ( isMapFilled == false );
    }

	private void BlendBiomes()
	{	
		Vector3[,] smoothMap = new Vector3[ mapWidth , mapHeight ];
		for ( int y = 0 ; y < mapHeight ; y++ )
		{
			for ( int x = 0 ; x < mapWidth ; x++ )
			{
				TileData tile = tileMap[ x , y ];
				Vector3 average = tile.Type;

				float count = 1;

				if ( MapHelper.isUp( x , y ) ) 
				{
					if ( x - 1 >= 0) 
					{
						average += tileMap[ x - 1 , y ].Type;
						count++;
					}

					if ( x + 1 < mapWidth) 
					{
						average += tileMap[ x + 1, y ].Type;
						count++;
					}

					if ( y - 1 >= 0) 
					{
						average += tileMap[ x , y - 1].Type;
						count++;
					}
				}
				else
				{
					if ( x - 1 >= 0) 
					{
						average += tileMap[ x - 1 , y ].Type;
						count++;
					}
					
					if ( x + 1 < mapWidth ) 
					{
						average += tileMap[ x + 1, y ].Type;
						count++;
					}
					
					if ( y + 1 < mapHeight ) 
					{
						average += tileMap[ x , y + 1].Type;
						count++;
					}
				}

				average /= count;
				average.x = Mathf.Clamp01( average.x );
				average.y = Mathf.Clamp01( average.y );
				average.z = Mathf.Clamp01( average.z );

				smoothMap[ x , y ] = average.normalized;
			}
		}	

		for ( int y = 0 ; y < mapHeight ; y++ )
		{
			for ( int x = 0 ; x < mapWidth ; x++ )
			{
				tileMap[ x , y ].Type = smoothMap[ x , y ]; 
			}
		}
	}


    private int PointDistanceSquared( int x , int y , Vector2 mapCoords )
    {
        return ( ( int ) ( Mathf.Pow( x - mapCoords.x , 2 ) + Mathf.Pow( y - mapCoords.y , 2 ) ) );
    }

    private class Biome
    {
        private Vector2 mapCoords;
        private Vector3 type;
        private float expandRate = Random.Range( 3f , 5f );
        private float radius = 0;

        public float Radius { get { return radius; } }
        public Vector3 Type { get { return type; } }


        public Biome( int x , int y , Vector3 type )
        {
            Init( x , y , type , expandRate );
        }

        public Biome( int x , int y , Vector3 type , float expandRate )
        {
            Init( x , y , type , expandRate );
        }

        public bool IsInRadius( int x , int y )
        {
            return ( ( Mathf.Pow( x - mapCoords.x , 2 ) + Mathf.Pow( y - mapCoords.y , 2 ) ) <= radius );
        }

        private void Init( int x , int y , Vector3 type , float expandRate )
        {
            mapCoords = new Vector2( x , y );
            this.type = type;
            this.expandRate = expandRate;
        }

        public void Expand()
        {
            radius += expandRate;
        }

    }

}
