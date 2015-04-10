using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {
	
	public GameObject gameOverElements;
	public Image gameOverScrim;
	public float gameOverFillSpeed = 1.0f;
	public BrickDisplay brickDisplay;
	public ShapeDisplay nextShape;

	public Text score;
	public Text level;
	public Text rows;

	public ShapeDisplay[] shapeCounterDisplays;
	public Text[] shapeCounterText;

	private TetrisGame _game; 

	public void Setup( TetrisGame game )
	{
		_game = game;
		Reset();
		
		for( int i = 0; i < shapeCounterDisplays.Length; ++i )
		{
			shapeCounterDisplays[i].Shape = game.GetGameData().Shapes[i];
		}
	}

	public void UpdateUI()
	{
		if( _game != null )
		{
			score.text = _game.GetScore().ToString();
			level.text = _game.GetLevel().ToString();
			rows.text = _game.GetRowsCleared().ToString();

			foreach( ShapeDisplay shapeCounter in shapeCounterDisplays )
			{
				shapeCounter.UpdateDisplay( _game.GetTileSet() );
			}

			for( int shapeIndex = 0; shapeIndex < shapeCounterText.Length; ++shapeIndex )
			{
				shapeCounterText[ shapeIndex ].text = _game.GetRandomizer().GetShapeCount( shapeIndex ).ToString();
			}

			nextShape.Shape = _game.GetRandomizer().PeekNextShape();
			nextShape.UpdateDisplay( _game.GetTileSet() );
		}
	}

	private void Reset()
	{
		gameOverElements.SetActive(false);
		gameOverScrim.fillAmount = 0.0f;
	}

	public void ShowGameOver()
	{
		this.StartCoroutine( GameOverCoroutine() );
	}

	public void DoRestartGame()
	{
		Main.LoadNewGame();
	}

	private IEnumerator GameOverCoroutine()
	{
		while( gameOverScrim.fillAmount < 1.0f )
		{
			gameOverScrim.fillAmount += gameOverFillSpeed * Time.deltaTime;
			yield return null;
		}

		gameOverElements.SetActive( true );
	}
}
