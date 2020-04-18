using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobbySpawnerShopItem : MonoBehaviour, IShopItem
{

	public Sprite shopItemSprite;
	public string shopItemName;
	public int shopCost;

	public Sprite GetSprite()
	{
		return shopItemSprite;
	}

	public string GetName()
	{
		return shopItemName;
	}

	public int GetCost()
	{
		return shopCost;
	}

	public bool Placed()
	{
		return true;
	}

	public void Destroyed()
	{

	}

	public GameObject GetGameObject()
	{
		return gameObject;
	}

}
