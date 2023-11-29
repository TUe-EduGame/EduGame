using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

[CreateAssetMenu(fileName = "newDialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Line[] lines;
}

[System.Serializable]
public class Line {
    [TextArea]
    public string text;
    public string type;
    public bool startsEvent;
    public string eventToStart;
    public bool hasOptions;
    public Options options;
    public bool canExit;
}

[System.Serializable]
public class Options {
    public string text;
    public Dialogue nextDialogue;
}
