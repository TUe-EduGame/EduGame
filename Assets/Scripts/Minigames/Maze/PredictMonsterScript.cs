using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictMonsterScript : MonoBehaviour
{
    [SerializeField] private int lives = 10;
    public float[] InitialPosition = new float[3];
    public float moveTime = 2.0f;
    public float scaleSpeed = 5.0f;
    private bool isMoving = false;
    private Queue<Vector3> queue = new Queue<Vector3>();
    private bool allowedToMove = true;

    // This function is called when the object becomes enabled and active.
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(InitialPosition[0], InitialPosition[1], InitialPosition[2]);
    }

    // Update is called once per frame
    void Update()
    {
        // If the monster is not moving and there are movements in the queue, start the next movement from the queue
        if (!IsMoving() && queue.Count > 0)
        {
            StartCoroutine(Move(queue.Dequeue()));
        }
    }

    // Moves the object to the position targetPos
    private IEnumerator Move(Vector3 targetPos)
    {
        if (allowedToMove)
        {
            isMoving = true;
            float moveSpeed = Vector3.Distance(transform.position, targetPos) / moveTime;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            isMoving = false;
        }
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

    // Called upon a collision
    public void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("PredictBullet") && !isMoving)
        {
            lives--;
            Debug.Log(lives);
        }
    }

    // Adds a new move to the movement queue
    public void AddMove(Vector3 target)
    {
        queue.Enqueue(target);
    }

    // Returns whether the monster is currently moving
    public bool IsMoving()
    {
        return isMoving;
    }

    // Returns the remaining lives
    // If it returns 0, the monster is (already) dead
    public int Lives()
    {
        return lives;
    }

    // Sets whether the monster is allowed to move
    public void allowMovement(bool allowed)
    {
        allowedToMove = allowed;
    }
}
