using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shape : MonoBehaviour {

	public static readonly int MAX_ROTATIONS = 4;
	private static readonly int STARTING_SIZE = 3;

	public int ColorSet = 1;
	public int Size = STARTING_SIZE;
	public int Rotations = 0;
	public bool[] Rotation0 = new bool[STARTING_SIZE];
	public bool[] Rotation1 = new bool[STARTING_SIZE];
	public bool[] Rotation2 = new bool[STARTING_SIZE];
	public bool[] Rotation3 = new bool[STARTING_SIZE];

	public bool[] GetRotation( int num )
	{
		switch( num )
		{
		case 3:
			return Rotation3;
		case 2:
			return Rotation2;
		case 1:
			return Rotation1;
		default:
			return Rotation0;
		}
	}
	
	public void SetRotation( int num, bool[] rotation )
	{
		switch( num )
		{
		case 3:
			Rotation3 = rotation;
			break;
		case 2:
			Rotation2 = rotation;
			break;
		case 1:
			Rotation1 = rotation;
			break;
		default:
			Rotation0 = rotation;
			break;
		}
	}

	public int[,] GetRotationAsArray( int num )
	{
		int[,] retArr = new int[ Size,Size ];
		bool[] rotation = GetRotation( num );
		for(int i = 0; i < Size; ++i)
		{
			for(int j = 0; j < Size; ++j)
			{
				retArr[i,j] = rotation[ i*Size+j ] ? ColorSet : 0;
			}
		}
		return retArr;
	}

}
