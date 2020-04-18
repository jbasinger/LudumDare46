using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ShopItemLabel : MonoBehaviour
{

	Image image;
	TextMeshProUGUI nameLabel;
	TextMeshProUGUI costLabel;
	Button button;

	public void SetShopItem(int itemIndex, IShopItem item, ShopManager shop)
	{
		image.sprite = item.GetSprite();
		nameLabel.text = item.GetName();
		costLabel.text = item.GetCost().ToString("n0");
		button.onClick.AddListener(delegate { shop.BuyItem(itemIndex); });
	}

	void Awake()
	{

		button = GetComponent<Button>();

		Image[] images = GetComponentsInChildren<Image>();
		for(int i = 0; i < images.Length; i++)
		{
			if (images[i].name == "ItemImage")
				image = images[i];
		}
		

		TextMeshProUGUI[] labels = GetComponentsInChildren<TextMeshProUGUI>();
		for(int i = 0; i < labels.Length; i++)
		{
			if (labels[i].name == "Name")
				nameLabel = labels[i];

			if (labels[i].name == "Cost")
				costLabel = labels[i];
		}
	}

}

