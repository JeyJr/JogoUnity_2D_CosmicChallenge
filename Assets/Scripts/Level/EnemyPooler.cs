using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int maxPoolSize = 20;
    public float minDelayTime, maxDelayTime;

    public Transform spawnPosition;
    public Queue<GameObject> pooledObjects = new();

    private void Start()
    {
        InitializePool();
        SpawnObject();
    }

    private void InitializePool()
    {
        for (int i = 0; i < maxPoolSize; i++)
        {
            GameObject obj = Instantiate(asteroidPrefab, spawnPosition.position, Quaternion.identity);
            obj.SetActive(false);
            obj.GetComponent<EnemyBehavior>().enemyPooler = this;
            pooledObjects.Enqueue(obj);
        }
    }

    private async void SpawnObject()
    {
        while (true)
        {
            while (pooledObjects.Count == 0)
            {
                await Task.Delay(1000);
            }
            int min = (int)minDelayTime * 1000;
            int max = (int)maxDelayTime * 1000;

            await Task.Delay(Random.Range(min, max));

            GameObject obj = pooledObjects.Dequeue();
            obj.SetActive(true);
            obj.GetComponent<EnemyBehavior>().MaxSpeed = Random.Range(3, 10);

            float x = Random.Range(spawnPosition.position.x - 10, spawnPosition.position.x + 10);
            obj.transform.position = new Vector3(x, spawnPosition.position.y, spawnPosition.position.z);
        }
    }

    public void HandleCollision(GameObject obj)
    {
        obj.SetActive(false);
        pooledObjects.Enqueue(obj);
    }
}
