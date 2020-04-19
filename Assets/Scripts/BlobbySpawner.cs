using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuedBlobby
{
	public QueuedBlobby()
	{
		Color = Random.ColorHSV(0, 1, .5f, 1, 1, 1);
		CurrentTime = 0;
	}
	public string Name { get; set; }
	public Color Color { get; set; }
	public float TimeDone { get; set; }
	public float CurrentTime { get; set; }
	public float PercentComplete()
	{
		return CurrentTime / TimeDone;
	}
	public BuildingItemLabel Label { get; set; }
}

public class BlobbySpawner : MonoBehaviour, IShopItem, IShopManager
{
	public Sprite energySprite;
	public Sprite blobbySprite;
	public int cost;
	public int queueLength = 5;
	public int blobbyCookTime = 10;

	public ShopItemLabel shopListItemPrefab;
	public BuildingItemLabel blobListItemPrefab;
	public GameObject blobbyPrefab;

	private HUD hud;
	private EventLogManager log;
	private List<QueuedBlobby> queuedBlobbies = new List<QueuedBlobby>();
	private List<QueuedBlobby> doneBlobbys = new List<QueuedBlobby>();
	private List<GameObject> itemLabels = new List<GameObject>();
	private bool isToggleUp = false;

	void Awake()
	{
		hud = HUD.GetHUD();
		log = EventLogManager.GetManager();
		StopSpawner();
	}

	void Update()
	{
		
		foreach (QueuedBlobby b in queuedBlobbies)
		{
			float progress = b.PercentComplete();

			if (b.Label != null)
				b.Label.UpdateProgress(progress);

			if (progress >= 1)
				doneBlobbys.Add(b);

			b.CurrentTime += Time.deltaTime;
		}

		bool didOne = false;
		foreach(QueuedBlobby b in doneBlobbys)
		{
			GameObject freshBlobby = Instantiate(blobbyPrefab);
			freshBlobby.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle.normalized * 3;
			log.AddEvent($"{b.Name} is born!");
			queuedBlobbies.Remove(b);
			didOne = true;
		}
		
		doneBlobbys.Clear();

		if (didOne && isToggleUp)
		{
			hud.ToggleMenu(Menu.None, new List<GameObject>());
			CleanGameObjectRefs();
			UpdateMenu();
		}

		if (queuedBlobbies.Count > 0)
		{
			StartSpawner();
		}
		else
		{
			StopSpawner();
		}

	}

	public void StopSpawner()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	public void StartSpawner()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	private void OnMouseUp()
	{
		UpdateMenu();
	}

	private void UpdateMenu()
	{
		itemLabels = new List<GameObject>();
		foreach (QueuedBlobby b in queuedBlobbies)
		{
			BuildingItemLabel item = Instantiate(blobListItemPrefab);
			item.SetBuildItem(blobbySprite, b.Name, b.PercentComplete());
			itemLabels.Add(item.gameObject);
			b.Label = item;
		}

		for (int i = 0; i < queueLength - queuedBlobbies.Count; i++)
		{
			ShopItemLabel item = Instantiate(shopListItemPrefab);
			item.SetShopItem(0, this, this);
			itemLabels.Add(item.gameObject);
		}

		isToggleUp = hud.ToggleMenu(Menu.Spawner, itemLabels);

		if (!isToggleUp)
		{
			CleanGameObjectRefs();
		}
	}

	private void CleanGameObjectRefs()
	{
		itemLabels.Clear();
		foreach (QueuedBlobby b in queuedBlobbies)
		{
			b.Label = null;
		}
	}

	public Sprite GetItemSprite()
	{
		return blobbySprite;
	}

	public Sprite GetCurrencySprite()
	{
		return energySprite;
	}

	public string GetName()
	{
		return "Blobby";
	}

	public int GetCost()
	{
		return cost;
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

	public void BuyItem(int index)
	{
		if (queuedBlobbies.Count < queueLength)
		{
			queuedBlobbies.Add(new QueuedBlobby() { Name = "Geophph", TimeDone = blobbyCookTime });
			hud.ToggleMenu(Menu.None, new List<GameObject>());
			CleanGameObjectRefs();
			UpdateMenu();
			log.AddEvent("You begin to grow a blobby.");
		}
	}
}
