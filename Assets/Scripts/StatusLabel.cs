using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatusLabel : MonoBehaviour
{
	public TextMeshProUGUI label;

	private void Awake()
	{
		label = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void SetLabel(string msg)
	{
		label.text = msg;
	}
}
