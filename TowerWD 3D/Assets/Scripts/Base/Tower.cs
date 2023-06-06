using CanasSource;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TowerState
{
    Idle,
    Attack
}

public enum TypeTargetTower
{
    First,
    Last,
    Strongest,
    Weakest,
    Random
}

public abstract class Tower : MonoBehaviour
{
    protected CircleCollider2D theCC;
    public TowerState state { get; protected set; }
    public TypeTargetTower typeTarget;
    //public TowerModel model { get; private set; }
    public TowerStat stat { get; private set; }
    public Enemy target { get; protected set; }
    public Cooldown attackCooldown { get; protected set; } = new();
    private bool isStop;
    public List<Enemy> listEnemy = new();


    [SerializeField] protected Transform firePointPos;
    [SerializeField] protected Bullet bulletPrefab;
    public void Init(TowerStat _stat)
    {
        //model = _model;
        stat = _stat;
        theCC = GetComponent<CircleCollider2D>();
        theCC.radius = stat.atkRange.Value;
    }

    private void Update()
    {
        if (isStop) return;
        LogicUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (isStop) return;
        LogicUpdate(Time.fixedDeltaTime);
    }

    protected virtual void LogicUpdate(float deltaTime)
    {

    }

    protected virtual void PhysicUpdate(float deltaTime)
    {

    }

    protected Enemy GetFirstEnemy()
    {
        if (listEnemy.Count != 0)
        {
            return listEnemy.Where(enemy => enemy != null).First();
        }
        return null;
    }

    protected Enemy GetLastEnemy()
    {
        if (listEnemy.Count != 0)
        {
            return listEnemy.Where(enemy => enemy != null).Last();
        }
        return null;
    }

    protected Enemy GetStrongestEnemy()
    {
        {
            if (listEnemy.Count != 0)
            {
                var max = listEnemy.Max(e => e.stat.currentHP.Value);
                return listEnemy.Where(e => e.stat.currentHP.Value == max).First();
            }
            return null;
        }
    }

    protected Enemy GetWeakestEnemy()
    {
        if (listEnemy.Count != 0)
        {
            var min = listEnemy.Min(e => e.stat.currentHP.Value);
            return listEnemy.Where(e => e.stat.currentHP.Value == min).First();
        }
        return null;
    }

    protected Enemy GetRandomEnemy()
    {
        var randomIndex = Random.Range(0, listEnemy.Count);
        if (listEnemy[randomIndex]!= null)
        {
            return (Enemy)listEnemy[randomIndex];
        }
        return null;
    }
}
