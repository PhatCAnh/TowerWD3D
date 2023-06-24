using System;
using System.Collections;
using System.Collections.Generic;
using State;
using CanasSource;
using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodState state;
    private InGameController inGameController => Singleton<InGameController>.Instance;

    private void Start()
    {
        inGameController.foods.Add(this);
        transform.SetParent(inGameController.Parent_Food);
    }

    public void Picked(Transform parent)
    {
        state = FoodState.Picked;
        transform.SetParent(parent);
    }

    public void Droped()
    {
        state = FoodState.Normal;
        transform.SetParent(null);
    }

    public void Lost()
    {
        state = FoodState.Lost;
        inGameController.CheckLoseGame();
    }
}
