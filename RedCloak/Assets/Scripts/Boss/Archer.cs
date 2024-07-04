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

    private float bossHealth = 300;
    public float bossMaxHealth = 300;

    SpriteRenderer spriteRenderer;
    Collider2D archerCol;

    public LayerMask ObstacleLayerMask;
    public LayerMask FloorLayerMask;

    Vector2[] appearPos = { new Vector2(15, 0), new Vector2(-15, 0), new Vector2(14, 2), new Vector2(13, 4), new Vector2(12, 6), new Vector2(-12, 6), new Vector2(-13, 4), new Vector2(-14, 2),new Vector2(-14, -2), new Vector2(14, -2) };

    private bool isPhase1 = true;
    private bool isPhase2 = false;
    private bool isPhase3 = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        archerCol = GetComponent<Collider2D>();
    }


    private void Start()
    {
        bossHealth = bossMaxHealth;
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("SilverBird", 0.15f);
        Discrimination();
        SetBossBar();
    }

    void Discrimination()
    {
        if (isBossDie) return;
        StartCoroutine(Iteration());
    }

    IEnumerator Iteration()
    {
        
        if (isPhase1)
        {
            yield return new WaitForSeconds(1f);
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
        }

        if (isPhase2)
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
        }

        if (isPhase3)
        {
           
            yield return new WaitForSeconds(0.25f);
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

            if (isPhase1)
            {
                rand = Random.Range(0, 2);
            }

            if (isPhase2 || isPhase3)
            {
                rand = Random.Range(0, appearPos.Length);
            }


            targetPos += appearPos[rand];

            RaycastHit2D hit = Physics2D.Raycast(CharacterManager.Instance.Player.transform.position+new Vector3(0,CharacterManager.Instance.Player.controller.playerCollider.bounds.extents.y * 2), (targetPos - (Vector2)CharacterManager.Instance.Player.transform.position).normalized, 22f, ObstacleLayerMask);
            
            if (hit.collider?.name == null) break;

            tempCount++;
        }

        if (isPhase1)
        {
            transform.position = new Vector3(targetPos.x, targetPos.y);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, archerCol.bounds.extents.y), new Vector2(0, -1), 10f, FloorLayerMask);

            transform.position = new Vector3(targetPos.x, hit.point.y+1);
            //Debug.Log(transform.position);
            //Debug.Log(isPhase1);
        }
        else
        {
            transform.position = targetPos;
            if (transform.position.y < -45f)
            {
                transform.position = new Vector3(transform.position.x, -45f);
            }
        }

        if (tempCount == 100)
        {
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, 115f, 168f), Mathf.Clamp(transform.position.y,-45f, -35f));
        }

        Flip();
    }


    void Attack()
    {
       
        StartCoroutine(DoAttack());
    }

    void SetBossBar()
    {
        UIBar.Instance.SetBossBar(bossMaxHealth, bossMaxHealth, 0);
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
        archerCol.enabled = true;
        AudioManager.instance.PlaySFX("Charge1sec", 0.2f);
    }

    void Vanish()
    {
        archerCol.enabled = false;
        AudioManager.instance.PlaySFX("Vanish", 0.2f);
    }

    void Appear()
    {
        if (isPhase3)
        {
            animator.Play("Special Attack", -1, 0f);
        }
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

            if (bossHealth < (bossMaxHealth * 2 / 3) && isPhase1)
            {
                isPhase1 = false;
                isPhase2 = true;
            }

            if (bossHealth < (bossMaxHealth / 3) && isPhase2)
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
        gameObject.layer = LayerMask.NameToLayer("Default");
        gameObject.tag = "Platform";
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //Collider2D[] archers = GetComponentsInChildren<Collider2D>();

        //for (int i = 0; i < archers.Length; i++)
        //{
        //    archers[i].enabled = false;
        //}


        animator.SetBool(isDead, true);
        isBossDie = true;
        StartCoroutine(ArcherDie());
    }

    IEnumerator ArcherDie()
    {
        transform.DORotateQuaternion(Quaternion.identity, 0.3f);
        AudioManager.instance.PlaySFX("ArcherDeath", 0.5f);
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX("Violin3", 0.05f);
        //StopAllCoroutines();
        //DOTween.KillAll();
        AudioManager.instance.StopBGM();

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isBossDie)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(Define.FLOOR_Layer))
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                //Debug.Log("enabled");

                //archerCol.isTrigger = true;
                archerCol.enabled = false;
            }
        }
    }



}
