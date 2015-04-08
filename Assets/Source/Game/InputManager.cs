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
		{ KeyCode.S, Instruction.RotateRight },
		{ KeyCode.DownArrow, Instruction.RotateRight }
	};

	private TetrisGame _game;

	private Instruction _currentInstruction = Instruction.None;
	private int _repeatDelay = 0;

	public InputManager( TetrisGame game )
	{
		_game = game;
	}

	public void Update( bool inputLock )
	{
		Instruction instructionToStart = Instruction.None;
		Instruction instructionToEnd = Instruction.None;

		foreach( KeyValuePair<KeyCode, Instruction> input in _controls )
		{
			if( Input.GetKeyDown( input.Key ) )
			{
				instructionToStart = input.Value;
			}
			
			if( Input.GetKeyUp( input.Key ) )
			{
				instructionToEnd = input.Value;
			}
		}

		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			_game.SoftDrop( true );
		}
		
		if( Input.GetKeyUp( KeyCode.Space ) )
		{
			_game.SoftDrop( false );
		}

		if( instructionToEnd == _currentInstruction )
		{
			EndInstruction();
		}

		if( instructionToStart != Instruction.None )
		{
			DoInstruction( instructionToStart, inputLock );
			_repeatDelay = Constants.INPUT_DAS_INITIAL;
		}
		else
		{
			RepeatInstruction( inputLock );
		}
	}

	private bool isDASActive()
	{
		return _currentInstruction == Instruction.MoveLeft || _currentInstruction == Instruction.MoveRight;
	}

	private void RepeatInstruction( bool inputLock )
	{
		if( !inputLock )
		{
			if( isDASActive() )
			{
				_repeatDelay -= 1;
			}

			if( _repeatDelay == 0 )
			{
				DoInstruction( _currentInstruction, inputLock );
				_repeatDelay = Constants.INPUT_DAS_REPEAT;
			}
		}
	}

	private void DoInstruction( Instruction instruction, bool inputLock )
	{
		_currentInstruction = instruction;
		
		if( !inputLock )
		{
			switch( _currentInstruction )
			{
			case Instruction.MoveLeft:
				_game.Move( -1, 0 );
				break;
			case Instruction.MoveRight:
				_game.Move( 1, 0 );
				break;
			case Instruction.RotateLeft:
				_game.Rotate( -1 );
				break;
			case Instruction.RotateRight:
				_game.Rotate( 1 );
				break;
			}
		}
	}

	private void EndInstruction()
	{
		_currentInstruction = Instruction.None;
	}
}
