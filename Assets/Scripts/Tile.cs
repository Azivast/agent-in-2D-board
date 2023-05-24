using System.Collections.Generic;
using BoardGame;
using UnityEngine;

public class Tile : TileParent {
    
    // 1. TileParent extends MonoBehavior, so you can add member variables here
    // to store data.
    public Material regularMaterial;
    public Material blockedMaterial;

    // Path finding 
    public Tile Parent;
    public List<Tile> Neighbours; //Todo: Set neighbours
    public float Cost = 0; // TODO: take into account Obstacle etc

    
    
    // This function is called when something has changed on the board. All 
    // tiles have been created before it is called.
    public override void OnSetup(Board board) {
        // 2. Each tile has a unique 'coordinate'
        Vector2Int key = Coordinate;
        
        // 3. Tiles can have different modifiers
        if (IsBlocked) {
            
        }
        
        if (IsObstacle(out int penalty)) {
            
        }
        
        if (IsCheckPoint) {
            
        }
        
        if (IsStartPoint) {
            
        }
        
        if (IsPortal(out Vector2Int destination)) {
            
        }
        
        // 4. Other tiles can be accessed through the 'board' instance
        // Add neighbours
        if (board.TryGetTile(coordinate + Vector2Int.up, out Tile neighbour)) Neighbours.Add(neighbour);
        if (board.TryGetTile(coordinate + Vector2Int.down, out neighbour)) Neighbours.Add(neighbour);
        if (board.TryGetTile(coordinate + Vector2Int.left, out neighbour)) Neighbours.Add(neighbour);
        if (board.TryGetTile(coordinate + Vector2Int.right, out neighbour)) Neighbours.Add(neighbour);

        // 5. Change the material color if this tile is blocked
        if (TryGetComponent<MeshRenderer>(out var meshRenderer)) {
            if (IsBlocked) {
                meshRenderer.sharedMaterial = blockedMaterial;
            } else {
                meshRenderer.sharedMaterial = regularMaterial;
            }
        }
    }

    // This function is called during the regular 'Update' step, but also gives
    // you access to the 'board' instance.
    public override void OnUpdate(Board board) {
        
    }
}