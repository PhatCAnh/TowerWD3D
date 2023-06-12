using CanasSource;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MapPoint
{
    public List<Transform> point = new();
}
public class GameController : MonoBehaviour
{
    private void Awake()
    {
        Singleton<GameController>.Instance = this;
    }

    [SerializeField] private DataEnemyStat dataEnemy;
    [SerializeField] private DataEnemyPrefab enemyPrefab;

    public List<MapPoint> mapPoints = new(); //will delete
    public List<Enemy> enemies = new();

    public GameObject Parent_HealthBar;
    public HealthBar PF_Healthbar;

    public AllWave wave = new();

    private void Start()
    {
        wave.state = WaveState.Start;
    }

    private void Update()
    {
        wave.LogicUpdate(Time.deltaTime);
    }
    public EnemyStat LoadConfigEnemyStat(string id)
    {
        return dataEnemy.listData.FirstOrDefault(item => item.id.Equals(id));
    }

    public EnemyPrefab LoadPrefabEnemy(string id)
    {
        return enemyPrefab.listPrefab.FirstOrDefault(item => item.id.Equals(id));
    }

    public void EnemyDie(Enemy enemy, bool isDestroyObject = true)
    {
        enemies.Remove(enemy);
        if (isDestroyObject) Destroy(enemy.gameObject);
    }

    public Transform GetMapPoint(int pathPoint, int index) //will delete
    {
        if (mapPoints[pathPoint].point.Count > index && index >= 0)
        {
            return mapPoints[pathPoint].point[index];

        }
        return null;
    }

    public Transform GetStartPoint(int pathPoint)
    {
        return mapPoints[pathPoint].point[0];
    }

    public void StartGame()
    {

    }

    public void WinGame()
    {

    }
        
    public void LoseGame()
    {

    }

    public void NextWave()
    {

    }

    public Enemy CreateEnemy(string id, int pathPoint)
    {
        Enemy enemy = Instantiate(LoadPrefabEnemy(id).prefab, GetStartPoint(pathPoint).position, Quaternion.identity);
        HealthBar hb = Instantiate(PF_Healthbar);
        enemy.Init(pathPoint, LoadConfigEnemyStat(id), hb);
        hb.Init(enemy);
        return enemy;
    }
}
    