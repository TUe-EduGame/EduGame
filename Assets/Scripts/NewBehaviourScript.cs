using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public int position = 0;
    public float myFloat = 0.0f;
    public string myString = "Hello World";
    public bool myBool = true;
    
    private AudioSource audioSource;

    // This function is called when the object becomes enabled and active.
    void Awake() {
    
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, position, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + 0.01f, position, 0);
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate() {

    }


}
