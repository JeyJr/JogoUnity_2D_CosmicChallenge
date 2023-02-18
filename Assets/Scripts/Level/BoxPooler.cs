using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BoxPooler : MonoBehaviour
{
    public GameObject boxPrefab;
    public int MaxPoolSize { get; set;}
    public float minDelayTime, maxDelayTime;

    public Transform spawnPosition;
    public Queue<GameObject> pooledObjects = new();

    private void Start()
    {
        InitializePool();
        SpawnObject();
    }

    //Pool sera inicializada quando o player definir a dificuldade
    public void InitializePool()
    {
        if(MaxPoolSize <= 0)
        {
            MaxPoolSize = 20;
        }

        GameObject player = GameObject.FindWithTag("Player");
        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        Transform playerPos = player.GetComponent<Transform>();

        for (int i = 0; i < MaxPoolSize; i++)
        {
            GameObject obj = Instantiate(boxPrefab, spawnPosition.position, Quaternion.identity);
            
            obj.SetActive(false);
            obj.GetComponent<BoxBehavior>().SetInitialValues(this, playerRB);
            obj.GetComponent<BoxBehavior>().DisableComponents();
            obj.GetComponent<LineRendererControl>().pos0 = playerPos;

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
