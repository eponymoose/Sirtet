using UnityEngine;
using System.Collections;

public class ShapeDisplay : MonoBehaviour {

	public Shape Shape;

	private BrickDisplay _display;

	void Start()
	{
	}

	public void UpdateDisplay( TileSet tileSet )
	{
		if( _display == null )
		{
		   	_display = this.GetComponent<BrickDisplay>();
			_display.Setup( tileSet );
		}

		if( _display != null && Shape != null )
		{
			_display.SetTileSet( tileSet );
			BoardModel board = new BoardModel( Shape.Size, Shape.Size );
			Shape.ApplyToBoard( board, 0 );
			_display.UpdateDisplay( board );
		}
	}
}
