using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarParent : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {

        transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
    }
}
