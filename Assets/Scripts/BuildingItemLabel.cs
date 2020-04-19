using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingItemLabel : MonoBehaviour
{

	Image itemImage;
	TextMeshProUGUI nameLabel;
	Image progressImage;

	void Awake()
	{
		Image[] images = GetComponentsInChildren<Image>();
		for(int i = 0; i < images.Length; i++)
		{
			if (images[i].name == "ItemImage")
				itemImage = images[i];
			if (images[i].name == "Fill")
				progressImage = images[i];
		}
		nameLabel = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void SetBuildItem(Sprite buildingSprite,string name, float progress)
	{
		itemImage.sprite = buildingSprite;
		nameLabel.text = name;
		UpdateProgress(progress);
	}

	public void UpdateProgress(float progress)
	{
		if (progress > 1)
			progress = 1;
		
		progressImage.transform.localScale = new Vector3(progress, 1, 1);
	}

}
