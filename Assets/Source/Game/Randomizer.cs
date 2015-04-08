using UnityEngine;
using System.Collections;

public class Randomizer {

	GameData _data;

	public Randomizer( GameData data )
	{
		_data = data;
	}

	public Shape GetNextShape()
	{
		return _data.Shapes[Random.Range(0, _data.Shapes.Length)];
	}
}
