using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BasicTower : Tower
{

    private void Awake()
    {
        Init(new TowerStat(10, 10, 1));
    }
}
