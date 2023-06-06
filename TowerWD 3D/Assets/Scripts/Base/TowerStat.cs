using CanasSource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStat
{
    public StatInt level = new();
    public StatInt exp = new();
    public StatInt atk = new();
    public StatFloat atkRange = new();
    public StatFloat atkSpeed = new();
    public StatFloat ProjectileSpeed = new();
    public StatFloat ProjectileCount = new();

    public TowerStat(int _atk, float _atkRange, float _atkSpeed, float _PS, float _PC)
    {
        level.BaseValue = 1;
        exp.BaseValue = 0;
        atk.BaseValue = _atk;
        atkRange.BaseValue = _atkRange;
        atkSpeed.BaseValue = _atkSpeed;
        ProjectileSpeed.BaseValue = _PS;
        ProjectileCount.BaseValue = _PC;
    }

    public TowerStat(int _atk, float _atkRange, float _atkSpeed)
    {
        level.BaseValue = 1;
        exp.BaseValue = 0;
        atk.BaseValue = _atk;
        atkRange.BaseValue = _atkRange;
        atkSpeed.BaseValue = _atkSpeed;
        ProjectileSpeed.BaseValue = 5f;
        ProjectileCount.BaseValue = 1f;
    }
}
