using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrappedBlobby : MonoBehaviour
{

	public float worth;
	public Color color;
	Image backgroundImage;
	

	void Awake()
	{
		
		Image[] imgs = GetComponentsInChildren<Image>();
		for (int i = 0; i < imgs.Length; i++)
		{
			if(imgs[i].name == "FillColor")
			{
				backgroundImage = imgs[i];
			}
		}
		
	}

	private void Update()
	{
		if(backgroundImage != null)
		{
			backgroundImage.color = color;
		}
	}

}
