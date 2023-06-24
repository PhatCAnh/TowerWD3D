using UnityEngine;

namespace State
{
    public enum FoodState
    {
        Normal,
        Picked,
        Lost
    }
    
    public enum EnemyState
    {
        Idle,
        Move,
        Skill,
        Die
    }
}