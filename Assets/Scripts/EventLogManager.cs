using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLogManager : MonoBehaviour
{
	public HUD hud;
	public EventLogEntry logEntryPrefab;
	public List<string> events = new List<string>();

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
