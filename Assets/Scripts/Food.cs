using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
  Chicken,
  Melon,
  Heal
}

public class Food : MonoBehaviour,IShopItem
{
  public string foodName;
  public Sprite itemSprite;
  public Sprite currencySprite;
  public int cost;
  public float replenish = 10;
  [SerializeField]
  public FoodType type;

  CircleCollider2D circleCollider;

  public void Awake()
  {
    circleCollider = GetComponent<CircleCollider2D>();
    circleCollider.enabled = false;
  }

  public void Destroyed()
  {

  }

  public int GetCost()
  {
    return cost;
  }

  public Sprite GetCurrencySprite()
  {
    return currencySprite;
  }

  public GameObject GetGameObject()
  {
    return gameObject;
  }

  public Sprite GetItemSprite()
  {
    return itemSprite;
  }

  public string GetName()
  {
    return foodName;
  }

  public bool Placed()
  {
    circleCollider.enabled = true;
    return true;
  }
}
