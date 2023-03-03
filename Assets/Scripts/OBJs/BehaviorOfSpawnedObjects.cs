using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BehaviorOfSpawnedObjects : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speedLimit = 0;
    [SerializeField] private float speedLimitValueBaseCal = 3;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        speedLimit = GameController.GetInstance().GetMoveSpeed(speedLimitValueBaseCal);
    }

    private void FixedUpdate()
    {
        SetSpeedLimit();
    }

    private void SetSpeedLimit()
    {
        Vector3 vel = rb.velocity;
        vel.y = Mathf.Clamp(vel.y, -speedLimit, speedLimit);
        rb.velocity = vel;
    }

}
