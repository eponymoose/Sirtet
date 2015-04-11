using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Randomizer {

	private static readonly int NO_SHAPE = -1;

	private GameData _data;
	private int _nextShape = NO_SHAPE;
	private Dictionary<int, int> _shapeStats = new Dictionary<int, int>();

	public Randomizer( GameData data )
	{
		_data = data;
	}

	private int GetNextShape()
	{
		int val = Random.Range( 0, _data.Shapes.Length + 1 );
		if( val == _nextShape || val == +_data.Shapes.Length )
		{
			val = Random.Range( 0, _data.Shapes.Length );
		}

		return val;
	}

	public Shape PopNextShape()
	{
		Shape retVal = PeekNextShape();
		CountShape( _nextShape );
		_nextShape = GetNextShape();
		return retVal;
	}

	public Shape PeekNextShape()
	{
		if( _nextShape == NO_SHAPE )
		{
			_nextShape = GetNextShape();
		}

		return _data.Shapes[ _nextShape ];
	}

	private void CountShape( int shape )
	{
		if( !_shapeStats.ContainsKey( shape ) )
		{
			_shapeStats[ shape ] = 0;
		}

		_shapeStats[ shape ]++;
	}

	public int GetShapeCount( int shape )
	{
		return _shapeStats.ContainsKey( shape) ? _shapeStats[ shape ] : 0;
	}
}
