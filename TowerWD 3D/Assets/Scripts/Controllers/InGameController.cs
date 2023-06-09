using CanasSource;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using State;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class MapPoint
{
    public List<Transform> point = new();
}

public class InGameController : MonoBehaviour
{
    private DataGameManager _dataGame => Singleton<DataGameManager>.Instance;

    private int _totalCoin;
    private int _countEnemy = 0;
    private int _countTower = 0;
    private float speedOfGame = 1f;
    
    public List<MapPoint> mapPoints = new(); //will delete
    public List<Enemy> listEnemy = new();
    public List<Food> foods = new();
    public List<Tower> listTower = new();

    public GameObject Parent_HealthBar;
    public Transform Parent_Bullet;
    public Transform Parent_Enemy;
    public BoxFood Parent_Food;

    public float gameSpeed => speedOfGame * Time.deltaTime;
    
    public AllWave wave = new();

    private bool isStop;

    public int Coin
    {
        get => _totalCoin;
        set
        {
            _totalCoin += value;
            changeCoinEvent.Invoke(_totalCoin);
        }
    }

    [HideInInspector] public UnityEvent<int> changeCoinEvent = new();

    private void Awake()
    {
        Singleton<InGameController>.Instance = this;
    }

    private void Start()
    {
        wave.state = WaveState.Start;
    }

    private void Update()
    {
        wave.LogicUpdate(gameSpeed);
    }

    

    public bool UpLevelTower(Tower tower)
    {
        TowerStat towerStat = _dataGame.LoadConfigTowerStat(tower.stat.id, tower.stat.levelEvolution + 1);
        if (Coin >= towerStat.coinToEvolution)
        {
            tower.Init(towerStat);
            Coin = -towerStat.coinToEvolution;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(Enemy victim, int damage)
    {
        if (victim != null && victim.isAlive)
        {
            victim.TakeDamage(damage);
        }
    }


    public void EnemyDie(Enemy enemy, bool isDestroyObject = true)
    {
        listEnemy.Remove(enemy);
        Coin = enemy.model.Coin;
        CheckWinGame();
        if (isDestroyObject) Destroy(enemy.gameObject);
    }

    public void DestroyTower(Tower tower)
    {
        listTower.Remove(tower);
        //give coin
        Destroy(tower.gameObject);
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
        Debug.Log("WinGame");
    }

    public void LoseGame()
    {
        Debug.Log("LoseGame");
    }

    public void NextWave()
    {
    }

    public void StopAndResumeGame()
    {
        if (isStop)
        {
            Time.timeScale = 1;
            isStop = false;
        }
        else
        {
            Time.timeScale = 0;
            isStop = true;
        }
    }
    

    public Enemy CreateEnemy(string id, int pathPoint)
    {
        var enemy = Instantiate(_dataGame.GetPrefab(id), GetStartPoint(pathPoint).position, Quaternion.identity,
            Parent_Enemy).GetComponent<Enemy>();
        var hb = Instantiate(_dataGame.GetPrefab("HealthBar"), Parent_HealthBar.transform).GetComponent<HealthBar>();
        enemy.Init(pathPoint, _dataGame.LoadConfigEnemyStat(id), hb);
        hb.Init(enemy);
        listEnemy.Add(enemy);
        return enemy;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public Tower CreateTower(string id, Node node, Material material)
    {
        var tower = Instantiate(_dataGame.GetPrefab(id), node.tower.position, Quaternion.identity, node.tower)
            .GetComponent<Tower>();
        var skin = tower.GetComponent<AnimationModelTower>();
        skin._bottomBody.GetComponent<MeshRenderer>().material = material;
        skin._midleBody.GetComponent<MeshRenderer>().material = material;
        skin.topBody.GetComponent<MeshRenderer>().material = material;
        tower.Init(_dataGame.LoadConfigTowerStat(id));
        listTower.Add(tower);
        return tower;
    }

    public Bullet CreateBullet(Tower attacker, Enemy victim)
    {
        var bullet = Instantiate(_dataGame.GetPrefab("Bullet_" + attacker.stat.id), attacker.firePointPos.position,
            Quaternion.identity, Parent_Bullet).GetComponent<Bullet>();
        bullet.Init(attacker, victim);
        return bullet;
    }


    public string SetIdForEnemy()
    {
        _countEnemy++;
        return "Enemy_" + _countEnemy;
    }


    public string SetIdForTower()
    {
        _countTower++;
        return "Tower_" + _countTower;
    }

    public void CheckLoseGame()
    {
        if (foods.Count != 0) return;
        LoseGame();
    }

    public void CheckWinGame()
    {
        if (listEnemy.Count == 0 && wave.state == WaveState.End)
        {
            WinGame();
        }
    }

    public void EnemyCompleteTakeFood(Enemy enemy)
    {
        listEnemy.Remove(enemy);
        foods.Remove(enemy.isPicked);
        enemy.isPicked.Lost();
        enemy.healthBar.Destroy();
        Destroy(enemy.gameObject);
    }
    
    private void StopObject()
    {
        listEnemy.ForEach(enemy => enemy.isStop = true);
        listTower.ForEach(tower => tower.isStop = true);
    }

    private void UnstopObject()
    {
        listEnemy.ForEach(enemy => enemy.isStop = false);
        listTower.ForEach(tower => tower.isStop = false);
    }
}