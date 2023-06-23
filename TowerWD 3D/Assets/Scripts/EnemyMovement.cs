using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D theRB;

    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private bool picked = false;

    // Start is called before the first frame update
    void Start()
    {
        target = WaveController.instance.path[1];
        theRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LogicMove();
    }

    private void LogicMove()
    {
        if (!picked)
        {
            if (Vector2.Distance(target.position, transform.position) <= 0.1f)
            {
                pathIndex++;

                if (pathIndex == WaveController.instance.path.Length)
                {
                    picked = true;
                }
                else
                {
                    target = WaveController.instance.path[pathIndex];
                }
            }
        }
        else
        {
            if (Vector2.Distance(target.position, transform.position) <= 0.1f)
            {
                pathIndex--;

                if (pathIndex == -1)
                {
                    picked = false;
                }
                else
                {
                    target = WaveController.instance.path[pathIndex];
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * direction, Space.World);
    }
}