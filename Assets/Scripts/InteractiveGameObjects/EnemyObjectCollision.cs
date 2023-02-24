using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyObjectCollision : MonoBehaviour, IPooledObject
{
    private IPoolable myPool;

    private List<IBoxInteractions> target = new();

    public void SetInitialValues(IPoolable pool)
    {
        myPool = pool;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var gameObject = collision.gameObject;

        if(gameObject != null)
        {
            if (gameObject.tag == "Ground")
            {
                myPool.ReturnToPool(this.gameObject);
            }

            if (gameObject.TryGetComponent<IBoxInteractions>(out var targetInteractions))
            {
                target.Add(targetInteractions);
                StartCoroutine(DisableConnections());
            }
        }
    }

    IEnumerator DisableConnections()
    {
        while (target.Count > 0)
        {
            for (int i = 0; i < target.Count; i++)
            {
                target[i].DisableInteractionsWithTarget();
                target.RemoveAt(i);
            }
            yield return null;
        }
    }
}
