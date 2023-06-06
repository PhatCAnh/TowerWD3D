using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BasicTower : Tower
{
    protected override void LogicUpdate(float deltaTime)
    {
        base.LogicUpdate(deltaTime);
        attackCooldown.Update(deltaTime);

        switch (state)
        {
            case TowerState.Idle:
                UpdateIdle();
                break;
            case TowerState.Attack:
                UpdateAttack(deltaTime);
                break;
        }
    }

    private void Awake()
    {
        Init(new TowerStat(10, 10, 1));
    }

    private void UpdateIdle()
    {
        if (target == null)
        {
            switch (typeTarget)
            {
                case TypeTargetTower.First:
                    target = GetFirstEnemy();
                    break;
                case TypeTargetTower.Last:
                    target = GetLastEnemy();
                    break;
                case TypeTargetTower.Strongest:
                    target = GetStrongestEnemy();
                    break;
                case TypeTargetTower.Weakest:
                    target = GetWeakestEnemy();
                    break;
                case TypeTargetTower.Random:
                    target = GetRandomEnemy();
                    break;
            }
        }
        if (target != null)
        {
            state = TowerState.Attack;
        }
    }

    

    private void UpdateAttack(float deltaTime)
    {
        if (target == null)
        {
            state = TowerState.Idle;
            return;
        }

        var distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > stat.atkRange.Value)
        {
            state = TowerState.Idle;
            target = null;
            return;
        }

        if (attackCooldown.isFinished)
        {
            attackCooldown.Restart(stat.atkSpeed.Value);
            Attack();
        }

        
    }

    private void Attack()
    {
        SpawnBullet();
    }

    public Bullet SpawnBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePointPos.position, Quaternion.identity, firePointPos);
        bullet.Init(this, new BulletStat(stat.atk.Value, stat.ProjectileSpeed.Value), target);
        return bullet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            listEnemy.Add(collision.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            listEnemy.Remove(collision.GetComponent<Enemy>());
        }
    }
}
