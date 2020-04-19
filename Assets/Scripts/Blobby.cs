using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blobby : MonoBehaviour
{
	public string name;
	public Color color;

	public float currentHP;
	public float maxHP=100;

	public float currentHunger;
	public float maxHunger=100;

	public float boredChance = .5f;
	public float hungerCooldown = 10;
	public float thinkingCooldown = 5;
	public float wanderCooldown = 10;
	public float attackCooldown = 10;
	public float hungerTick = 5f;

	public float wanderMaxDistance = 5;
	public float maxMovementSpeed = 2.5f;

	public float dontWanderCheckRadius = 0.25f;
	public float blobbyBumpChance = .5f;
	public float blobbyAttack = 10;

	public float worth = 1;

	float speed;
	Vector3 wanderLocation;
	bool isWandering = false;
	bool isDead = false;
	bool isHungry = false;
	bool isAngry = false;
	float hungerTime;
	float thinkingTime;
	float wanderingTime;
	float attackTime;
	SpriteRenderer renderer;
	EventLogManager log;
	ShopManager shopManager;
	Image hpBar;
	Image hungerBar;

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		log = EventLogManager.GetManager();
		shopManager = ShopManager.GetManager();

		Image[] imgs = GetComponentsInChildren<Image>();
		for(int i = 0; i < imgs.Length; i++)
		{
			if (imgs[i].name == "CurrentHP")
				hpBar = imgs[i];
			if (imgs[i].name == "CurrentFood")
				hungerBar = imgs[i];
		}

		currentHP = maxHP;
		currentHunger = maxHunger/2;
		hungerTime = Random.Range(hungerCooldown / 2, hungerCooldown * 1.5f);
	}
	
	// Update is called once per frame
	void Update()
	{
		renderer.color = color;

		if (isDead)
		{
			return;
		}

		UpdateHunger();
		CheckDeath();
		DoAI();
		CheckLimits();
		UpdateUI();
		
	}

	void CheckLimits()
	{
		if (currentHP < 0)
			currentHP = 0;
		if (currentHP > maxHP)
			currentHP = maxHP;

		if (currentHunger < 0)
			currentHunger = 0;
		if (currentHunger > maxHunger)
			currentHunger = maxHunger;
	}

	void UpdateHunger()
	{
		hungerTime -= Time.deltaTime;
		if (hungerTime <= 0)
		{

			currentHunger -= Random.Range(hungerTick / 2, hungerTick);
			hungerTime = Random.Range(hungerCooldown / 2, hungerCooldown * 1.5f);

			if (currentHunger <= 0)
			{
				float hit = Random.Range(5, 10);
				currentHP -= hit;
				worth -= hit / 50;
				if (worth <= 1)
					worth = 1;
				log.AddEvent($"{name} is starving!");
			}

			if (currentHunger >= maxHunger)
			{
				currentHP += Random.Range(5, 10);
			}

			isHungry = (currentHunger / maxHP) <= 0.5;
			if (isHungry)
			{
				log.AddEvent($"{name} is getting hungry.");
			}
		}
	}

	void CheckDeath()
	{
		if (currentHP < 0)
		{
			color = Color.white;
			log.AddEvent($"{name} has died!");
			isDead = true;
			Destroy(gameObject, 3f);
		}
	}

	void UpdateUI()
	{
		hpBar.transform.localScale = new Vector3(currentHP / maxHP, 1, 1);
		hungerBar.transform.localScale = new Vector3(currentHunger / maxHunger, 1, 1);
		transform.localScale = new Vector2(worth, worth);
	}

	void DoAI()
	{

		attackTime -= Time.deltaTime;

		if (isWandering)
		{
			Wander();
		}

		thinkingTime -= Time.deltaTime;
		if (thinkingTime <= 0)
		{
			if (isHungry)
			{
				FindFood();
			}
			if (!isWandering)
			{

				wanderLocation = Random.insideUnitCircle.normalized * Random.Range(wanderMaxDistance / 2, wanderMaxDistance);

				while (Physics.CheckSphere(wanderLocation, dontWanderCheckRadius))
				{
					wanderLocation = Random.insideUnitCircle.normalized * Random.Range(wanderMaxDistance / 2, wanderMaxDistance);
				}

				speed = Random.Range(maxMovementSpeed / 2, maxMovementSpeed);
				wanderingTime = wanderCooldown;
				isWandering = true;
			}
			thinkingTime = Random.Range(thinkingCooldown / 2, thinkingCooldown * 1.5f);
		}
	}

	void Wander()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, wanderLocation, step);

		wanderingTime -= Time.deltaTime;

		if (wanderingTime <= 0 || Vector2.Distance(transform.position, wanderLocation) < 0.1)
		{
			isWandering = false;
		}

	}

	void FindFood()
	{
		//Change the wander position to the nearest food source.
		GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
		
		if (foods.Length == 0)
			return;

		wanderLocation = foods[Random.Range(0, foods.Length)].transform.position;
		isWandering = true;
		speed = Random.Range(maxMovementSpeed / 2, maxMovementSpeed);
		wanderingTime = wanderCooldown;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Food f = collision.gameObject.GetComponent<Food>();
		Blobby b = collision.gameObject.GetComponent<Blobby>();

		if (f != null)
		{
			float foodValue = Random.Range(f.replenish * .75f, f.replenish * 1.25f);
			currentHunger += foodValue;
			if(currentHunger > maxHunger)
			{
				worth += foodValue / 100;
			}
			isWandering = false;
			Destroy(f.gameObject);
		}

		if (b != null && Random.value > blobbyBumpChance && attackTime<=0)
		{
			b.currentHP -= Random.Range(blobbyAttack * 0.5f, blobbyAttack * 1.5f);
			attackTime += Random.Range(attackCooldown * 0.5f, attackCooldown * 1.5f);
			log.AddEvent($"{name} attacks {b.name}!");
		}			

	}

}


