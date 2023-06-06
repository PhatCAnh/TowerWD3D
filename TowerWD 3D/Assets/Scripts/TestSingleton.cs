using System.Collections;
using System.Collections.Generic;
using CanasSource;
using UnityEngine;

public class TestSingleton : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Singleton<TestSingleton>.Instance = this;
        Debug.Log(Singleton<TestSingleton>.Instance.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
