using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class DialogueUI : MonoBehaviour
{
    private NPC npc;
    private Dialogue dialogue;

    [SerializeField] private TMP_Text nameField;
    [SerializeField] private TMP_Text dialogueField;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Image leftNPCImg;
    [SerializeField] private Image rightNPCImg;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Image graphics;
    private TMP_Text option1BtnTxt;
    private TMP_Text option2BtnTxt;
    private TMP_Text option3BtnTxt;

    private Sprite graphic;
    private GameObject graphic2;

    void Awake()
    {
        option1BtnTxt = optionsPanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
        option2BtnTxt = optionsPanel.transform.GetChild(1).GetComponentInChildren<TMP_Text>();
        option3BtnTxt = optionsPanel.transform.GetChild(2).GetComponentInChildren<TMP_Text>();
    }

    private void DisplayLine(int index)
    {
        bool hasOptions = dialogue.lines[index].hasOptions;
        bool hasGraphic = dialogue.lines[index].hasGraphic;

        if (hasOptions)
        {
            print(option1BtnTxt);
            option1BtnTxt.text = dialogue.lines[index].options[0].text;
            option2BtnTxt.text = dialogue.lines[index].options[1].text;
            option3BtnTxt.text = dialogue.lines[index].options[2].text;
        }

        optionsPanel.SetActive(hasOptions);
        nextBtn.gameObject.SetActive(!hasOptions);
        dialogueField.text = dialogue.lines[index].text;

        if (hasGraphic)
        {
            graphic = dialogue.lines[index].graphic;
            graphics.GetComponent<Image>().sprite = graphic;
            graphics.gameObject.SetActive(hasGraphic);
        }

    }

    public void StartDialogue(NPC npc)
    {
        this.npc = npc;
        dialogue = npc.dialogue;

        exitBtn.gameObject.SetActive(dialogue.canExit);
        nameField.text = this.npc.nameNPC;

        DisplayLine(0);
    }

    public void NextDialogue()
    {
        bool hasGraphic = dialogue.lines[npc.progress].hasGraphic;
        if (hasGraphic)
        {
            graphic = dialogue.lines[npc.progress].graphic;
            graphics.GetComponent<Image>().sprite = graphic;
            graphics.gameObject.SetActive(!hasGraphic);
        }
        
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
