using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLogManager : MonoBehaviour
{
	
	public EventLogEntry logEntryPrefab;
	public List<string> events = new List<string>();
	
	HUD hud;

	public static EventLogManager GetManager()
	{
		return GameObject.Find("EventLogManager").GetComponent<EventLogManager>();
	}

	private void Start()
	{
		hud = HUD.GetHUD();
		AddEvent("Welcome!");
	}

	public void AddEvent(string evt){
		events.Insert(0, evt);
		hud.UpdateEvent(evt);
	}

	public void ToggleEventList()
	{
		List<GameObject> itemList = new List<GameObject>();

		foreach (string item in events)
		{
			EventLogEntry label = Instantiate(logEntryPrefab);
			label.SetEntry(item);
			itemList.Add(label.gameObject);
		}

		hud.ToggleMenu(Menu.Log, itemList);
	}

}
