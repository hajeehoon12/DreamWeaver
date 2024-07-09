using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneBoundary : MonoBehaviour
{
    public Archer archer;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (archer.isBossDie) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            archer.CallArcherBoss();
            //StageChangeManager.Instance.SceneChange(1);
        }
    }


}
