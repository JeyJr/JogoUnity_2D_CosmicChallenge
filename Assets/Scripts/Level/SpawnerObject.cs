using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObject : MonoBehaviour
{
    public bool spawn;
    public float x;

    public float delayToSpawnEnemies;
    public GameObject boxPrefab;

    void Start()
    {
        StartCoroutine(SpawnBox());
    }

    IEnumerator SpawnBox()
    {
        while (true)
        {
            if (spawn)
            {
                float xPosition = Random.Range(transform.position.x - x, transform.position.x + x);
                Instantiate(boxPrefab, new Vector3(xPosition, transform.position.y, transform.position.z), Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(2,5));
        }
    }
}
