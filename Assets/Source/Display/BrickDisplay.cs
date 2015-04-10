using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrickDisplay : MonoBehaviour {

	public Brick BrickPrototype;
	public Effect ClearEffect;
	public int Height = Constants.HEIGHT_VISIBLE;
	public int Width = Constants.WIDTH;

	private List<Brick> _bricks = new List<Brick>();
	private TileSet _tileSet;

	public void Setup( TileSet tileSet)
	{
		SetTileSet( tileSet );

		for( int i = 0; i < Width; ++i)
		{
			for( int j = 0; j < Height; ++j )
			{
				Brick brick = Instantiate( BrickPrototype ) as Brick;
				brick.transform.parent = this.transform;
				brick.Setup( i, j );
				_bricks.Add( brick );
			}
		}
	}

	public void SetTileSet( TileSet set )
	{
		_tileSet = set;
	}

	public void UpdateDisplay( BoardModel board, ActiveBricks activeBricks = null )
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

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(
			this.transform.position + new Vector3( 0.5f*Width-0.5f, 0.5f*Height-0.5f, 0),
			new Vector3( Width, Height, 1));

		Debug.DrawLine( this.transform.position, this.transform.position + Height * Vector3.up );
		Debug.DrawLine( this.transform.position, this.transform.position + Width * Vector3.right );
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawCube(
			this.transform.position + new Vector3( 0.5f*Width-0.5f, 0.5f*Height-0.5f, 0),
			new Vector3( Width, Height, 1));

	}
}
