using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class RadialTree
{
    private int nrOfCircles;        // the number of circles around the root (so excluding the root)
    private int nrOfNodes;          // the number of nodes in this graph
    private float nodeSize;         // the size of a node
    private float circleSize;       // the radius of the first circle around the root
    private AdjacencyList circles;      // an adjacency matrix, where each "node" represents a circle
                                            // so each adjacency list contains the ids of the nodes in that circle
    private AdjacencyList adj;      // the actual adjacency matrix of the graph
    private Vector3[] p;        // the positions of the vertices
    private int root = 0;               // the id of the node that is the root of this tree

    // Creates a radial tree with a set number of circles around the root
    public RadialTree(string mode, int nrOfCircles, int nrOfNodes, float circleSize) {
        this.nrOfCircles = nrOfCircles;
        circles = new AdjacencyList(nrOfNodes);
        this.nrOfNodes = nrOfNodes;
        this.circleSize = circleSize;
        adj = new AdjacencyList(nrOfNodes);
        // Add the root to the 0th circle
        circles.AddNeighbor(0, root);
        if (mode.Equals("deterministic without adj")) {
            // Add 1 node to the first circle, 2 to the second and 3 to the third
            circles.AddNeighbor(1, 1);
            circles.AddNeighbor(2, 2);
            circles.AddNeighbor(2, 3);
            circles.AddNeighbor(3, 4);
            circles.AddNeighbor(3, 5);
            circles.AddNeighbor(3, 6);
        } else if (mode.Equals("somewhat random with adj")) {
            // get number of children in a circle
                // ie generate the children of the parent circle
            int id = 1;     // the id of the next child to be assigned
            for (int i = 1; i <= nrOfCircles; i++) {
                int totalOffspring = 0;
                float avgOffspring = 0;
                // for each parent in the ith circle
                List<int> parents = circles.GetNeighbors(i - 1);
                for (int j = 1; j <= parents.Count; j++) {
                    int parent = parents[j - 1];
                    // generate a random number of children (between 0 and 3, where average degree for each layer must be around 2)
                    int offspring = 0;
                    do {
                        offspring = Random.Range(0, 3);
                        totalOffspring += offspring;
                        avgOffspring = totalOffspring / j;
                        totalOffspring -= offspring;
                    } while (avgOffspring < 1.5 || avgOffspring > 3);
                    totalOffspring += offspring;
                    // Debug.Log("generating " + offspring + " children of " + parent);
                    for (int k = 0; k < offspring && id < nrOfNodes; k++) {
                        // connect each child to its parent
                        adj.AddNeighbor(parent, id);
                        adj.AddNeighbor(id, parent);
                            // Debug.Log("Added " + id + " as child of " + parent);
                        id++;
                    }
                }
                // starting at top parent, divide children over the circle
                for (int j = 0; j < parents.Count; j++) {
                    List<int> children = adj.GetNeighbors(parents[j]);
                    // Debug.Log("Adding" + children.Count + " children of " + parents[j] + " to circle " + i);
                    foreach(int child in children) {
                        // add this child as the next node in the circle
                        // when determining the position of the nodes in the circle, nodes that have been added to a circle first
                        // will also be placed first in the circle
                        // check if the id of the child is larger than that of the parent, otherwise it's actually the parent of the node you're adding
                        if (child > parents[j]) {
                            circles.AddNeighbor(i, child);
                            // Debug.Log("Added " + child + " to circle " + i);
                        }
                    }
                }
            }
            CalcPosition();
        }
    }

    // Calculates the position of all nodes in the tree
    public void CalcPosition() {
        p = new Vector3[nrOfNodes];
        // place the root in the middle
        p[GetRoot()] = new Vector3(0, 0, 0);
        
        // for each circle, place the nodes that are in it evenly over the circle
        for (int i = 1; i <= nrOfCircles; i++) {
            List<int> nodes = NodesInCircle(i);
            if (nodes.Count > 0) {
                float radius = circleSize * i;              // set the radius of this circle
                // Debug.Log(radius + " radius of " + i);
                float degreePerNode = 360f / nodes.Count;       // the degree by each point is moved over the circle to spread evenly
                // Debug.Log(degreePerNode + " degree per node of " + i);
                for (int node = 0; node < nodes.Count; node++) {
                    // calculate x and y position
                    float x = radius * Mathf.Cos(degreePerNode * node * Mathf.Deg2Rad);
                    float y = radius * Mathf.Sin(degreePerNode * node * Mathf.Deg2Rad);
                    // Debug.Log(i + " at " + new Vector3(x, y, 0));
                    p[nodes[node]] = new Vector3(x, y, 0);
                }
            }
        }
        // Use force-directed layout to space the nodes out nicely over the screen
        ForceDirected(1000, 2, 1, 2);  // TODO: figure out numbers
    }

    public void ForceDirected(int maxIterations, float cRep, float cSpring, float length) {
        // for(int i = 0; i < nrOfNodes; i++) {
        //     Debug.Log(i + " at " + p[i]);
        // }
        Vector3[,] fRep = new Vector3[nrOfNodes,nrOfNodes];     // the repulsive force between all vertices
        Vector3[,] fSpring = new Vector3[nrOfNodes,nrOfNodes];  
        Vector3[,] fAttr = new Vector3[nrOfNodes, nrOfNodes];   // the attractive force between connected vertices
        // note that since I don't have the number of edges, I just make it between all nodes and make it equal to fRep for nodes that are disconnected
        Vector3[] fU = new Vector3[nrOfNodes];                  // displacement vector
        int t = 1;      // number of repetitions
        while (t < maxIterations) {
            for (int u = 0; u < nrOfNodes; u++) {
                for (int v = 0; v < nrOfNodes; v++) {
                    if (u != v) {
                        // Calculate a unit vector of the direction from v to u
                        Vector3 direction = (p[u] - p[v]).normalized;
                        // Debug.Log(direction + " direction from " + u + " to " + v);
                        // Calculate the repulsive force on u from v
                        float distance = Vector3.Distance(p[u], p[v]);
                        // Debug.Log(distance + " distance from " + u + " to " + v);
                        // if(distance == 0 && u != v) {
                        //     Debug.Log(u + ": " + p[u] + " " + v + ": " + p[v]);
                        // }
                        fRep[u,v] = cRep / Mathf.Pow(distance, 2) * direction;
                        // Debug.Log(fRep[u,v] + " fRep from " + u + " to " + v);
                        // Debug.Log(cRep / Mathf.Pow(distance, 2) + " cRep / dis^2 from " + u + " to " + v);
                        // Calculate the spring force on u from v
                        if (NeighborOf(v, u)) {
                            fSpring[u,v] = cSpring * Mathf.Log(distance / length) * -1 * direction;
                            // Debug.Log(fSpring[u,v] + " (u - v) from " + u + " to " + v);  
                        } else {
                            fSpring[u,v] = fRep[u,v];
                            // Debug.Log(fSpring[u,v] + " (u !- v) from " + u + " to " + v);   
                        }                        
                        // Calculate the attractive force on u from v
                        // Supposed to be 0 if they're not neighbors, which is why fSpring = fRep in that case
                        fAttr[u,v] = fSpring[u,v] - fRep[u,v];
                        fU[u] += fRep[u,v] + fAttr[u,v];
                    }                    
                }
            }
            // Change the positions based on the force
            for (int u = 0; u < nrOfNodes; u++) {
                p[u] += Time.deltaTime * fU[u];
            }
            t++;
        }
    }

    // Returns the position of all nodes in the tree
    public Vector3[] GetPositions() {
        return p;
    }

    // Returns the available positions for @children in @circle
    private Vector3[] GetPositions(int children, int circle) {
        Vector3[] positions = new Vector3[children];
        // place the nodes that are in it evenly over the circle
        float radius = circleSize + 2 * circle;         // set the radius of this circle
        float degreePerNode = 360f / children;       // the degree by each point is moved over the circle to spread evenly
        for (int node = 0; node < children; node++) {
            // calculate x and y position
            float x = radius * Mathf.Cos(degreePerNode * node * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(degreePerNode * node * Mathf.Deg2Rad);
            positions[node] = new Vector3(x, y, 0);
        }
        return positions;
    }



    // Returns whether @target is a neighbor of @of
    public bool NeighborOf(int of, int neighbor) {
        return adj.CheckNeighbor(of, neighbor);
    }

    // Returns the number of neighbors of @of
    public int CountNeighbors(int of) {
        return adj.CountNeighbors(of);
    }

    // Returns a copy of the adjacency adjacency lists of this graph
    public List<int>[] GetAdj() {
        List<int>[] list = new List<int>[nrOfNodes];
        for (int i = 0; i < nrOfNodes; i++) {
            list[i] = new List<int>(adj.GetNeighbors(i));
        }
        return list;
    }

    // Returns the id of the root
    public int GetRoot() {
        return root;
    }

    // Returns the number of circles this tree has
    public int GetNrOfCircles() {
        return nrOfCircles;
    }

    // Returns a list of the nodes in the @circle-th circle around the root
    public List<int> NodesInCircle(int circle) {
        return circles.GetNeighbors(circle);
    }

    // Returns the number of nodes in this graph
    public int GetNrOfNodes() {
        return nrOfNodes;
    }

    // Returns the neighbors of @of
    public List<int> GetNeighbors(int of) {
        return adj.GetNeighbors(of);
    }
}
