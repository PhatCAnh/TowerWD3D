using CanasSource;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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
    private Cooldown enemyCoolDown = new Cooldown();
    private int index = 0;

    public List<EnemyInTurn> listEnemy = new();
    public float timeCdEnemy;
    public WaveState state;

    public void LogicUpdate(float deltaTime)
    {
        switch (state)
        {
            case WaveState.Start:
                Start();
                break;
            case WaveState.Run:
                Run(deltaTime);
                break;
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
        enemyCoolDown.Update(deltaTime);
        if (enemyCoolDown.isFinished)
        {
            index++;
            enemyCoolDown.Restart(timeCdEnemy);
            foreach (var item in listEnemy)
            {
                if (item.quantity < index) continue;
                Singleton<InGameController>.Instance.CreateEnemy(item.idEnemy, item.pathPoint);
            }

            if (listEnemy.Max(quantity => quantity.quantity) < index)
            {
                state = WaveState.End;
            }
        }
    }
}

[System.Serializable]
public class Wave
{
    private Cooldown turnCooldown = new Cooldown();
    private int index = 0;
    private Turn thisTurn;

    public List<Turn> listTurn = new();
    public float timeCdTurn;
    public WaveState state;


    public void LogicUpdate(float deltaTime)
    {
        switch (state)
        {
            case WaveState.Start:
                Start();
                break;
            case WaveState.Run:
                Run(deltaTime);
                break;
            case WaveState.End:
                break;
        }
    }

    public void Start()
    {
        state = WaveState.Run;
        thisTurn = listTurn[index];
    }

    public void Run(float deltaTime)
    {
        thisTurn.LogicUpdate(deltaTime);
        switch (thisTurn.state)
        {
            case WaveState.End:
                turnCooldown.Restart(timeCdTurn);
                index++;
                if (listTurn.Count == index)
                {
                    state = WaveState.End;
                    return;
                }

                thisTurn = listTurn[index];
                break;
            case WaveState.Ready:
                turnCooldown.Update(deltaTime);
                if (turnCooldown.isFinished) thisTurn.state = WaveState.Start;
                break;
        }
    }
}

[System.Serializable]
public class AllWave
{
    private Cooldown waveCoolDown = new Cooldown();
    private int index = 0;
    private Wave thisWave;

    public List<Wave> listWave = new();
    public float timeCdWave;
    public WaveState state;


    public void LogicUpdate(float deltaTime)
    {
        switch (state)
        {
            case WaveState.Start:
                Start();
                break;
            case WaveState.Run:
                Run(deltaTime);
                break;
            case WaveState.End:
                break;
        }
    }

    public void Start()
    {
        state = WaveState.Run;
        thisWave = listWave[index];
    }

    public void Run(float deltaTime)
    {
        thisWave.LogicUpdate(deltaTime);
        switch (thisWave.state)
        {
            case WaveState.End:
                waveCoolDown.Restart(timeCdWave);
                index++;
                if (listWave.Count == index)
                {
                    state = WaveState.End;
                    return;
                }

                thisWave = listWave[index];
                break;
            case WaveState.Ready:
                waveCoolDown.Update(deltaTime);
                if (waveCoolDown.isFinished) thisWave.state = WaveState.Start;
                break;
        }
    }
}