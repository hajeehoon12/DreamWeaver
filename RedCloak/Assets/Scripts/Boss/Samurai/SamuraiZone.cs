using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiZone : MonoBehaviour
{
    public Samurai samurai;
    public bool enterZone = false;

    public void EndStageBoss()
    { 
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (samurai.isBossDie) return;
        if (collision.gameObject.CompareTag("Player") && !enterZone)
        {
            samurai.CallSamurai();
            enterZone = true;
        }
    }
}
