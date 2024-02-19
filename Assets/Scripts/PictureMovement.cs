using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveInScreen() 
    {
        Vector3 goal = new Vector3(0,0,0);
        transform.position = Vector3.MoveTowards(transform.position, goal, 50);
    }

    public void MoveOutOfScreen() 
    {
        Vector3 goal = new Vector3(-50,0,0);
        transform.position = Vector3.MoveTowards(transform.position, goal, 50);
    }
}
