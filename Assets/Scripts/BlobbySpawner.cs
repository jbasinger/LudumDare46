﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BlobbySpawner : MonoBehaviour
{

	public HUD hud;
	public int queueLength = 3;

	void Awake()
	{
		StopSpawner();
	}

	public void StopSpawner()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	public void StartSpawner()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	private void OnMouseUp()
	{

		List<GameObject> itemLabels = new List<GameObject>();
		

	}
}
