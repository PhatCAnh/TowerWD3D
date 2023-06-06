using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

/*public enum EnemyType
{
    Tank,Archer,Melee,Wizard,Speeder
}*/

public class Enemy_Nguyen : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] float hp;
    [SerializeField] public float moveSpeed;
    [SerializeField] float rangeAttack;
    [SerializeField] float armor;
    [SerializeField] bool haveSpell;
    [SerializeField] bool collectedFood;
    [SerializeField] EnemyType enemyType;
    
    
    public delegate void EnemyTypeDelegate();
}
