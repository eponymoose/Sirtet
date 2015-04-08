using UnityEngine;
using System.Collections;

public class LevelManager {

	public int Gravity { get { return GetGravity(); } }
	public int Level { get { return _currentLevel; } }
	public int ScoreMuliplier { get { return _currentLevel; } }
	public int PassAmount { get { return GetPassAmount(); } }

	private int[] _gravityChart = new int[]{
		48,
		43,
		38,
		33,
		28,
		23,
		18,
		13,
		8,
		6,
		5,
		5,
		5,
		4,
		4,
		4,
		3,
		3,
		3,
		2,
		2,
		2,
		2,
		2,
		2,
		2,
		2,
		2,
		2,
		1
	};

	private int _currentLevel;
	public int _startingLevel;

	public LevelManager()
	{
		Reset ( 1 );
	}

	public void Reset( int startingLevel)
	{
		_startingLevel = _currentLevel = startingLevel;
	}

	public void PassLevel()
	{
		_currentLevel += 1;
	}

	private int GetGravity()
	{
		int gravity = 1;
		int levelIndex = _currentLevel - 1;
		if( levelIndex < _gravityChart.Length )
		{
			gravity = _gravityChart[ levelIndex ];
		}
		else
		{
			gravity = _gravityChart[ _gravityChart.Length-1 ];
		}
		return gravity;
	}

	private int GetPassAmount()
	{
		int startIndex = _startingLevel - 1;
		int levelDiff = _currentLevel - _startingLevel;
		return Mathf.Min( startIndex * 10, 100 ) + 10 * levelDiff + 10;
	}
}
