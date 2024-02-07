using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PredictBulletScript : MonoBehaviour
{
    public UnityEvent OnBulletHit;
    public float[] InitialPosition = new float[3];
    public float moveTime = 2.0f;
    public float scaleSpeed = 5.0f;
    private bool isMoving = false;
    private bool isShrinking = false;
    private bool allowedToMove = true;
    private Queue<Vector3> queue = new Queue<Vector3>();

    // This function is called when the object becomes enabled and active.
    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(InitialPosition[0], InitialPosition[1], InitialPosition[2]);
    }

    // Update is called once per frame
    void Update()
    {
        // If the bullet is not moving and there are movements in the queue, start the next movement from the queue
        if (!IsMoving() && queue.Count > 0) {
            StartCoroutine(Move(queue.Dequeue()));
        }
    }

    // Moves the object to the position targetPos
    private IEnumerator Move(Vector3 targetPos) {
        // Only move if the bullet is allowed to
        if (allowedToMove) {
            isMoving = true;
            float moveSpeed = Vector3.Distance(transform.position, targetPos) / moveTime;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Once the target has been reached, send a message that the bullet has reached its target
            OnBulletHit.Invoke();

            isMoving = false;
        } 
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

    // Adds a new move to the movement queue
    public void AddMove(Vector3 target) {
        queue.Enqueue(target);
    }

    // Returns whether the monster is currently moving
    public bool IsMoving() {
        return isMoving;
    }

    // Returns the bullet's current position
    public Vector3 GetPosition() {
        return transform.position;
    }

    // Resets the bullet to its start position
    public void Reset() {
        transform.position = new Vector3(InitialPosition[0], InitialPosition[1], InitialPosition[2]);
    }

    // Sets whether the bullet is allowed to move
    public void allowMovement(bool allowed) {
        allowedToMove = allowed;
    }
}
