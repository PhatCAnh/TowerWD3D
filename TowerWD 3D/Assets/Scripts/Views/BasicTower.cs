using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BasicTower : Tower
{
    protected override void LogicUpdate(float deltaTime)
    {
        base.LogicUpdate(deltaTime);
        
    }

    private void Awake()
    {
        Init(new TowerStat(10, 10, 1));
    }
}
