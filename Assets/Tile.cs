using BoardGame;
using UnityEngine;

public class Tile : TileParent {
    
    // 1. TileParent extends MonoBehavior, so you can add member variables here
    // to store data.
    public Material regularMaterial;
    public Material blockedMaterial;
    
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
        if (board.TryGetTile(new Vector2Int(2, 1), out Tile otherTile)) {
            
        }
        
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