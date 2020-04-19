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
	public float coinCooldown = 2;
	public float coinPerProc = 1;
	public float coinTime;

	HUD hud;
	EventLogManager logManager;
	PotentialPurchaseItem itemBeingBought;
	SoundManager sound;
	
	public static ShopManager GetManager()
	{
		return GameObject.Find("ShopManager").GetComponent<ShopManager>();
	}

	void Start()
	{
		sound = SoundManager.GetManager();
		hud = HUD.GetHUD();
		logManager = EventLogManager.GetManager();
		coinTime = coinCooldown;
	}

	void Update()
	{
		hud.UpdateCoins(coins);

		coinTime -= Time.deltaTime;
		if (coinTime <= 0)
		{
			MakeMoney((int)coinPerProc);
			coinTime = coinCooldown;
		}

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
					logManager.AddEvent($"Bought {itemBeingBought.Item.GetName().ToLower()}");
				}
			}
			if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
			{
				Destroy(itemBeingBought.ItemInstance);
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
			sound.PlayRandomNoMoneyClip();
			Destroy(itemBeingBought.ItemInstance);
			isPlacingItem = false;
			return;
		}

		sound.PlayChachingClip();
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
		sound.PlayDrawerClip();

	}

}
