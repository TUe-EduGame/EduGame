using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueUI : MonoBehaviour
{
    NPC npc;
    TMP_Text nameField;
    TMP_Text dialogueField;
    Button nextButton;
    TMP_Text nextButtonTMPro;

    void Awake() {
        nameField = transform.GetChild(1).GetComponent<TMP_Text>();
        nextButton = transform.GetChild(2).GetComponent<Button>();
        dialogueField = transform.GetChild(3).GetComponent<TMP_Text>();
        nextButtonTMPro = nextButton.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void StartDialogue(NPC npc)
    {   
        this.npc = npc; 
        nameField.text = this.npc.nameNPC;
        dialogueField.text = this.npc.dialogue.lines[npc.progress].text;
    }

    public void NextDialogue()
    {
        if(npc.progress == npc.dialogue.lines.Length - 1) {
            CloseDialogue();
            return;
        }

        npc.progress++;
        dialogueField.text = npc.dialogue.lines[npc.progress].text;
    }

    public void CloseDialogue()
    {
        nextButtonTMPro.text = "Next";
        gameObject.SetActive(false);
    }
}
