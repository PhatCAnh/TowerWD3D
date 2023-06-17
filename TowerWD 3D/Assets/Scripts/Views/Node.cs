using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Node : MonoBehaviour
{
    protected Animator animator;
    [SerializeField] protected Transform tower;
    public bool isOpen;

    public Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        StartCoroutine(SetAnimSelected(1));
    }

    public void Unselected()
    {
        StartCoroutine(SetAnimUnselected(1));
    }

    public void AppearTower()
    {
        StartCoroutine(SetAnimAppearTower(0.5f));
    }

    public void DestroyTower()
    {
        StartCoroutine(SetAnimDestroyTower(0.5f));
    }

    private Tweener CloseDoor(float duration)
    {
        Tweener tweenerRight = _rightDoor.DOLocalMove(new Vector3(0, 0.375f, 0), duration / 2);
        tweenerRight.OnStart(() =>
        {
            Tweener tweenerLeft = _leftDoor.DOLocalMove(new Vector3(0, 0.375f, 0), duration / 2);
        });
        return tweenerRight;
    }

    private Tweener OpenDoor(float duration)
    {
        Tweener tweenerRight = _rightDoor.DOLocalMove(new Vector3(0.5f, 0.375f, 0), duration / 2);
        tweenerRight.OnStart(() =>
        {
            Tweener tweenerLeft = _leftDoor.DOLocalMove(new Vector3(-0.5f, 0.375f, 0), duration / 2);
        });
        return tweenerRight;
    }

    private IEnumerator SetAnimSelected(float duration)
    {
        yield return OpenDoor(duration).AsyncWaitForCompletion();
    }

    private IEnumerator SetAnimUnselected(float duration)
    {
        yield return CloseDoor(duration).AsyncWaitForCompletion();
    }

    private IEnumerator SetAnimAppearTower(float duration)
    {
        // Tạo một sequence
        Sequence sequence = DOTween.Sequence();

        // Thêm các tweener vào sequence
        sequence.Append(OpenDoor(duration * 3 / 4));
        sequence.Join(tower.DOLocalMove(new Vector3(0, 1, 0), duration).SetDelay(duration * 3 / 4));
        sequence.Join(tower.DOScale(new Vector3(1, 1, 1), duration).SetDelay(duration / 4));
        sequence.Join(CloseDoor(duration * 3 / 4).SetDelay(duration / 4));
        sequence.AppendCallback(() =>
        {
            tower.GetComponentInChildren<C_Tower>().AppearTower(duration);
        }); // Gọi một hàm callback khi sequence hoàn thành

        // Bắt đầu chạy sequence
        sequence.Play();
        yield return sequence.AsyncWaitForCompletion();
    }

    public IEnumerator SetAnimDestroyTower(float duration)
    {
        Sequence sequence = DOTween.Sequence();

        // Thêm các tweener vào sequence
        sequence.OnStart(() =>
        {
            tower.GetComponentInChildren<C_Tower>().DestroyTower(duration);
        });
        sequence.Join(tower.DOScale(new Vector3(0.4f, 0.4f, 0.4f), duration * 3 / 4).SetDelay(duration));
        sequence.Join(tower.DOLocalMove(new Vector3(0, -0.3f, 0), duration).SetDelay(duration / 2));
        sequence.Join(OpenDoor(duration * 3 / 4));
        sequence.Join(CloseDoor(duration * 3 / 4).SetDelay(duration * 3 / 4));
        sequence.AppendCallback(() => Debug.Log("Sequence completed")); // Gọi một hàm callback khi sequence hoàn thành

        // Bắt đầu chạy sequence
        sequence.Play();
        yield return sequence.AsyncWaitForCompletion();
    }

    public void SetAnimDestroyTowerInNode()
    {
        animator.SetBool("isHaveTower", false);
    }
}
