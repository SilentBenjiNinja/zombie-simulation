using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private List<T> pooledObjects = new List<T>();
    private Transform parentTransform;

    private T prefab;
    public string defaultObjectName;

    private void SpawnNewObject()
    {
        T newObject = Object.Instantiate(prefab, parentTransform);
        newObject.name = defaultObjectName;
        FreeObject(newObject);
        pooledObjects.Add(newObject);
    }

    public ObjectPool(T prefab, Transform parentTransform, int startAmount, string defaultObjectName = "PooledObject")
    {
        this.prefab = prefab;
        this.parentTransform = parentTransform;
        this.defaultObjectName = defaultObjectName;

        for (int i = 0; i < startAmount; i++)
            SpawnNewObject();
    }

    public T Next()
    {
        if (pooledObjects.Count(x => !x.gameObject.activeSelf) == 0)
            SpawnNewObject();

        return pooledObjects.FirstOrDefault(x => !x.gameObject.activeSelf);
    }

    public void FreeObject(T objectToFree) => objectToFree.gameObject.SetActive(false);
}
