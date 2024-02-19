using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LilypadScript : MonoBehaviour
{
    public float scaleSpeed = 5f;
    private LilypadController controller;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<LilypadController>();
        controller.restart.AddListener(Reset);
        transform.position = controller.GetPosition(id);
        transform.localScale = controller.GetScale();
    }

    // Called to reset a lilypad to its original size
    public void Reset() {
        transform.localScale = controller.GetScale();
    }

    // Called when the object is clicked
    void OnMouseDown()
    {
        controller.MoveTo(id);
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

    // Called when another GameObject collides with this one
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!controller.isAccessible(id))
        {
            StartCoroutine(Shrink(new Vector3(0.001f, 0.001f, 0.001f)));
            controller.Lose();
        } else {
            if (controller.Finished()) {
                controller.Win();
            }
        }
    }

}
