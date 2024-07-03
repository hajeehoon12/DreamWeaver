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
    Collider2D archerCol;

    public LayerMask ObstacleLayerMask;

    Vector2[] appearPos = { new Vector2(15, 0), new Vector2(14, 2), new Vector2(13, 4), new Vector2(12, 6), new Vector2(-12, 6), new Vector2(-13, 4), new Vector2(-14, 2), new Vector2(-15, 0), new Vector2(-14, -2), new Vector2(14, -2) };

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        archerCol = GetComponent<Collider2D>();
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

    void Flip() // Look At Player
    {
        //transform.localEulerAngles += new Vector3(0, 180, 0);

        transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 direction = transform.position - CharacterManager.Instance.Player.transform.position;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        

        if (direction.x > 0)
        {
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Euler(0, 180, -rotZ);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 180 + rotZ);
        }
    }

    void AppearPos() //When archer do Special Attack appear near by player
    {
        int tempCount = 0;
        int rand = 0;
        Vector2 targetPos = CharacterManager.Instance.Player.transform.position;
        while (tempCount < 100 || rand >= appearPos.Length-2)
        {
            targetPos = CharacterManager.Instance.Player.transform.position;
            rand = Random.Range(0, appearPos.Length);
            targetPos += appearPos[rand];

            RaycastHit2D hit = Physics2D.Raycast(CharacterManager.Instance.Player.transform.position+new Vector3(0,CharacterManager.Instance.Player.controller.playerCollider.bounds.extents.y * 2), (targetPos - (Vector2)CharacterManager.Instance.Player.transform.position).normalized, 22f, ObstacleLayerMask);
            
            if (hit.collider?.name == null) break;

            tempCount++;
        }
        transform.position = targetPos;

        Flip();
    }


    void Attack()
    {
        Flip();
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
        Collider2D[] archers = GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < archers.Length; i++)
        {
            archers[i].enabled = false;
        }


        animator.SetBool(isDead, true);
        isBossDie = true;
        StartCoroutine(ArcherDie());
    }

    IEnumerator ArcherDie()
    {
        AudioManager.instance.PlaySFX("ArcherDeath", 0.5f);
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX("Violin3", 0.15f);
        //StopAllCoroutines();
        //DOTween.KillAll();
        AudioManager.instance.StopBGM();
    }



}
