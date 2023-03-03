using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformControlOfSpawnedObjects : MonoBehaviour
{

    [Header("Position")]
    [SerializeField] private float startXPosition = 0;
    [SerializeField] private Vector2 position = new Vector2(10, 40);

    [Header("Scale and Rotation")]
    [SerializeField] private Vector2 randomScale = new Vector2(3, 10);
    [SerializeField] private Vector2 randomRot = new Vector2(0, 360);

    private void OnEnable()
    {
        SetInitialPosition();
        SetInitialScale();
        SetInitialRotation();
    }

    private void SetInitialPosition()
    {
        float x = Random.Range(startXPosition - position.x, startXPosition + position.x);
        transform.position = new Vector3(x, position.y, position.x);
    }

    private void SetInitialScale()
    {
        float scale = Random.Range(randomScale.x, randomScale.y);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void SetInitialRotation()
    {
        float rotation = Random.Range(randomRot.x, randomRot.y);
        transform.rotation = Quaternion.Euler(rotation, rotation, rotation);
    }
}
