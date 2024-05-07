using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveData
{
    public Vector3 playerLocation = new Vector3(0.5f, 0.5f, 0);
    public Vector3 playerRotation = new Vector3(0, 0, 0);
    public int gameState;
}

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 PlayerInput;
    public bool isMoving;
    public LayerMask solidObjectsLayer;
    public LayerMask waterLayer;
    public LayerMask interactableLayer;
    private Vector3 targetPos = new Vector3(0.5f, 0.5f, 0);
    private PlayerData playerData = new PlayerData();
    private SaveData saveData = new SaveData();

    private Animator animator;
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] walkingSounds;

    int soundClip = 0;

    public int gameState = 1;

    public NPC[] npcs;

    private void Awake()
    {
        Load();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public Tilemap obstacles;

    bool isInteracting = false;

    public void Save()
    {
        saveData.playerLocation = transform.position;
        saveData.playerRotation = transform.eulerAngles;
        saveData.gameState = gameState;
        string jsonData = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", jsonData);
    }

    void Load()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            string[] stream = System.IO.File.ReadAllLines(Application.persistentDataPath + "/PlayerData.json");
            string jsonData = string.Concat(stream);
            saveData = JsonUtility.FromJson<SaveData>(jsonData);

        }

        transform.position = saveData.playerLocation;
        transform.eulerAngles = saveData.playerRotation;
        gameState = saveData.gameState;

        foreach (NPC npc in npcs) 
        {
            npc.Activate();
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
            if (!isInteracting)
                Interact();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //PauseMenu
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/PlayerData.json");
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
            isInteracting = true;
            collider.GetComponent<Interactable>()?.Interact(this);
            Save();
        }
    }

    public void StopInteract()
    {
        isInteracting = false;
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
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer | waterLayer) != null)
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

[Serializable]
public class PlayerData
{
    public Vector3 savedLocation = new Vector3(0.5f, 0.5f, 0);
    public Vector3 savedRotation = new Vector3(0, 0, 0);
}