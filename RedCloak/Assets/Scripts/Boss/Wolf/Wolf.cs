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
     
        animator.SetBool(isDead, false);
        AudioManager.instance.PlaySFX("Howling", 0.1f);
        CharacterManager.Instance.Player.controller.cantMove = true;
        
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX("Nervous", 0.1f);

        CameraManager.Instance.ModifyCameraInfo(new Vector2(20, 10), new Vector2(308, -145));
        StartCoroutine(WolfStageOn());
    }

    IEnumerator WolfStageOn()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool(isRun, true);
        UIBar.Instance.CallBossBar("Cave Wolf");
        StartCoroutine(WolfBossStageStart());
        
        isPhase1 = true;
        isPhase2 = false;
        isPhase3 = false;

        transform.DOMove(new Vector3(306, -146, 0), 3f);
        AudioManager.instance.PlayPitchSFX("ShockWave", 0.2f);
        transform.DOScale(10, 3f);
        spriteRenderer.DOFade(1, 3f).OnComplete(() =>
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
            animator.SetBool(isJump, false);
            //animator.SetBool(isRun, true);
            yield return new WaitForSeconds(2f);
            //animator.SetBool(isRun, false);
            switch (count%2)
            {
                case 0:
                    JumpDashAttack();
                    break;
                case 1:
                    Jump();
                    break;
                default:
                    break;
            }
            
            
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



    void JumpDashAttack()
    {
        animator.SetBool(isRun, false);
        animator.SetBool(isDashAttack, true);
        StartCoroutine(ChargeSound());
        transform.DOMove(CharacterManager.Instance.Player.transform.position+new Vector3(0,wolfCol.bounds.extents.y), 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            animator.SetBool(isDashAttack, false);
            Discrimination();
        }
        );
        //Discrimination();
    }

    void Jump()
    {
        animator.SetBool(isRun, false);
        animator.SetBool(isJump, true);

        Vector3 firstPos = transform.position;
        Vector3 secondPos = (firstPos- new Vector3(0, wolfCol.bounds.extents.y) + CharacterManager.Instance.Player.transform.position) / 2 + new Vector3(0, 8, 0)+ new Vector3(0, wolfCol.bounds.extents.y);
        Vector3 thirdPos = CharacterManager.Instance.Player.transform.position + new Vector3(0, wolfCol.bounds.extents.y+0.35f);
        transform.DOPath(new[] { secondPos, firstPos + 2*Vector3.up , secondPos, thirdPos,secondPos, thirdPos - Vector3.up }, 1.5f, PathType.CubicBezier).SetEase(Ease.OutCubic).OnComplete(() => {

            //animator.SetBool(isJump, false);
            Discrimination();
        });
        
        //Discrimination();
    }


    IEnumerator ChargeSound()
    {
        yield return new WaitForSeconds(0.15f);
        AudioManager.instance.PlaySFX("ChargeAttack", 0.1f);

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
