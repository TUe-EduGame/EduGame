using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeController : MonoBehaviour
{
    public GraphGameController controller;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(11, 3, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //If all vertices are placed, move edge to its location
    public void Move(Vector3 targetPos) 
    {
        if(controller.vertexCounter == controller.numberOfVertices) 
        {
            if(targetPos != transform.position) 
            {
                controller.EdgeCounterUpdate();
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Vector3.Distance(transform.position, targetPos));
        }
    }
}
