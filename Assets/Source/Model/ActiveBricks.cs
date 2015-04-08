using UnityEngine;
using System.Collections;

public class ActiveBricks {

	private int _x = 5;
	private int _y = 10;
	public int _bottom;
	public int _top;
	
	public int X { get{ return _x; } }
	public int Y { get{ return _y; } }
	public int Top { get{ return _top; } }
	public int Bottom { get{ return _bottom; } }

	private int[,] _bricks;

	public void SetActiveBricks( int[,] shape )
	{
		_bricks = shape;
		_top = GetTop();
		_bottom = GetBottom();
	}

	public void Clear()
	{
		_bricks = new int[0,0];
	}

	public void Reset()
	{
		_x = Constants.WIDTH/2 - _bricks.GetLength(1)/2;
		_y = Constants.SPAWN_HEIGHT - Top;
	}

	public int GetBrickAt( int x, int y )
	{
		int retVal = 0;
		x -= _x;
		y -= _y;
		if( y >= 0 && y < _bricks.GetLength(0) )
		{
			if( x >= 0 && x < _bricks.GetLength(1) )
			{
				retVal = _bricks[y,x];
			}
		}

		return retVal;
	}

	public void Move( int x, int y )
	{
		_x += x;
		_y += y;
	}

	public bool CheckCollision( BoardModel board, int xOffset, int yOffset )
	{
		bool isClear = true;

		for( int y = 0; y < _bricks.GetLength(0); ++y )
		{
			for( int x = 0; x < _bricks.GetLength(1); ++x )
			{
				isClear = _bricks[y,x] == 0 || board.IsClear( _x+x+xOffset, _y+y+yOffset );
				if( !isClear )
				{
					break;
				}
			}

			if( !isClear )
			{
				break;
			}
		}

		return !isClear;
	}

	private int GetBottom()
	{
		int top = 0;

		for( int y = 0; y < _bricks.GetLength(0); ++y )
		{
			top = y;
			bool rowEmpty = true;
			for( int x = 0; x < _bricks.GetLength(1); ++x )
			{
				if( _bricks[y,x] > 0 )
				{
					rowEmpty = false;
				}
			}

			if( !rowEmpty )
			{
				break;
			}
		}

		return top;
	}
	
	private int GetTop()
	{
		int bottom = 0;
		
		for( int y = _bricks.GetLength(0) - 1; y >= 0 ; --y )
		{
			bottom = y;
			bool rowEmpty = true;
			for( int x = 0; x < _bricks.GetLength(1); ++x )
			{
				if( _bricks[y,x] > 0 )
				{
					rowEmpty = false;
				}
			}
			
			if( !rowEmpty )
			{
				break;
			}
		}
		
		return bottom;
	}

	public void ApplyToBoard( BoardModel board)
	{
		for( int y = 0; y < _bricks.GetLength(0); ++y )
		{
			for( int x = 0; x < _bricks.GetLength(1); ++x )
			{
				if( _bricks[y,x] > 0 )
				{
					board.SetTile( _x+x, _y+y, _bricks[y,x] );
				}
			}
		}
	}

}
