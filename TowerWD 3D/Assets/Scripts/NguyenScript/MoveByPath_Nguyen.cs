using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByPath_Nguyen : MonoBehaviour
{

    [Space]
    [SerializeField] List<Transform> list;
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed;
    [SerializeField] int index;
    [SerializeField] bool isMoving;
    [SerializeField] bool movingForward = true; // Biến để kiểm tra hướng di chuyển

    private void Awake()
    {
        //SetStat();
    }


    // Start is called before the first frame update
    void Start()
    {
        
        moveSpeed = GetComponent<Enemy_Nguyen>().moveSpeed;
        list = GameController_Nguyen.instance.getPathMoving();
        target = list[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (Vector2.Distance(transform.position, target.position) < 0.001f)
            {
                transform.position = target.position;
                if (movingForward)
                {
                    index = (index + 1) % list.Count;
                    target = list[index];
                    if (index == list.Count - 1)
                    {
                        movingForward = false;

                    }
                }
                else
                {
                    index = (index - 1) % list.Count;
                    target = list[index];
                    if (index == 0)
                    {
                        movingForward = true;

                    }
                }
            }
            MoveToTarget();
        }

        
    }
    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
    [SerializeField] Enemy enemy;
    /*void SetStat()
    {
        enemy = new Enemy();
    }*/
}
