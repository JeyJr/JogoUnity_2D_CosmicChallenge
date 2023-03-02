using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class ItemCollisionControl : MonoBehaviour, IPoolReference
{
    private IReturnToPool returnToPool;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform playerPosition;

    private bool playerGrabbedItem = false;

    public void PoolReference(IReturnToPool pool)
    {
        returnToPool = pool;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerGrabbedItem)
        {
            playerGrabbedItem = true;

            playerPosition = other.GetComponent<Transform>();
            AddSpringJoint(other.GetComponent<Rigidbody>());

            GameEvent.GetInstance().CollectItems();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerGrabbedItem)
        {
            playerGrabbedItem = false;
            RemoveSpringJoint();

            GameEvent.GetInstance().RemoveItems();
        }

        else if (collision.gameObject.CompareTag("Ground"))
        {
            returnToPool.ReturnToPool(gameObject);
        }
    }

    private void AddSpringJoint(Rigidbody rbTarget)
    {
        SpringJoint spring = gameObject.AddComponent<SpringJoint>();

        spring.connectedBody = rbTarget;
        spring.spring = 10;
        spring.damper = 10;
        spring.minDistance = 3;
        spring.maxDistance = 10;
    }

    private void RemoveSpringJoint() => Destroy(GetComponent<SpringJoint>());

    private void Update()
    {
        lineRenderer.enabled = playerGrabbedItem;

        if (playerGrabbedItem && playerPosition != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, playerPosition.position);
        }
    }


}
