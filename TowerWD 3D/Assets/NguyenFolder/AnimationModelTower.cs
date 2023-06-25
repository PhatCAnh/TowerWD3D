using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;
using CanasSource;

public class AnimationModelTower : MonoBehaviour
{
    private int currentLevel => tower.stat.levelEvolution;
    protected InGameController inGameController => Singleton<InGameController>.Instance;
    public Transform topBody;
    public Transform _midleBody;
    public Transform _bottomBody;
    private bool isStop;
    public Tower tower => GetComponent<Tower>();

    public async UniTask AppearTower()
    {
        await SetAnimAppearTower();
        tower.state = TowerState.Idle;
        Idle();
    }

    public async UniTask DestroyTower()
    {
        tower.state = TowerState.RunningAnim;
        Pause();
        await SetAnimDestroyTower();
    }

    public void Idle()
    {
        topBody.DORotate(new Vector3(0f, currentLevel == 2 ? -360f : 360f, 0f), 1f,
                RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);

        _midleBody.DORotate(new Vector3(0f, currentLevel > 1 ? -360f : 360f, 0f), 1f,
                RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);

        _bottomBody.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public void Pause()
    {
        topBody.DOPause();
        _midleBody.DOPause();
        _bottomBody.DOPause();
    }

    public async UniTask UpLevel()
    {
        if (!Singleton<InGameController>.Instance.UpLevelTower(tower)) return;
        tower.state = TowerState.RunningAnim;
        await SetAnimUpLevel(0.5f, currentLevel - 1);
        tower.state = TowerState.Idle;
    }

    private async UniTask SetAnimAppearTower()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_midleBody.DOLocalMove(Vector3.zero, 0.5f));
        sequence.Join(topBody.DOLocalMove(Vector3.zero, 0.3f).SetDelay(0.2f));
        sequence.AppendCallback(() => { sequence.Kill(); });
        await sequence.AsyncWaitForCompletion();
    }

    private async UniTask SetAnimDestroyTower()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_midleBody.DORotate(new Vector3(0f, _bottomBody.localEulerAngles.y, 0f), 0.1f)
            .OnStart(() => _midleBody.DOLocalMove(new Vector3(0, 0, 0), 0.1f)));

        sequence.Append(topBody.DORotate(new Vector3(0f, _bottomBody.localEulerAngles.y, 0f), 0.1f)
            .OnStart(() => topBody.DOLocalMove(new Vector3(0, 0, 0), 0.1f)));

        sequence.Join(topBody.DOLocalMove(new Vector3(0, -1, 0), 0.1f).SetDelay(0.15f));
        sequence.Join(_midleBody.DOLocalMove(new Vector3(0, -0.35f, 0), 0.1f).SetDelay(0.15f));
        sequence.AppendCallback(() => { sequence.Kill(); });
        await sequence.AsyncWaitForCompletion();
    }

    private async UniTask SetAnimUpLevel(float duration, int level)
    {
        Sequence sequence = DOTween.Sequence();
        Vector3 endValue = new(0, 0.3f, 0);
        sequence.OnStart(() => Pause());
        sequence.Join(topBody.DOLocalMove(endValue * level, duration));
        sequence.Join(_midleBody.DOLocalMove(endValue, duration));
        sequence.AppendCallback(() =>
        {
            Idle();
            sequence.Kill();
        });
        sequence.Play();
        await sequence.AsyncWaitForCompletion();
    }
}