using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ChooseTower : MonoBehaviour
{
    public Node node;

    protected virtual void Start()
    {
        node = GetComponentInParent<Node>();
    }
    public abstract void OnClicked();
}
