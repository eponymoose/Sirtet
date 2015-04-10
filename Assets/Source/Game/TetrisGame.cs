using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TetrisGame {

	//Display
	private BrickDisplay _display = null;
	private GameUI _UI = null;

	//Model
	private BoardModel _board = null;
	private ActiveBricks _activeBricks = null;

	//Systems
	private Randomizer _randomizer;
	private InputManager _input;

	//Data
	private GameData _data;
	private LevelManager _level;

	//game state
	private bool _gameOver = false;
	private long _frame = 0;
	private bool _boardDrity = false;
	private int _gravityCount = 0;
	private int _areDelay = 0;
	private bool _softDropEnabled = false;
	private int _currentRotation = 0;
	private Shape _currentShape;

	private long _score;
	private int _rowsCleared;
	private int _softDropPoints;

	public TetrisGame()
	{
	}

	public long GetScore()
	{
		return _score;
	}

	public int GetRowsCleared()
	{
		return _rowsCleared;
	}
	
	public int GetLevel()
	{
		return _level.Level;
	}

	public TileSet GetTileSet()
	{
		return _data.GetTileSet( _level.Level );
	}

	public GameData GetGameData()
	{
		return _data;
	}

	public Randomizer GetRandomizer()
	{
		return _randomizer;
	}

	public void Setup( GameData data )
	{
		_data = data;

		_level = new LevelManager();

		_board = new BoardModel( Constants.WIDTH, Constants.HEIGHT );

		_activeBricks = new ActiveBricks();

		_UI = GameObject.FindObjectOfType<GameUI>();
		_UI.Setup( this );

		_display = _UI.brickDisplay;
		_display.Setup( _data.GetTileSet( _level.Level ) );

		_randomizer = new Randomizer( _data );
		_input = new InputManager( this );

		_score = 0;
		_rowsCleared = 0;

		NewShape();
		
		_display.UpdateDisplay( _board, _activeBricks );
	}

	public void NewShape()
	{
		_currentShape = _randomizer.PopNextShape();
		_currentRotation = 0;
		int[,] r = _currentShape.GetRotationAsArray( _currentRotation );
		_activeBricks.SetActiveBricks( r );
		_activeBricks.Reset();
		_gravityCount = 0;
		_areDelay = 0;
		_softDropPoints = 0;
		_boardDrity = true;

		if( _activeBricks.CheckCollision( _board, 0 ,0 ) )
		{
			GameOver();
		}

		_UI.UpdateUI();

		Debug.Log( "Spawn new shape " + _currentShape.name + " at " + _activeBricks.Y );
	}

	private void GameOver()
	{
		_gameOver = true;
		_UI.ShowGameOver();
	}

	public void FrameUpdate()
	{
		_frame += 1;

		if( !_gameOver )
		{
			bool inputLock = true;

			if(_areDelay <= 0)
			{
				inputLock = false;

				if( _softDropEnabled )
				{
					_gravityCount += _level.Gravity/2;
				}
				else
				{
					_gravityCount += 1;
				}

				if( _gravityCount >= _level.Gravity )
				{
					GravityUpdate();
					_gravityCount = 0;
				}
			}

			_input.Update( inputLock  );

			
			if(_areDelay > 0)
			{
				_areDelay -= 1;
				if( _areDelay <= 0 )
				{
					//Next Level!
					if( _rowsCleared >= _level.PassAmount )
					{
						_level.PassLevel();
						_display.SetTileSet( _data.GetTileSet( _level.Level ) );
					}
					NewShape();
				}
			}
		}

		if( _boardDrity )
		{
			_display.UpdateDisplay( _board, _activeBricks );
			_boardDrity = false;
		}
	}

	public void GravityUpdate()
	{
		if( _softDropEnabled )
		{
			_softDropPoints += 1;
		}

		bool collision = !Move( 0, -1 );
		if( collision )
		{
			_activeBricks.ApplyToBoard( _board );
			int bucket = (_activeBricks.Y + _activeBricks.Bottom) / Constants.DELAY_ARE_BUCK_SIZE;
			_areDelay = Constants.DELAY_ARE_BASE + Constants.DELAY_ARE_BONUS * bucket;			
			_activeBricks.SetActiveBricks( new int[0,0] );

			List<int> clearedLines = _board.CheckAndClear();

			ScoreLines( clearedLines.Count );
			_rowsCleared += clearedLines.Count;

			for( int i = 0; i < clearedLines.Count; ++i )
			{
				_display.PlayClear( clearedLines[i] );
			}

			if( clearedLines.Count > 0 )
			{
				_areDelay += Constants.LINE_CLEAR_DELAY + (int)(_frame%4);
			}
			else
			{
				_boardDrity = true;
			}

			_UI.UpdateUI();

			Debug.Log("Starting ARE. Bucket " + bucket + " Delay " + _areDelay);
		}
	}

	private void ScoreLines( int amount )
	{
		_score += _softDropPoints;

		switch( amount )
		{
		case 1:
			_score += _level.ScoreMuliplier * Constants.LINE_SCORE_1;
			break;
		case 2:
			_score += _level.ScoreMuliplier * Constants.LINE_SCORE_2;
			break;
		case 3:
			_score += _level.ScoreMuliplier * Constants.LINE_SCORE_3;
			break;
		case 4:
			_score += _level.ScoreMuliplier * Constants.LINE_SCORE_4;
			break;
		}
	}

	public bool Move( int x, int y )
	{
		bool success = false;
		if( !_activeBricks.CheckCollision( _board, x, y ) )
		{
			_activeBricks.Move(x, y);
			_boardDrity = true;
			success = true;
		}
		return success;
	}

	public bool Rotate( int amount )
	{
		bool success = false;
		int oldRotation = _currentRotation;
		_currentRotation = _currentRotation + amount;

		while( _currentRotation < 0 )
		{
			_currentRotation += _currentShape.Rotations;
		}
		while( _currentRotation >= _currentShape.Rotations )
		{
			_currentRotation -= _currentShape.Rotations;
		}

		_activeBricks.SetActiveBricks( _currentShape.GetRotationAsArray( _currentRotation ) );

		if( _activeBricks.CheckCollision( _board, 0, 0 ) )
		{
			success = false;
			_currentRotation = oldRotation;
			_activeBricks.SetActiveBricks( _currentShape.GetRotationAsArray( _currentRotation ) );
		}
		else
		{
			_boardDrity = true;
		}

		return success;

	}

	public void SoftDrop( bool enabled )
	{
		_softDropEnabled = enabled;
	}
}
