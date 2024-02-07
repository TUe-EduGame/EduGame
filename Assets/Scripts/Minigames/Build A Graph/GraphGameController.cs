using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGameController : MonoBehaviour
{
    public int counter = 0;
    public int numberOfVertices;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CounterUpdate()
    {
        counter ++;
    }
}
