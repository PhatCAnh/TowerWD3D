using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Tower : MonoBehaviour
{
    [Min(1)]
    public int currentLevel;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentLevel = 0;
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
