using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using State;
using UnityEngine;

public class BoxFood : MonoBehaviour
{
    public List<Food> foodsInBox = new();

    public Food TakeFood(Enemy enemyTake)
    {
        var foodTake = foodsInBox.FirstOrDefault(food => food.state == FoodState.InBox);
        if (foodTake == null) return null;
        foodTake.state = FoodState.Normal;
        foodTake.Picked(enemyTake);
        return foodTake;
    }
}