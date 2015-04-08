using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrickDisplay : MonoBehaviour {

	public Brick BrickPrototype;
	public Effect ClearEffect;

	private List<Brick> _bricks = new List<Brick>();
	private TileSet _tileSet;

	public void Setup( TileSet tileSet)
	{
		SetTileSet( tileSet );

		for( int i = 0; i < Constants.WIDTH; ++i)
		{
			for( int j = 0; j < Constants.HEIGHT_VISIBLE; ++j )
			{
				Brick brick = Instantiate( BrickPrototype ) as Brick;
				brick.transform.parent = this.transform;
				brick.Setup( i, j );
				_bricks.Add( brick );
			}
		}
	}

	public void SetTileSet( TileSet set)
	{
		_tileSet = set;
	}

	public void UpdateDisplay( BoardModel board, ActiveBricks activeBricks )
	{
		_bricks.ForEach( (Brick b) => {
			b.UpdateDisplay( board, activeBricks, _tileSet );
		});
	}

	public void PlayClear( int row )
	{
		Effect effect = Instantiate( ClearEffect ) as Effect;
		effect.transform.parent = this.transform;
		effect.transform.localPosition = row * Vector3.up;
	}
}
