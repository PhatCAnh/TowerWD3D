using CanasSource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletState
{
    idle,
    move,
    explose,
    despawn,
}

public enum BulletType
{
    parabola,
    straight,
    target
}

public enum BulletDirectionType
{
    lookAt,
    rotate,
}

public enum BulletExploseType
{
    single,
    aoe,
}

public class BulletStat
{
    public StatInt atk = new StatInt();
    public StatFloat moveSpeed = new StatFloat();

    public BulletStat(int atk, float moveSpeed)
    {
        this.atk.BaseValue = atk;
        this.moveSpeed.BaseValue = moveSpeed;
    }
}

public abstract class Bullet : MonoBehaviour
{
    public Tower owner { get; private set; }
    public Enemy target { get; private set; }
    public BulletStat stat { get; private set; }
    public BulletState state { get; protected set; }

    [SerializeField] protected BulletDirectionType bulletDirectionType;

    public void Init(Tower _owner, BulletStat _stat, Enemy _target)
    {
        owner = _owner;
        target = _target;
        stat = _stat;
    }

    private void Update()
    {
        LogicUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        PhysicUpdate(Time.fixedDeltaTime);
    }

    public virtual void LogicUpdate(float deltaTime)
    {

    }

    public virtual void PhysicUpdate(float deltaTime)
    {

    }
}
