using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalAfterImagePool : MonoBehaviour
{
    [SerializeField] private GameObject afterImagePrefab;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static CoalAfterImagePool Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < 20; i++)
        {
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool(Vector3 scale)
    {
        if(availableObjects.Count == 0)
        {
            GrowPool();
        }

        var instance = availableObjects.Dequeue();
        instance.transform.localScale = scale;
        instance.SetActive(true);
        return instance;
    }
}
