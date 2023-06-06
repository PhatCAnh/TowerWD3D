using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    protected override void LogicUpdate(float deltaTime)
    {
        base.LogicUpdate(deltaTime);
        if (!isAlive) return;
        switch(state)
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

    private void UpdateIdle(float deltaTime)
    {
        state = EnemyState.Move;
    }

    private void UpdateMove(float deltaTime)
    {
        var mapPoint = gameController.GetMapPoint(pathPoint, index);
        if (mapPoint == null)
        {
            theRB.velocity = Vector2.zero;
            return;
        }
        if (!isPicked)
        {
            if (target != null)
            {
                mapPoint = target;
                if (Vector2.Distance(target.position, transform.position) < 0.1f)
                {
                    isPicked = true;
                    target.transform.SetParent(transform);
                    index--;
                    return;
                }
            }

        }
        Moving(mapPoint);
        if (Vector2.Distance(mapPoint.position, transform.position) < 0.1f)
        {
            _ = !isPicked ? index++ : index--;
        }
    }

    private void Moving(Transform mapPoint)
    {
        Vector3 _direction = mapPoint.position - transform.position;
        theRB.velocity = new Vector2(_direction.x, _direction.y).normalized * stat.moveSpeed.Value;
    }

    private void UpdateSkill(float deltaTime)
    {

    }
}
;