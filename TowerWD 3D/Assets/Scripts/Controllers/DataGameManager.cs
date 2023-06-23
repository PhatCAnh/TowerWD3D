using CanasSource;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataGameManager : MonoBehaviour
{
    [SerializeField] private DataEnemyStat dataEnemy;
    [SerializeField] private DataTowerStat dataTower;

    private readonly Dictionary<string, GameObject> _dictPrefab = new(); 

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Singleton<DataGameManager>.Instance = this;
        LoadPrefab();
    }

    public GameObject GetPrefab(string id)
    {
        return _dictPrefab.GetValueOrDefault(id);
    }

    public EnemyStat LoadConfigEnemyStat(string id)
    {
        return dataEnemy.listData.FirstOrDefault(item => item.id.Equals(id));
    }

    public TowerStat LoadConfigTowerStat(string id, int level = 1)
    {
        var stat = dataTower.listData.FirstOrDefault(t => t.id.Equals(id))?.statLevelTower[level - 1];
        if (stat == null) return null;
        stat.id = id;
        stat.levelEvolution = level;
        return stat;

    }

    private void LoadPrefab()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");
        foreach (var item in prefabs)
        {
            _dictPrefab.Add(item.name, item);
        }
    }
}
