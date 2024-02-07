using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LilypadScript : MonoBehaviour
{
    public float scaleSpeed = 5f;
    private bool isShrinking = false;
    private LilypadController controller;
    public int id;

    // This function is called when the object becomes enabled and active.
    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<LilypadController>();
        transform.position = controller.GetPosition(id);
        transform.localScale = controller.GetScale();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate() {

    }

    // Called when the object is clicked
    void OnMouseDown() {
        controller.MoveTo(id);
    }

    // Shrinks the object to the targetScale
    public IEnumerator Shrink(Vector3 targetScale) {
        isShrinking = true;

        while ((targetScale - transform.localScale).sqrMagnitude > Mathf.Epsilon) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

        isShrinking = false;
    }

    // Called when another GameObject collides with this one
    private void OnTriggerEnter2D(Collider2D other) {
        if (!controller.isAccessible(id)) {
            StartCoroutine(Shrink(new Vector3(0.001f, 0.001f, 0.001f)));
            controller.Shrink();
        } 
    }

}
