using System;
using UnityEngine;

public class NPC : Entity, Interactable
{
    public String nameNPC = "NPC";
    public Dialogue dialogue;

    public bool triggerDialogueOnStart = false;
    public bool dialogueStarted = false;
    public int progress = 0;

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
}
