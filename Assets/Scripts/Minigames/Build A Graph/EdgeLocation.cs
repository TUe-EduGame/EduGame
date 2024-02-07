using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeLocation : MonoBehaviour
{
    public EdgeController controller;
    private Vector3 position;
    
    
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Move the corresponding vertex to this location
    void OnMouseDown() {
        controller.Move(position);
    }
}
