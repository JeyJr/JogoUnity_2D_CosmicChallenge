using UnityEngine;

public class GameObjectMovement : MonoBehaviour
{
    private float maxSpeed;
    public float MaxSpeed { 
        
        get => maxSpeed; 
        set 
        { 
            if(enemy)
            {
                maxSpeed = 3 + value;
            }
            else
            {
                maxSpeed = value;
            }
        } 
    }

    
    [SerializeField] private Rigidbody myRB;
    [SerializeField] private bool enemy;


    private void FixedUpdate()
    {
        if (myRB.velocity.magnitude > MaxSpeed)
        {
            myRB.velocity = Vector3.ClampMagnitude(myRB.velocity, MaxSpeed);
        }
    }
}
