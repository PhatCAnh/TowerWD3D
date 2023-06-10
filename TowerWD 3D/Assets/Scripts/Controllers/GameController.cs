using CanasSource;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WaveState
{
    Ready,
    Start,
    Run,
    End,
}

[System.Serializable]
public class EnemyInTurn
{
    public Enemy EnemyPrefab;
    public int quantity;
    public int pathPoint;
}

[System.Serializable]
public class Turn
{
    public List<EnemyInTurn> enemiesInTurn = new();
    private Cooldown turnCoolDown = new Cooldown();
    public float timeCoolDown;
    private int index = 0;
    public WaveState state;

    public void LogicUpdate(float deltaTime)
    {
        switch (state)
        {
            case WaveState.Start:
                Start(); break;
            case WaveState.Run:
                Run(deltaTime); break;
            case WaveState.End:
                break;
        }
    }

    public void Start()
    {
        state = WaveState.Run;
    }

    public void Run(float deltaTime)
    {
        turnCoolDown.Update(deltaTime);
        if (turnCoolDown.isFinished)
        {
            index++;
            turnCoolDown.Restart(timeCoolDown);
            foreach (var item in enemiesInTurn)
            {
                if (item.quantity < index) continue;
                var enemy = item.EnemyPrefab;
                Singleton<GameController>.Instance.CreateEnemy(
                    enemy,
                    new EnemyModel(700, 0, 1, 10),
                    item.pathPoint
                    );
            }
            if (enemiesInTurn.Max(quantity => quantity.quantity) < index)
            {
                state = WaveState.End;
            }
        }
    }


}

[System.Serializable]
public class Wave
{
    public List<Turn> listTurn = new();
    private Cooldown waveCoolDown = new Cooldown();
    public float timeWaveCoolDown;
    private int index = 0;
    public WaveState state;

    public void LogicUpdate(float deltaTime)
    {
        switch (state)
        {
            case WaveState.Start:
                Start(); break;
            case WaveState.Run:
                Run(deltaTime); break;
            case WaveState.End:
                break;
        }
    }

    public void Start()
    {
        state = WaveState.Run;
    }

    public void Run(float deltaTime)
    {
        waveCoolDown.Update(deltaTime);
        if (waveCoolDown.isFinished)
        {
            if (listTurn.Count == index)
            {
                state = WaveState.End;
                return;
            }
            listTurn[index].state = WaveState.Start;
            index++;
            waveCoolDown.Restart(timeWaveCoolDown);
            
        }
        foreach (var item in listTurn)
        {
            item.LogicUpdate(deltaTime);
        }
    }
}

[System.Serializable]
public class AllWave
{
    public List<Wave> listWave = new();
    private Cooldown allWaveCoolDown = new Cooldown();
    public float timeCoolDown;
    private int index = 0;
    public WaveState state;

    public void LogicUpdate(float deltaTime)
    {
        switch (state)
        {
            case WaveState.Start:
                Start(); break;
            case WaveState.Run:
                Run(deltaTime); break;
            case WaveState.End:
                break;
        }
    }

    public void Start()
    {
        state = WaveState.Run;
    }

    public void Run(float deltaTime)
    {
        allWaveCoolDown.Update(deltaTime);
        if (allWaveCoolDown.isFinished)
        {
            if (listWave.Count == index)
            {
                state = WaveState.End;
                return;
            }
            listWave[index].state = WaveState.Start;
            index++;
            allWaveCoolDown.Restart(timeCoolDown);
        }
        foreach (var item in listWave)
        {
            item.LogicUpdate(deltaTime);
        }
    }
}

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
    public string SetIdForEnemy()
    {
        //chua viet
        return "";
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

    public Enemy CreateEnemy(Enemy pfEnemy, EnemyModel model, int pathPoint)
    {
        Enemy enemy = Instantiate(pfEnemy, GetStartPoint(pathPoint).position, Quaternion.identity);
        HealthBar hb = Instantiate(PF_Healthbar);
        enemy.Init(pathPoint, model, hb);
        hb.Init(enemy);
        return enemy;
    }
}
