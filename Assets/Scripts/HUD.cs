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
		panelParent.transform.localScale = Vector3.zero;
	}

	void ClearAll()
	{
		for (int i = 0; i < menuPanel.transform.childCount; i++)
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

	public bool ToggleMenu(Menu item, List<GameObject> itemList)
	{

		ClearAll();

		if (currentMenu == item)
		{
			panelParent.transform.localScale = Vector3.zero;
			currentMenu = Menu.None;
			foreach(GameObject g in itemList)
			{
				Destroy(g);
			}
			return false;
		}

		currentMenu = item;
		panelParent.transform.localScale = Vector3.one;

		foreach (GameObject g in itemList)
		{
			g.transform.SetParent(menuPanel.transform);
		}
		
		return true;

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


