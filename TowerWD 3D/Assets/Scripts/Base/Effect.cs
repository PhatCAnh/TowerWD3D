using CanasSource;

public enum EffectType
{
    Fire,
    Ice,
    Stun,
    Poison
}

public abstract class Effect
{
    public string nameEffect;
    public Enemy owner;
    public EffectType type;
    public Cooldown coolDownTime = new();

    public abstract void Interact(float deltaTime);
    public Effect(string _nameEffect, Enemy _owner, EffectType _type, float _timeCooldown)
    {
        nameEffect = _nameEffect;
        owner = _owner;
        type = _type;
        coolDownTime.Restart(_timeCooldown);
    }
}

public class PoisonEffect : Effect
{
    public int dmg;
    public Cooldown coolDownInteract = new();

    public PoisonEffect(string _nameEffect, Enemy _owner, EffectType _type, float _timeCooldown, int _dmg) : base(_nameEffect, _owner, _type, _timeCooldown)
    {
        dmg = _dmg;
    }

    public override void Interact(float deltaTime)
    {
        coolDownTime.Update(deltaTime);
        coolDownInteract.Update(deltaTime);
        if(coolDownInteract.isFinished)
        {
            coolDownInteract.Restart(1);
            owner.TakeDamage(dmg);
        }
    }
}
