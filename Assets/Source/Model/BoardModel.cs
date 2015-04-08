using System.Collections;
using System.Collections.Generic;

public class BoardModel {

	private List<int[]> _rows;
	private int _height;
	private int _width;

	public BoardModel( int width, int height )
	{
		_rows = new List<int[]>();
		_width = width;
		_height = height;
		Fill();
	}

	private int[] CreateEmptyRow()
	{
		return new int[_width];
	}

	private void Fill()
	{
		while(_rows.Count < _height)
		{
			_rows.Add( CreateEmptyRow() );
		}
	}

	public int GetTile( int x, int y)
	{
		int retVal = 0;
		if( x >= 0 && x < _width )
		{
			if( y >= 0 && y < _height )
			{
				retVal = _rows[y][x];
			}
		}
		return retVal;
	}

	public void SetTile( int x, int y, int val)
	{
		_rows[y][x] = val;
	}

	public bool IsClear( int x, int y )
	{
		bool blocked = x < 0 || x >= _width || y < 0 || GetTile( x, y ) > 0;
		return !blocked;
	}

	public List<int> CheckAndClear()
	{
		List<int> clearedLines = new List<int>();

		int row = _height - 1;

		while( row  >= 0 )
		{
			if( CheckRow( row ))
			{
				clearedLines.Add( row );
				_rows.RemoveAt( row );
				_rows.Add( CreateEmptyRow() );
			}
			row -= 1;
		}

		return clearedLines;
	}

	public bool CheckRow( int row )
	{
		bool full = true;
		for( int i = 0; i < _width; ++i )
		{
			if( _rows[row][i] == 0 )
			{
				full = false;
				break;
			}
		}
		return full;
	}
}

