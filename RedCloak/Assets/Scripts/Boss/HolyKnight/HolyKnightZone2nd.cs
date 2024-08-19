using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyKnightZone2nd : MonoBehaviour
{
    public HolyKnight holy;
    public bool enterZone = false;
    Collider2D collider2d;
    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }

    private void Start()
    {
        collider2d.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enterZone && collision.gameObject.layer == LayerMask.NameToLayer(Define.PLAYER))
        {
            enterZone = true;
            holy.CallHolyStage();
            gameObject.SetActive(false);
        }
    }

}
