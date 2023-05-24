using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BoardGame;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class Board : BoardParent
{
    // This function is called whenever the board or any tile inside the board
    // is modified.
    public override void SetupBoard() {
        
        // 1. Get the size of the board
        var boardSize = BoardSize;
        
        // 2. Iterate over all tiles
        foreach (Tile tile in Tiles) {
            if (tile.IsStartPoint)
            {
                var paths = SearchPath(tile);
                for (int i = 0; i < paths.Count - 1; i++)
                {
                    Debug.Log(paths[i]);
                }
                
            }
        }
        
        // 3. Find a tile with a particular coordinate
        Vector2Int coordinate = new Vector2Int(2, 1);
        if (TryGetTile(coordinate, out Tile tile2)) {
            
        }
    }
    
    // Find path to checkpoints
    public List<Vector2Int[]> SearchPath(Tile start)
    {
        List<Vector2Int[]> result = new List<Vector2Int[]>();

        SortedList<float, Tile> open = new SortedList<float, Tile>(); // tiles to visit
        open.Add(start.movementPenalty, start);
        Dictionary<Tile, float> closed = new Dictionary<Tile, float>(); // visited tiles

        float totalPenalty = 0;
        for(int i = 0; i < open.Count; i++)
        {
            Tile current = open[0];
            totalPenalty += current.Cost;
            closed.Add(current, totalPenalty);
            open.RemoveAt(0);

            
            if (current.IsCheckPoint)
            {
                List<Vector2Int> path = new List<Vector2Int>();
                ExtractPath(start, current, path);
                result.Add(path.ToArray());
            }
            
            foreach (Tile neighbour in current.Neighbours)
            {
                neighbour.Parent = current;
                if (closed.ContainsKey(neighbour)) // If == -> multiple paths with same cost, doesn't matter which one we pick
                {
                    if (closed[neighbour] > neighbour.movementPenalty + current.movementPenalty)
                        closed.Add(neighbour, neighbour.movementPenalty + current.movementPenalty);
                }
                else
                {
                    closed.Add(neighbour, neighbour.movementPenalty+current.movementPenalty);
                    i--; // one more to check
                }
            }
        }
        // TODO replace this with something reasonable
        Vector2Int[] tmp = new Vector2Int[closed.Keys.Count];
        int j = 0;
        foreach (Tile tile in closed.Keys)
        {
            tmp[j] = tile.coordinate;
            j++;
        }
        
        result.Add(tmp);
        return result;
    }

    // Extract the shortest path between two tiles using their parents.
    // Relies on result being provided from SearchPath()
    private void ExtractPath(Tile end, Tile start, List<Vector2Int> result)
    {
        if (end == start) return;

        result.Add(end.coordinate);
        ExtractPath(end.Parent, start, result);
    }
    
    

}
