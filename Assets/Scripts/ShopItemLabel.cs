using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ShopItemLabel : MonoBehaviour
{

	Image itemImage;
	Image currencyImage;
	TextMeshProUGUI nameLabel;
	TextMeshProUGUI costLabel;
	Button button;

	public void SetShopItem(int itemIndex, IShopItem item, IShopManager shop)
	{
		itemImage.sprite = item.GetItemSprite();
		currencyImage.sprite = item.GetCurrencySprite();
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
				itemImage = images[i];

			if (images[i].name == "CurrencyImage")
				currencyImage = images[i];
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

