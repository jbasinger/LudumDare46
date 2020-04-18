using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum Menu { 
	None,
	Bots,
	Shop,
	Blobs,
	Log,
	Power,
	Spawner
}

public class HUD : MonoBehaviour
{
	public GameObject panelParent;
	public GameObject menuPanel;
	public TextMeshProUGUI coinLabel;
	public TextMeshProUGUI clockLabel;
	public TextMeshProUGUI latestEventLabel;
	public float removeEventTime = 4f;

	Menu currentMenu = Menu.None;
	float nextTime = 0;

	void Start()
	{
		ClearAll();
		panelParent.SetActive(false);
	}

	void ClearAll()
	{
		for(int i = 0; i < menuPanel.transform.childCount; i++)
		{
			Destroy(menuPanel.transform.GetChild(i).gameObject);
		}
	}

	void Update()
	{
		if (Time.time > nextTime)
		{
			latestEventLabel.text = "";
		}
	}

	public static HUD GetHUD()
	{
		return GameObject.Find("HUD").GetComponent<HUD>();
	}

	public void ToggleMenu(Menu item, List<GameObject> itemList)
	{
		if (currentMenu == item)
		{
			panelParent.SetActive(false);
			currentMenu = Menu.None;
			return;
		}

		currentMenu = item;
		panelParent.SetActive(true);
		ClearAll();

		foreach(GameObject g in itemList)
		{
			g.transform.SetParent(menuPanel.transform,false);
		}

	}

	public void UpdateCoins(int coins)
	{
		coinLabel.text = coins.ToString("n0");
	}

	public void UpdateTime(int hours, int minutes)
	{
		clockLabel.text = string.Format($"{hours.ToString("D2")}:{minutes.ToString("D2")}");
	}

	public void UpdateEvent(string evt)
	{
		latestEventLabel.text = evt;
		nextTime = Time.time + removeEventTime;
	}

}


