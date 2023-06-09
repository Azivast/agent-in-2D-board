using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectors;

[ExecuteAlways]
[RequireComponent(typeof(VectorRenderer))]
[RequireComponent(typeof(Board))]
public class VectorVisualizer : MonoBehaviour
{
    [NonSerialized] 
    private VectorRenderer vectors;
    private Board board;
    
    void OnEnable() {
        vectors = GetComponent<VectorRenderer>();
        board = GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        using (vectors.Begin())
        {
            foreach (List<Tile> solution in board.Solutions)
            {
                for (int i = 0; i < solution.Count-1; i++)
                {
                    vectors.Draw(new Vector3( solution[i].coordinate.x,  0.5f, solution[i].coordinate.y),
                            new Vector3(solution[i+1].coordinate.x,  0.5f, solution[i+1].coordinate.y),
                            Color.green);
                }
            }

            foreach (Tile tile in board.Reachable)
            {
                tile.Reachable(true);
            }
        }
    }
}
