using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
public class AnimationModelTower : MonoBehaviour
{
    [Range(1, 3)]
    public int currentLevel = 1;

    public Transform topBody;
    public Transform _midleBody;
    public Transform _bottomBody;

    public Tower tower;

    private void Start()
    {
        tower = GetComponent<Tower>();
    }

    public void AppearTower(float duration)
    {
        StartCoroutine(SetAnimAppearTower(duration));
    }

    public void DestroyTower(float duration)
    {
        StartCoroutine(SetAnimDestroyTower(duration));
        currentLevel = 1;
    }

    public void Idle()
    {
        topBody.DORotate(new Vector3(0f, currentLevel == 2 ? -360f : 360f, 0f), 1f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);

        _midleBody.DORotate(new Vector3(0f, currentLevel > 1 ? -360f : 360f, 0f), 1f, RotateMode.LocalAxisAdd)
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

    public void UpLevelOne()
    {
        StartCoroutine(SetAnimUpLevel(0.5f, 1));
    }
    public void UpLevelTwo()
    {
        StartCoroutine(SetAnimUpLevel(0.5f, 2));
    }

    private IEnumerator SetAnimAppearTower(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_midleBody.DOLocalMove(Vector3.zero, duration));
        sequence.Join(topBody.DOLocalMove(Vector3.zero, duration).SetDelay(duration / 2));
        sequence.AppendCallback(() =>
        {
            tower.state = TowerState.Idle;
            Idle();
            sequence.Kill();
        }); // Gọi một hàm callback khi sequence hoàn thành
        sequence.Play();
        yield return sequence.AsyncWaitForCompletion();
    }

    private IEnumerator SetAnimDestroyTower(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() =>
        {
            tower.state = TowerState.RunningAnim;
            Pause();
            topBody.DORotate(new Vector3(0f, _bottomBody.localEulerAngles.y, 0f), duration / 2);
            _midleBody.DORotate(new Vector3(0f, _bottomBody.localEulerAngles.y, 0f), duration / 2);
        });
        sequence.Append(topBody.DOLocalMove(new Vector3(0, -1, 0), duration));
        sequence.Join(_midleBody.DOLocalMove(new Vector3(0, -0.35f, 0), duration).SetDelay(duration / 2));
        sequence.AppendCallback(() =>
        {
            sequence.Kill();
        }); // Gọi một hàm callback khi sequence hoàn thành
        sequence.Play();
        yield return sequence.AsyncWaitForCompletion();
    }

    private IEnumerator SetAnimUpLevel(float duration, int level)
    {
        currentLevel++;
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
        yield return sequence.AsyncWaitForCompletion();
    }
}
