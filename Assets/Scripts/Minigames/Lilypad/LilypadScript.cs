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
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int id;
    [SerializeField] private Sprite emptyLilypad;
    [SerializeField] private Sprite budLilypad;
    [SerializeField] private Sprite bloomLilypad;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<LilypadController>();
        controller.restart.AddListener(Reset);
        transform.position = controller.GetPosition(id);
        transform.localScale = controller.GetScale();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Called to delete a lilypad when making a new graph
    public void Reset() {
        Destroy(gameObject);
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
        if (other.CompareTag("Player") && controller.CurrentTarget() == id) {
            if (!controller.isAccessible(id))
            {
                StartCoroutine(Shrink(new Vector3(0.001f, 0.001f, 0.001f)));
                controller.Lose();
            } else {
                if (controller.Finished()) {
                    controller.Win();
                    ChangeSprite(2);
                } else {
                    ChangeSprite(1);
                }
            }
        }
        
    }

    // Called when another Gameobject leaves this one
    private void OnTriggerExit2D(Collider2D other) {
        ChangeSprite(controller.GetSprite(id));
    }

    // Set the id of this lilypad
    public void SetId(int id) {
        this.id = id;
    }

    // Sets the sprite of this lilypad
    public void ChangeSprite(int sprite) {
        if (sprite == 2) {
            spriteRenderer.sprite = bloomLilypad;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
            spriteRenderer.sprite = budLilypad;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }
}
