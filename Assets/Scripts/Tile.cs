using System.Collections.Generic;
using BoardGame;
using UnityEngine;

public class Tile : TileParent {
    
    // 1. TileParent extends MonoBehavior, so you can add member variables here
    // to store data.
    public Material regularMaterial;
    public Material penaltyMaterial;

    // Path finding 
    public Tile Parent;
    public List<Tile> Neighbours;
    public int? MinCostToStart = null; // Penalty to travel here from start position

    // This function is called when something has changed on the board. All 
    // tiles have been created before it is called.
    public override void OnSetup(Board board) {

        // 2. Each tile has a unique 'coordinate'
        Vector2Int key = Coordinate;
        // TODO: Optimize
        var blocked = transform.GetChild(0).gameObject;
        var portal = transform.GetChild(1).gameObject;
        var start = transform.GetChild(3).gameObject;
        var finish = transform.GetChild(2).gameObject;
        blocked.SetActive(false);
        portal.SetActive(false);
        start.SetActive(false);
        finish.SetActive(false);
        
        // 3. Tiles can have different modifiers
        if (IsBlocked) {
            blocked.SetActive(true);
        }
        
        if (IsObstacle(out int penalty)) {
            
        }
        
        if (IsCheckPoint) {
            finish.SetActive(true);
            board.numberOfCheckpoints++;
        }
        
        if (IsStartPoint) {
            start.SetActive(true);
        }
        
        if (IsPortal(out Vector2Int destination)) {
            portal.SetActive(true);
        }
        
        // 4. Other tiles can be accessed through the 'board' instance
        // Add neighbours
        Neighbours.Clear();
        if (board.TryGetTile(coordinate + Vector2Int.up, out Tile neighbour) && !neighbour.IsBlocked) 
            Neighbours.Add(neighbour);
        if (board.TryGetTile(coordinate + Vector2Int.down, out neighbour) && !neighbour.IsBlocked)
            Neighbours.Add(neighbour);
        if (board.TryGetTile(coordinate + Vector2Int.left, out neighbour) && !neighbour.IsBlocked)
            Neighbours.Add(neighbour);
        if (board.TryGetTile(coordinate + Vector2Int.right, out neighbour) && !neighbour.IsBlocked)
            Neighbours.Add(neighbour);

        // 5. Change the material color if this tile has penalty
        if (TryGetComponent<MeshRenderer>(out var meshRenderer)) {
            if (IsObstacle(out penalty)) {
                meshRenderer.sharedMaterial = penaltyMaterial;
            } else {
                meshRenderer.sharedMaterial = regularMaterial;
            }
        }
    }

    // This function is called during the regular 'Update' step, but also gives
    // you access to the 'board' instance.
    public override void OnUpdate(Board board) 
    {
    }
}