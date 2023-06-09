using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BoardGame;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Vectors;

[ExecuteAlways]
public class Board : BoardParent
{
    [HideInInspector] public int NumberOfCheckpoints;
    [HideInInspector] public List<List<Tile>> Solutions = new();
    [HideInInspector] public List<Tile> Reachable = new ();
    private int oldMaxSteps;
    public int MaxSteps;
    

    protected override void Update()
    {
        base.Update();
        // Re-calc board when changing maximum steps.
        if (MaxSteps != oldMaxSteps)
        {
            oldMaxSteps = MaxSteps;
            SetupBoard();
        }
    }

    // This function is called whenever the board or any tile inside the board
    // is modified.
    public override void SetupBoard() {
        
        // Get the size of the board
        var boardSize = BoardSize;
        
        // Clear old data
        Solutions.Clear();
        Reachable.Clear();
        NumberOfCheckpoints = 0;
        
        // Setup each tile
        foreach (var tile in Tiles) { 
            tile.OnSetup(this);
        }
        
        // Run Dijkstra's algorithm from starting point
        foreach (Tile tile in Tiles)
        {
            if (tile.IsStartPoint)
            {
                SearchPath(tile);
            }
        }
    }
    
    // Find path to checkpoints using Dijkstra's algorithm.
    private void SearchPath(Tile start)
    {
        // Create two sets
        var open = new List<Tile> { start }; // nodes to visit
        var closed = new List<Tile>(); // nodes that have been visited
        
        do
        {
            // Move current/first tile in open to closed.
            Tile current = open[0];
            open.Remove(current);
            closed.Add(current);

            // If current tile is a checkpoint a solution has been found -> save it.
            if (current.IsCheckPoint)
            {
                List<Tile> solution = new();
                Backtrack(current, start, solution); // step through parents to find exact path
                Solutions.Add(solution);
            }

            // Check each of the current tile's neighbours.
            foreach (Tile neighbour in current.Neighbours)
            {
                if (closed.Contains(neighbour)) continue; // ignore if already visited

                // Update the neighbour's values if it exists in open but with a more expensive path
                if (open.Contains(neighbour))
                {
                    if (neighbour.MinCostToStart > current.MinCostToStart + neighbour.movementPenalty)
                    {
                        neighbour.Parent = current;
                        neighbour.MinCostToStart = current.MinCostToStart + neighbour.movementPenalty;
                    }
                }
                // Or otherwise update it's values and add them to open.
                else
                {
                    neighbour.Parent = current;
                    neighbour.MinCostToStart = current.MinCostToStart + neighbour.movementPenalty;
                    open.Add(neighbour);
                }
                
            }
            // Sort list in ascending MinCostToStart.
            open.Sort((x, y) => x.MinCostToStart-y.MinCostToStart); 

            // Mark tile as reachable if within MaxSteps.
            if (current.MinCostToStart <= MaxSteps) Reachable.Add(current);
            // If outside MaxSteps, break when all checkpoints have been found.
            else if (NumberOfCheckpoints != 0 && Solutions.Count == NumberOfCheckpoints) return; 
            
        } while (open.Any()); // loop for as long as open isn't empty
    }

    // Extract the shortest path between two tiles using their parents.
    // Relies on result being provided from SearchPath()
    private void Backtrack(Tile end, Tile start, List<Tile> result)
    {
        result.Add(end);
        if (end == start)
        {
            result.Reverse();
            return;
        }
        Backtrack(end.Parent, start, result);
    }
    
    

}
