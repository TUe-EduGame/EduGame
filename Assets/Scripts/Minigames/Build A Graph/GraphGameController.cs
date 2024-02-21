using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGameController : MonoBehaviour
{
    public int vertexCounter = 0;
    public int edgeCounter = 0;
    public int numberOfVertices;
    public int numberOfEdges;

    // Start is called before the first frame update
    void Start()
    {
        vertexCounter = 0;
        edgeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VertexCounterUpdate()
    {
        vertexCounter ++;
    }

    public void EdgeCounterUpdate()
    {
        edgeCounter ++;
    }
}
