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
    public string idEnemy;
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
                Singleton<GameController>.Instance.CreateEnemy("BasicEnemy", item.pathPoint);
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
