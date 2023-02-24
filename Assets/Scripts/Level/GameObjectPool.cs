using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour, IPoolable, ISpawnObjects
{
    [Header("Game Object")]
    [SerializeField] private GameObject prefab;

    [Header("Pool Setup")]
    [SerializeField] private int poolSize = 10;
    private Queue<GameObject> objPool = new();
    private List<GameObject> objActivated = new();

    [Header("Spawn Setup")]
    [SerializeField] private float delayTimeToActive = 2;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Vector2 randomRange = new Vector2(-10, 10);
    [SerializeField] private bool spawn = false;

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AddToPool(Instantiate(prefab));
        }
    }

    public void AddToPool(GameObject obj)
    {
        obj.SetActive(false);
        objPool.Enqueue(obj);

        if(obj.TryGetComponent<IPooledObject>(out var objectBehavior))
        {
            objectBehavior.SetInitialValues(this);
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        objPool.Enqueue(obj);

        if(objActivated.Contains(obj))
            objActivated.Remove(obj);
    }

    public void ExitPool()
    {
        GameObject obj = objPool.Dequeue();

        if (!objActivated.Contains(obj))
            objActivated.Add(obj);

        obj.SetActive(true);
        float x = Random.Range(randomRange.x, randomRange.y);
        obj.transform.position = new Vector3(x, spawnPosition.position.y, spawnPosition.position.z);
        obj.GetComponent<GameObjectMovement>().MaxSpeed = Random.Range(3, 6);
    }

    public IEnumerator RestartPool()
    {
        float delayTime = delayTimeToActive / 2;
        yield return new WaitForSeconds(delayTime);

        while (objActivated.Count > 0)
        {
            for (int i = 0; i < objActivated.Count; i++)
            {
                ReturnToPool(objActivated[i]);
            }
        }
        objActivated.Clear();
        Debug.Log("Restart!"); //Remover
    }

    public void SetSpawnState(bool spawnState)
    {
        this.spawn = spawnState;

        if (spawn)
        {
            StartCoroutine(SpawnLoop());
        }
    }

    public IEnumerator SpawnLoop()
    {
        while (spawn)
        {
            if(objPool.Count > 0)
            {
                yield return new WaitForSeconds(delayTimeToActive);
                ExitPool();
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("EndLoop!"); //Remover
        StartCoroutine(RestartPool());
    }


}

