using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexController : MonoBehaviour
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

    //Move vertex to its location, and update counter if neccessary
    public void Move(Vector3 targetPos) 
    {
        if(targetPos != transform.position) 
        {
            controller.CounterUpdate();
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Vector3.Distance(transform.position, targetPos));
    }
}
