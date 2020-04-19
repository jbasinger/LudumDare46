using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobbyTrap : MonoBehaviour, IShopItem
{

	public Sprite itemSprite;
	public Sprite currencySprite;
	public TrappedBlobby trappedPrefab;
	public int cost;
	BoxCollider2D boxCollider;
	ShopManager shop;
	GameData data;
	SoundManager sound;

	// Start is called before the first frame update
	void Awake()
	{
		sound = SoundManager.GetManager();
		shop = ShopManager.GetManager();
		data = GameData.GetData();
		boxCollider = GetComponent<BoxCollider2D>();
		boxCollider.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Blobby b = collision.gameObject.GetComponent<Blobby>();
		if(b == null)
			return;

		TrappedBlobby trapped = Instantiate(trappedPrefab);

		trapped.color = b.color;
		trapped.worth = b.chonk;

		trapped.gameObject.transform.position = transform.position;

		shop.MakeMoney((int)b.GetBlobWorth());
		data.blobbiesSacrificed += 1;

		sound.PlayTrapClip();

		Destroy(gameObject);
		Destroy(b.gameObject);
		Destroy(trapped.gameObject, 10);
	}

	public Sprite GetItemSprite()
	{
		return itemSprite;
	}

	public Sprite GetCurrencySprite()
	{
		return currencySprite;
	}

	public string GetName()
	{
		return "Trap";
	}

	public int GetCost()
	{
		return cost;
	}

	public bool Placed()
	{
		boxCollider.enabled = true;
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
