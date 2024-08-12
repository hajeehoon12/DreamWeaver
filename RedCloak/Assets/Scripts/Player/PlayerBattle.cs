using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerBattle : MonoBehaviour
{


    public float healthChangeDelay = 0.8f;


    private float timeSinceLastChange = float.MaxValue; // time calculate from last hit

    public bool isAttacked = false;


    public event Action OnDamage;
    public event Action<Vector3> OnDamagePos;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public bool onInvincible = false;

    public bool isDead = false;

    private bool inTrap = false;


    public float MaxHealth => CharacterManager.Instance.Player.stats.playerMaxHP;

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                OnInvincibilityEnd?.Invoke();
                //isAttacked = false;

                onInvincible = true;

            }
            else
            {
                onInvincible = false;
            }
        }
    }

    private void Start()
    {
        OnDamage += OnCollisionDelay;
    }

    public bool ChangeHealth(float change)
    {
        if (timeSinceLastChange < healthChangeDelay) // if not attacked
        {
            return false;
        }

        CharacterManager.Instance.Player.stats.playerHP += change; // health change value
        CharacterManager.Instance.Player.stats.playerHP = Mathf.Clamp(CharacterManager.Instance.Player.stats.playerHP, 0, CharacterManager.Instance.Player.stats.playerMaxHP); // restrict health range for 0<= health <= maxHealth

        //Debug.Log("Player Health : " + change);

        if (CharacterManager.Instance.Player.stats.playerHP <= 0f)
        {
            timeSinceLastChange = 0f;
            OnDamage?.Invoke();
            //Debug.Log("Player Dead");
            CallDeath();
            return true;
        }
        if (change >= 0) // when change is positive = Healing character
        {
            OnHeal?.Invoke();
        }
        else // When Damage to Player
        {
            //Debug.Log("Damage Motion called");
            OnDamage?.Invoke();
            timeSinceLastChange = 0f;


            // Get Damaged Sound
        }

        return true;
    }


    public bool ChangeHealth(float change, string trap)
    {
        if (timeSinceLastChange < healthChangeDelay) // if not attacked
        {
            return false;
        }

        CharacterManager.Instance.Player.stats.playerHP += change; // health change value
        CharacterManager.Instance.Player.stats.playerHP = Mathf.Clamp(CharacterManager.Instance.Player.stats.playerHP, 0, CharacterManager.Instance.Player.stats.playerMaxHP); // restrict health range for 0<= health <= maxHealth

        //Debug.Log("Player Health : " + change);

        if (CharacterManager.Instance.Player.stats.playerHP <= 0f)
        {
            timeSinceLastChange = 0f;
            OnDamage?.Invoke();
            Debug.Log("Player Dead");
            CallDeath();
            return true;
        }
        if (change >= 0) // when change is positive = Healing character
        {
            OnHeal?.Invoke();
        }
        else // When Damage to Player
        {
            //Debug.Log("Damage Motion called");
            OnDamage?.Invoke();
            timeSinceLastChange = -healthChangeDelay * 2;
            inTrap = true;


            // Get Damaged Sound
        }

        return true;
    }

    public void OnCollisionDelay()
    {
        StartCoroutine(CollisionDelay());
    }

    IEnumerator CollisionDelay()
    {
        float delay = inTrap ? healthChangeDelay * 3: healthChangeDelay;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.TRAP), LayerMask.NameToLayer(Define.PLAYER), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.ENEMY), LayerMask.NameToLayer(Define.PLAYER), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.BOSS), LayerMask.NameToLayer(Define.PLAYER), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MONSTERPROJECTILE), LayerMask.NameToLayer(Define.PLAYER), true);
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.TRAP), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.ENEMY), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.BOSS), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MONSTERPROJECTILE), LayerMask.NameToLayer(Define.PLAYER), false);
        inTrap = false;
    }

    public bool ChangeHealth(float change, Vector3 position)
    {
        if (timeSinceLastChange < healthChangeDelay) // if not attacked
        {
            return false;
        }

        CharacterManager.Instance.Player.stats.playerHP += change; // health change value
        CharacterManager.Instance.Player.stats.playerHP = Mathf.Clamp(CharacterManager.Instance.Player.stats.playerHP, 0, CharacterManager.Instance.Player.stats.playerMaxHP); // restrict health range for 0<= health <= maxHealth

        //Debug.Log("Player Health : " + change);

        if (CharacterManager.Instance.Player.stats.playerHP <= 0f)
        {
            OnDamagePos?.Invoke(position);
            timeSinceLastChange = 0f;
            //Debug.Log("Player Dead");
            CallDeath();

            
            return true;
        }
        if (change >= 0) // when change is positive = Healing character
        {
            OnHeal?.Invoke();
        }
        else // When Damage to Player
        {
            //Debug.Log("Damage Motion called");
            OnDamagePos?.Invoke(position);
            timeSinceLastChange = 0f;


            // Get Damaged Sound
        }

        return true;
    }


    private void CallDeath()
    {
        //if(!isDead) return; // temp for test// can't die exception


        if (isDead) return;
        isDead = true;
        CharacterManager.Instance.CallDeath();
        //OnDamage?.Invoke();
        OnDeath?.Invoke();
    }

    

}
