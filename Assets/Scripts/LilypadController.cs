using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LilypadController : MonoBehaviour
{
    private Vector3[] position = {
        new Vector3(-6f,0,0),
        new Vector3(-3.5f,1.5f,0),
        new Vector3(-3.5f,-1.5f,0),
        new Vector3(-1.5f,2.5f,0),
        new Vector3(-1.5f,0.5f,0),
        new Vector3(-1.5f,-1.5f,0),
        new Vector3(0.5f,2.5f,0)
    }; 
    private int nrOfLilypads = 7;
    private Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
    private List<int>[] adj;
    private bool[] accessible;
    private bool[] visited;
    private int current;
    private LilypadCharacterScript character;

    // Returns the position of lilypad id
    public Vector3 GetPosition(int id) {
        return position[id];
    }

    // Returns the scale of the lilypads
    public Vector3 GetScale() {
        return scale;
    }

    // Returns whether the lilypad id is accessible from the current 
    public bool isAccessible(int id) {
        return accessible[id];
    }

    // Moves the character to the target lilypad
    // If this is not a legal move, it sinks both the target and the character
    public void MoveTo(int target) {        
        if (adj[current].Contains(target)) {
            if (visited[target] == false || adj[current].Count == 1) {
                accessible[target] = true;
                accessible[current] = false;
                adj[current].Remove(target);
                visited[current] = true;
                if (!adj[current].Any()) {
                    // maybe change the color?
                }
                current = target;
            }
        } else {
            accessible[target] = false;
        }
        StartCoroutine(character.Move(position[target]));
    }

    public void Shrink() {
        StartCoroutine(character.Shrink(new Vector3(0.001f, 0.001f, 0.001f)));
    }

    // This function is called when the object becomes enabled and active.
    void Awake() {
    
    }

    // Start is called before the first frame update
    void Start() {
        adj = new List<int>[nrOfLilypads];
        adj[0] = new List<int> {1,2};
        adj[1] = new List<int> {0,3,4};
        adj[2] = new List<int> {0,5};
        adj[3] = new List<int> {1,6};
        adj[4] = new List<int> {1};
        adj[5] = new List<int> {2};
        adj[6] = new List<int> {3};
        accessible = new bool[nrOfLilypads];
        accessible[0] = true;
        visited = new bool[nrOfLilypads];
        current = 0;
        character = FindObjectOfType<LilypadCharacterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate() {

    }


}
