using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypadCharacterScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float scaleSpeed = 5.0f;
    // Whether the character is currently moving
    private bool isMoving = false;
    // The position the character starts in
    // [SerializeField] private float[] initialPosition = new float[3];
    private Vector3 initialPosition;
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
        transform.position = new Vector3(initialPosition[0], initialPosition[1], initialPosition[2]);
        transform.localScale = new Vector3(initialScale[0], initialScale[1], initialScale[2]);
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
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }

    // Shrinks the object to the targetScale
    public IEnumerator Shrink(Vector3 targetScale) {

        while ((targetScale - transform.localScale).sqrMagnitude > Mathf.Epsilon) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

    }

    public bool IsMoving() {
        return isMoving;
    }

    public void SetInitialPosition(Vector3 position) {
        initialPosition = position;
    }

}
