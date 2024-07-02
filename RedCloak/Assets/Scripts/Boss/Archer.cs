using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    private static readonly int doAttack = Animator.StringToHash("DoAttack");
    private static readonly int doSpecialAttack = Animator.StringToHash("DoSpecialAttack");
    private static readonly int isRunning = Animator.StringToHash("IsRunning");
    private static readonly int isDead = Animator.StringToHash("IsDead");

    private Animator animator;
    private float count = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Start()
    {
        Discrimination();
    }

    void Discrimination()
    {
        StartCoroutine(Iteration());

    }

    IEnumerator Iteration()
    {
        yield return new WaitForSeconds(0.5f);

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




}
