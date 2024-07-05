using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Queue<Node> pathToPlayer;
    [SerializeField] private Rigidbody2D _rigidbody;
    private Vector2 dest;
    private bool arrived = true;

    [SerializeField] private float moveSpeed;

    private void Update()
    {
        /*
        pathToPlayer = PathManager.Instance.GetNavDate(transform.position, player.transform.position);
        while (pathToPlayer.Count > 0)
        {
            Node node = pathToPlayer.Dequeue();
            node.DebugWay(true);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            
        }*/
    }

    private void FixedUpdate()
    {
        //Move();
        
    }

    public void Chase(Transform target)
    {
        
    }

    private void Move()
    {
        if (pathToPlayer.Count > 0)
        {
            if (arrived)
            {
                dest = pathToPlayer.Dequeue().transform.position;
                arrived = false;
            }
            else
            {
            }
        }
    }
}
