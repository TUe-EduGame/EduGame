using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class PredictController : MonoBehaviour
{
    // Whether the monster has ever moved to the cell
    private bool[] opened;
    // Whether all the cell's neighbors have been fully searched
    private bool[] completed;
    // Position of each of the cells
    private Vector3[] pos;
    // The number of nodes in this graph. Note that it includes cell 0 to make ids match indices
    public int nrOfCells;
    // Indices of points where the player is asked to make a decision
    public List<int> decisions = new List<int>();
    // For each checkpoint, state stores whether it's a checkpoint when it's unopened (0), opened (1) or completed (2)
    public List<int> states = new List<int>();
    // The cell that the monster is in/is moving towards
    private int current;
    // The cell that the player is shooting at
    private int target;
    // Whether the player's last guess was correct
    private bool correct = false;
    private PredictMonsterScript monster;
    private PredictBulletScript bullet;
    // Keeps track of the adjacencies of all the cells
    private AdjacencyList adj;

    void Awake() {
        adj = new AdjacencyList(nrOfCells);
        pos = new Vector3[nrOfCells];
        opened = new bool[nrOfCells];
        // Since the monster starts in 0, that one is opened from the beginning
        opened[0] = true;
        completed = new bool[nrOfCells];
    }

    // Start is called before the first frame update
    void Start()
    {
        monster = FindObjectOfType<PredictMonsterScript>();
        bullet = FindObjectOfType<PredictBulletScript>();
        bullet.OnBulletHit.AddListener(Hit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called by a cell if it is clicked
    public void Click(int target) {
        // Only allow clicking if the monster isn't moving
        if (!monster.IsMoving()) {
            this.target = target;
            // Moving to @target is allowed if:
            // It is adjacent to @current
            if (adj.CheckNeighbor(current, target)) {
                // It is unopened (exploring a new part)
                // Or it is opened but @current is completed so there's nowhere else to go 
                if (!opened[target] || (opened[target] && completed[current]) ) {
                    // Target has not been completed
                    if (!completed[target]) {
                        MoveTo(target);
                        correct = true;
                    }
                }
            }
            // Shoot at that square
            bullet.AddMove(pos[target]);
            if (!correct) {
                // Let the monster take its next steps immediately
                Dfs(current, 0);
            }            
        }
    }

    // Adds "to" to "from"'s adjacency list
    public void AddNeighbor(int from, int to) {
        adj.AddNeighbor(from, to);
    }

    // Records cell i's position
    public void SetPos(int i, Vector3 position) {
        pos[i] = position;
    }

    // Moves the monster to cell @target
    public void MoveTo(int target) {
        // Schedule a move to the target
        monster.AddMove(pos[target]);

        opened[target] = true;
        // If @target has 0 or 1 neighbors left (can only go back), it has been completed
        if (adj.GetNeighbors(target).Count() <= 1) {
            completed[target] = true;
        }
        // Remove @target from @current's adjacency list so we don't move from current to target again
        adj.RemoveNeighbor(current, target);
        current = target;
    }

    // Moves the monster using dfs until a decision point is reached
    public void Dfs(int start, int stepsTaken) {
        List<int> neighbors = new List<int>(adj.GetNeighbors(start));
        // After 3 steps a decision point is reached
        // If there are multiple options then, randomly pick one step further
        if (stepsTaken < 3 || neighbors.Count() > 2) {

            // Find a random valid neighbor of @start
            while (neighbors.Count() > 0) {
                // Pick a random neighbor of @start
                int i = new System.Random().Next(0, neighbors.Count());
                int id = neighbors.ElementAt(i);
                // @id is valid if:
                // It is adjacent to @current
                if (adj.CheckNeighbor(current, id)) {
                    // It is unopened (exploring a new part)
                    // Or it is opened but @current is completed so there's nowhere else to go 
                    if (!opened[id] || (opened[id] && completed[current]) ) {
                        // @id has not been completed
                        if (!completed[id]) {
                            MoveTo(id);
                            Dfs(id, ++stepsTaken);
                            break;
                        }
                    }
                } else {
                    // Remove id and try another neighbor
                    neighbors.Remove(id);
                }
            }
        }   
    }

    // Called when the bullet hit its target
    public void Hit() {
        bullet.Reset();
        // If the monster went back to 0, the player hasn't killed it in time so the player lost
        if (completed[1]) {
            End(false);
        }
        // If the monster is out of lives, the player has killed it and won
        if (monster.Lives() <= 0) {
            End(true);
        }
        if (correct) {
            // Let the monster take its next steps after the bullet has hit
            Dfs(current, 0);
            correct = false;
        }
        
    }

    public void End(bool win) {
        // Disable moving
        monster.allowMovement(false);
        bullet.allowMovement(false);
        if (win) {
            // kill the monster
        } else {
            // Make monster destroy something or so
        }
    }
}
