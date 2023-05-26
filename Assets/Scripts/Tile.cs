using System;
using System.Collections.Generic;
using BoardGame;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Vectors;

[SelectionBase]
[RequireComponent(typeof(VectorRenderer))]
public class Tile : TileParent {

    // Path finding 
    [SerializeField] public Tile Parent;
    [HideInInspector] public List<Tile> Neighbours;
    
    [SerializeField] private int minCostToStartInternal = 0;
    public int MinCostToStart // Penalty to travel here from start position
    {
        get { return minCostToStartInternal; }
        set
        {
            minCostToStartInternal = value;
            stepsText.text = (value == 0) ? "-" : value.ToString();
        }
    }

    private VectorRenderer vectors;
    private TMP_Text stepsText;
    private GameObject blocked;
    private GameObject portal;
    private GameObject start;
    private GameObject finish;
    private GameObject obstacle;

    public void Reachable(bool reachable)
    {
        var gameobj = transform.GetChild(5).gameObject;
        gameobj.SetActive(reachable); 
        
    }

    // This function is called when something has changed on the board. All 
    // tiles have been created before it is called.
    public override void OnSetup(Board board) {
       // Fetch components and game objects needed
        if(!TryGetComponent<VectorRenderer>(out vectors)) vectors = this.AddComponent<VectorRenderer>();
        if(stepsText == null) stepsText = transform.GetComponentInChildren<TMP_Text>();
        blocked = transform.GetChild(0).gameObject;
        portal = transform.GetChild(1).gameObject;
        start = transform.GetChild(3).gameObject;
        finish = transform.GetChild(2).gameObject;
        obstacle = transform.GetChild(4).gameObject;
        
        // Clear old data
        MinCostToStart = 0;
        Vector2Int key = Coordinate;

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


        // Disable all visuals
        blocked.SetActive(false);
        portal.SetActive(false);
        start.SetActive(false);
        finish.SetActive(false);
        obstacle.SetActive(false);
        
        // And enable only active ones
        if (IsBlocked) {
            blocked.SetActive(true);
        }
        
        if (IsObstacle(out movementPenalty)) {
            obstacle.SetActive(true);
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
            if (board.TryGetTile(destination, out neighbour) && !neighbour.IsBlocked)
            {
                Neighbours.Add(neighbour); // additional neighbour to target
            }
        }

        Reachable(false); // assume tile cant be reached until Board.cs says otherwise
    }

    // This function is called during the regular 'Update' step, but also gives
    // you access to the 'board' instance.
    public override void OnUpdate(Board board)
    {
        if (!IsPortal(out var target)) return;
        using (vectors.Begin())
        {
            vectors.Draw(new Vector3(coordinate.x, 0.8f, coordinate.y),
                new Vector3(target.x, 0.8f, target.y),
                Color.magenta);
            vectors.Draw(new Vector3(target.x, 0.8f, target.y),
                new Vector3(target.x, 0f, target.y),
                Color.magenta);
        }
    }
}