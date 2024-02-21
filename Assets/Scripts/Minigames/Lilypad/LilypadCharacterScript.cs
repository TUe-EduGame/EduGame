using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LilypadCharacterScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float scaleSpeed = 5.0f;
    // The position the character starts in
    [SerializeField] private float[] initialPosition = new float[3];
    [SerializeField] private float[] initialScale = new float[3];

    // This function is called when the object becomes enabled and active.
    void Awake() {
    
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(initialPosition[0], initialPosition[1], initialPosition[2]);
        transform.localScale = new Vector3(initialScale[0], initialScale[1], initialScale[2]);
    }

    // Called to reset the character to its original position
    public void Reset() {
        // TODO
        // transform.position = new Vector3(initialPosition[0], initialPosition[1], initialPosition[2]);
        // transform.localScale = new Vector3(initialScale[0], initialScale[1], initialScale[2]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
