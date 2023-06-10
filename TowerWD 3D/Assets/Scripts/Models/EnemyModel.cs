using CanasSource;
using UnityEngine;


public class EnemyModel
{
    private string _id;
    private EnemyType _enemyType;
    private int _level;
    private int _maxHp;
    private int _currentHp;
    private int _armor;
    private float _moveSpeed;
    private int _coin;

    public EnemyModel(string id, EnemyType enemyType, int hp, int armor, float moveSpeed, int coin)
    {
        _id = id;
        _enemyType = enemyType;
        _level = 1;
        _maxHp = hp;
        _currentHp = hp;
        _armor = armor;
        _moveSpeed = moveSpeed;
        _coin = coin;
    }

    public EnemyModel(EnemyType enemyType, int hp, int armor, float moveSpeed, int coin)
    {
        _id = Singleton<GameController>.Instance.SetIdForEnemy();
        _enemyType = enemyType;
        _maxHp = hp;
        _currentHp = hp;
        _armor = armor;
        _moveSpeed = moveSpeed;
        _coin = coin;
    }

    public EnemyModel(int hp, int armor, float moveSpeed, int coin)
    {
        _id = Singleton<GameController>.Instance.SetIdForEnemy();
        _enemyType = EnemyType.Normal;
        _maxHp = hp;
        _currentHp = hp;
        _armor = armor;
        _moveSpeed = moveSpeed;
        _coin = coin;
    }

    public string Id => _id;
    public EnemyType EnemyType => _enemyType;
    public int Level
    {
        get => _level;
        set
        {
            if (_level != value)
            {
                _level = value;
            }
        }
    }
    public int MaxHp
    {
        get => _maxHp;
        set
        {
            if (_maxHp != value)
            {
                _maxHp = value;
            }
        }
    }
    public int CurrentHp
    {
        get => _currentHp;
        set
        {
            if (_currentHp != value)
            {
                _currentHp = value;
            }
        }
    }
    public int Armor
    {
        get => _armor;
        set
        {
            if (_armor != value)
            {
                _armor = value;
            }
        }
    }
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            if (_moveSpeed != value)
            {
                _moveSpeed = value;
            }
        }
    }
    public int Coin
    {
        get => _coin;
        set
        {
            if (_coin != value)
            {
                _coin = value;
            }
        }
    }
}
