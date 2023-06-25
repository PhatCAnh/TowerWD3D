using CanasSource;
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
    protected InGameController inGameController => Singleton<InGameController>.Instance;

    [SerializeField] protected BulletDirectionType bulletDirectionType;

    public void Init(Tower owner, Enemy target)
    {
        this.owner = owner;
        this.target = target;
        stat = new BulletStat(owner.model.Atk, owner.model.ProjectileSpeed);
    }

    private void Update()   
    {
        LogicUpdate(inGameController.gameSpeed);
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
