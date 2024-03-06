using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public List<Vector3> vertices;
    private RadialTree graph;
    private LineRenderer lineRenderer;
    private bool[] visited;         // Used in DFS
    private Vector3[] positions;    // Locations of the vertices
    
    // Start is called before the first frame update
    void Start()
    {
        vertices = DFS(graph.GetRoot(), new List<Vector3>());
        lineRenderer = GetComponent<LineRenderer>();
        // Ensure that it draws a line between only 2 points
        lineRenderer.positionCount = vertices.Count;

        for (int i = 0; i < vertices.Count - 1; i++) {
            Vector3 startpoint = vertices[i];
            Vector3 endpoint = vertices[i + 1];
            if (startpoint != null && endpoint != null) {
                lineRenderer.SetPosition(i, startpoint);
                lineRenderer.SetPosition(i + 1, endpoint);
            } else {
                throw new NullReferenceException("Vertex " + i + " or " + (i + 1) + " is null");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Gives the graph to be drawn to the lineconnector
    public void SetGraph(RadialTree graph) {
        this.graph = graph;
        visited = new bool[graph.GetNrOfNodes()];
        positions = graph.GetPositions();
    }

    // Returns a list of all start- and endpoints of edges in this graph
    // by doing an in-order tree traversal using DFS
    private List<Vector3> DFS(int current, List<Vector3> edgepoints) {
        edgepoints.Add(positions[current]);
        if (!visited[current]) {
            visited[current] = true;
            foreach (int neighbor in graph.GetNeighbors(current)) {
                DFS(neighbor, edgepoints);
                edgepoints.Add(positions[current]);
            }
        }
        return edgepoints;
    }
}
