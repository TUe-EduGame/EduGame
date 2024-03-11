using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LilypadController : MonoBehaviour
{
    private Vector3[] positions;
    private int nrOfLilypads = 7;
    [SerializeField] private float circleSize = 2;      // the radius of the first circle around the root in Unity coordinates
    private Vector3 scale = new Vector3(1.7f, 1.7f, 1.7f);
    private List<int>[] adj;        // holds a copy of the adjacency list of the graph
    private RadialTree graph;       // holds the graph of lilypads
    private bool[] accessible;
    private bool[] visited;
    private int current;
    // Whether movement is currently allowed
    private bool allowedToMove;
    private LilypadCharacterScript character;
    private GameObject deathscreen;
    public UnityEvent restart;

    // This function is called when the object becomes enabled and active.
    void Awake() {
        // Generate the graph of lilypads
        graph = new RadialTree("somewhat random with adj", 3, nrOfLilypads, circleSize);
        positions = graph.GetPositions();
        adj = graph.GetAdj();
        // for(int id = 0; id < positions.Count(); id++) {
        //     Debug.Log( id + " at " + positions[id]);
        // }
        // Give the lineConnector the data it needs to draw the edges
        LineConnector lineConnector = GetComponent<LineConnector>();
        lineConnector.SetGraph(graph);
        accessible = new bool[nrOfLilypads];
        accessible[0] = true;
        character = FindObjectOfType<LilypadCharacterScript>();
        character.SetInitialPosition(positions[0]);
    }

    // Start is called before the first frame update
    void Start() {
        // adj = new List<int>[nrOfLilypads];
        // adj[0] = new List<int> {1,2};
        // adj[1] = new List<int> {0,3,4};
        // adj[2] = new List<int> {0,5};
        // adj[3] = new List<int> {1,6};
        // adj[4] = new List<int> {1};
        // adj[5] = new List<int> {2};
        // adj[6] = new List<int> {3};
        visited = new bool[nrOfLilypads];
        current = 0;
        deathscreen = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        allowedToMove = true;
    }
    
    // Returns the position of lilypad id
    public Vector3 GetPosition(int id) {
        return positions[id];
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
        if (allowedToMove && !character.IsMoving()) {
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
            StartCoroutine(character.Move(positions[target]));
        }          
    }

    public bool Finished() {
        return adj[current].Count == 0;
    }

    // Called when the player has visited all lilypads and came back
    public void Win() {
        allowedToMove = false;
        GameObject winNPC = GameObject.Find("WinNPC");
        winNPC.GetComponent<NPC>().Interact();
    }

    // Called when the player has failed, tragically
    public void Lose() {
        allowedToMove = false;
        StartCoroutine(character.Shrink(new Vector3(0.001f, 0.001f, 0.001f)));
        deathscreen.SetActive(true);
    }

    // Called to reset the data to be able to start a new game
    public void Reset() {
        // adj[0] = new List<int> {1,2};
        // adj[1] = new List<int> {0,3,4};
        // adj[2] = new List<int> {0,5};
        // adj[3] = new List<int> {1,6};
        // adj[4] = new List<int> {1};
        // adj[5] = new List<int> {2};
        // adj[6] = new List<int> {3};
        adj = graph.GetAdj();
        accessible = new bool[nrOfLilypads];
        accessible[0] = true;
        visited = new bool[nrOfLilypads];
        current = 0;
        restart.Invoke();
        character.Reset();
        deathscreen.SetActive(false);
        allowedToMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate() {

    }


}
