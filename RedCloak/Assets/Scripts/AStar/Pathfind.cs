using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : MonoBehaviour
{
    [SerializeField] private AStarGrid grid;

    public Transform StartPos;
    public Transform TargetPos;

    private void Update()
    {
        FindPath(StartPos.position, TargetPos.position);
    }

    private void FindPath(Vector2 _startPos, Vector2 _targetPos)
    {
        AStarNode start = grid.GetNode(_startPos);
        AStarNode dest = grid.GetNode(_targetPos);

        List<AStarNode> openList = new List<AStarNode>();
        HashSet<AStarNode> closedList = new HashSet<AStarNode>();
        openList.Add(start);

        while (openList.Count > 0)
        {
            AStarNode current = openList[0];

            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost < current.fCost ||
                    (openList[i].fCost == current.fCost && openList[i].hCost < current.hCost))
                    current = openList[i];
            }

            openList.Remove(current);
            closedList.Add(current);

            if (current == dest)
            {
                Retrace(start, dest);
                return;
            }

            foreach (AStarNode node in grid.GetNeighbor(current))
            {
                if (closedList.Contains(node)) continue;

                int currentToNeighbor = current.gCost + DistanceCost(current, node);
                if (currentToNeighbor < node.gCost || !openList.Contains(node))
                {
                    node.gCost = currentToNeighbor;
                    node.hCost = DistanceCost(node, dest);
                    node.parentNode = current;
                    
                    if (!openList.Contains(node))
                        openList.Add(node);
                }
            }
        }
    }

    private void Retrace(AStarNode start, AStarNode end)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parentNode;
        }

        path.Reverse();
        grid.Path = path;
    }

    private int DistanceCost(AStarNode start, AStarNode dest)
    {
        int distX = Mathf.Abs(start.gridX - dest.gridX);
        int distY = Mathf.Abs(start.gridY - dest.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        else
            return 14 * distX + 10 * (distY - distX);
    }
}
