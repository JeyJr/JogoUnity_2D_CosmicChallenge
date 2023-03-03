using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsável por gerenciar um pool de objetos e realizar spawns controlados durante a partida.
/// </summary>
public class Pool : MonoBehaviour, IStartSpawnObj, IReturnToPool
{
    [SerializeField] private List<GameObject> objPrefab = new();

    [SerializeField] private float delayToSpawnObjBaseCal = 3;
    [SerializeField] private float delayToSpawn = 0;

    [SerializeField] private int maxObjInPool = 10;
    private Queue<GameObject> objPool = new Queue<GameObject>();
    private List<GameObject> objSpawned = new List<GameObject>();


    [SerializeField] private Material[] material;

    private void Start()
    {
        GameEvent.GetInstance().OnStartSpawn += StartSpawnObj;
        GameEvent.GetInstance().OnEndGame += ClearPool;
    }

    public void StartSpawnObj()
    {
        StartCoroutine(InitializePool());

        delayToSpawn = GameController.GetInstance().GetSpawnSpeed(delayToSpawnObjBaseCal);
    }

    IEnumerator InitializePool()
    {
        int index = 0;
        for (int i = 0; i < maxObjInPool; i++)
        {
            if (index >= objPrefab.Count) index = 0;

            GameObject obj = Instantiate(objPrefab[index]);
            index++;

            if (obj.TryGetComponent<IPoolReference>(out var poolReference))
            {
                poolReference.PoolReference(this);

                if(material!= null)
                {
                    obj.GetComponent<MeshRenderer>().material = material[Random.Range(0, material.Length)];
                }

                obj.SetActive(false);
                objPool.Enqueue(obj);
            }
            else
            {
                Destroy(obj);
                throw new System.Exception($"Prefab não implementa interface IPoolReference! {objPrefab}");
            }
        }

        yield return StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        yield return new WaitForSeconds(delayToSpawn);

        while (GameController.GetInstance().GameState == GameState.StartMatch)
        {
            try
            {
                if(objPool != null && objPool.Count > 0)
                {
                    GameObject obj = objPool.Dequeue();
                    obj.SetActive(true);
                    objSpawned.Add(obj);
                }
            }
            catch
            {
                Debug.Log("Pool empty!");
                break;
            }

            yield return new WaitForSeconds(delayToSpawn);
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        if (!objSpawned.Contains(obj))
        {
            objSpawned.Remove(obj);
        }
        objPool.Enqueue(obj);
    }

    public void ClearPool()
    {
        StartCoroutine(DestroyObj());
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(1);
        if (objPool.Count > 0)
        {
            foreach (var item in objPool)
            {
                Destroy(item);
            }
        }
        if (objSpawned.Count > 0)
        {
            foreach (var item in objSpawned)
            {
                Destroy(item);
            }
        }
        
        objPool.Clear();
        objSpawned.Clear();
    }
}
