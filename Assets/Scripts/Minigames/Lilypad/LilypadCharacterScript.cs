using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypadCharacterScript : MonoBehaviour
{

    public int position = 0;
    public float moveSpeed = 5.0f;
    public float scaleSpeed = 5.0f;
    public string myString = "Hello World";
    
    private AudioSource audioSource;

    // This function is called when the object becomes enabled and active.
    void Awake() {
    
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-6, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate() {

    }

    // Moves the object to the position targetPos
    public IEnumerator Move(Vector3 targetPos) {

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

    }

    // Shrinks the object to the targetScale
    public IEnumerator Shrink(Vector3 targetScale) {

        while ((targetScale - transform.localScale).sqrMagnitude > Mathf.Epsilon) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

    }

}
