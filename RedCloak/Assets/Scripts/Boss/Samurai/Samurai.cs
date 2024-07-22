using AnimationImporter.PyxelEdit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Samurai : MonoBehaviour, IDamage
{
    private float bossHealth = 0;
    public float bossMaxHealth;

    private bool isInvincible = false;

    private bool isPhase1 = false;
    private bool isPhase2 = false;
    private bool isPhase3 = false;

    public bool isBossDie = false;

    public bool isStageStart = false;

    SpriteRenderer spriteRenderer;
    Animator animator;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        bossHealth = 0;
        CallSamurai();
    }

    public void CallSamurai()
    {
       
        StartCoroutine(SamuraiStageOn());
        
    }

    IEnumerator SamuraiStageOn()
    {
        CharacterManager.Instance.Player.controller.cantMove = true;


        
        yield return new WaitForSeconds(1f);

        UIBar.Instance.CallBossBar("Samurai");

        float time = 0f;
        float totalTime = 2f;

        while (time < totalTime)
        {
            bossHealth += (bossMaxHealth * Time.deltaTime / totalTime);
            SetBossBar();
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("StarSky", 0.2f);
        CharacterManager.Instance.Player.controller.cantMove = false;
    }

    void SetBossBar()
    {
        UIBar.Instance.SetBossBar(bossHealth, bossMaxHealth, 0);
    }

    public void GetDamage(float damage)
    {
        if (isInvincible) return;

        if (bossHealth > damage)
        {
            bossHealth -= damage;
            Debug.Log($"남은 보스 체력 : {bossHealth}");
            if (bossHealth < (bossMaxHealth * 2 / 3) && isPhase1)
            {
                isPhase1 = false;
                isPhase2 = true;
                isPhase3 = false;

                //Debug.Log("next phase");
                //StopCoroutine(mainCoroutine);
                //mainCoroutine = StartCoroutine(Iteration());
            }

            if (bossHealth < (bossMaxHealth * 1 / 3) && isPhase2)
            {
                isPhase1 = false;
                isPhase3 = true;
                isPhase2 = false;
                
                
            }

            UIBar.Instance.SetBossBar(bossHealth, bossMaxHealth, damage);
            StartCoroutine(ColorChanged());
        }
        else
        {
            
            if (isBossDie) return;
            if (isPhase3) isBossDie = true;

            isStageStart = false;
            UIBar.Instance.SetBossBar(0, bossMaxHealth, bossHealth);
           
            CallDie();
            
        }
    }

    IEnumerator ColorChanged()
    {
        float durTime = 0.2f; // invincibleTime; Have To Change
        spriteRenderer.DOColor(Color.red, durTime);
        yield return new WaitForSeconds(durTime);
        spriteRenderer.DOColor(Color.white, durTime);

    }

    void CallDie()
    { 
        
    }

}
