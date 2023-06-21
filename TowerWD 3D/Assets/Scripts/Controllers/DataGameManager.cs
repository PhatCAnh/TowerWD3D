using CanasSource;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataGameManager : MonoBehaviour
{
    [SerializeField] private DataEnemyStat dataEnemy;
    [SerializeField] private DataTowerStat dataTower;

    private readonly Dictionary<string, Enemy> _enemyDict = new();
    private readonly Dictionary<string, Tower> _towerDic = new();
    private readonly Dictionary<string, Bullet> _bulletDic = new();

    public HealthBar PF_Healthbar;

    private void Awake()
    {
        Singleton<DataGameManager>.Instance = this;
        LoadPrefabsEnemy();
        LoadPrefabsTower();
        LoadPrefabsBullet();
    }

    public Enemy GetPrefabEnemy(string id)
    {
        return _enemyDict.GetValueOrDefault(id);
    }

    public Tower GetPrefabTower(string id)
    {
        return _towerDic.GetValueOrDefault(id);
    }

    public Bullet GetPrefabBullet(string nameOfTower)
    {
        return _bulletDic.GetValueOrDefault("Bullet_" + nameOfTower);
    }

    public EnemyStat LoadConfigEnemyStat(string id)
    {
        return dataEnemy.listData.FirstOrDefault(item => item.id.Equals(id));
    }

    public TowerStat LoadConfigTowerStat(string id, int level = 1)
    {
        var stat = dataTower.listData.FirstOrDefault(t => t.id.Equals(id)).statLevelTower[level - 1];
        stat.id = id;
        stat.levelEvolution = level;
        return stat;
    }

    private void LoadPrefabsEnemy()
    {
        Enemy[] prefabsEnemy = Resources.LoadAll<Enemy>("Prefabs/Enemies");
        foreach (var prefab in prefabsEnemy)
        {
            _enemyDict.Add(prefab.name, prefab);
        }
    }

    private void LoadPrefabsTower()
    {
        Tower[] prefabsTower = Resources.LoadAll<Tower>("Prefabs/Towers");
        foreach (var prefab in prefabsTower)
        {
            _towerDic.Add(prefab.name, prefab);
        }
    }

    private void LoadPrefabsBullet()
    {
        Bullet[] prefabsBullet = Resources.LoadAll<Bullet>("Prefabs/Bullets");
        foreach (var prefab in prefabsBullet)
        {
            _bulletDic.Add(prefab.name, prefab);
        }
    }
}
