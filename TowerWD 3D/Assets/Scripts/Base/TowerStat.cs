using CanasSource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TowerStat
{
    [HideInInspector] public string id;
    public int atk;
    public float atkRange;
    public float atkSpeed;
    public float ProjectileSpeed;
    public float ProjectileCount;

    [HideInInspector] public int levelEvolution;
    public int coinToEvolution;
}
[Serializable]
public class TowerData
{
    public string id;
    public TowerStat[] statLevelTower = new TowerStat[3];
}
