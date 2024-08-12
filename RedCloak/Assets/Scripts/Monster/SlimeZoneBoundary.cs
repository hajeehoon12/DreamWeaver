using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeZoneBoundary : MonoBehaviour
{
    Collider2D col;
    public Door door;

    float xPos = 0;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Define.PLAYER))
        { 
            xPos = gameObject.transform.position.x;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Define.PLAYER))
        {
            if (collision.gameObject.transform.position.x < xPos)
            {
                col.isTrigger = false;
                door.CloseDoor();
            }
        }
    }

    public void OpenForever()
    {
        col.isTrigger = true;
        gameObject.SetActive(false);
    }


}
