using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{

	GameData gameData;
	HUD hud;
	ShopManager shop;

	[Range(0, 23)]
	public int hour = 8;
	[Range(0, 60)]
	public int minute = 0;
	public float minutesPerSecond = 1;
	private float nextMinute = 0;

	// Start is called before the first frame update
	void Start()
	{
		hud = HUD.GetHUD();
		gameData = GameData.GetData();
		shop = ShopManager.GetManager();
		nextMinute = Time.time;
	}

	// Update is called once per frame
	void Update()
	{

		if (minute <= 0 && hour <= 0) {
			CompleteGame();
			return;
		}
			

		if (Time.time > nextMinute)
		{
			minute--;
			nextMinute = Time.time + minutesPerSecond;
		}

		if (minute > 59)
		{
			hour++;
			minute = 0;
		}

		if (minute <= 0 && hour > 0)
		{
			minute = 59;
			hour--;
		}

		if (hour > 23)
		{
			hour = 0;
		}

		if (hour <= 0)
			hour = 0;
		
		hud.UpdateTime(hour, minute);

	}

	void CompleteGame()
	{
		GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blobby");
		gameData.blobbiesKeptAlive = blobs.Length;
		foreach(GameObject go in blobs)
		{
			Blobby blob = go.GetComponent<Blobby>();
			gameData.finalScore += (int)(blob.GetBlobWorth() * 2) + shop.coins;
		}
		SceneManager.LoadScene("GameComplete");
	}

}
