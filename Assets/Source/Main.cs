using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public GameData Data;

	private TetrisGame game = new TetrisGame();
	
	public static void LoadNewGame()
	{
		Application.LoadLevel( "Main" );
	}

	public void Start()
	{
		Time.fixedDeltaTime = 1.0f / Constants.FRAME_RATE;
		
		game = new TetrisGame();
		game.Setup( Data );
	}

	void FixedUpdate()
	{
		game.FrameUpdate();
	}
}
