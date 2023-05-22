using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    private List<GameObject> objectPool = new List<GameObject>();

    private void Start()
    {
        MakePool();
    }

    private void MakePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }
    public GameObject GetPool()
    {
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject newObj = Instantiate(prefab);
        objectPool.Add(newObj);
        return newObj;
    }
    public void ReturnPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
