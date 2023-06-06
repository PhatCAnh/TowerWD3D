using CanasSource;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize;
    [SerializeField] private bool expandable = true;

    private List<GameObject> freeList;
    private List<GameObject> usedList;

    protected virtual void Awake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();
        for (int i = 0; i < poolSize; ++i)
        {
            GenerateNewObject();
        }
    }


    //Get an object from the pool
    public GameObject GetObject(Vector3 position)
    {
        int totalFree = freeList.Count;

        if (totalFree == 0 && !expandable) return null;
        else if (totalFree == 0)
        {
            GenerateNewObject();
            totalFree = 1;
        }

        GameObject g = freeList[totalFree - 1];
        freeList.RemoveAt(totalFree - 1);
        usedList.Add(g);
        g.transform.position = position;
        g.SetActive(true);
        return g;

    }

    //Return an object to the pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }

    //Instantiate new GameObject
    private void GenerateNewObject()
    {
        GameObject game = Instantiate(prefab);
        game.transform.SetParent(transform, false);
        game.SetActive(false);
        freeList.Add(game);
    }

    public abstract void GetObjectFromPool(Vector3 position, params object[] arrObject);

    public abstract void RemoveObjectToPool(GameObject theGO, params object[] arrObject);
}
