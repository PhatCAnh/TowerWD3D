using UnityEngine;

namespace State
{
    public enum FoodState
    {
        InBox,
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