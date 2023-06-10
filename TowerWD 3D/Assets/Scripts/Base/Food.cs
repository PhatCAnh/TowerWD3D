using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public bool isPicked;

    public void Picked(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void Droped()
    {
        isPicked = false;
        transform.SetParent(null);
    }
}
