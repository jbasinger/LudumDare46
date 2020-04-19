using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

	HUD hud;

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
		nextMinute = Time.time;
	}

	// Update is called once per frame
	void Update()
	{

		if (minute <= 0 && hour <= 0)
			return;

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


}
