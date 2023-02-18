using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererControl : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform pos0, pos1;

    private void Start()
    {        
        lineRenderer = GetComponent<LineRenderer>();
        pos1 = GetComponent<Transform>();
    }

    private void Update()
    {
        if(pos0 != null && pos1 != null)
        {
            lineRenderer.SetPosition(0, pos0.position);
            lineRenderer.SetPosition(1, pos1.position);
        }
    }
}
