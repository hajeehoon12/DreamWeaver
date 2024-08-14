using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HolyKnight : MonoBehaviour, IDamage
{
    
    private static readonly int notStart = Animator.StringToHash("NotStart");
    private static readonly int isStart = Animator.StringToHash("IsStart");
    private static readonly int castBuff = Animator.StringToHash("CastBuff");
    private static readonly int dashEnd = Animator.StringToHash("DashEnd");
    private static readonly int lightCut = Animator.StringToHash("LightCut");
    private static readonly int holySlash = Animator.StringToHash("HolySlash");
    private static readonly int run = Animator.StringToHash("Run");
    private static readonly int frontHeavy = Animator.StringToHash("FrontHeavy");
    private static readonly int jumpAttack = Animator.StringToHash("JumpAttack");
    private static readonly int block = Animator.StringToHash("Block");
    private static readonly int airAttack = Animator.StringToHash("AirAttack");
    private static readonly int backDash = Animator.StringToHash("BackDash");

    //private static readonly int isNextPhase = Animator.StringToHash("IsNextPhase");
    //private static readonly int isJump = Animator.StringToHash("IsJump");
    //private static readonly int isRun = Animator.StringToHash("IsRun");
    //private static readonly int isDashAttack = Animator.StringToHash("IsDashAttack");
    //private static readonly int isDead = Animator.StringToHash("IsDead");
    //private static readonly int isAttack = Animator.StringToHash("IsAttack");
    //private static readonly int animSpeed = Animator.StringToHash("AnimSpeed");
    //private static readonly int thunder = Animator.StringToHash("Thunder");

    //public LayerMask groundLayerMask;
    public LayerMask groundLayerMask;

    private float bossHealth = 0;
    public float bossMaxHealth;

    private bool isInvincible = false;
    
    public bool isPhase1 = false;
    public bool isPhase2 = false;
    public bool isPhase3 = false;

    public bool isBossDie = false;

    public bool isStageStart = false;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator animator;
    public GhostDash ghostDash;

    //private int count = 0;

    Coroutine mainCoroutine;
    Coroutine tempCoroutine;

    public bool isFlip = false;
    public bool canFlip = true;

    public bool isFrontDash = false;

    Vector3 Right = new Vector3(0, 180, 0);
    Vector3 Left = new Vector3(0, 0, 0);

    public bool isDefending = false;

    public float animSpeed = 1.0f;

    public HolyKnightZone zone;

    //public Door door;

    Player player;

    float count = 0;

    public GameObject AttackRange;
    public GameObject LightCutRange;
    public GameObject fog;
    public GameObject holySlashRange;
    public GameObject Aura;
    public GameObject GroundDust;
    public GameObject HolyCharge;
    public GameObject HolyStarExplosion;
    public GameObject SpinSlash;
    public GameObject HolyRapid;

    public bool slashMove = false;
    public bool TempStage = false;

    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = CharacterManager.Instance.Player;
        rigid = GetComponent<Rigidbody2D>();

        groundLayerMask = LayerMask.GetMask("Floor") | LayerMask.GetMask("Wall") | LayerMask.GetMask("Minimap");
    }

    private void Start()
    {
        isStageStart = true;
        animator.SetBool(notStart, true);
        fog.SetActive(true);
        holySlashRange.SetActive(false);
        Aura.SetActive(false);
        GroundDust.SetActive(false);
        HolyCharge.SetActive(false);
        HolyStarExplosion.SetActive(false);
        LightCutRange.SetActive(false);
    }

    private void Update()
    {
        if (isStageStart && canFlip)
        {
            LookPlayer();
        }

        //if (Input.GetKeyUp(KeyCode.V))
        //{
        //    CallHolyStage();
        //}

    }

    void LookPlayer()
    {
        if (isBossDie) return;

        if (CharacterManager.Instance.Player.transform.position.x > transform.position.x)
        {
            transform.localEulerAngles = Left;
            isFlip = false;
        }
        else
        {
            transform.localEulerAngles = Right;
            isFlip = true;
        }

    }

    public void CallHolyStage()
    {
        StartCoroutine(HolyStageOn());
    }


    IEnumerator HolyStageOn()
    {
        if (CameraManager.Instance.stageNum == 3)
        {
            TempStage = true;
        }
        Aura.SetActive(true);
        CameraManager.Instance.MakeCameraShake(transform.position + new Vector3(9, 7, 0) , 4.7f, 0.05f, 0.1f);
        //
        AudioManager.instance.PlayHoly("WindBlow", 0.1f);
        fog.transform.DOMoveX(transform.position.x - 30, 4f);
        yield return new WaitForSeconds(1f);
        animator.SetBool(isStart, true);
        animator.SetBool(notStart, false);
        AudioManager.instance.StopBGM();
        
        isStageStart = true;
        CameraManager.Instance.ModifyCameraInfo(new Vector2(31, 11), new Vector2(8, -275));
        CharacterManager.Instance.Player.controller.cantMove = true;
        CharacterManager.Instance.Player.controller.MakeIdle();

        yield return new WaitForSeconds(0.6f);
        AudioManager.instance.PlayHoly("Sigh", 0.07f, 1.1f);
        yield return new WaitForSeconds(0.4f);
        
        //AudioManager.instance.PlayHoly("Winter", 0.15f);

        UIManager.Instance.uiBar.CallBossBar("HolyKnight");
        
        

        float time = 0f;
        float totalTime = 1.5f;
        StartCoroutine(AuraEffect1sec());
        while (time < totalTime)
        {
            bossHealth += (bossMaxHealth * Time.deltaTime / totalTime);
            SetBossBar();
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }
        //Aura.SetActive(false);
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("Winter", 0.15f);
        
        isPhase1 = true;
        animator.SetBool(castBuff, true);
        yield return new WaitForSeconds(2f);
        CharacterManager.Instance.Player.controller.cantMove = false;
        Discrimination();
    }

    IEnumerator AuraEffect1sec()
    {
        yield return new WaitForSeconds(1f);
        Aura.SetActive(false);
    }

    public void Discrimination()
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
        canFlip = true;
        if (isPhase1)
        {
            animator.SetBool(run, true);
            yield return new WaitForSeconds(2f);
            animator.SetBool(run, false);
            //Flip();
            switch (count % 4)
            {
                case 0:
                    JumpHeavy();

                    break;
                case 1:
                    HolySlash();
                    break;
                case 2:
                    FrontHeavy();
                    break;
                case 3:
                    LightCut();
                    break;
                case 4:
                    
                    break;
                default:
                    BlockStart();
                    break;
            }
        }

        if (isPhase2)
        {
            if (!TempStage)
            {
                animator.SetBool(run, true);
                yield return new WaitForSeconds(2f);
                animator.SetBool(run, false);
            }
            // Flip();
            switch (count % 2)
            {
                case 0:
                   
                    JumpHeavy();
                    //Attack();
                    break;
                case 1:
                    BackDash();
                    //SpecialAttack();
                    break;
                case 2:
                    if (CameraManager.Instance.stageNum == 3)
                    {
                        
                    }

                    break;
            }
        }

        if (isPhase3)
        {

            yield return new WaitForSeconds(1f);
            //Flip();
            switch (count % 2)
            {
                case 0:
                    //Attack();
                    break;
                case 1:
                    //SpecialAttack();
                    break;
            }
        }

        count++;
    }

    void BlockStart()
    {
        animator.SetBool(block, true);
        Aura.SetActive(false);
    }

    public void BlockEnd()
    {
        animator.SetBool(block, false);
    }

    void JumpHeavy()
    {
        HolyCharge.SetActive(true);

        if (isPhase1)
        {
            animator.SetBool(airAttack, false);
        }
        else animator.SetBool(airAttack, true);

        AudioManager.instance.PlayHolyPitch("LongBattleCry", 0.15f);
        AudioManager.instance.PlayHoly("JumpDash", 0.2f);
        float Dir = isFlip ? -1f : 1f;
        animator.SetBool(jumpAttack, true);
        rigid.gravityScale = 0;
        transform.DOMove(new Vector3(player.transform.position.x - Dir * 2f , player.transform.position.y + 12f, 1), 0.75f).OnComplete(()=>
        {
            if (isPhase1)
            {
                HeavyAttackDown();
            }
            else
            {
                AirAttack();
            }
        }
        );
    }

    void BackDash()
    {
        if (CameraManager.Instance.stageNum != 4)
        {
            this.gameObject.layer = LayerMask.NameToLayer(Define.PLAYERPROJECTILE);
            AudioManager.instance.PlayHoly("HolyDefeat", 0.2f, 1.2f);
        }
        animator.SetBool(backDash, true);

    }

    public void DisAppear()
    {
        HolyCharge.SetActive(true);
        GroundDust.SetActive(true);
        AudioManager.instance.PlayHoly("JumpDash", 0.2f);
        AudioManager.instance.PlayHoly("HolyWarp", 0.2f);
        spriteRenderer.DOFade(0, 1f).OnComplete(() =>
        {
            StageClear();
            HolyCharge.SetActive(false);
            GroundDust.SetActive(false);
            

        }

        );
    }

    public void EndBackDash()
    {
        animator.SetBool(backDash, false);
    }


    void AirAttack()
    {
        AudioManager.instance.PlayHoly("DoubleAttack", 0.05f);
        animator.SetBool(airAttack, true);
        animator.SetBool(jumpAttack, false);
        //HeavyAttackDown();
    }

    void AirRapidSlash()
    {
        AudioManager.instance.PlayHoly("Rapid", 0.1f);
        AttackRange.SetActive(true);
        float Dir = isFlip ? -1f : 1f;
        GameObject blueRapid = Instantiate(HolyRapid, transform.position + new Vector3(Dir * 2, 1, 0), Quaternion.identity);
        blueRapid.transform.LookAt(CharacterManager.Instance.Player.transform.position);
    }

    public void HeavyAttackDown()
    {
        AttackRange.SetActive(true);
        animator.SetBool(jumpAttack, false);
        HolyCharge.SetActive(false);
        StartCoroutine(HeavyAttackSound());
        transform.DOMoveY(-288.5f, 1f).SetEase(Ease.InCirc).OnComplete(()=>
        {
            AttackRange.SetActive(false);
            AudioManager.instance.PlayHoly("OnGround", 0.1f);
            rigid.gravityScale = 1f;
            CreateSlash();
        }
        );
    }

    IEnumerator HeavyAttackSound()
    {
        yield return new WaitForSeconds(0.15f);
        AudioManager.instance.PlayHoly("HeavyAir", 0.2f);
    }

    void CreateSlash()
    {
        float Dir = isFlip ? -1f : 1f;
        AudioManager.instance.PlaySamurai("SpinBlade", 0.2f);
        GameObject SpinBlade = Instantiate(SpinSlash, new Vector3(transform.position.x + Dir *2, transform.position.y, 1f), Quaternion.identity);
        SpinBlade.transform.DOMoveX(transform.position.x + Dir * 40, 2f).SetEase(Ease.InBack).OnComplete(() => Discrimination());
        Destroy(SpinBlade, 3);
        //Discrimination();
    }


    void FrontHeavy()
    {
        animator.SetBool(frontHeavy, true);
    }

    void FrontAttack()
    {    
        holySlashRange.SetActive(true);
        HolyStarExplosion.SetActive(true);
        GroundDust.SetActive(true);
        AudioManager.instance.PlayHoly("Explosion", 0.1f);
        isFrontDash = true;
        ghostDash.makeGhost = true;
        StartCoroutine(FrontAttackDuring());
    }

    void FrontAttackEnd()
    {
        holySlashRange.SetActive(false);
        
        isFrontDash = false;
    }

    IEnumerator FrontAttackDuring()
    {
        //yield return new WaitForSeconds(1f);
        float Dir;
        bool isWall;

        while (isFrontDash)
        {
            Dir = isFlip ? -1f : 1f;
            isWall = Physics2D.Raycast(transform.position + new Vector3(2, 0), Vector2.right * Dir, 4f, groundLayerMask);
            if (!isWall)
            {
                transform.position += new Vector3(Dir * Time.deltaTime * 60, 0, 0);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void FrontEnd()
    {
        ghostDash.makeGhost = false;
        animator.SetBool(frontHeavy, false);
        HolyStarExplosion.SetActive(false);
        GroundDust.SetActive(false);
        Discrimination();
    }


    void HolySlash()
    {
        HolyCharge.SetActive(true);
        animator.SetBool(holySlash, true);
       
    }

    public void HolySlashEffects()
    {
        GroundDust.SetActive(true);
    }

    void HolySlashRangeAttack()
    {
        HolyCharge.SetActive(false);
        holySlashRange.SetActive(true);
        GroundDust.SetActive(false);
        StartCoroutine(RangeMove());
        HolyStarExplosion.SetActive(true);
        ghostDash.makeGhost = true;
        canFlip = false;
    }

    IEnumerator RangeMove()
    {
        canFlip = true;
        AudioManager.instance.PlayHoly("SlowMotion", 0.25f);
        yield return new WaitForSeconds(0.1f);
        holySlashRange.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0f);
        holySlashRange.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 140), 0.5f);
        float Dir = isFlip ? -1f : 1f;
        slashMove = true;
        
        //Debug.Log("Start");
    }

    void HolySlashRangeEnd()
    {
        holySlashRange.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.2f);
        slashMove = false;
        //Debug.Log("End");
    }


    public void HolySlashEnd()
    {
        ghostDash.makeGhost = false;
        HolyStarExplosion.SetActive(false);
        animator.SetBool(holySlash, false);
        holySlashRange.SetActive(false);
        canFlip = true;
        Discrimination();
        
        //transform.DOLocalMoveX(0.3f, 0.25f);
    }




    void LightCut()
    {
        animator.SetBool(lightCut, true);
        Aura.SetActive(true);
        //Discrimination();
    }

    public void LightCutEffects()
    {
        HolyCharge.SetActive(true);
    }

    void LightCutSlashStart()
    {
        Aura.SetActive(false);
        HolyCharge.SetActive(false);
        LightCutRange.transform.localPosition = new Vector3(0, 0, 0);
        LightCutRange.SetActive(true);
        float Dir = isFlip ? -1f : 1f;
        LightCutRange.transform.DOMoveX(LightCutRange.transform.position.x + Dir * 15, 0.34f);
        AudioManager.instance.PlayHoly("LightSlash", 0.1f);
        AudioManager.instance.PlayHolyPitch("BattleCry1", 0.15f);
    }

    void LightCutSlashEnd()
    {
        LightCutRange.transform.localPosition = new Vector3(0, 0, 0);
        LightCutRange.SetActive(false);

    }
    public void LightCutEnd()
    {
        animator.SetBool(lightCut, false);
        Discrimination();
    }


    

    void SetBossBar()
    {
        UIManager.Instance.uiBar.SetBossBar(bossHealth, bossMaxHealth, 0);
    }


    public void GetDamage(float damage)
    {
        if (isInvincible) return;
        if (!isStageStart) return;
        if (animator.GetBool(block))
        {
            AudioManager.instance.PlaySamurai("DefendSuccess", 0.1f);
            return;
        }

        if (isDefending)
        {
            //DoCounterAttack();
            return;
        }



        if (bossHealth > damage)
        {
            bossHealth -= damage;
            //Debug.Log($"남은 보스 체력 : {bossHealth}");

            if (bossHealth < (bossMaxHealth * 2 / 3) && isPhase1)
            {
                isPhase1 = false;
                isPhase2 = true;
                isPhase3 = false;

                //Debug.Log("next phase");
                //StopCoroutine(mainCoroutine);
                //Discrimination();
                //mainCoroutine = StartCoroutine(Iteration());
            }

            if (bossHealth < (bossMaxHealth * 1 / 3) && isPhase2)
            {
                count = 0;
                isPhase1 = false;
                isPhase3 = true;
                isPhase2 = false;

            }

            UIManager.Instance.uiBar.SetBossBar(bossHealth, bossMaxHealth, damage);
            StartCoroutine(ColorChanged());
        }
        else
        {

            if (isBossDie) return;
            if (isPhase3) isBossDie = true;

            animator.Play("Death", -1, 0f);
            //AudioManager.instance.PlaySamurai("SamuraiDie", 0.15f);
            UIManager.Instance.uiBar.SetBossBar(0, bossMaxHealth, bossHealth);

            CallDie();

        }
    }

    public void DashEnd()
    {
        animator.SetBool(dashEnd, false);
    }

    public void RunEnd()
    {
        animator.SetBool(run, false);
    }

    void StageClear()
    {
        
        this.gameObject.layer = LayerMask.NameToLayer(Define.PLAYERPROJECTILE);
        UIManager.Instance.uiBar.CallBackBossBar();
        CameraManager.Instance.CallStage3CameraInfo("HolyKnight");
        StartCoroutine(TempStageAfter());
    }

    IEnumerator TempStageAfter()
    {
        yield return new WaitForSeconds(0.5f);

        FadeManager.instance.FadeOut(0.2f);
        yield return new WaitForSeconds(2f);
        FadeManager.instance.FadeIn(0.2f);
        CharacterManager.Instance.doors[3].OpenDoor();
        CharacterManager.Instance.ChangeDoorOpenStat(3);
        zone.RemoveWall();
        StopAllCoroutines();
        MonsterDataManager.ChangeCatchStat("ENS00130");
        isBossDie = true;
        gameObject.SetActive(false);
    }

    void CallDie()
    {
        isStageStart = false;
        if (mainCoroutine != null) StopCoroutine(mainCoroutine);


        AudioManager.instance.PlaySFX("Success", 0.05f);

        ClearAfter();
        //AudioManager.instance.PlaySFX()
    }

    IEnumerator ColorChanged()
    {
        float durTime = 0.2f; // invincibleTime; Have To Change
        spriteRenderer.DOColor(Color.red, durTime);
        yield return new WaitForSeconds(durTime);
        spriteRenderer.DOColor(Color.white, durTime);
    }


    void ClearAfter()
    {
        //samuraiZone.EndStageBoss();
        this.gameObject.layer = LayerMask.NameToLayer(Define.PLAYERPROJECTILE);
        zone.RemoveWall();
        //door.OpenDoor();
        UIManager.Instance.uiBar.CallBackBossBar();
        //SwordAuraOff();
        CameraManager.Instance.CallStage3CameraInfo();
    }






}
