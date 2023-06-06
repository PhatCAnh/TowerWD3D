using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_Nguyen : MonoBehaviour
{
    public static GameController_Nguyen instance;
    private void Awake()
    {
        instance = this;
    }
    // Path find group
    [Header("Path Find Group")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform[] betweenPoints;
    [SerializeField] Transform endPoint;
    // Start is called before the first frame update

    public List<Transform> getPathMoving()
    {
        List<Transform> path = new List<Transform>();
        // add from start -> end point
        path.Add(startPoint);
        foreach (Transform x in betweenPoints)
        {
            path.Add(x);
        }
        path.Add(endPoint);
        return path;
    }

}
