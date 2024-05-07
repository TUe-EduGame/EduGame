using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : Entity, Interactable
{
    public string nameNPC = "NPC";
    public Dialogue dialogue;

    public bool triggerDialogueOnStart = false;
    public bool dialogueStarted = false;
    public int progress = 0;

    public bool hasSuccessor = false;
    public NPC successor;

    public bool changeScene = false;
    public string nextScene;

    public int stateNumber = 0;

    public bool appear = false;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        

        if (triggerDialogueOnStart && !dialogueStarted)
        {
            DialogueUI dialogueUI = GameObject.Find("Canvas").transform.GetChild(0).GetComponentInChildren<DialogueUI>();
            dialogueUI.gameObject.SetActive(true);
            dialogueUI.StartDialogue(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (!dialogueStarted)
        {
            DialogueUI dialogueUI = GameObject.Find("Canvas").transform.GetChild(0).GetComponentInChildren<DialogueUI>();
            dialogueUI.gameObject.SetActive(true);
            dialogueUI.StartDialogue(this);
        }
    }

    public void Interact(Player player)
    {
        this.player = player; 
        Interact();
    }

    public void EndDialogue()
    {
        if(player != null)
            player.StopInteract();
    }

    public void Activate() 
    {
        if (player.gameState >= stateNumber)
        {
            print(name + " should activate");
            gameObject.SetActive(appear);
        }
    }

    public void UpdateGameState()
    {
        player.gameState = math.max(player.gameState, stateNumber + 1);
        player.Save();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
