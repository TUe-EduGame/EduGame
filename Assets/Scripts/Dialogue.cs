using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

[CreateAssetMenu(fileName = "newDialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Line[] lines;
    public Dialogue nextDialogue;
    public bool canExit = false;
    // public bool startsEvent;
    // public string eventToStart;
}

[System.Serializable]
public class Line {
    [TextArea]
    public string text;
    public bool hasOptions;
    public Option[] options;
}

[System.Serializable]
public class Option {
    public string text;
    public Dialogue nextDialogue;
}
