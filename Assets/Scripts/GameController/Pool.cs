using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsável por gerenciar um pool de objetos e realizar spawns controlados durante a partida.
/// </summary>
public class Pool : MonoBehaviour, IStartSpawnObj, IReturnToPool
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private List<GameObject> objPrefab = new();

    [SerializeField] private float rangeAxisXtoSpawn = 10;
    [SerializeField] private int maxObjInPool = 10;
    [SerializeField] private float delayToSpawnObj = 3;
    [SerializeField] private float speedLimitValue = 3;

    private Queue<GameObject> objPool = new Queue<GameObject>();
    private List<GameObject> objSpawned = new List<GameObject>();
    
    
    private void Start()
    {
        GameEvent.GetInstance().OnStartMatch += StartSpawnObj;
        GameEvent.GetInstance().OnEndGame += ClearPool;
    }

    public void StartSpawnObj()
    {
        StartCoroutine(InitializePool());

        speedLimitValue = GameController.GetInstance().GetMoveSpeed(speedLimitValue);
        delayToSpawnObj = GameController.GetInstance().GetSpawnSpeed(delayToSpawnObj);
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
                obj.SetActive(false);

                //Atribuir o novo valor de limite de velocidade conforme o modo escolhido
                if (obj.TryGetComponent(out ISetSpeedLimit setSpeedLimit))
                {
                    setSpeedLimit.SetSpeedLimit(speedLimitValue);
                }
                else
                {
                    Debug.LogError($"Prefab não implementa interface ISetSpeedLimit! {objPrefab}");
                }

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
        yield return new WaitForSeconds(delayToSpawnObj);

        while (GameController.GetInstance().GameState == GameState.StartMatch)
        {
            try
            {
                if(objPool != null && objPool.Count > 0)
                {
                    GameObject obj = objPool.Dequeue();

                    float x = Random.Range(spawnPosition.position.x - rangeAxisXtoSpawn, spawnPosition.position.x + rangeAxisXtoSpawn);
                    obj.transform.position = new Vector3(x, 40, 10);
                    obj.SetActive(true);
                    objSpawned.Add(obj);
                }
            }
            catch
            {
                Debug.Log("Pool empty!");
                break;
            }

            yield return new WaitForSeconds(delayToSpawnObj);
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
