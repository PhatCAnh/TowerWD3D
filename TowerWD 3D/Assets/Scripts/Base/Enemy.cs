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
    Fly,
    Boss
}


public enum EnemyState
{
    Idle,
    Move,
    Skill,
    Hide,
    Die
}

public class EnemyStat
{
    public StatInt maxHP = new();
    public StatInt currentHP = new();
    public StatInt armor = new();
    public StatFloat moveSpeed = new();
    public StatInt coin = new();

    public EnemyStat(int _hp, int _armor, float _moveSpeed, int _coin)
    {
        maxHP.BaseValue = _hp;
        currentHP.BaseValue = _hp;
        armor.BaseValue = _armor;
        moveSpeed.BaseValue = _moveSpeed;
        coin.BaseValue = _coin;
    }
}

public abstract class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody theRB;
    public HealthBar healthBar { get ; protected set; }
    public Transform target { get ; protected set; }
    public EnemyState state {  get; protected set; }
    public EnemyModel model { get; protected set; }
    public EnemyStat stat { get; private set; }
    public bool isAlive => stat.currentHP.Value > 0;
    public bool isFullHp => stat.currentHP.Value >= stat.maxHP.Value;
    public Food isPicked { get; protected set; }
    public GameController gameController => Singleton<GameController>.Instance;

    public int pathPoint;
    protected int index;

    public bool isStop;
  
    public List<Effect> negativeEffect = new();
    public List<Effect> positiveEffect = new();

    #region Base Method

    private void Start()
    {
        anim = GetComponent<Animator>();
        theRB = GetComponent<Rigidbody>();
        /*HealthBar hb = Instantiate(Singleton<GameController>.Instance.PF_Healthbar);
        hb.Init(this);
        Init(0, new EnemyModel(350, 0, 1, 10), hb);*/
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

    public void Init(int pathPoint , EnemyModel model, HealthBar hpView)
    {
        this.pathPoint = pathPoint;
        this.model = model;
        this.healthBar = hpView;
        UpdateStat();
        gameController.enemies.Add(this);
    }

    private void UpdateStat()
    {
        stat = new EnemyStat(model.MaxHp, model.Armor, model.MoveSpeed, model.Coin);
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
        if (stat.currentHP.Value == 0)
        {
            Die();
        }

        SpawnEffectTakeDamage();
    }

    private int AddHp(int amount)
    {
        var remain = stat.currentHP.Value + amount;
        var newValue = Mathf.Clamp(remain, 0, stat.maxHP.Value);
        var oldValue = stat.currentHP.Value;
        stat.currentHP.BaseValue = newValue;
        return newValue - oldValue;
    }

    protected void Die()
    {
        PlayeSound(EnemyState.Die);
        state = EnemyState.Die;
        isPicked?.Droped();
        Singleton<GameController>.Instance.EnemyDie(this);
    }

    public void Stop()
    {
        isStop = true;
    }

    private void Moving(Transform mapPoint)
    {
        Vector3 _direction = mapPoint.position - transform.position;
        transform.Translate(stat.moveSpeed.Value * Time.deltaTime * _direction.normalized);
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
        if(isNegative)
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
