using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    [SerializeField] private Color ableColor;
    [SerializeField] private Color unableColor;

    [SerializeField] private LayerMask TerrainLayer;
    [SerializeField] private Vector2 worldSize;
    [SerializeField] private float nodeRaidus;
    public AStarNode[,] grid { get; set; }
    public List<AStarNode> Path { get; set; }

    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRaidus * 2;
        gridSizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
        
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new AStarNode[gridSizeX, gridSizeY];
        Vector2 bottomLeft = Vector2.zero - Vector2.right * gridSizeX / 2 - Vector2.up * gridSizeY / 2;
        Vector2 worldPos;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPos = bottomLeft + Vector2.right * (x * nodeDiameter + nodeRaidus) +
                           Vector2.up * (y * nodeDiameter + nodeRaidus);
                bool moveable = !(Physics2D.CircleCast(worldPos, nodeRaidus, Vector2.zero, TerrainLayer));
                grid[x, y] = new AStarNode(moveable, worldPos, x, y);
            }
        }
    }

    public List<AStarNode> GetNeighbor(AStarNode node)
    {
        List<AStarNode> neighbor = new List<AStarNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                if (node.gridX + x < 0 || node.gridY + y < 0)
                    continue;
                
                if (node.MoveAble)
                    neighbor.Add(grid[node.gridX + x, node.gridY + y]);
            }
        }

        return neighbor;
    }

    public AStarNode GetNode(Vector2 pos)
    {
        float x = (pos.x + worldSize.x / 2) / worldSize.x;
        float y = (pos.y + worldSize.y / 2) / worldSize.y;
        
        x = Mathf.Clamp01(x);
        y = Mathf.Clamp01(y);

        int posX = Mathf.RoundToInt((gridSizeX - 1) * x);
        int posY = Mathf.RoundToInt((gridSizeY - 1) * y);

        return grid[posX, posY];
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, gridSizeY));
        
        
        if (grid != null)
        {
            foreach (AStarNode node in grid)
            {
                Gizmos.color = node.MoveAble ? ableColor : unableColor;

                if (Path != null)
                {
                    if (Path.Contains(node))
                        Gizmos.color = Color.black;
                }
                
                Gizmos.DrawCube(node.NodePos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
