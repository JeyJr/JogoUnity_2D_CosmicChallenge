using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererControl : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private Transform targetPosition;
    private bool enableLineRenderer;

    public void SetLineRendererEnabled(bool state, Transform targetPosition)
    {
        this.targetPosition = targetPosition;
        enableLineRenderer = state;


        lineRenderer.enabled = enableLineRenderer;
        enabled = enableLineRenderer;
    }

    private void Update()
    {
        if (enableLineRenderer && targetPosition != null)
        {
            lineRenderer.SetPosition(0, targetPosition.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
