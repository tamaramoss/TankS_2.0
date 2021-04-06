using System.Collections.Generic;
using UnityEngine;

public class SpawnPools : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    
    // The pools we want to initialize
    public List<Pool> pools;
    
    // The pools we can spawn from by its tag
    private Dictionary<string, Queue<GameObject>> poolsDictionary;
    
    // Singleton
    public static SpawnPools Instance;
    private void Awake()
    {
        Instance = this;
        poolsDictionary = new Dictionary<string, Queue<GameObject>>();

        // Initialize the objects defined in the pools, add them to the Queue and add to dictionary  
        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (var i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolsDictionary.Add(pool.tag, objectPool);
        }
    }
    
    // Return the active game objects of a pool
    public List<GameObject> GetActivePoolObjects(string tag)
    {
        if (!poolsDictionary.ContainsKey(tag))
        {
            Debug.Log($"Pool with ${tag} doesn't exist!");
            return null;
        }

        List<GameObject> wantedObjects = new List<GameObject>();
        foreach (var obj in poolsDictionary[tag])
        {
            if (obj.active)
                wantedObjects.Add(obj);
        }

        return wantedObjects;
    }

    // Spawn an object from a certain pool, optional: with given transform 
    public GameObject SpawnFromPool(string tag, Transform tf = null)
    {
        if (!poolsDictionary.ContainsKey(tag))
        {
            Debug.Log($"Pool with ${tag} doesn't exist!");
            return null;
        }
        
        var wantedObject = poolsDictionary[tag].Dequeue();
        wantedObject.SetActive(true);

        if (tf)
        {
            wantedObject.transform.position = tf.position;
            wantedObject.transform.rotation = tf.rotation;
            wantedObject.transform.forward = tf.forward;
        }

        var pooledObject = wantedObject.GetComponent<IPoolable>();

        pooledObject?.OnSpawn();
        
        poolsDictionary[tag].Enqueue(wantedObject);
        return wantedObject;
    }
}
