using UnityEngine;
using System.Collections;

public static class MapHelper
{

	public static bool isUp ( int x , int y )
	{
		return ( x + y ) % 2 == 0;
	}

	public static bool isUp( Vector2 coords )
	{
		return ( coords.x + coords.y ) % 2 == 0;
	}
}
