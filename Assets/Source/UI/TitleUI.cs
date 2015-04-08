using UnityEngine;
using System.Collections;

public class TitleUI : MonoBehaviour {

	public void OnPlayClicked()
	{
		Main.LoadNewGame();
	}
}
