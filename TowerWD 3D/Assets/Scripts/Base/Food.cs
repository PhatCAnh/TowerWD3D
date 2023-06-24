using System;
using System.Collections;
using System.Collections.Generic;
using State;
using CanasSource;
using UnityEngine;

public class Food : MonoBehaviour
{
    protected Collider theColl;
    protected Rigidbody theRB;
    
    public FoodState state;
    private InGameController inGameController => Singleton<InGameController>.Instance;

    private void Start()
    {
        theColl = GetComponent<Collider>();
        theRB = GetComponent<Rigidbody>();
        inGameController.foods.Add(this);
        inGameController.Parent_Food.foodsInBox.Add(this);
        transform.SetParent(inGameController.Parent_Food.transform);
    }

    public void Picked(Enemy enemyTake)
    {
        if (state == FoodState.InBox) return;
        theRB.useGravity = false;
        theRB.constraints = RigidbodyConstraints.FreezePosition;
        state = FoodState.Picked;
        transform.SetParent(enemyTake.transform);
        transform.position = enemyTake.pickedPos.position;
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
