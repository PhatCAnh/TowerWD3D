using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tower_Nguyen : MonoBehaviour
{
    [SerializeField] string nameTower;
    [SerializeField] float attackStat;
    [SerializeField] float rangeAttack;
    [SerializeField] float speedAttack;
    [SerializeField] bool isActive;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] List<GameObject> enemiesDetected;

    [Space]
    [Header("Temp")]
    public Color circleColor = Color.red;

    

    // Start is called before the first frame update
    void Start()
    {
        enemiesDetected = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            DetectEnemyInRange();
        }
    }

    void DetectEnemyInRange()
    {
        GameObject[] enemyDetect = GameObject.FindObjectsOfType<GameObject>();
        for(int i = 0; i < enemyDetect.Length - 1; i++)
        {
            
            if (Vector2.Distance(enemyDetect[i].transform.position, transform.position)<= rangeAttack)
            {
                if (((1 << enemyDetect[i].layer) & enemyLayer) != 0)
                {
                    if(!enemiesDetected.Contains(enemyDetect[i]))
                    { enemiesDetected.Add(enemyDetect[i]); }
                    // Layer của GameObject khớp với LayerMask

                }
                
            } else
            {
                if (enemiesDetected.Count!=0)
                {
                    if (((1 << enemyDetect[i].layer) & enemyLayer) != 0 && enemyDetect[i] == enemiesDetected[0])
                    {
                        enemiesDetected.RemoveAt(0);
                        // Layer của GameObject khớp với LayerMask
                        
                    }
                }
            }
        }
    }
    
}
