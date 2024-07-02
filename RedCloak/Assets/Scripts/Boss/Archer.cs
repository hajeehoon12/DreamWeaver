using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Archer : MonoBehaviour
{
    private static readonly int doAttack = Animator.StringToHash("DoAttack");
    private static readonly int doSpecialAttack = Animator.StringToHash("DoSpecialAttack");
    private static readonly int isRunning = Animator.StringToHash("IsRunning");
    private static readonly int isDead = Animator.StringToHash("IsDead");

    private Animator animator;
    private float count = 0;

    private bool isBossDie = false;

    public float bossHealth = 100;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("SilverBird", 0.15f);
        Discrimination();
    }

    void Discrimination()
    {
        if (isBossDie) return;
        StartCoroutine(Iteration());
    }

    IEnumerator Iteration()
    {
        yield return new WaitForSeconds(0.5f);
        Flip();
        switch (count % 2)
        {
            case 0:
                Attack();
                break;
            case 1:
                SpecialAttack();
                break;

        }

        count++;
    }

    void Flip()
    {
        transform.localEulerAngles += new Vector3(0, 180, 0);
    }

    void Attack()
    {
        StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        animator.SetTrigger(doAttack);
        animator.ResetTrigger(doSpecialAttack);
        yield return new WaitForSeconds(1.52f);
        Discrimination();
    }

    void SpecialAttack()
    {
        StartCoroutine(DoSpeciatAttack());
    }

    IEnumerator DoSpeciatAttack()
    {
        animator.SetTrigger(doSpecialAttack);
        animator.ResetTrigger(doAttack);
        yield return new WaitForSeconds(2.62f);
        Discrimination();
    }

    void WhenDie()
    {
        StartCoroutine(DoDie());
    }

    IEnumerator DoDie()
    {
        animator.SetBool(isDead, true);
        yield return new WaitForSeconds(0.8f);
        
        // do die
    }

    void Charge()
    {
        AudioManager.instance.PlaySFX("Charge1sec", 0.2f);
    }

    void Vanish()
    {
        AudioManager.instance.PlaySFX("Vanish", 0.2f);
    }

    void Appear()
    {
        AudioManager.instance.PlaySFX("Appear", 0.2f);
    }

    void Laser()
    {
        AudioManager.instance.PlaySFX("Laser", 0.2f);
    }

    public void GetDamage(float damage)
    {
        if (bossHealth > damage)
        {
            bossHealth -= damage;
            StartCoroutine(ColorChanged());
        }
        else
        {
            if (isBossDie) return;
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
        isBossDie = true;
        StartCoroutine(ArcherDie());
    }

    IEnumerator ArcherDie()
    {
        AudioManager.instance.PlaySFX("ArcherDeath", 0.2f);
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX("Violin3", 0.15f);
        //StopAllCoroutines();
        //DOTween.KillAll();
        AudioManager.instance.StopBGM();
    }



}
