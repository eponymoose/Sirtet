using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public Shape[] Shapes;
	public TileSet[] TileSets;

	public TileSet GetTileSet(int level)
	{
		int index = (level-1) % TileSets.Length;
		return TileSets[ index ];
	}
}
