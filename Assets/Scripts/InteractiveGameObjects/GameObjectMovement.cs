using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMovement : MonoBehaviour
{
    public float MaxSpeed { get; set; }
    [SerializeField] private Rigidbody myRB;

    private void FixedUpdate()
    {
        if (myRB.velocity.magnitude > MaxSpeed)
        {
            myRB.velocity = Vector3.ClampMagnitude(myRB.velocity, MaxSpeed);
        }
    }
}
