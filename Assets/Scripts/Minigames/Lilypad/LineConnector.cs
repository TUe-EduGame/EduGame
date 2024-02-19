using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public List<GameObject> vertices;

    private LineRenderer lineRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // Ensure that it draws a line between only 2 points
        lineRenderer.positionCount = vertices.Count;

        for (int i = 0; i < vertices.Count - 1; i++) {
            GameObject startpoint = vertices[i];
            GameObject endpoint = vertices[i + 1];
            if (startpoint != null && endpoint != null) {
                lineRenderer.SetPosition(i, startpoint.transform.position);
                lineRenderer.SetPosition(i + 1, endpoint.transform.position);
            } else {
                throw new NullReferenceException("Vertex " + i + " or " + (i + 1) + " is null");
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
