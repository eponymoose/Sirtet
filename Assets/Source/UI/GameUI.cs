using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {
	
	public GameObject gameOverElements;
	public Image gameOverScrim;
	public float gameOverFillSpeed = 1.0f;

	public Text score;
	public Text level;
	public Text rows;

	private TetrisGame _game;

	public void Setup( TetrisGame game )
	{
		_game = game;
		Reset();
	}

	void Update()
	{
		if( _game != null )
		{
			score.text = _game.GetScore().ToString();
			level.text = _game.GetLevel().ToString();
			rows.text = _game.GetRowsCleared().ToString();
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
