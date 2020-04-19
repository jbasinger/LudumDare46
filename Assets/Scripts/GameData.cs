using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

	public int blobbiesKeptAlive;
	public int blobbiesSacrificed;
	public int finalScore;
	public void ResetData()
	{
		blobbiesKeptAlive = 0;
		blobbiesSacrificed = 0;
		finalScore = 0;
	}
	// Start is called before the first frame update
	void Start()
	{
		ResetData();
		DontDestroyOnLoad(gameObject);
	}

	public static GameData GetData()
	{
		return GameObject.Find("GameData").GetComponent<GameData>();
	}
}
