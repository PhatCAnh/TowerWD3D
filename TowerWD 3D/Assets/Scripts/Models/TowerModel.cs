using System.Collections;
using System.Collections.Generic;

namespace  Models
{
    public class TowerModel : Model
    {
        public TowerModel(int atk, float attackRange, float attackSpeed, float projectileSpeed, float projectileCount)
        {
            this.Atk = atk;
            this.AtkRange = attackRange;
            this.AtkSpeed = attackSpeed;
            this.ProjectileSpeed = projectileSpeed;
            this.ProjectileCount = projectileCount;
        }

        private string _id;
        //private int _level;
        //private int _exp;
        private int _atk;
        private float _atkRange;
        private float _atkSpeed;
        private float _projectileSpeed;
        private float _projectileCount;
        /*public int Level
        {
            get => _level;
            set
            {
                if (_level == value) return;
                _level = value;
            }
        }
    
        public int Exp
        {
            get => _exp;
            set
            {
                if (_exp == value) return;
                _exp = value;
            }
        }*/

        public int Atk
        {
            get => _atk;
            set
            {
                if (_atk == value)
                    return;
                _atk = value;
                //DataChange(nameof(Atk));
            }
        }

        public float AtkRange
        {
            get => _atkRange;
            set
            {
                if (_atkRange == value)
                    return;
                _atkRange = value;
                //DataChange(nameof(AtkRange));
            }
        }

        public float AtkSpeed
        {
            get => _atkSpeed;
            set
            {
                if (_atkSpeed == value) return;
                _atkSpeed = value;
                //DataChange(nameof(AtkSpeed));
            }
        }

        public float ProjectileSpeed
        {
            get => _projectileSpeed;
            set
            {
                if (_projectileSpeed == value)
                    return;
                _projectileSpeed = value;
                //DataChange(nameof(ProjectileSpeed));
            }
        }

        public float ProjectileCount
        {
            get => _projectileCount;
            set
            {
                if (_projectileCount == value)
                    return;
                _projectileCount = value;
                //DataChange(nameof(ProjectileCount));
            }
        }
    }
}

