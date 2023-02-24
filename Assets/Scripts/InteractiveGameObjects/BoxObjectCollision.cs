using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxObjectCollision : MonoBehaviour, IPooledObject, IBoxInteractions
{
    private IPoolable myPool;

    private GameObject player;
    private LineRendererControl lineRendererControl;

    public void SetInitialValues(IPoolable pool)
    {
        myPool = pool;
        lineRendererControl = GetComponent<LineRendererControl>();
        DisableInteractionsWithTarget();
    }

    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            myPool.ReturnToPool(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            ActiveInteractionsWithTarget();
        }
    }

    public void DisableInteractionsWithTarget()
    {
        lineRendererControl.SetLineRendererEnabled(false, null);
        RemoveSpringJoint();
    }

    public void ActiveInteractionsWithTarget()
    {
        lineRendererControl.SetLineRendererEnabled(true, player.transform);
        AddSpringJointValues();
    }

    void RemoveSpringJoint() => Destroy(gameObject.GetComponent<SpringJoint>());

    void AddSpringJointValues()
    {
        var springJoint = gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = player.GetComponent<Rigidbody>();

        springJoint.minDistance = 0.2f;
        springJoint.maxDistance = Random.Range(1.5F, 3);

        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.anchor = new Vector3(0, 0, 0);
        springJoint.connectedAnchor = new Vector3(0, 0, 0);

        springJoint.spring = 3;
        springJoint.damper = 1;
    }
}
