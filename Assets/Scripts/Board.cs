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
    [HideInInspector] public int numberOfCheckpoints;
    public int MaxSteps;
    public List<List<Tile>> Solutions = new();
    public List<Tile> Reachable = new ();

    // This function is called whenever the board or any tile inside the board
    // is modified.
    public override void SetupBoard() {
        
        // 1. Get the size of the board
        var boardSize = BoardSize;
        
        Solutions.Clear();
        Reachable.Clear();
        numberOfCheckpoints = 0;
        
        
        // Setup each tile
        foreach (var tile in Tiles) { 
            tile.OnSetup(this);
        }
        
        // Run Djikstras algorithm from starting point
        foreach (Tile tile in Tiles)
        {
            if (tile.IsStartPoint)
            {
                SearchPath(tile);
            }
        }
        
        // 3. Find a tile with a particular coordinate
        Vector2Int coordinate = new Vector2Int(2, 1);
        if (TryGetTile(coordinate, out Tile tile2)) {
            
        }
    }
    
    // Find path to checkpoints
    private void SearchPath(Tile start)
    {
        start.movementPenalty = 0;
        var open = new List<Tile> { start };
        var closed = new List<Tile>();
        
        do
        {
            Tile current = open[0];
            open.Remove(current);

            if (current.IsCheckPoint)
            {
                List<Tile> solution = new();
                Backtrack(current, start, solution);
                Solutions.Add(solution);
            }

            foreach (Tile neighbour in current.Neighbours)
            {
                if (closed.Contains(neighbour)) continue; // ignore if already visited

                if (open.Contains(neighbour))
                {
                    if (neighbour.MinCostToStart > current.MinCostToStart + neighbour.movementPenalty)
                    {
                        neighbour.Parent = current;
                        neighbour.MinCostToStart = current.MinCostToStart + neighbour.movementPenalty;
                    }
                }
                else
                {
                    neighbour.Parent = current;
                    if (current.IsPortal(out _))
                    {
                        neighbour.MinCostToStart = current.MinCostToStart; // not applying movement cost when traveling though portals
                    }
                    else neighbour.MinCostToStart = current.MinCostToStart + neighbour.movementPenalty;
                    open.Add(neighbour);
                }
                
            }
            open.Sort((x, y) => x.MinCostToStart-y.MinCostToStart); // sort list in ascending MinCostToStart
            closed.Add(current);
            
            if (current.MinCostToStart <= MaxSteps) Reachable.Add(current);
            else if (numberOfCheckpoints != 0 && Solutions.Count == numberOfCheckpoints) return;
            
        } while (open.Any());
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
