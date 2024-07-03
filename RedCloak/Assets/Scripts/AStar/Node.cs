using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Node : MonoBehaviour
{
    public List<Node> Neighbors;
    [SerializeField] private float neighborRadius;
    
    public Node Parent { get; set; }
    public float Distance { get; set; }
    private LineRenderer line;

    private void Start()
    {
        Neighbors = new List<Node>();
        FindNeighbor();
    }

    private void FindNeighbor()
    {
        RaycastHit2D[] nodes = Physics2D.CircleCastAll(transform.position, neighborRadius, Vector2.zero,0, 1 << 3);
        foreach (var node in nodes)
        {
            Debug.Log("Test");
            if(node.collider == GetComponent<BoxCollider2D>())
                continue;
            Neighbors.Add(node.collider.GetComponent<Node>());
        }
    }
    
    public void DebugWay(bool status)
    {
        if (line == null)
            line = gameObject.AddComponent<LineRenderer>();
        line.enabled = false;

        if (status) {
            line.enabled = true;
            line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            line.startColor = Color.yellow;
            line.endColor = Color.yellow;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.positionCount = 2;
            line.useWorldSpace = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, Parent.transform.position);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position,neighborRadius);
        Gizmos.color = new Color(1, 1, 1, 0.3f);
    }
}
