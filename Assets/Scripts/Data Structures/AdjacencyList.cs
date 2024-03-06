using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdjacencyList
{
    // Stores for each node which nodes they are adjacent to
    List<int>[] list;

    // Constructs an array of adjacency lists for @nrOfNodes nodes
    public AdjacencyList(int nrOfNodes)
    {
        list = new List<int>[nrOfNodes];
        for (int i = 0; i < list.Count(); i++)
        {
            list[i] = new List<int>();
        }
    }

    // Returns a list of the neighbors of @node
    public List<int> GetNeighbors(int node)
    {
        try
        {
            List<int> neighbors = list[node];
            return neighbors;
        }
        catch (IndexOutOfRangeException)
        {
            throw new Exception(node + " out of bounds for AdjacencyList of size " + list.Count());
        }
    }

    // Adds @neighbor to @to's adjacency list
    public void AddNeighbor(int to, int neighbor)
    {
        try
        {
            list[to].Add(neighbor);
        }
        catch (IndexOutOfRangeException)
        {
            throw new Exception("Can't add " + neighbor + " to adj of " + to + " for AdjacencyList of size " + list.Count());
        }
    }

    // Removes @neighbor from the adjacency list of @from. Note that @from is not removed from @neighbor's adjacency list!
    public void RemoveNeighbor(int from, int neighbor)
    {
        try
        {
            list[from].Remove(neighbor);
        }
        catch (IndexOutOfRangeException)
        {
            throw new Exception("Can't remove " + neighbor + " from adj of " + from + " for AdjacencyList of size " + list.Count());
        }
    }

    // Checks if @neighbor is in @of's adjacency list
    public bool CheckNeighbor(int of, int neighbor)
    {
        try
        {
            if (list[of].Contains(neighbor))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (IndexOutOfRangeException)
        {
            throw new Exception("Can't check if " + neighbor + " is in adj of " + of + " for AdjacencyList of size " + list.Count());
        }

    }

    // Returns the size of the adjacency list
    public int CountNeighbors(int of)
    {
        return list[of].Count();
    }
}