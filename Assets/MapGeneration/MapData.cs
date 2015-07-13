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
        int numBiomes = Random.Range( 5 , 12 );
        Biome[] biomes = new Biome[ numBiomes ];

        for (int i = 0; i < numBiomes; i++) {
            biomes[ i ] = new Biome( 
                Random.Range( 0 , mapWidth ),
                Random.Range( 0 , mapHeight ),
                Random.Range( 0 , 8 )
            );
        }

        ExpandBiomes( biomes , numBiomes );
    }

    private void Init()
    {
        tileMap = new TileData[ mapWidth , mapHeight ];
        InitTileMap();
    }

    private void InitTileMap ()
    {
        for ( int j = 0 ; j < mapHeight ; j++ )
        {
            for ( int i = 0 ; i < mapWidth ; i++ )
            {
                tileMap [ i , j ] = new TileData ( i , j );
            }
        }
    }

    private void ExpandBiomes( Biome[] biomes , int numBiomes )
    {
        int offset;
        bool isMapFilled;


        do {
            isMapFilled = true;

            
            for ( int i = 0 ; i < numBiomes ; i++ )
            {
                biomes[ i ].Expand();
            }

            for ( int j = 0 ; j < mapHeight ; j++ )
            {
                for ( int i = 0 ; i < mapWidth ; i++ )
                {
                    TileData tile = tileMap[ i , j ]; 
                    //Debug.Log( tile.Type );
                    if ( tile.Type == -1 )
                    {
                        isMapFilled = false;
                        offset = Random.Range( 0 , numBiomes );
                        for (int k = 0; k < numBiomes; k++) 
                        {
                            if (  biomes[ ( k + offset ) % numBiomes ].IsInRadius( i , j )  ) 
                            {
                                tile.Type = biomes[ ( k + offset ) % numBiomes ].Type;
                                tile.Height = biomes[ ( k + offset ) % numBiomes ].Type * 10;
                                break;
                            }
                        }
                    }
                }
            }
        } while ( isMapFilled == false );
    }

    private int PointDistanceSquared( int x , int y , Vector2 mapCoords )
    {
        return ( ( int ) ( Mathf.Pow( x - mapCoords.x , 2 ) + Mathf.Pow( y - mapCoords.y , 2 ) ) );
    }

    private class Biome
    {
        private Vector2 mapCoords;
        private int type;
        private float expandRate = Random.Range( 3f , 5f );
        private float radius = 0;

        public float Radius { get { return radius; } }
        public int Type { get { return type; } }


        public Biome( int x , int y , int type )
        {
            Init( x , y , type , expandRate );
        }

        public Biome( int x , int y , int type , float expandRate )
        {
            Init( x , y , type , expandRate );
        }

        public bool IsInRadius( int x , int y )
        {
            return ( ( Mathf.Pow( x - mapCoords.x , 2 ) + Mathf.Pow( y - mapCoords.y , 2 ) ) <= radius );
        }

        private void Init( int x , int y , int type , float expandRate )
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
