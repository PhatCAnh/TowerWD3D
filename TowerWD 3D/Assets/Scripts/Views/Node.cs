﻿using CanasSource;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Node : MonoBehaviour
{
    protected InGameController inGameController => Singleton<InGameController>.Instance;

    public bool isHaveTower;
    public Transform attackRange;
    public Transform tower;
    public Transform leftDoor;
    public Transform rightDoor;
    public Transform circleSelect;
    public ChooseTower[] childCircle;
    public Stopwatch stopWatch = new();

    private Tower _tower;
    public AnimationModelTower animTower => tower?.GetComponentInChildren<AnimationModelTower>();

    private void Start()
    {
        childCircle = circleSelect.GetComponentsInChildren<ChooseTower>();
    }


    public async UniTask Selected()
    {
        if(_tower?.stat.levelEvolution > 2 && isHaveTower)
        {
            foreach (var item in childCircle)
            {
                item.gameObject.SetActive(item.GetComponent<ChooseSellTower>());
            }
        }
        else
        {
            SetCircleSelect();
        }

        OpenAttackRange();
        TurnCircle();
        await SetAnimSelected();
    }

    public async UniTask Unselected()
    {
        StopCricle();
        CloseAttackRange();
        await SetAnimUnselected();
    }

    public async void AppearTower(string idTower, Material material)
    {
        isHaveTower = true;
        await Unselected();
        _tower = Singleton<InGameController>.Instance.CreateTower(idTower, this, material);
        animTower.tower.state = TowerState.RunningAnim;
        await SetAnimAppearTower();
        await animTower.AppearTower();

    }

    public async void DestroyTower()
    {
        isHaveTower = false;
        await Unselected();
        await animTower.DestroyTower();
        await SetAnimDestroyTower();
        _tower = null;
        inGameController.DestroyTower(_tower);
    }

    public async void LevelUp()
    {
        await Unselected();
        await animTower.UpLevel();
    }

    private Tweener CloseDoor()
    {
        Tweener tweenerRight = rightDoor.DOLocalMove(new Vector3(0, 0.375f, 0), 0.2f);
        tweenerRight.OnStart(() =>
        {
            Tweener tweenerLeft = leftDoor.DOLocalMove(new Vector3(0, 0.375f, 0), 0.2f);
        });
        return tweenerRight;
    }

    private Tweener OpenDoor()
    {
        Tweener tweenerRight = rightDoor.DOLocalMove(new Vector3(0.5f, 0.375f, 0), 0.2f);
        tweenerRight.OnStart(() =>
        {
            Tweener tweenerLeft = leftDoor.DOLocalMove(new Vector3(-0.5f, 0.375f, 0), 0.2f);
        });
        return tweenerRight;
    }

    private async UniTask SetAnimSelected()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(OpenDoor());
        sequence.Join(circleSelect.DOLocalMove(new Vector3(0, 1, 0), 0.4f).SetDelay(0.1f));
        sequence.Join(circleSelect.DOScale(new Vector3(150, 150, 100), 0.3f).SetDelay(0.1f));
        //sequence.Join(circleSelect.DOLocalRotate(new Vector3(0f, 0f, 180f), duration / 2, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetDelay(duration / 4));

        foreach (var item in childCircle)
        {
            sequence.Join(item.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.2f));
        }

        sequence.OnComplete(() =>
        {
            sequence.Kill();
        });
        await sequence.AsyncWaitForCompletion();
    }

    private async UniTask SetAnimUnselected()
    {
        Sequence sequence = DOTween.Sequence();
        foreach (var item in childCircle)
        {
            sequence.Join(item.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f));
        }
        sequence.Join(circleSelect.DOLocalMove(new Vector3(0, -0.1f, 0), 0.25f).SetDelay(0.15f));
        sequence.Join(circleSelect.DOScale(new Vector3(25, 25, 16.666f), 0.2f));
        //sequence.Join(circleSelect.DOLocalRotate(new Vector3(-90f, 0f, 0f), duration / 2).SetEase(Ease.Linear));
        sequence.Join(CloseDoor().SetDelay(0.15f));

        sequence.OnComplete(() =>
        {
            sequence.Kill();
        });
        await sequence.AsyncWaitForCompletion();
    }

    private async UniTask SetAnimAppearTower()
    {
        // Tạo một sequence
        Sequence sequence = DOTween.Sequence();
        // Thêm các tweener vào sequence
        sequence.Append(OpenDoor());
        sequence.Join(tower.DOLocalMove(new Vector3(0, 1, 0), 0.5f).SetDelay(0.5f * 3 / 4));
        sequence.Join(tower.DOScale(new Vector3(1, 1, 1), 0.5f).SetDelay(0.5f / 4));
        sequence.Join(CloseDoor().SetDelay(0.5f / 4));
        sequence.OnComplete(() =>
        {
            sequence.Kill();
        });
        await sequence.AsyncWaitForCompletion();
    }

    private async UniTask SetAnimDestroyTower()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(tower.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.35f).SetDelay(0.1f));
        sequence.Join(tower.DOLocalMove(new Vector3(0, -0.3f, 0), 0.5f).SetDelay(0.3f));
        sequence.Join(OpenDoor());
        sequence.Join(CloseDoor().SetDelay(0.4f));
        sequence.OnComplete(() =>
        {
            sequence.Kill();
        });
        await sequence.AsyncWaitForCompletion();
    }

    private void SetCircleSelect()
    {
        foreach (var item in childCircle)
        {
            item.gameObject.SetActive(item.GetComponent<ChooseBuildTower>() ? !isHaveTower : isHaveTower);
        }
    }

    private void TurnCircle()
    {
        foreach(var item in childCircle)
        {
            item.transform.DORotate(new Vector3(0f, 0f, 360f), 10f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        }
    }

    private void StopCricle()
    {
        foreach (var item in childCircle)
        {
            item.transform.DOPause();
        }
    }

    private void OpenAttackRange()
    {
        if (!_tower) return;
        attackRange.gameObject.SetActive(true);
        var range = _tower.model.AtkRange;
        attackRange.DOScale(new Vector3(range, range, range), 0.2f);
    }
    
    private void CloseAttackRange()
    {
        if (!_tower) return;
        var range = _tower.model.AtkRange;
        attackRange.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f)
            .OnComplete(() =>
        {
            attackRange.gameObject.SetActive(false);
        });
    }
}