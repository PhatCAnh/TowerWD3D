using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnScript : MonoBehaviour
{
    [SerializeField] float roationSpeed;
    Vector3 currentEulerAngles;
    [SerializeField] float z;

    private void Update()
    {
        currentEulerAngles = Vector3.up * roationSpeed;
        transform.Rotate(currentEulerAngles * Time.time);
    }
}
