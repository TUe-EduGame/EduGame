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
        if (vertexCounter == numberOfVertices)
        {
            HalfWay();
        }
    }

    public void EdgeCounterUpdate()
    {
        edgeCounter ++;
        if (edgeCounter == numberOfEdges)
        {
            Win();
        }
    }

    public void HalfWay() {
        GameObject winNPC = GameObject.Find("MiddleNPC");
        winNPC.GetComponent<NPC>().Interact();
    }
    public void Win() {
        GameObject winNPC = GameObject.Find("EndNPC");
        winNPC.GetComponent<NPC>().Interact();
    }

}
