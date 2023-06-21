using CanasSource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public enum EnemyType
{
    Normal,
    Hide,
    Fly,
    Boss
}


public enum EnemyState
{
    Idle,
    Move,
    Skill,
    Die
}

[CreateAssetMenu(fileName = "DataStatEnemies", menuName = "GameConfiguration/EnemyStat")]
public class DataEnemyStat : ScriptableObject
{
    public List<EnemyStat> listData;
}

[Serializable]
public class EnemyPrefab
{
    public string id;
    public Enemy prefab;
}

[Serializable]
public class EnemyStat
{
    public string id;
    public int HealthPoint;
    public int armor;
    public float moveSpeed;
    public int coin;

}
public abstract class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody theRB;
    public HealthBar healthBar { get; protected set; }
    public Transform target { get; protected set; }
    public EnemyState state { get; protected set; }
    public EnemyModel model { get; protected set; }
    public EnemyStat stat { get; private set; }
    public EnemyType type;
    public bool isAlive => model.CurrentHp > 0;
    public bool isFullHp => model.CurrentHp >= model.MaxHp;
    public Food isPicked { get; protected set; }
    public InGameController gameController => Singleton<InGameController>.Instance;

    public int pathPoint;
    protected int index;

    public bool isStop;

    public Transform healthBarPos;

    public List<Effect> negativeEffect = new();
    public List<Effect> positiveEffect = new();

    #region Base Method

    private void Start()
    {
        anim = GetComponent<Animator>();
        theRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isStop) return;
        LogicUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (isStop) return;
        PhysicUpdate(Time.deltaTime);
    }

    protected virtual void LogicUpdate(float deltaTime)
    {
        UpdateEffect(deltaTime);
        if (!isAlive) return;
        switch (state)
        {
            case EnemyState.Idle:
                UpdateIdle(deltaTime);
                break;
            case EnemyState.Move:
                UpdateMove(deltaTime);
                break;
            case EnemyState.Skill:
                UpdateSkill(deltaTime);
                break;
        }
    }

    protected virtual void PhysicUpdate(float deltaTime)
    {

    }
    #endregion

    public void Init(int pathPoint, EnemyStat stat, HealthBar hpView)
    {
        this.pathPoint = pathPoint;
        this.healthBar = hpView;
        this.stat = stat;
        UpdateModel();
        gameController.enemies.Add(this);
    }

    private void UpdateModel()
    {
        model = new EnemyModel(stat.HealthPoint, stat.armor, stat.moveSpeed, stat.coin);
    }

    private void UpdateIdle(float deltaTime)
    {
        target = gameController.GetMapPoint(pathPoint, index);
        if (!target) return;
        state = EnemyState.Move;
    }

    private void UpdateMove(float deltaTime)
    {
        Moving(target);
        if (Vector3.Distance(target.position, transform.position) < 0.1f)
        {
            if (target.TryGetComponent(out Food food))
            {
                isPicked = food;
                isPicked.Picked(transform);
            }
            _ = !isPicked ? index++ : index--;
            state = EnemyState.Idle;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!isAlive) return;
        var lossHP = AddHp(-dmg);
        healthBar.HpChanged();
        if (model.CurrentHp == 0)
        {
            Die();
        }

        SpawnEffectTakeDamage();
    }

    private int AddHp(int amount)
    {
        var remain = model.CurrentHp + amount;
        var newValue = Mathf.Clamp(remain, 0, model.CurrentHp);
        var oldValue = model.CurrentHp;
        model.CurrentHp = newValue;
        return newValue - oldValue;
    }

    protected void Die()
    {
        PlayeSound(EnemyState.Die);
        state = EnemyState.Die;
        isPicked?.Droped();
        Singleton<InGameController>.Instance.EnemyDie(this);
    }

    public void Stop()
    {
        isStop = true;
    }

    private void Moving(Transform mapPoint)
    {
        Vector3 _direction = mapPoint.position - transform.position;
        transform.Translate(model.MoveSpeed * Time.deltaTime * _direction.normalized);
    }

    private void UpdateSkill(float deltaTime)
    {

    }

    protected void SpawnEffectTakeDamage()
    {

    }

    protected void PlayeSound(EnemyState enemyState)
    {

    }

    public void AddEffect(bool isNegative, Effect effect)
    {
        if (isNegative)
        {
            negativeEffect.Add(effect);
        }
        else
        {
            positiveEffect.Add(effect);
        }
    }

    private void UpdateEffect(float deltaTime)
    {
        for (int i = 0; i < positiveEffect.Count; i++)
        {
            positiveEffect[i].Interact(deltaTime);
            if (positiveEffect[i].coolDownTime.isFinished)
            {
                positiveEffect.Remove(positiveEffect[i]);
            }
        }

        for (int i = 0; i < negativeEffect.Count; i++)
        {
            negativeEffect[i].Interact(deltaTime);
            if (negativeEffect[i].coolDownTime.isFinished)
            {
                negativeEffect.Remove(negativeEffect[i]);
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Food food) && !food.isPicked)
        {
            food.isPicked = true;
            target = food.transform;
        }
    }
}
