using CanasSource;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Models;

public enum TowerState
{
    RunningAnim,
    Idle,
    Attack,}

public enum TypeTargetTower
{
    First,
    Last,
    Strongest,
    Weakest,
    Random
}

[CreateAssetMenu(fileName = "DataStatTower", menuName = "GameConfiguration/TowerStat")]
public class DataTowerStat : ScriptableObject
{
    public List<TowerData> listData;
}

public abstract class Tower : MonoBehaviour
{
    protected SphereCollider theCC;
    public TowerState state { get; set; }
    public TowerModel model { get; protected set; }
    public TypeTargetTower typeTarget;
    public EnemyType[] typeEnemyTarget;
    //public TowerModel model { get; private set; }
    public TowerStat stat { get; private set; }
    public Enemy target { get; protected set; }
    public Cooldown attackCooldown { get; protected set; } = new();
    private bool isStop;
    [SerializeField] protected List<Enemy> listEnemy = new();

    public Transform firePointPos;
    public void Init(TowerStat _stat)
    {
        //model = _model;
        stat = _stat;
        UpdateModel();
        theCC = GetComponent<SphereCollider>();
        theCC.radius = model.AtkRange;
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

    public bool CheckTypeEnemy(EnemyType enemy)
    {
        return typeEnemyTarget.Contains(enemy);
    }

    protected virtual void LogicUpdate(float deltaTime)
    {
        attackCooldown.Update(deltaTime);

        switch (SetTowerState())
        {
            case TowerState.Idle:
                UpdateIdle();
                break;
            case TowerState.Attack:
                UpdateAttack();
                break;
        }
    }

    protected virtual void PhysicUpdate(float deltaTime)
    {

    }

    protected virtual TowerState SetTowerState()
    {
        if(state == TowerState.RunningAnim) return state;
        if (target != null && CheckTypeEnemy(target.type)) return TowerState.Attack;
        return TowerState.Idle;
    }

    protected virtual void UpdateAttack()
    {
        if (attackCooldown.isFinished)
        {
            attackCooldown.Restart(1 / model.AtkSpeed);
            Attack();
        }
    }

    protected virtual void UpdateIdle()
    {
        
        switch (typeTarget)
        {
            case TypeTargetTower.First:
                target = GetFirstEnemy();
                break;
            case TypeTargetTower.Last:
                target = GetLastEnemy();
                break;
            case TypeTargetTower.Strongest:
                target = GetStrongestEnemy();
                break;
            case TypeTargetTower.Weakest:
                target = GetWeakestEnemy();
                break;
            case TypeTargetTower.Random:
                target = GetRandomEnemy();
                break;
        }
        listEnemy.RemoveAll(enemy => !enemy.isAlive);
    }

    protected virtual void Attack()
    {
        Singleton<InGameController>.Instance.CreateBullet(this, target);
    }

    //protected abstract void SetTypeEnemyTarget();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy) && CheckTypeEnemy(enemy.type))
        {
            listEnemy.Add(enemy);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy) && listEnemy.Contains(enemy))
        {
            listEnemy.Remove(enemy);
            target = null;
        }
    }

    protected Enemy GetFirstEnemy()
    {
        return listEnemy.FirstOrDefault(enemy => enemy.isAlive);
    }

    protected Enemy GetLastEnemy()
    {
        return listEnemy.LastOrDefault(enemy => enemy.isAlive);
    }

    protected Enemy GetStrongestEnemy()
    {
        return listEnemy.FirstOrDefault(enemy => enemy.model.CurrentHp == listEnemy.Max(e => e.model.CurrentHp));
    }

    protected Enemy GetWeakestEnemy()
    {
        return listEnemy.FirstOrDefault(enemy => enemy.model.CurrentHp == listEnemy.Min(e => e.model.CurrentHp));
    }

    protected Enemy GetRandomEnemy()
    {
        var list = listEnemy.Where(enemy => enemy.isAlive).ToList();
        return list[Random.Range(0, list.Count())];
    }

    private void UpdateModel()
    {
        model = new TowerModel(
            stat.atk,
            stat.atkRange,
            stat.atkSpeed,
            stat.ProjectileSpeed,
            stat.ProjectileCount
            );
    }
}
