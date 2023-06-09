using CanasSource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBullet : Bullet
{
    private Cooldown despawnCooldown = new Cooldown();
    private Vector3 targetPos;

    public override void LogicUpdate(float deltaTime)
    {
        switch (state)
        {
            case BulletState.idle:
                {
                    UpdateIdle(deltaTime);
                    break;
                }
            case BulletState.move:
                {
                    UpdateMove(deltaTime);
                    break;
                }
            case BulletState.explose:
                {
                    UpdateExplose(deltaTime);
                    break;
                }
            case BulletState.despawn:
                {
                    UpdateDespawn(deltaTime);
                    break;
                }
        }
    }

    private void UpdateIdle(float deltaTime)
    {
        state = BulletState.move;
    }

    private void UpdateMove(float deltaTime)
    {
        if (target != null)
        {
            targetPos = target.transform.position;
        }
        Vector3 direction = (targetPos - transform.position).normalized;
        transform.Translate(stat.moveSpeed.Value * deltaTime * direction, Space.World);

        if (bulletDirectionType == BulletDirectionType.lookAt)
        {
            Quaternion rotGoal = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, 1f);
        }

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            state = BulletState.explose;
        }
    }

    private void UpdateExplose(float deltaTime)
    {
        ExploseNormal();
        SpawnExplose(transform.position);
        despawnCooldown.Restart(0);
        state = BulletState.despawn;
    }
    private void UpdateDespawn(float deltaTime)
    {
        despawnCooldown.Update(deltaTime);
        if (!despawnCooldown.isFinished)
            return;

        Destroy(gameObject);
    }

    public void SpawnExplose(Vector3 position)
    {
        
    }

    private void ExploseNormal()
    {
        inGameController.TakeDamage(target, stat.atk.Value);
    }
}
