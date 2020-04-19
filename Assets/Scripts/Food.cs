using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
  Chicken
}

public class Food : MonoBehaviour
{
  public float replenish = 10;
  [SerializeField]
  public FoodType type;
}
