using CanasSource;

namespace Models
{
    public class EnemyModel : Model
    {
        private EnemyType _enemyType;
        private int _level;
        private int _maxHp;
        private int _currentHp;
        private int _armor;
        private float _moveSpeed;
        private int _coin;

        public EnemyModel(EnemyType enemyType, int hp, int armor, float moveSpeed, int coin) : base()
        {
            _enemyType = enemyType;
            _maxHp = hp;
            _currentHp = hp;
            _armor = armor;
            _moveSpeed = moveSpeed;
            _coin = coin;
        }

        public EnemyModel(int hp, int armor, float moveSpeed, int coin)
        {
            _enemyType = EnemyType.Normal;
            _maxHp = hp;
            _currentHp = hp;
            _armor = armor;
            _moveSpeed = moveSpeed;
            _coin = coin;
        }


        public EnemyType EnemyType
        {
            get => _enemyType;
            set
            {
                if (_enemyType != value)
                {
                    _enemyType = value;
                    //DataChange(nameof(EnemyType));
                }
            }
        }

        // public int Level
        // {
        //     get => _level;
        //     set
        //     {
        //         if (_level != value)
        //         {
        //             _level = value;
                        //DataChange(nameof(Level));
        //         }
        //     }
        // }

        public int MaxHp
        {
            get => _maxHp;
            set
            {
                if (_maxHp.Equals(value)) return;
                _maxHp = value;
                //DataChange(nameof(MaxHp));
            }
        }

        public int CurrentHp
        {
            get => _currentHp;
            set
            {
                if (_currentHp == value) return;
                _currentHp = value;
                DataChange(nameof(CurrentHp));
            }
        }

        // public int Armor
        // {
        //     get => _armor;
        //     set
        //     {
        //         // ReSharper disable once RedundantCheckBeforeAssignment
        //         if (_armor == value) return;
        //         _armor = value;
        //         //DataChange(nameof(Armor));
        //     }
        // }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                if (_moveSpeed.Equals(value)) return;
                _moveSpeed = value;
                //DataChange(nameof(MoveSpeed));
            }
        }

        public int Coin
        {
            get => _coin;
            set
            {
                if (_coin.Equals(value)) return;
                _coin = value;
                //DataChange(nameof(Coin));
            }
        }
    }
}