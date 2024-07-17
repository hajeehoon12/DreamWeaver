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

    public Collider2D rightAttack;
    public Collider2D leftAttack;

    public GameObject lightening;

    public GameObject transparentWall;

    public WolfZone wolfZone;

    private ParticleSystem shockWave;

    public bool isStageStart = false;

    [SerializeField] private LayerMask floorLayerMask;

    private int count = 0;

    private int comboCount = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wolfCol = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        shockWave = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        animator.SetBool(isDead, true);
        lightening.SetActive(false);
        shockWave.gameObject.SetActive(false);
        rightAttack.enabled = false;
        leftAttack.enabled = false;
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
        if (isBossDie) return;
        spriteRenderer.flipX = (CharacterManager.Instance.Player.transform.position.x > transform.position.x) ? true : false;
    }



    public void CallWolfBoss()
    {
        lightening.SetActive(true);
        isStageStart = true;
        CameraManager.Instance.ModifyCameraInfo(new Vector2(20, 10), new Vector2(308, -145));
        animator.SetBool(isDead, false);
        AudioManager.instance.PlayWolf("Howling", 0.1f);
        AudioManager.instance.PlaySFX("Nervous", 0.1f);
        CharacterManager.Instance.Player.controller.cantMove = true;
        
        AudioManager.instance.StopBGM();

        CameraManager.Instance.MakeCameraShake(new Vector3(306, -146, 0), 6f, 0.05f, 0.1f);

        StartCoroutine(WolfStageOn());
    }

    IEnumerator WolfStageOn()
    {
        yield return new WaitForSeconds(1f);
        shockWave.gameObject.SetActive(true);
        animator.SetBool(isRun, true);
        UIBar.Instance.CallBossBar("Thunder Wolf");
        StartCoroutine(WolfBossStageStart());
        
        isPhase1 = true;
        isPhase2 = false;
        isPhase3 = false;

        transform.DOMove(new Vector3(306, -146, 0), 3f);
        StartCoroutine(ShockWave(3f));
        transform.DOScale(10, 3f);
        spriteRenderer.DOFade(1, 3f).OnComplete(() =>
        {
            animator.SetBool(isRun, false);
            transparentWall.SetActive(false);
            shockWave.gameObject.SetActive(false);
        });
    }

    IEnumerator ShockWave(float totalTime)
    {
        float time = 0f;
        while (time < totalTime)
        {
            AudioManager.instance.PlayWolf("ShockWave", 0.2f);
            time += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator WolfBossStageStart()
    {

        
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
            switch (count%3)
            {
                case 0:
                    ThreeSlash();
                    break;
                case 1:
                    Jump();
                    break;
                case 2:
                    JumpDashAttack();
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

    void ThreeSlash()
    {
        //comboCount = 0;
        animator.SetBool(isAttack, true);
        AudioManager.instance.PlayWolf("WolfMulti", 0.2f);


        StartCoroutine(SlashMove());
    }

    IEnumerator SlashMove()
    {
        float dir = CharacterManager.Instance.Player.transform.position.x > transform.position.x ? 1f : -1f;

        float time = 0f;

        while (time < 0.9f)
        {
            transform.position += new Vector3(Time.deltaTime * 10 * dir,0);

            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void DoCombo()
    {
        float dist = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        Debug.Log(dist);
        if (dist < 5 && comboCount < 1)
        {
            comboCount++;
            ThreeSlash();
        }
        else
        {
            comboCount = 0;
            animator.SetBool(isAttack, false);
            Discrimination();
        }
    }

    void Combo()
    {
        StartCoroutine(Combos());
    }

    IEnumerator Combos()
    {
        float dir = spriteRenderer.flipX ? 1f : -1f;
        float time = 0f;

        if (dir == 1f) rightAttack.enabled = true;
        else leftAttack.enabled = true;

        while (time < 0.1f)
        {
            transform.position += new Vector3(Time.deltaTime * 20 * dir, 0);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (dir == 1f) rightAttack.enabled = false;
        else leftAttack.enabled = false;


    }



    void JumpDashAttack()
    {
        animator.SetBool(isRun, false);
        animator.SetBool(isDashAttack, true);
        //AudioManager.instance.PlayWolf("ShockWave", 0.2f);
        StartCoroutine(ChargeSound());
        transform.DOMove(CharacterManager.Instance.Player.transform.position+new Vector3(0,wolfCol.bounds.extents.y), 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            animator.SetBool(isDashAttack, false);
            Discrimination();
            StartCoroutine(Combos());
        }
        );
        //Discrimination();
    }

    void Jump()
    {
        animator.SetBool(isRun, false);
        animator.SetBool(isJump, true);
        shockWave.gameObject.SetActive(true);
        AudioManager.instance.PlayWolf("ShockWave", 0.2f);
        Vector3 firstPos = transform.position;
        Vector3 secondPos = (firstPos- new Vector3(0, wolfCol.bounds.extents.y) + CharacterManager.Instance.Player.transform.position) / 2 + new Vector3(0, 8, 0)+ new Vector3(0, wolfCol.bounds.extents.y);

        RaycastHit2D hit = Physics2D.Raycast(CharacterManager.Instance.Player.transform.position+new Vector3(0, 0.5f, 0), new Vector2(0, -1), 20f,floorLayerMask);
        Debug.Log(hit.point);
        AudioManager.instance.PlayWolf("WindJump", 0.25f);
        Vector3 thirdPos = new Vector3(hit.point.x, hit.point.y , 0) + new Vector3(0, wolfCol.bounds.extents.y+0.35f);
        transform.DOPath(new[] { secondPos, firstPos + 2*Vector3.up , secondPos, thirdPos,secondPos, thirdPos - Vector3.up }, 1.5f, PathType.CubicBezier).SetEase(Ease.OutCubic).OnComplete(() => {
            //
            //animator.SetBool(isJump, false);
            Discrimination();
            shockWave.gameObject.SetActive(false);
        });
        
        //Discrimination();
    }


    IEnumerator ChargeSound()
    {
        yield return new WaitForSeconds(0.15f);
        AudioManager.instance.PlayWolf("ChargeAttack", 0.1f);

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
                animator.SetBool(isJump , false);
                animator.SetBool(isRun, false);
                animator.SetTrigger(isNextPhase);
            }

            if (bossHealth < (bossMaxHealth * 1 / 3) && isPhase2)
            {
                isPhase1 = false;
                isPhase3 = true;
                isPhase2 = false;
                animator.SetBool(isJump, false);
                animator.SetBool(isRun, false);
                animator.SetTrigger(isNextPhase);
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
        animator.Play("Death", -1, 0f);
        animator.SetTrigger(isNextPhase);
        animator.SetBool(isDead, true);
        lightening.SetActive(false);
        isBossDie = true;
        wolfZone.RemoveWall();
        AudioManager.instance.StopBGM();
        
    }

}
