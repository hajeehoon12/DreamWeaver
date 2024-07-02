using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform portalDestination;
    [SerializeField] private Transform player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.transform.position = portalDestination.position;
    }
}
