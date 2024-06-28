using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public Monster monster;

    public void OnDamage()
    {
        monster.currentHealth -= 5f;
    }
}
