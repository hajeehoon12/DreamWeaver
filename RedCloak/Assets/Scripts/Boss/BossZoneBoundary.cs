using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneBoundary : MonoBehaviour
{
    public Archer archer;
    public bool enterZone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (archer.isBossDie) return;
        if (collision.gameObject.CompareTag("Player") && !enterZone)
        {
            archer.CallArcherBoss();
            enterZone = true;
            //StageChangeManager.Instance.SceneChange(1);
        }
    }


}
