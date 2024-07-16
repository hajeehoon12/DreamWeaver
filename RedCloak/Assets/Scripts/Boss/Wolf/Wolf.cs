using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wolf : MonoBehaviour, IDamage
{
    private static readonly int isNextPhase = Animator.StringToHash("IsNextPhase");
    private static readonly int isJump = Animator.StringToHash("IsJump");
    private static readonly int isRun = Animator.StringToHash("IsRun");
    private static readonly int isDashAttack = Animator.StringToHash("IsDashAttack");
    private static readonly int isDead = Animator.StringToHash("IsDead");
    private static readonly int isAttack = Animator.StringToHash("IsAttack");

    private bool isPhase1 = false;
    private bool isPhase2 = false;
    private bool isPhase3 = false;

    private float bossHealth = 0;
    public float bossMaxHealth;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D wolfCol;
    private Rigidbody2D rigid;

    public bool isBossDie = false;

    public GameObject wolfZone1;
    public GameObject wolfZone2;

    public GameObject lightening;

    public GameObject transparentWall;

    public bool isStageStart = false;

    private int count = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wolfCol = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator.SetBool(isDead, true);
        lightening.SetActive(false);
    }

    private void Update()
    {
        if (isStageStart)
        {
            LookPlayer();
        }
    }

    void LookPlayer()
    {
        spriteRenderer.flipX = (CharacterManager.Instance.Player.transform.position.x > transform.position.x) ? true : false;
    }



    public void CallWolfBoss()
    {
        lightening.SetActive(true);
        isStageStart = true;
        animator.SetBool(isRun, true);
        animator.SetBool(isDead, false);
        AudioManager.instance.PlaySFX("Howling", 0.1f);
        CharacterManager.Instance.Player.controller.cantMove = true;
        
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX("Nervous", 0.1f);
        
        
        
        UIBar.Instance.CallBossBar("Cave Wolf");
        StartCoroutine(WolfBossStageStart());
        CameraManager.Instance.ModifyCameraInfo(new Vector2(20, 10), new Vector2(308, -145));
        isPhase1 = true;
        isPhase2 = false;
        isPhase3 = false;

        transform.DOMove(new Vector3(306, -146, 0), 4f);
        transform.DOScale(10, 4f);
        spriteRenderer.DOFade(1, 4f).OnComplete(() =>
        {
            animator.SetBool(isRun, false);
            transparentWall.SetActive(false);
        });
    }

    IEnumerator WolfBossStageStart()
    {

        CameraManager.Instance.MakeCameraShake(new Vector3(306, -146, 0), 5f, 0.05f, 0.1f);
        yield return new WaitForSeconds(1f);

        float time = 0f;
        float totalTime = 2f;

        while (time < totalTime)
        {
            bossHealth += (bossMaxHealth * Time.deltaTime / totalTime);
            SetBossBar();
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }
        CharacterManager.Instance.Player.controller.cantMove = false;
        
        
        AudioManager.instance.StopBGM();
        AudioManager.instance.StopBGM2();
        AudioManager.instance.PlayBGM("Wolf", 0.05f);
        
        spriteRenderer.flipX = false;

        Discrimination();
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
            yield return new WaitForSeconds(2f);
            Jump();
            
        }

        if (isPhase2)
        {
            yield return new WaitForSeconds(1f);
            
        }

        if (isPhase3)
        {

            yield return new WaitForSeconds(0.5f);
            //Flip();
            switch (count % 2)
            {
                case 0:
                    
                    break;
                case 1:
                    
                    break;
            }
        }

        count++;
    }



    void Jump()
    {
        animator.SetBool(isRun, false);
        animator.SetBool(isJump, true);

        transform.DOMove(CharacterManager.Instance.Player.transform.position+new Vector3(0,wolfCol.bounds.extents.y), 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            animator.SetBool(isJump, false);
            Discrimination();
        }
        );

        //Discrimination();
    }




    void SetBossBar()
    {
        UIBar.Instance.SetBossBar(bossHealth, bossMaxHealth, 0);
    }

    public void GetDamage(float damage)
    {

        if (bossHealth > damage)
        {
            bossHealth -= damage;
            Debug.Log($"남은 보스 체력 : {bossHealth}");
            if (bossHealth < (bossMaxHealth * 2 / 3) && isPhase1)
            {
                isPhase1 = false;
                isPhase2 = true;
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
            isStageStart = false;
            UIBar.Instance.SetBossBar(0, bossMaxHealth, bossHealth);
            CallDie();
            wolfCol.enabled = true;
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
        UIBar.Instance.CallBackBossBar();
        gameObject.layer = LayerMask.NameToLayer(Define.DEAD);
        animator.SetBool(isDead, true);
        lightening.SetActive(false);
        isBossDie = true;
        AudioManager.instance.StopBGM();
        
    }

}
