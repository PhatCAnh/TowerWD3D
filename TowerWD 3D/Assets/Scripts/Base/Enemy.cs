using CanasSource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyState
{
    Idle,
    Move,
    Skill,
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
    protected Rigidbody2D theRB;
    public HealthBar healthBar { get ; protected set; }
    public Transform target { get ; protected set; }
    public EnemyState state {  get; protected set; }
    public EnemyModel model { get; protected set; }
    public EnemyStat stat { get; private set; }
    public bool isAlive => stat.currentHP.Value > 0;
    public bool isFullHp => stat.currentHP.Value >= stat.maxHP.Value;
    public GameController gameController => Singleton<GameController>.Instance;

    public int pathPoint;
    protected int index;

    public bool isStop;
  
    protected bool isPicked;

    public List<Effect> negativeEffect = new();
    public List<Effect> positiveEffect = new();

    #region Base Method

    private void Start()
    {
        anim = GetComponent<Animator>();
        theRB = GetComponent<Rigidbody2D>();
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
    }

    private void UpdateStat()
    {
        stat = new EnemyStat(model.MaxHp, model.Armor, model.MoveSpeed, model.Coin);
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
        //anim?.SetTrigger("Die");
        PlayeSound(EnemyState.Die);
        state = EnemyState.Die;
        Singleton<GameController>.Instance.EnemyDie(this);
    }

    public void Stop()
    {
        isStop = true;
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Food food) && !food.isPicked)
        {
            food.isPicked = true;
            target = collision.transform;
            //collision.transform.SetParent(transform);
        }
    }
}
