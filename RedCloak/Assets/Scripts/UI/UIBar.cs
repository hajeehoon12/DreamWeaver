using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider playerStaminaBar;
    [SerializeField] private Slider bossHealthBar;
    

    public Monster monster;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bossHealthBar.value = monster.currentHealth / monster.maxHealth;
    }
}
