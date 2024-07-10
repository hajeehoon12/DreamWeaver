using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform portalDestination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            AudioManager.instance.PlaySFX("Summon", 1f);
            collision.gameObject.transform.position = portalDestination.position;
        }
    }
}
