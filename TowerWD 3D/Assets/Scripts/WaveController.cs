using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController instance;

    public Transform[] path;

    private void Awake()
    {
        instance = this;
    }
}
