﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopManager {
	void BuyItem(int index);
}

public interface IShopItem
{
	Sprite GetItemSprite();
	Sprite GetCurrencySprite();
	string GetName();
	int GetCost();
	bool Placed();
	void Destroyed();
	GameObject GetGameObject();
}

public class PotentialPurchaseItem
{
	public PotentialPurchaseItem(GameObject obj, Transform parent)
	{
		ItemInstance = GameObject.Instantiate(obj, parent);
		Item = ItemInstance.GetComponent<IShopItem>();
	}
	public IShopItem Item { get; set; }
	public GameObject ItemInstance { get; set; }
}

public class ShopManager : MonoBehaviour, IShopManager
{

	public bool isPlacingItem;
	public GameObject itemParent;
	public int coins = 10000;
	public ShopItemLabel shopItemLabelPrefab;
	public List<GameObject> shopItemPrefabs = new List<GameObject>();

	HUD hud;
	EventLogManager logManager;
	PotentialPurchaseItem itemBeingBought;
	
	public static ShopManager GetManager()
	{
		return GameObject.Find("ShopManager").GetComponent<ShopManager>();
	}

	void Start()
	{
		hud = HUD.GetHUD();
		logManager = EventLogManager.GetManager();
	}

	void Update()
	{
		hud.UpdateCoins(coins);
		if (isPlacingItem)
		{

			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Camera.main.transform.position.z * -1;
			itemBeingBought.ItemInstance.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
			if (Input.GetMouseButtonDown(0))
			{
				if (itemBeingBought.Item.Placed())
				{
					coins -= itemBeingBought.Item.GetCost();
					isPlacingItem = false;
					logManager.AddEvent($"You buy a {itemBeingBought.Item.GetName()}");
				}
			}
			if (Input.GetMouseButtonDown(1))
			{
				isPlacingItem = false;
			}
		}
	}

	public void BuyItem(int index)
	{

		itemBeingBought = new PotentialPurchaseItem(shopItemPrefabs[index], transform);

		if (coins < itemBeingBought.Item.GetCost())
		{
			logManager.AddEvent($"You can't afford that.");
			Destroy(itemBeingBought.ItemInstance);
			isPlacingItem = false;
			return;
		}

		isPlacingItem = true;
		ToggleShoppingList();
	}

	public void MakeMoney(int c)
	{
		coins += c;
	}

	public void ToggleShoppingList()
	{
		List<GameObject> itemList = new List<GameObject>();
		int i = 0;
		foreach (GameObject item in shopItemPrefabs)
		{
			ShopItemLabel label = Instantiate(shopItemLabelPrefab);
			IShopItem shopItem = item.GetComponent<IShopItem>();
			label.SetShopItem(i++, shopItem, this);
			itemList.Add(label.gameObject);
		}

		hud.ToggleMenu(Menu.Shop, itemList);

	}

}
