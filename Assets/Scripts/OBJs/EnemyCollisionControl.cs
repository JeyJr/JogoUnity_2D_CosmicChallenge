using UnityEngine;

public class EnemyCollisionControl : MonoBehaviour, IPoolReference
{
    private IReturnToPool returnToPool;

    public void PoolReference(IReturnToPool pool)
    {
        returnToPool = pool;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            returnToPool.ReturnToPool(gameObject);
        }
    }
}
