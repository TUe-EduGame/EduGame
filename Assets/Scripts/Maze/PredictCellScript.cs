using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PredictCellScript : MonoBehaviour
{
    public float scaleSpeed = 5f;
    private bool isShrinking = false;
    private PredictController controller;
    private SpriteRenderer renderer;
    private Vector3 center;
    public int id;
    public List<int> adj;

    // This function is called when the object becomes enabled and active.
    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start() {
        // Find the Predictcontroller
        GameObject obj = GameObject.FindGameObjectWithTag("PredictController");
        try {
            controller = obj.GetComponent<PredictController>();
        } catch (NullReferenceException e) {
            throw new Exception("PredictController is null");
        }
        
        // Give controller adjacency list
        foreach (int i in adj) {
            try {
                controller.AddNeighbor(id, i);
            } catch (NullReferenceException e) {
                throw new Exception("controller is null");
            }
        }

        // Find sprite renderer
        try {
            renderer = GetComponent<SpriteRenderer>();
        } catch (NullReferenceException e) {
            throw new Exception("SpriteRenderer of object " + name + " is null");
        }
        
        // Get the center position of the sprite
        if (renderer.sprite != null) {
            center = renderer.bounds.center;
        } else {
            throw new Exception("SpriteRenderer or Sprite is null.");
        }
        
        // Give the center position of the object to the controller
        controller.SetPos(id, center);
    }

    // Update is called once per frame
    void Update() {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate() {

    }

    // Called when the object is clicked
    void OnMouseDown() {
        controller.Click(id);
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

}
