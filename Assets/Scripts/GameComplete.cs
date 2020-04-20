using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameComplete : MonoBehaviour
{

	public TextMeshProUGUI blobsSaved;
	public TextMeshProUGUI blobsSacd;
	public TextMeshProUGUI coinsMade;

	GameData data;

	// Start is called before the first frame update
	void Start()
	{
		data = GameData.GetData();
		blobsSaved.text = $"You saved {data.blobbiesKeptAlive} blobbies!";
		blobsSacd.text = $"You used {data.blobbiesSacrificed} blobbies!";
		coinsMade.text = $"You made {data.finalScore.ToString("n0")} coins!";
		data.startingCoins = data.finalScore;
	}

	public void GoToMainMenu()
	{
		data.ResetData();
		SceneManager.LoadScene("MainMenu");
	}

}
