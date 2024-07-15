using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfZone : MonoBehaviour
{
    private Collider2D collider2d;
    public Wolf wolf;

    float xPos = 0;

    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        //wolf = GetComponentInParent<Wolf>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            xPos = collision.transform.position.x;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            if (xPos < collision.transform.position.x && !wolf.isBossDie)
            {
                wolf.CallWolfBoss();
                collider2d.isTrigger = false;
                collision.gameObject.transform.position += new Vector3(4, 0, 0);
            }
        }
    }

}
