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
            controller.restart.AddListener(PassAdj);
        } catch (NullReferenceException e) {
            throw new Exception("PredictController is null");
        }
        
        // Give controller adjacency list
        PassAdj();

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

    // Called when the object is clicked
    void OnMouseDown() {
        controller.Click(id);
    }

    // Passes this cell's adjacency list to the controller
    private void PassAdj() {
        foreach (int i in adj) {
            try {
                controller.AddNeighbor(id, i);
            } catch (NullReferenceException e) {
                throw new Exception("controller is null");
            }
        }
    }
}
