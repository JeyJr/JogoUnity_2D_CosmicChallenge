using System.Collections;
using UnityEngine;

public interface IPoolable
{
    public void AddToPool(GameObject obj);
    public void ReturnToPool(GameObject obj);
    public void ExitPool();
    IEnumerator RestartPool();
}
