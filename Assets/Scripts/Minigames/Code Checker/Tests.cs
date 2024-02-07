using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "newTests", menuName = "Games/Code Game/Tests")]

public class Tests : ScriptableObject
{
    public string problemName;
    public string problemDescription;
    public string[] tests;

}
