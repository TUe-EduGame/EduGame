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
    private bool allowedToMove = true;
    private Queue<Vector3> queue = new Queue<Vector3>();

    // Start is called before the first frame update
    void Start() {
        transform.position = new Vector3(InitialPosition[0], InitialPosition[1], InitialPosition[2]);
    }

    // Update is called once per frame
    void Update()
    {
        // If the bullet is not moving and there are movements in the queue, start the next movement from the queue
        if (!IsMoving() && queue.Count > 0)
        {
            StartCoroutine(Move(queue.Dequeue()));
        }
    }

    // Moves the object to the position targetPos
    private IEnumerator Move(Vector3 targetPos) {
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

    // Shrinks the object to the targetScale
    public IEnumerator Shrink(Vector3 targetScale)
    {
        while ((targetScale - transform.localScale).sqrMagnitude > Mathf.Epsilon)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Adds a new move to the movement queue
    public void AddMove(Vector3 target) {
        // Only move if the bullet is allowed to
        if (allowedToMove) {    
            queue.Enqueue(target);
        }
    }

    // Returns whether the monster is currently moving
    public bool IsMoving()
    {
        return isMoving;
    }

    // Returns the bullet's current position
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // Resets the bullet to its initial state
    public void Restart() {
        transform.position = new Vector3(InitialPosition[0], InitialPosition[1], InitialPosition[2]);
        // Reset variables used to keep track of the bullet's state
        AllowMovement(true);
    }

    // Resets the bullet to its starting position
    public void Reset() {
        transform.position = new Vector3(InitialPosition[0], InitialPosition[1], InitialPosition[2]);
    }

    // Sets whether the bullet is allowed to move
    public void AllowMovement(bool allowed) {
        allowedToMove = allowed;
    }
}
