using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public Monster monster;
    public GameObject damageEffect;

    public void OnDamage()
    {
        monster.currentHealth -= 5f;
        damageEffect.SetActive(true);
        StartCoroutine(disableEffect());
        Debug.Log("damage");
    }

    IEnumerator disableEffect()
    {
        yield return new WaitForSeconds(0.5f);

        damageEffect.SetActive(false);
    }
}
