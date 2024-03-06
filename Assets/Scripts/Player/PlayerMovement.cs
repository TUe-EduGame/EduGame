using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 PlayerInput;
    public bool isMoving;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    private Vector3 targetPos = new Vector3(0.5f, 0.5f, 0);

    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] walkingSounds;

    private Vector3 savedLocation = new Vector3(0.5f, 0.5f, 0);

    int soundClip = 0;

    private void Awake()
    {
        Load();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        transform.position = savedLocation;
    }

    public Tilemap obstacles;

    //TODO: Move from here and make a proper class in the future
    void Save()
    {
        savedLocation = transform.position;
        string playerData = JsonUtility.ToJson(savedLocation);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", playerData);
    }

    void Load()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            string[] stream = System.IO.File.ReadAllLines(Application.persistentDataPath + "/PlayerData.json");
            string playerData = string.Concat(stream);
            savedLocation = JsonUtility.FromJson<Vector3>(playerData);
        }
    }

    void Start()
    {
        moveSpeed = 4f;
    }

    void Update()
    {
        if (!isMoving)
        {
            PlayerInput.x = Input.GetAxisRaw("Horizontal");
            PlayerInput.y = Input.GetAxisRaw("Vertical");

            if (PlayerInput.x != 0) PlayerInput.y = 0;

            if (PlayerInput != Vector2.zero)
            {
                animator.SetFloat("moveX", PlayerInput.x);
                animator.SetFloat("moveY", PlayerInput.y);

                targetPos = transform.position + new Vector3(PlayerInput.x, PlayerInput.y, 0);

                Vector3Int obstacleMap = obstacles.WorldToCell(targetPos);
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //PauseMenu
        }

    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"), 0);
        var interactPos = transform.position + facingDir;

        // Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }



    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        audioSource.clip = walkingSounds[soundClip];
        audioSource.Play();
        soundClip = (soundClip + UnityEngine.Random.Range(1, walkingSounds.Length - 1)) % walkingSounds.Length;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPos, 0.2f);
    }
}