using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
    public EnemyPooler enemyPooler;
    private Rigidbody rb;
    public float MaxSpeed { get; set; }

    public UnityEvent boxEvent;
    public List<GameObject> box;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            enemyPooler.HandleCollision(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            box.Add(collision.gameObject);
            boxEvent.Invoke();
        }
    }
    public void RemoveComponents()
    {
        StartCoroutine(Remove());
    }

    private IEnumerator Remove()
    {
        while (box.Count > 0)
        {
            for (int i = 0; i < box.Count; i++)
            {
                box[i].GetComponent<BoxBehavior>().DisableComponents();
                box.RemoveAt(i);
            }
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if(rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
        }
    }
}
