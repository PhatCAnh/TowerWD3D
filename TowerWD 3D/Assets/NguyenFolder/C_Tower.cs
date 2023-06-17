using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System;

public class C_Tower : MonoBehaviour
{
    [Min(1)]
    public int currentLevel;

    public Transform _topBody;
    public Transform _midleBody;
    public Transform _bottomBody;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentLevel = 0;
    }

    public void AppearTower(float duration)
    {
        StartCoroutine(SetAnimAppearTower(duration));
    }

    public void DestroyTower(float duration)
    {
        StartCoroutine(SetAnimDestroyTower(duration));
    }

    public void Idle()
    {
        transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
    public void Pause()
    {
        transform.DOPause();
    }

    private IEnumerator SetAnimAppearTower(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_midleBody.DOLocalMove(Vector3.zero, duration));
        sequence.Join(_topBody.DOLocalMove(Vector3.zero, duration).SetDelay(duration / 2));
        sequence.AppendCallback(() =>
        {
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
            Pause();
        });
        sequence.Append(_topBody.DOLocalMove(new Vector3(0, -1, 0), duration));
        sequence.Join(_midleBody.DOLocalMove(new Vector3(0, -0.35f, 0), duration).SetDelay(duration / 2));
        sequence.AppendCallback(() =>
        {
            sequence.Kill();
        }); // Gọi một hàm callback khi sequence hoàn thành
        sequence.Play();
        yield return sequence.AsyncWaitForCompletion();
    }

    public void Level_Up()
    {
        animator.SetTrigger("Level_Up");
    }

    public void SetTriggerAnim(string strigger)
    {
        animator.SetTrigger(strigger);
    }

    public void SetAnimDestroyTowerInNode()
    {
        GetComponentInParent<Node>().SetAnimDestroyTowerInNode();
    }
}
