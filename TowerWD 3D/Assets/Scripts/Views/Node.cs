using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    protected Animator animator;

    public bool isOpen;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
}
