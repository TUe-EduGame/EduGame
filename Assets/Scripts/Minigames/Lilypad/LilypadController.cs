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
    private int target; // The node that was last clicked
    // Whether movement is currently allowed
    private bool allowedToMove;
    private LilypadCharacterScript character;
    private GameObject deathscreen;
    public UnityEvent restart;
    public GameObject lilypadPrefab;    // the prefab from which lilypads are created

    // This function is called when the object becomes enabled and active.
    void Awake() {
        // Generate the graph of lilypads
        NewGraph("somewhat random with adj");
        accessible = new bool[nrOfLilypads];
        accessible[0] = true;
        character = FindObjectOfType<LilypadCharacterScript>();
        character.SetInitialPosition(positions[graph.GetRoot()]);
    }

    // Start is called before the first frame update
    void Start() {
        visited = new bool[nrOfLilypads];
        current = 0;
        deathscreen = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        allowedToMove = true;
    }
    
    // Returns the position of lilypad @id
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
        this.target = target;
        if (allowedToMove && !character.IsMoving()) {
            if (adj[current].Contains(target)) {
                if (visited[target] == false || adj[current].Count == 1) {
                    accessible[target] = true;
                    accessible[current] = false;
                    adj[current].Remove(target);
                    visited[current] = true;
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
        NewGraph("somewhat random with adj");
        restart.Invoke();
        accessible = new bool[nrOfLilypads];
        accessible[0] = true;
        visited = new bool[nrOfLilypads];
        current = 0;
        deathscreen.SetActive(false);
        character.SetInitialPosition(positions[graph.GetRoot()]);
        character.Reset();
        allowedToMove = true;
    }

    // Called to make a new graph
    public void NewGraph(string type) {
        // Generate the graph of lilypads
        graph = new RadialTree(type, 3, 9, circleSize);
        positions = graph.GetPositions();
        nrOfLilypads = graph.GetNrOfNodes();
        for (int i = 0; i < nrOfLilypads; i++) {
            GameObject lilypad = Instantiate(lilypadPrefab, positions[i], Quaternion.Euler(0, 0, 180));
            lilypad.GetComponent<LilypadScript>().SetId(i);
        }
        adj = graph.GetAdj();

        // Give the lineConnector the data it needs to draw the edges
        LineConnector lineConnector = GetComponent<LineConnector>();
        lineConnector.SetGraph(graph);
    }

    // Returns which node is the current target
    public int CurrentTarget() {
        return target;
    }

    public int GetSprite(int id) {
        if (adj[id].Count < 1 && id != graph.GetRoot()) {
            return 2;
        } else if (adj[id].Count < graph.GetAdj()[id].Count) {
            return 1;
        } else {
            return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate() {

    }
}
