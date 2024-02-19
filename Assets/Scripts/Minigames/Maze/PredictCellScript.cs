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
    private PredictController controller;
    private SpriteRenderer spriteRenderer;
    private Vector3 center;
    public int id;
    public List<int> adj;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find the Predictcontroller
        GameObject obj = GameObject.FindGameObjectWithTag("PredictController");
        try
        {
            controller = obj.GetComponent<PredictController>();
        }
        catch (NullReferenceException)
        {
            throw new Exception("PredictController is null\n");
        }

        // Give controller adjacency list
        foreach (int i in adj)
        {
            try
            {
                controller.AddNeighbor(id, i);
            }
            catch (NullReferenceException)
            {
                throw new Exception("controller is null\n");
            }
        }

        // Find sprite renderer
        try
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        catch (NullReferenceException)
        {
            throw new Exception("SpriteRenderer of object " + name + " is null\n");
        }

        // Get the center position of the sprite
        if (spriteRenderer.sprite != null)
        {
            center = spriteRenderer.bounds.center;
        }
        else
        {
            throw new Exception("SpriteRenderer or Sprite is null.");
        }

        // Give the center position of the object to the controller
        controller.SetPos(id, center);
    }

    // Called when the object is clicked
    void OnMouseDown()
    {
        controller.Click(id);
    }

    // Shrinks the object to the targetScale
    public IEnumerator Shrink(Vector3 targetScale)
    {
        while ((targetScale - transform.localScale).sqrMagnitude > Mathf.Epsilon)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
