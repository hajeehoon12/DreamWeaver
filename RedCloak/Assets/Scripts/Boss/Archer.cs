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
        //Attack();
        //WhenDie();
        //SpecialAttack();
    }


    void Attack()
    {
        StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        animator.SetTrigger(doAttack);
        yield return new WaitForSeconds(1f);
        Discrimination();
    }

    void SpecialAttack()
    {
        StartCoroutine(DoSpeciatAttack());
    }

    IEnumerator DoSpeciatAttack()
    {
        animator.SetTrigger(doSpecialAttack);
        yield return new WaitForSeconds(1f);
        Discrimination();
    }

    void WhenDie()
    {
        StartCoroutine(DoDie());
    }

    IEnumerator DoDie()
    {
        animator.SetBool(isDead, true);
        yield return new WaitForSeconds(1f);
        
        // do die
    }






}
