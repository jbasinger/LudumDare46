using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blobby : MonoBehaviour
{
	public string blobbyName;
	public Color color;

	public float currentHP;
	public float maxHP = 100;

	public float currentHunger;
	public float maxHunger = 100;

	public float angryChance = .1f;
	public float hungerCooldown = 10;
	public float thinkingCooldown = 5;
	public float wanderCooldown = 10;
	public float attackCooldown = 7.5f;
	public float hungerTick = 10f;

	public float wanderMaxDistance = 5;
	public float maxMovementSpeed = 2.5f;

	public float dontWanderCheckRadius = 0.25f;
	public float blobbyBumpChance = .5f;
	public float blobbyAttack = 10;

	public float chonk = 1;

	float speed;
	Vector3 wanderLocation;
	bool isWandering = false;
	bool isDead = false;
	bool isHungry = false;
	float hungerTime;
	float thinkingTime;
	float wanderingTime;
	float attackTime;
	float timeAlive = 0;
	SpriteRenderer spriteRenderer;
	EventLogManager log;
	ShopManager shopManager;
	SoundManager sound;
	Image hpBar;
	Image hungerBar;

	private void Awake()
	{
		sound = SoundManager.GetManager();
		spriteRenderer = GetComponent<SpriteRenderer>();
		log = EventLogManager.GetManager();
		shopManager = ShopManager.GetManager();

		Image[] imgs = GetComponentsInChildren<Image>();
		for (int i = 0; i < imgs.Length; i++)
		{
			if (imgs[i].name == "CurrentHP")
				hpBar = imgs[i];
			if (imgs[i].name == "CurrentFood")
				hungerBar = imgs[i];
		}

		currentHP = maxHP;
		currentHunger = maxHunger * .75f;
		hungerTime = Random.Range(hungerCooldown / 2, hungerCooldown * 1.5f);
	}

	// Update is called once per frame
	void Update()
	{
		spriteRenderer.color = color;

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
				chonk -= hit / 50;
				if (chonk <= 1)
					chonk = 1;
				log.AddEvent($"{blobbyName} is starving!");
			}

			if (currentHunger >= maxHunger)
			{
				currentHP += Random.Range(5, 10);
			}

			isHungry = currentHunger < maxHunger;
			if (isHungry)
			{
				log.AddEvent($"{blobbyName} is getting hungry.");
			}
		}
	}

	void CheckDeath()
	{
		if (currentHP < 0)
		{
			color = Color.white;
			log.AddEvent($"{blobbyName} has died!");
			isDead = true;
			Destroy(gameObject, 3f);
		}
	}

	void UpdateUI()
	{
		hpBar.transform.localScale = new Vector3(currentHP / maxHP, 1, 1);
		hungerBar.transform.localScale = new Vector3(currentHunger / maxHunger, 1, 1);
		transform.localScale = new Vector2(chonk, chonk);
	}

	void DoAI()
	{

		timeAlive += Time.deltaTime;
		attackTime -= Time.deltaTime;

		if (isWandering)
		{
			Wander();
		}

		thinkingTime -= Time.deltaTime;
		if (thinkingTime <= 0)
		{

			if (Random.value < (angryChance*chonk)) {
				HulkSmash();
			}

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

		if (Random.value < 0.25)
		{
			sound.PlayRandomMovementClip();
		}

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

			if (f.type == FoodType.Heal)
			{
				currentHP += foodValue;
				if (currentHP > maxHP)
					currentHP = maxHP;
				sound.PlayRandomHealClip();
			}
			else
			{
				currentHunger += foodValue;
				if (currentHunger > maxHunger)
				{
					chonk += foodValue / 200;
				}
				sound.PlayRandomFoodClip();
			}
			
			isWandering = false;
			Destroy(f.gameObject);
		}

		if (b != null && Random.value > blobbyBumpChance && attackTime <= 0)
		{
			b.currentHP -= Random.Range(blobbyAttack * 0.5f, blobbyAttack * 1.5f) * chonk;
			attackTime += Random.Range(attackCooldown * 0.5f, attackCooldown * 1.5f);
			log.AddEvent($"{blobbyName} attacks {b.blobbyName}!");
			sound.PlayRandomHitClip();
		}

	}

	public float GetBlobWorth()
	{
		return 10 + ((timeAlive) * chonk * (currentHP / maxHP));
	}

	public void HulkSmash()
	{
		GameObject[] blobbies = GameObject.FindGameObjectsWithTag("Blobby");
		if (blobbies.Length <= 1)
			return;

		GameObject victim = blobbies[Random.Range(0, blobbies.Length)];
		while(victim == gameObject)
		{
			victim = blobbies[Random.Range(0, blobbies.Length)];
		}
		
		Blobby b = victim.GetComponent<Blobby>();

		log.AddEvent($"{blobbyName} gets mad at {b.blobbyName} and attacks!");

		wanderLocation = victim.transform.position;
		attackTime = 0;
		isWandering = true;
		speed = Random.Range(maxMovementSpeed / 2, maxMovementSpeed);
		wanderingTime = wanderCooldown;

	}

}


