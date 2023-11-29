using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueUI : MonoBehaviour
{
    private NPC npc;
    private Dialogue dialogue;

    [SerializeField] private TMP_Text nameField;
    [SerializeField] private TMP_Text dialogueField;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image leftNPCImage;
    [SerializeField] private Image rightNPCImage;
    [SerializeField] private GameObject optionsPanel;

    void Awake()
    {

    }

    private void DisplayLine(int index)
    {
        bool hasOptions = dialogue.lines[index].hasOptions;
        
        optionsPanel.SetActive(hasOptions);
        nextButton.gameObject.SetActive(!hasOptions);
        dialogueField.text = dialogue.lines[index].text;
    }

    public void StartDialogue(NPC npc)
    {
        this.npc = npc;
        dialogue = npc.dialogue;

        exitButton.gameObject.SetActive(dialogue.canExit);
        nameField.text = this.npc.nameNPC;

        DisplayLine(0);
    }

    public void NextDialogue()
    {
        if (npc.progress == dialogue.lines.Length - 1)
        {
            CloseDialogue();
            return;
        }

        DisplayLine(++npc.progress);
    }

    public void Option1()
    {
        print("Option1");
        CloseDialogue();
    }

    public void Option2()
    {
        print("Option2");
        CloseDialogue();
    }

    public void Option3()
    {
        print("Option3");
        CloseDialogue();
    }

    public void CloseDialogue()
    {
        gameObject.SetActive(false);
    }
}
