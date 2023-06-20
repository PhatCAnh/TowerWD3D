using CanasSource;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class MapPoint
{
    public List<Transform> point = new();
}
public class GameController : MonoBehaviour
{
    [SerializeField] private DataEnemyStat dataEnemy;
    [SerializeField] private DataTowerStat dataTower;

    private readonly Dictionary<string, Enemy> _enemyDict = new();
    private readonly Dictionary<string, Tower> _towerDic = new();
    private readonly Dictionary<string, Bullet> _bulletDic = new();

    public List<MapPoint> mapPoints = new(); //will delete
    public List<Enemy> enemies = new();

    public GameObject Parent_HealthBar;
    public Transform Parent_Bullet;
    public HealthBar PF_Healthbar;

    public AllWave wave = new();

    private void Awake()
    {
        Singleton<GameController>.Instance = this;
        LoadPrefabsEnemy();
        LoadPrefabsTower();
        LoadPrefabsBullet();
    }

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

    public TowerStat LoadConfigTowerStat(string id)
    {
        return dataTower.listData.FirstOrDefault(t => t.id.Equals(id));
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
        Enemy enemy = Instantiate(GetPrefabEnemy(id), GetStartPoint(pathPoint).position, Quaternion.identity);
        HealthBar hb = Instantiate(PF_Healthbar);
        enemy.Init(pathPoint, LoadConfigEnemyStat(id), hb);
        hb.Init(enemy);
        return enemy;
    }

    public Tower CreateTower(string id, Node node, Material material)
    {
        Tower tower = Instantiate(GetPrefabTower(id), node.tower.position, Quaternion.identity, node.tower);
        var skin = tower.GetComponent<AnimationModelTower>();
        skin._bottomBody.GetComponent<MeshRenderer>().material = material;
        skin._midleBody.GetComponent<MeshRenderer>().material = material;
        skin.topBody.GetComponent<MeshRenderer>().material = material;
        tower.Init(LoadConfigTowerStat(id));
        return tower;
    }

    public Bullet CreateBullet(Tower attacker, Enemy victim)
    {
        Bullet bullet = Instantiate(GetPrefabBullet(attacker.stat.id), attacker.firePointPos.position, Quaternion.identity, Parent_Bullet);
        bullet.Init(attacker, victim);
        return bullet;
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

    private Enemy GetPrefabEnemy(string id)
    {
        return _enemyDict.GetValueOrDefault(id);
    }

    private Tower GetPrefabTower(string id)
    {
        return _towerDic.GetValueOrDefault(id);
    }

    private Bullet GetPrefabBullet(string nameOfTower)
    {
        return _bulletDic.GetValueOrDefault("Bullet_" + nameOfTower);
    }
}
