using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class VertexLocation : MonoBehaviour
{
    public VertexController controller;
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
