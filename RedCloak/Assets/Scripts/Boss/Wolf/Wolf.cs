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
    private static readonly int animSpeed = Animator.StringToHash("AnimSpeed");
    private static readonly int thunder = Animator.StringToHash("Thunder");

    public bool isPhase1 = false;
    public bool isPhase2 = false;
    public bool isPhase3 = false;

    private float bossHealth = 0;
    public float bossMaxHealth;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D wolfCol;
    private Rigidbody2D rigid;
    private GhostDash wolfGhost;

    public bool isBossDie = false;

    public GameObject wolfZone1;
    public GameObject wolfZone2;

    public Collider2D rightAttack;
    public Collider2D leftAttack;

    public GameObject lightening;

    public GameObject transparentWall;

    public WolfZone wolfZone;

    public GameObject Tornado;

    //public Collider2D wolfUpper;

    private ParticleSystem shockWave;

    public bool isStageStart = false;

    [SerializeField] private LayerMask floorLayerMask;

    public GameObject electricWave;

    private int count = 0;

    private int comboCount = 0;

    public Coroutine mainCoroutine;

    public float AnimSpeed = 1.0f;

    private bool isInvincible = false;

    public GameObject shouting;

    private Coroutine ComboCoroutine;

    private bool isPhase2Start = false;

    public GameObject wolfLight;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wolfCol = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        shockWave = GetComponentInChildren<ParticleSystem>();
        wolfGhost = GetComponentInChildren<GhostDash>();
    }

    private void Start()
    {
        animator.SetBool(isDead, true);
        lightening.SetActive(false);
        shockWave.gameObject.SetActive(false);
        rightAttack.enabled = false;
        leftAttack.enabled = false;
        shouting.SetActive(false);
        
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
        UIManager.Instance.uiBar.CallBossBar("Thunder Wolf");
        StartCoroutine(WolfBossStageStart());
        
        isPhase1 = true;
        isPhase2 = false;
        isPhase3 = false;

        transform.DOMove(new Vector3(306, -150, 0), 3f);
        //StartCoroutine(ShockWave(3f));
        AudioManager.instance.PlayWolf("Thunder", 0.5f);
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

        if (mainCoroutine != null)
        {
            //Debug.Log("Stop");
            StopCoroutine(mainCoroutine);
        }
        mainCoroutine = StartCoroutine(Iteration());
    }

    IEnumerator Iteration()
    {
        //Debug.Log("Start");

        
        if (isPhase1)
        {
            animator.SetBool(isJump, false);
            animator.SetBool(isRun, false);
            yield return new WaitForSeconds(1.5f);
            
            switch (count%3)
            {
                case 0:
                    JumpDashAttack();
                    break;
                case 1:
                    ThreeSlash();
                    break;
                case 2:
                    Jump();
                    ElectricWave();

                    break;
                default:
                    break;
            }
            
            
        }

        if (isPhase2)
        {
            if (!isPhase2Start) yield break;

            animator.SetBool(isJump, false);
            animator.SetBool(isRun, false);
            yield return new WaitForSeconds(1f);

            switch (count % 3)
            {
                case 0:
                    ThreeSlash();
                    break;
                case 1:
                    Jump();
                    ElectricWave();
                    break;
                case 2:
                    JumpDashAttack();
                    break;
                default:
                    break;
            }


        }

        if (isPhase3)
        {
            yield return new WaitForSeconds(0.5f);
            //Flip();
            switch (count % 7)
            {
                case 0:
                    yield return new WaitForSeconds(0.5f);
                    Phase3Start();
                    break;
                case 1:
                case 4:                  
                    ThreeSlash();
                    ElectricWave();
                    break;
                case 2:
                case 5:
                    Jump();
                    ElectricWave();
                    break;
                case 3:
                case 6:
                    JumpDashAttack();
                    ElectricWave();
                    break;
                default:
                    //Phase3Start();
                    break;
            }
        }
        count++;
    }

    void ElectricWave()
    {
        GameObject elecProjectile;
        if (spriteRenderer.flipX) elecProjectile = Instantiate(electricWave, rightAttack.transform);
        else elecProjectile = Instantiate(electricWave, leftAttack.transform);
        elecProjectile.transform.LookAt(CharacterManager.Instance.Player.transform.position);
        //elecProjectile.transform.DOScale(8 * elecProjectile.transform.localScale.x, 2f);
    }


    void ThreeSlash()
    {
        //comboCount = 0;
        animator.SetBool(isAttack, true);
        AudioManager.instance.PlayWolf("WolfMulti", 0.2f);


        ComboCoroutine = StartCoroutine(SlashMove());
    }

    IEnumerator SlashMove()
    {

        if (!isPhase1) wolfGhost.makeGhost = true;
        float dir = CharacterManager.Instance.Player.transform.position.x > transform.position.x ? 1f : -1f;

        float time = 0f;



        while (time < 0.9f/AnimSpeed)
        {
            transform.position += new Vector3(Time.deltaTime * 10 * AnimSpeed * dir,0);

            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            
        }
        wolfGhost.makeGhost = false;
    }

    public void DoCombo()
    {
        float dist = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        //Debug.Log(dist);
        if (dist < 5.5f && comboCount < 1)
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

        while (time < 0.1f/AnimSpeed)
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
        if (!isPhase1) wolfGhost.makeGhost = true;
        animator.SetBool(isRun, false);
        animator.SetBool(isAttack, false);
        animator.SetBool(isJump, false);
        animator.SetBool(isDashAttack, true);
        //AudioManager.instance.PlayWolf("ShockWave", 0.2f);
        StartCoroutine(ChargeSound());
        transform.DOMove(CharacterManager.Instance.Player.transform.position+new Vector3(0,wolfCol.bounds.extents.y), 1f/AnimSpeed).SetEase(Ease.InBack).OnComplete(() =>
        {
            animator.SetBool(isDashAttack, false);
            
            StartCoroutine(Combos());
            wolfGhost.makeGhost = false;
        }
        );
        StartCoroutine(WaitDisc(1));
        //Discrimination();
    }

    IEnumerator WaitDisc(float waitTime)
    { 
        yield return new WaitForSeconds(waitTime);
        Discrimination();
    }

    void Jump()
    {
        if(!isPhase1) wolfGhost.makeGhost = true;
        animator.SetBool(isRun, false);
        animator.SetBool(isJump, true);
        shockWave.gameObject.SetActive(true);
        AudioManager.instance.PlayWolf("ShockWave", 0.2f);
        Vector3 firstPos = transform.position;
        Vector3 secondPos = (firstPos- new Vector3(0, wolfCol.bounds.extents.y) + CharacterManager.Instance.Player.transform.position) / 2 + new Vector3(0, 8, 0)+ new Vector3(0, wolfCol.bounds.extents.y);

        RaycastHit2D hit = Physics2D.Raycast(CharacterManager.Instance.Player.transform.position+new Vector3(0, 0.5f, 0), new Vector2(0, -1), 20f,floorLayerMask);
        //Debug.Log(hit.point);
        AudioManager.instance.PlayWolf("WindJump", 0.25f);
        Vector3 thirdPos = new Vector3(hit.point.x, hit.point.y , 0) + new Vector3(0, wolfCol.bounds.extents.y+0.35f);
        transform.DOPath(new[] { secondPos, firstPos + 2*Vector3.up , secondPos, thirdPos,secondPos, thirdPos - Vector3.up }, 1.5f/AnimSpeed, PathType.CubicBezier).SetEase(Ease.OutCubic).OnComplete(() => {
            //
            //animator.SetBool(isJump, false);
            Discrimination();
            shockWave.gameObject.SetActive(false);
            wolfGhost.makeGhost = false;
        });
        
        //Discrimination();
    }


    IEnumerator ChargeSound()
    {
        yield return null;

        if (isPhase1)
        {
            yield return new WaitForSeconds(0.15f);
        }
        AudioManager.instance.PlayWolf("ChargeAttack", 0.1f);

    }


    void AnimatorInit()
    {
        count = 0;
        animator.SetBool(isJump, false);
        animator.SetBool(isRun, false);
        animator.SetBool(isAttack, false);
        //animator.SetBool(isDashAttack, false);
        animator.SetTrigger(isNextPhase);
        Discrimination();
    }

    void SetBossBar()
    {
        UIManager.Instance.uiBar.SetBossBar(bossHealth, bossMaxHealth, 0);
    }

    public void GetDamage(float damage)
    {
        if (isInvincible) return;

        if (bossHealth > damage)
        {
            bossHealth -= damage;
            //Debug.Log($"남은 보스 체력 : {bossHealth}");
            if (bossHealth < (bossMaxHealth * 2 / 3) && isPhase1)
            {
                isPhase1 = false;
                isPhase2 = true;
                isPhase3 = false;

                AnimatorInit();
                Phase2Start();

                AnimSpeed = 1.2f;
                animator.SetFloat(animSpeed, AnimSpeed);
                //Debug.Log("next phase");
                //StopCoroutine(mainCoroutine);
                //mainCoroutine = StartCoroutine(Iteration());
            }

            if (bossHealth < (bossMaxHealth * 1 / 3) && isPhase2)
            {
                isPhase1 = false;
                isPhase3 = true;
                isPhase2 = false;
                AnimSpeed = 1.2f;
                animator.SetFloat(animSpeed, AnimSpeed);

                AnimatorInit();

                wolfZone.RainOn();
                //Debug.Log("next phase");
                //StopCoroutine(mainCoroutine);
                //mainCoroutine = StartCoroutine(Iteration());
            }

            UIManager.Instance.uiBar.SetBossBar(bossHealth, bossMaxHealth, damage);
            StartCoroutine(ColorChanged());
        }
        else
        {
            if (isBossDie) return;

            isStageStart = false;
            UIManager.Instance.uiBar.SetBossBar(0, bossMaxHealth, bossHealth);
            wolfZone.RainOff();
            CallDie();
            wolfCol.enabled = true;
            //wolfUpper.enabled = false;
        }
    }

    IEnumerator ColorChanged()
    {
        float durTime = 0.2f; // invincibleTime; Have To Change
        spriteRenderer.DOColor(Color.red, durTime);
        yield return new WaitForSeconds(durTime);
        spriteRenderer.DOColor(Color.white, durTime);

    }

    void Phase2Start()
    {
        isPhase2Start = true;
        if(mainCoroutine != null) StopCoroutine(mainCoroutine);
        if (ComboCoroutine != null)
        {
            animator.SetBool(isAttack, false);
            StopCoroutine(ComboCoroutine);
        }
        StopAllCoroutines();

        animator.Play("Thunder", -1, 0f);
        AudioManager.instance.PlayWolf("Howling", 0.15f);
        isInvincible = true;

        shouting.SetActive(true);
        animator.SetBool(thunder, true);

        StartCoroutine(Phase2Howling());
    }

    IEnumerator Phase2Howling()
    {
        yield return new WaitForSeconds(3f);
        animator.SetBool(thunder, false);
        shouting.SetActive(false);
        isInvincible = false;
        Discrimination();
    }

    void Phase3Start()
    {
        if (isBossDie) return;
        if (ComboCoroutine != null)
        {
            animator.SetBool(isAttack, false);
            StopCoroutine(ComboCoroutine);
        }

        shockWave.gameObject.SetActive(true);
        
        animator.Play("Thunder", -1, 0f);
        AudioManager.instance.PlayWolf("Howling", 0.1f);
        isInvincible = true;

        CameraManager.Instance.ModifyCameraInfo(new Vector2(10, 10), new Vector2(308, -145));
        CameraManager.Instance.ChangeFOV(9);

        shouting.SetActive(true);
        animator.SetBool(thunder, true);
        
        StartCoroutine(Phase3Thunder());
    }

    IEnumerator Phase3Thunder()
    {

        yield return new WaitForSeconds(3f);
        shouting.SetActive(false);
        AudioManager.instance.PlayWolf("Thunder", 0.5f);
        animator.SetBool(thunder, false);
        animator.SetBool(isDashAttack, true);
        wolfGhost.makeGhost = true;

        Vector3 Dest = new Vector3(306, -100, 0);
        switch (Random.Range(0, 3))
        {
            case 0:
                Dest = new Vector3(306, -100, 0);
                break;
            case 1:
                Dest = new Vector3(294, -100, 0);
                break;
            case 2:
                Dest = new Vector3(319, -100, 0);
                break;
            default:
                Dest = new Vector3(306, -100, 0);
                break;

        }


        transform.DOMove(Dest + new Vector3(0, wolfCol.bounds.extents.y), 1f / AnimSpeed).SetEase(Ease.InBack).OnComplete(() =>
        {
            rigid.gravityScale = 0f;
            StartCoroutine(ShootThunderBolt());
        }
        );
        
    }

    IEnumerator ShootThunderBolt()
    {
        yield return null;

        
        for (int i = 0; i < 10; i++)
        {
            ThunderBolt();
            yield return new WaitForSeconds(0.3f);
        }
        CreateTornado();

        rigid.gravityScale = 3f;
        animator.SetBool(isDashAttack, false);

        yield return new WaitForSeconds(1f);
        

        isInvincible = false;
        Phase3End();

        yield return new WaitForSeconds(1f);
        Discrimination();
    }

    void CreateTornado()
    {
        GameObject Tornados = Instantiate(Tornado);
        Tornados.transform.position = new Vector3(270, -130, 0);
        Tornados.transform.DOMoveX(340, 2f).SetEase(Ease.InSine);
        Tornados.transform.DOMoveY(-160, 2f).SetEase(Ease.OutQuad).OnComplete(() =>
        { 
            Destroy(Tornados, 0.1f);
        }
        );

        GameObject Tornados2 = Instantiate(Tornado);
        Tornados2.transform.position = new Vector3(340, -130, 0);
        Tornados2.transform.DOMoveX(270, 2f).SetEase(Ease.InSine);
        Tornados2.transform.DOMoveY(-160, 2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Destroy(Tornados2, 0.1f);
        }
        );
    }

    void ThunderBolt()
    {
        //Debug.Log("Thunder!!");
        AudioManager.instance.PlayWolf("Thunder", 0.2f);
        GameObject elecProjectile;
       
        elecProjectile = Instantiate(electricWave, transform);
        elecProjectile.transform.position -= new Vector3(0, 20, 0);
        elecProjectile.transform.DOScale(new Vector3(1, 1, 1), 0f);
        elecProjectile.transform.LookAt(CharacterManager.Instance.Player.transform.position);
        elecProjectile.GetComponent<MonsterProjectile>().speed = 60f;
    }


    void Phase3End()
    {
        shockWave.gameObject.SetActive(false);
        wolfGhost.makeGhost = false;
        
        rigid.gravityScale = 3f;
        CameraManager.Instance.ModifyCameraInfo(new Vector2(20, 10), new Vector2(308, -145));
        CameraManager.Instance.ChangeFOV(6);
        
    }


    void CallDie()
    {
        UIManager.Instance.uiBar.CallBackBossBar();
        gameObject.layer = LayerMask.NameToLayer(Define.DEAD);
        animator.Play("Death", -1, 0f);
  
        animator.SetTrigger(isNextPhase);
        animator.SetBool(isDead, true);
        lightening.SetActive(false);
        isBossDie = true;
        //MonsterDataManager.ChangeCatchStat("ENS00046");
        Instantiate(wolfLight, transform.position, Quaternion.identity);

        wolfZone.RemoveWall();
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX("Success", 0.05f);
        AudioManager.instance.PlayBGM("Chloe", 0.1f);
    }



}
