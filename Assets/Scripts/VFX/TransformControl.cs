using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformControl : MonoBehaviour
{
    [SerializeField] private Vector3 rotateAxis;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAxis);
    }
}
