using CanasSource;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isHaveTower;
    public Transform tower;
    public Transform leftDoor;
    public Transform rightDoor;
    public Transform circleSelect;
    public ChooseTower[] childCircle;

    private void Start()
    {
        childCircle = circleSelect.GetComponentsInChildren<ChooseTower>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //AppearTower();
        }
    }

    public void Selected()
    {
        StartCoroutine(SetAnimSelected(0.4f));
    }

    public void Unselected()
    {
        StartCoroutine(SetAnimUnselected(0.4f));
    }

    public void AppearTower()
    {
        Unselected();
        StartCoroutine(SetAnimAppearTower(0.5f));
    }

    public void DestroyTower()
    {
        StartCoroutine(SetAnimDestroyTower(0.5f));
    }

    private Tweener CloseDoor(float duration)
    {
        Tweener tweenerRight = rightDoor.DOLocalMove(new Vector3(0, 0.375f, 0), duration / 2);
        tweenerRight.OnStart(() =>
        {
            Tweener tweenerLeft = leftDoor.DOLocalMove(new Vector3(0, 0.375f, 0), duration / 2);
        });
        return tweenerRight;
    }

    private Tweener OpenDoor(float duration)
    {
        Tweener tweenerRight = rightDoor.DOLocalMove(new Vector3(0.5f, 0.375f, 0), duration / 2);
        tweenerRight.OnStart(() =>
        {
            Tweener tweenerLeft = leftDoor.DOLocalMove(new Vector3(-0.5f, 0.375f, 0), duration / 2);
        });
        return tweenerRight;
    }

    private IEnumerator SetAnimSelected(float duration)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(OpenDoor(duration));
        sequence.Join(circleSelect.DOLocalMove(new Vector3(0, 1, 0), duration / 2));
        sequence.Join(circleSelect.DOScale(new Vector3(150, 150, 100), duration / 2).SetDelay(duration / 4));
        sequence.Join(circleSelect.DOLocalRotate(new Vector3(0f, 0f, 180f), duration / 2, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetDelay(duration / 4));

        foreach (var item in childCircle)
        {
            sequence.Join(item.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), duration));
        }

        sequence.AppendCallback(() =>
        {
            sequence.Kill();
        });
        sequence.Play();
        yield return sequence.AsyncWaitForCompletion();
    }

    private IEnumerator SetAnimUnselected(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        foreach (var item in childCircle)
        {
            sequence.Join(item.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), duration));
        }
        sequence.Join(circleSelect.DOLocalMove(new Vector3(0, -0.1f, 0), duration / 2));
        sequence.Join(circleSelect.DOScale(new Vector3(25, 25, 16.666f), duration / 2));
        sequence.Join(circleSelect.DOLocalRotate(new Vector3(-90f, 0f, 0f), duration / 2).SetEase(Ease.Linear));
        sequence.Join(CloseDoor(duration).SetDelay(duration / 2));
        sequence.AppendCallback(() =>
        {
            sequence.Kill();
        });
        sequence.Play();

        yield return sequence.AsyncWaitForCompletion();
    }

    private IEnumerator SetAnimAppearTower(float duration)
    {
        // Tạo một sequence
        Sequence sequence = DOTween.Sequence();

        sequence.OnStart(() =>
        {
            
            isHaveTower = true;
            Singleton<GameController>.Instance.CreateTower("BasicTower", this);
            tower.TryGetComponent(out AnimationModelTower anim);
            anim.tower.state = TowerState.RunningAnim;
        });

        // Thêm các tweener vào sequence
        sequence.Append(OpenDoor(duration * 3 / 4));
        sequence.Join(tower.DOLocalMove(new Vector3(0, 1, 0), duration).SetDelay(duration * 3 / 4));
        sequence.Join(tower.DOScale(new Vector3(1, 1, 1), duration).SetDelay(duration / 4));
        //sequence.Join(CloseDoor(duration * 3 / 4).SetDelay(duration / 4));
        sequence.AppendCallback(() =>
        {
            tower.GetComponentInChildren<AnimationModelTower>().AppearTower(duration);
            sequence.Kill();
        });
        sequence.Play();
        yield return sequence.AsyncWaitForCompletion();
    }

    public IEnumerator SetAnimDestroyTower(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        tower.GetChild(0).TryGetComponent(out AnimationModelTower anim);
        // Thêm các tweener vào sequence
        sequence.OnStart(() =>
        {
            anim.DestroyTower(duration);
        });
        sequence.Join(tower.DOScale(new Vector3(0.4f, 0.4f, 0.4f), duration * 3 / 4).SetDelay(duration));
        sequence.Join(tower.DOLocalMove(new Vector3(0, -0.3f, 0), duration).SetDelay(duration / 2));
        sequence.Join(OpenDoor(duration * 3 / 4));
        sequence.Join(CloseDoor(duration * 3 / 4).SetDelay(duration * 3 / 4));
        sequence.AppendCallback(() =>
        {
            isHaveTower = false;
            Destroy(anim.gameObject);
            sequence.Kill();
        }); // Gọi một hàm callback khi sequence hoàn thành

        // Bắt đầu chạy sequence
        sequence.Play();
        yield return sequence.AsyncWaitForCompletion();
    }
}
