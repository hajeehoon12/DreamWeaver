using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void GetDamage(float damage)
    {
        Debug.Log("InDamage");
        if (currentHealth > damage) currentHealth -= damage;
        else Destroy(gameObject);

    }




}
