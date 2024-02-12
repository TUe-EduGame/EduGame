using System;
using UnityEngine;

public class NPC : Entity, Interactable
{
    public String nameNPC;
    public Dialogue dialogue;
    public int progress = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact() {
        DialogueUI dialogueUI = GameObject.Find("Canvas").transform.GetChild(0).GetComponentInChildren<DialogueUI>();
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.StartDialogue(this);
    }
}
