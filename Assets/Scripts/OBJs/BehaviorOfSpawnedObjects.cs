using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BehaviorOfSpawnedObjects : MonoBehaviour, ISetSpeedLimit
{
    [SerializeField] private Rigidbody rb;
    private float maxSpeed = 3;

    public void SetSpeedLimit(float maxSpeedLimit)
    {
        maxSpeed += maxSpeedLimit;
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();        
    }

    private void FixedUpdate()
    {
        SetSpeedLimit();
    }

    private void SetSpeedLimit()
    {
        Vector3 vel = rb.velocity;
        vel.y = Mathf.Clamp(vel.y, -maxSpeed, maxSpeed);
        rb.velocity = vel;
    }

}
