using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager {

	private enum Instruction {
		None,
		MoveLeft,
		MoveRight,
		RotateRight,
		RotateLeft,
		SoftDrop,
		Pause
	};

	private Dictionary<KeyCode, Instruction> _controls = new Dictionary<KeyCode, Instruction>
	{
		{ KeyCode.A, Instruction.MoveLeft },
		{ KeyCode.LeftArrow, Instruction.MoveLeft },
		{ KeyCode.D, Instruction.MoveRight },
		{ KeyCode.RightArrow, Instruction.MoveRight },
		{ KeyCode.W, Instruction.RotateLeft },
		{ KeyCode.UpArrow, Instruction.RotateLeft },
		{ KeyCode.JoystickButton0, Instruction.RotateLeft },
		{ KeyCode.JoystickButton2, Instruction.RotateLeft },
		{ KeyCode.S, Instruction.RotateRight },
		{ KeyCode.DownArrow, Instruction.RotateRight },
		{ KeyCode.JoystickButton1, Instruction.RotateRight },
		{ KeyCode.JoystickButton3, Instruction.RotateRight },
		{ KeyCode.Space, Instruction.SoftDrop },
	};

	private string[] MoveAxes = new string[]
	{
		"Horizontal",
		"DPad Horiz"
	};
	
	private string[] DropAxes = new string[]
	{
		"Vertical",
		"DPad Vert"
	};

	private HashSet<Instruction> _repeatableCommands = new HashSet<Instruction>()
	{
		Instruction.MoveLeft,
		Instruction.MoveRight,
		Instruction.SoftDrop,
	};

	private TetrisGame _game;

	private int _repeatDelay = 0;
	private Instruction _currentMoveInstruction = Instruction.None;

	public InputManager( TetrisGame game )
	{
		_game = game;

		Debug.Log( "Inputs: " +  string.Join(",", Input.GetJoystickNames() ) );
	}

	public void Update( bool inputLock )
	{
		//collect commands

		HashSet< Instruction > commands = new HashSet<Instruction>();

		foreach( KeyValuePair<KeyCode, Instruction> control in _controls )
		{
			if( Input.GetKeyDown( control.Key ) )
			{
				commands.Add( control.Value );
			}

			if( _repeatableCommands.Contains( control.Value ) && Input.GetKey( control.Key ) )
			{
				commands.Add( control.Value );
			}
		}

		foreach( string axis in MoveAxes )
		{
			if( Input.GetAxis( axis ) >= 1 )
			{
				commands.Add( Instruction.MoveRight );
			}
			if( Input.GetAxis( axis ) <= -1 )
			{
				commands.Add( Instruction.MoveLeft );
			}
		}
		
		foreach( string axis in DropAxes )
		{
			if( Input.GetAxis( axis ) <= -1 )
			{
				commands.Add( Instruction.SoftDrop );
			}
		}

		//execute commands

		//move
		Instruction moveCommand = HashContainsXOR( commands, Instruction.MoveLeft, Instruction.MoveRight );

		if( moveCommand != _currentMoveInstruction )
		{
			_currentMoveInstruction = moveCommand;
			_repeatDelay = Constants.INPUT_DAS_INITIAL;
			DoInstruction( moveCommand, inputLock );
		}
		else
		{
			RepeatInstruction( moveCommand, inputLock );
		}

		//rotate
		Instruction rotationCommand = HashContainsXOR( commands, Instruction.RotateLeft, Instruction.RotateRight );
		DoInstruction( rotationCommand, inputLock );

		//soft drop
		_game.SoftDrop( commands.Contains( Instruction.SoftDrop ) );
	}

	private static Instruction HashContainsXOR( HashSet<Instruction> set, Instruction itemA, Instruction itemB )
	{
		Instruction result = Instruction.None;
		
		if( set.Contains( itemA ) && set.Contains( itemB ) )
		{
			result = Instruction.None;
		}
		else if( set.Contains( itemA ) )
		{
			result = itemA;
		}
		else if( set.Contains( itemB ) )
		{
			result = itemB;
		}

		return result;
	}

	private bool isDASActive()
	{
		return _currentMoveInstruction != Instruction.None;
	}

	private void RepeatInstruction( Instruction instruction, bool inputLock )
	{
		if( !inputLock )
		{
			if( isDASActive() )
			{
				_repeatDelay -= 1;
			}

			if( _repeatDelay == 0 )
			{
				DoInstruction( instruction, inputLock );
				_repeatDelay = Constants.INPUT_DAS_REPEAT;
			}
		}
	}

	private void DoInstruction( Instruction instruction, bool inputLock )
	{
		if( !inputLock )
		{
			bool shortCircutDAS = false;

			switch( instruction )
			{
			case Instruction.MoveLeft:
				shortCircutDAS = !_game.Move( -1, 0 );
				break;
			case Instruction.MoveRight:
				shortCircutDAS = !_game.Move( 1, 0 );
				break;
			case Instruction.RotateLeft:
				_game.Rotate( 1 );
				break;
			case Instruction.RotateRight:
				_game.Rotate( -1 );
				break;
			}

			if( shortCircutDAS )
			{
				_repeatDelay = 0;
			}
		}
	}
}
