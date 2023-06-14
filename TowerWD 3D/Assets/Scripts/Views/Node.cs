using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    protected Animator animator;
    [SerializeField] protected Transform Tower;
    public bool isOpen;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            AppearTower();
        }
    }

    public void Open()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
    }

    public void Close()
    {
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
    }

    public void AppearTower()
    {
        animator.SetBool("isHaveTower", true);
    }

    public void DestroyTower()
    {
        animator.SetBool("isHaveTower", false);
    }

    public void SetTriggerAnim(string strigger)
    {
        animator.SetTrigger(strigger);
    }

    public void SetAppearTower()
    {
        Tower.GetComponentInChildren<C_Tower>().SetTriggerAnim("Appear");
    }

    public void SetAnimDestroyTower()
    {
        Tower.GetComponentInChildren<C_Tower>().SetTriggerAnim("Destroy");
    }

    public void SetAnimDestroyTowerInNode()
    {
        animator.SetBool("isHaveTower", false);
    }
}
