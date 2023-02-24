using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public GameObject spawnEnemies;
    public GameObject spawnBox;

    public void StartSpawnEnemies()
    {
        if (spawnEnemies.TryGetComponent<ISpawnObjects>(out var enemy))
        {
            enemy.SetSpawnState(true);
        }

        if (spawnBox.TryGetComponent<ISpawnObjects>(out var box))
        {
            box.SetSpawnState(true);
        }
    }

    public void StopSpawnEnemies()
    {
        if (spawnEnemies.TryGetComponent<ISpawnObjects>(out var enemy))
        {
            enemy.SetSpawnState(false);
        }

        if (spawnBox.TryGetComponent<ISpawnObjects>(out var box))
        {
            box.SetSpawnState(false);
        }
    }
}
