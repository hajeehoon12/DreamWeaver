using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] public Node[] Nodes;

    private void Start()
    {
        foreach (var node in Nodes)
        {
            node.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public Queue<Node> GetNavDate(Vector3 start, Vector3 target)
    {
        Queue<Node> path = new Queue<Node>();
        Node current = FindNode(start);
        Node dest = FindNode(target);

        if (current == null || dest == null || current == dest)
            return path;

        SortedList<float, Node> openList = new SortedList<float, Node>();
        List<Node> closedList = new List<Node>();
        
        openList.Add(0, current);
        current.Parent = null;
        current.Distance = 0;

        while (openList.Count > 0)
        {
            current = openList.Values[0];
            openList.RemoveAt(0);
            float dist = current.Distance;
            closedList.Add(current);

            if (current == dest)
                break;

            foreach (var neighbor in current.Neighbors)
            {
                if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor))
                    continue;

                neighbor.Parent = current;
                neighbor.Distance = dist + Vector2.Distance(neighbor.transform.position, current.transform.position);
                float distanceToTarget = Vector2.Distance(neighbor.transform.position, target);
                openList.Add(neighbor.Distance + distanceToTarget, neighbor);
            }
        }

        if (current == dest)
        {
            while (current.Parent != null)
            {
                path.Enqueue(current);
                current = current.Parent;
            }
        }
        
        return path;
    }

    private Node FindNode(Vector2 target)
    {
        Node closest = null;

        float minDistance = float.MaxValue;

        for (int i = 0; i < Nodes.Length; i++)
        {
            float distance = Vector2.Distance(Nodes[i].transform.position, target);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = Nodes[i];
            }
        }

        return closest;
    }
        
}
