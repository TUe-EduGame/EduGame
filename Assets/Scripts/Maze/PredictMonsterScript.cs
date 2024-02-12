using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PredictMonsterScript : MonoBehaviour
{
    private Animator animator;
    // The number of lives the monster starts with
    [SerializeField] private int initialLives = 10;
    // The number of lives the monster currently has
    private int lives;
    [SerializeField] private float[] initialPosition = new float[3];
    [SerializeField] private float[] finalPosition = new float[3];
    [SerializeField] private float[] initialScale = new float[3];
    [SerializeField] private float[] finalScale = new float[3];
    [SerializeField] private float moveTime = 2.0f;
    [SerializeField] private float scaleSpeed = 5.0f;
    private bool isMoving = false;
    private bool isShrinking = false;
    private Queue<Vector3> queue = new Queue<Vector3>();
    private bool allowedToMove = true;
    private bool rage = false;
    private SpriteRenderer renderer;
    // Event that notifies other scripts that the monster finished changing its scale
    public UnityEvent shrunk;

    // This function is called when the object becomes enabled and active.
    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(initialPosition[0], initialPosition[1], initialPosition[2]);
        transform.localScale = new Vector3(initialScale[0], initialScale[1], initialScale[2]);
        animator = GetComponent<Animator>();
        animator.SetFloat("Alive", 1);
        renderer = GetComponent<SpriteRenderer>();
        lives = initialLives;
    }

    // Puts the monster back at its starting position, scale and animation
    public void Restart() {
        transform.position = new Vector3(initialPosition[0], initialPosition[1], initialPosition[2]);
        transform.localScale = new Vector3(initialScale[0], initialScale[1], initialScale[2]);
        animator.SetFloat("Alive", 1);
        // Reset the variables keeping track of the monster's state
        allowMovement(true);
        lives = initialLives;
        rage = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the monster is not moving and there are movements in the queue, start the next movement from the queue
        if (!IsMoving() && queue.Count > 0) {
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

        isMoving = false;
        if (rage && queue.Count == 0) {
            SetRage(false);
            Rage();
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
        shrunk.Invoke();
    }

    // Called upon a collision
    public void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("PredictBullet") && !isMoving) {
            lives--;
            Debug.Log(lives);
        }
    }

    // Adds a new move to the movement queue
    public void AddMove(Vector3 target) {
        if (allowedToMove) {
            queue.Enqueue(target);            
        }
    }

    // Returns whether the monster is currently moving
    public bool IsMoving() {
        return isMoving;
    }

    // Returns the remaining lives
    // If it returns 0, the monster is (already) dead
    public int Lives() {
        return lives;
    }

    // Sets whether the monster is allowed to move
    public void allowMovement(bool allowed) {
        allowedToMove = allowed;
    }

    // Sets whether the monster can rage
    public void SetRage(bool toRage) {
        rage = toRage;
    }

    // Start the dying animation
    public void Die() {
        if (lives <= 0) {
            animator.SetFloat("Alive", 0.5f);
        } else {
            Debug.LogWarning("Monster dies when lives > 0");
        }
    }

    // Start the dead animation
    public void Dead() {
        if (lives <= 0) {
            animator.SetFloat("Alive", 0);
        } else {
            Debug.LogWarning("Monster died when lives = " + lives);
        }
    }

    // Start the raging animation
    private void Rage() {
        if (lives > 0) {
            animator.SetFloat("Alive", 1.5f);
            StartCoroutine(Move(new Vector3(finalPosition[0], finalPosition[1], finalPosition[2])));
            StartCoroutine(Shrink(new Vector3(finalScale[0], finalScale[1], finalScale[2])));
            renderer.sortingOrder = 3;
        }
    }
}
