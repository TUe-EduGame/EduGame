using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : Entity
{
    public String nameNPC;
    public Dialogue dialogue;
    public int progress = 0;

    // Start is called before the first frame update
    void Start()
    {
        DialogueUI dialogueUI = GameObject.Find("DialogueUI").GetComponent<DialogueUI>();
        dialogueUI.StartDialogue(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
