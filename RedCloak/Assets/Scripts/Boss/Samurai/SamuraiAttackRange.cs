using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiAttackRange : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Define.PLAYERPROJECTILE))
        {
            AudioManager.instance.PlaySamurai("DefendSuccess", 0.2f);
        }
    }

}
