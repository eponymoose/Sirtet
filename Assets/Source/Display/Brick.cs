using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	private int _x;
	private int _y;
	
	private Renderer _renderer;

	public void Setup( int x, int y )
	{
		_x = x;
		_y = y;
		this.transform.localPosition = _x * Vector3.right + _y * Vector3.up;
	}

	public void UpdateDisplay( BoardModel board, ActiveBricks bricks, TileSet tileSet )
	{
		int color = board.GetTile( _x, _y );
		if( color == 0 )
		{
			color = bricks.GetBrickAt( _x, _y );
		}

		if( _renderer == null )
		{
			_renderer = this.GetComponentInChildren<Renderer>();
		}

		if( color > 0 )
		{
			_renderer.enabled = true;
			_renderer.material = tileSet.TileMaterials[ color-1 ];
		}
		else
		{
			_renderer.enabled = false;
		}
	}
}
