using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

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
    private static readonly int animSpeed = Animator.StringToHash("AnimSpeed");
    private static readonly int sprint = Animator.StringToHash("Sprint");
    private static readonly int greatHealMotion = Animator.StringToHash("GreatHealMotion");

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

    public bool isInvincible = false;
    
    public bool isPhase1 = false;
    public bool isPhase2 = false;
    public bool isPhase3 = false;

    public bool isPhase3Start = false;

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
    public bool defendSuccess = false;

    public float AnimSpeed = 1.5f;

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
    public GameObject HolyArrow;
    public GameObject holyLight;

    public bool slashMove = false;
    public bool TempStage = false;

    public Door door1;
    public Door door2;

    private bool earthQuakeEnd = false;

    public bool phaseModify = false;
 
    

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
        if (TempStage)
        {
            fog.SetActive(true);
        }
        holySlashRange.SetActive(false);
        Aura.SetActive(false);
        GroundDust.SetActive(false);
        HolyCharge.SetActive(false);
        HolyStarExplosion.SetActive(false);
        LightCutRange.SetActive(false);
        if (!TempStage)
        {
            animator.SetFloat(animSpeed, AnimSpeed);
        }
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
        CharacterManager.Instance.Player.GetComponent<Light2D>().lightType = Light2D.LightType.Global;
        StartCoroutine(HolyStageOn());
    }


    IEnumerator HolyStageOn()
    {
        CharacterManager.Instance.Player.controller.cantMove = true;
        CharacterManager.Instance.Player.controller.MakeIdle();

        if (CameraManager.Instance.stageNum == 3)
        {
            TempStage = true;
        }
        else
        {
            door1.CloseDoor();
            door2.CloseDoor();
            yield return new WaitForSeconds(1.5f);
        }
        Aura.SetActive(true);
        float Dir = isFlip ? -1f : 1f;
        CameraManager.Instance.MakeCameraShake(transform.position + new Vector3(9 * Dir, 7, 0) , 4.7f, 0.05f, 0.1f);
        //
        if (TempStage)
        {
            AudioManager.instance.PlayHoly("WindBlow", 0.1f);
        }
        else
        {
            AudioManager.instance.PlayHoly("HolyStageOn", 0.1f);
        }
        if (TempStage)
        {
            fog.transform.DOMoveX(transform.position.x - 30, 4f);
        }
        yield return new WaitForSeconds(1f);
        animator.SetBool(isStart, true);
        animator.SetBool(notStart, false);
        AudioManager.instance.StopBGM();
        
        isStageStart = true;
        if (TempStage)
        {
            CameraManager.Instance.ModifyCameraInfo(new Vector2(31, 11), new Vector2(8, -275));
        }
        else
        {
            CameraManager.Instance.ModifyCameraInfo(new Vector2(35, 15), new Vector2(-226, 160));
        }
        

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
        if (TempStage)
        {
            AudioManager.instance.PlayBGM("Winter", 0.15f);
        }
        else
        {
            AudioManager.instance.PlayBGM("Mountain", 0.15f);
        }
        
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
            phaseModify = false;
            Aura.SetActive(false);
            if (!TempStage)
            {
                animator.SetBool(run, true);
                yield return new WaitForSeconds(2f);
                animator.SetBool(run, false);
            }
            // Flip();
            switch (count % 3)
            {
                case 0:
                   
                    JumpHeavy();
                    //Attack();
                    break;
                case 1:
                    if (TempStage)
                    {
                        BackDash();
                    }
                    else
                    {
                        BlockStart();
                    }
                    //SpecialAttack();
                    break;
                case 2:
                    BackDash();

                    break;
            }
        }

        if (isPhase3)
        {
            phaseModify = false;
            animator.SetBool(run, false);
            animator.SetBool(sprint, true);
            yield return new WaitForSeconds(1f);
            animator.SetBool(sprint, false);
            //Flip();
            switch (count % 4)
            {
                case 0:
                    if (isPhase3Start) GreatHeal();
                    else BackDash();
                    
                    break;
                case 1:
                    JumpHeavy();
                    
                    break;
                case 2:
                    BlockStart();
                    break;
                case 3:
                    LightCut();
                    break;
            }
        }

        count++;
    }

    void GreatHeal()
    {
        animator.SetBool(greatHealMotion, true);
    }

    public void GreatHealStart()
    {
        //player.controller.cantMove = true;
        CameraManager.Instance.MakeCameraShake(transform.position + new Vector3(0, 4, 0), 2.5f, 0.2f, 0.2f);
        AudioManager.instance.PlayHoly("HolyEarthQuake", 0.2f);
        StartCoroutine(EarthQuake());
    }

    IEnumerator EarthQuake()
    {
        yield return new WaitForSeconds(1.5f);
        while (!earthQuakeEnd)
        {
            // x -267 -190
            // y 168

            Instantiate(HolyArrow, new Vector3(Random.Range(-267, -190), 170, 0),Quaternion.Euler(90, 0, 0));

            yield return new WaitForSeconds(0.4f);
        }
    }

    void GreatHealEffects()
    {
        StartCoroutine(Heal());
    }

    IEnumerator Heal()
    {
        float time = 0f;
        float duringTime = 1.5f;
        while (time < duringTime)
        {
            bossHealth += (bossMaxHealth * Time.deltaTime / (duringTime *3));
            
            SetBossBar();
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void GreatHealEnd()
    {
        canFlip = true;
        isInvincible = false;
        Aura.SetActive(false);
        animator.SetBool(greatHealMotion, false);
        isPhase3Start = false;
        player.controller.cantMove = false;
        Discrimination();
    }

    void BlockStart()
    {
        animator.SetBool(block, true);
        Aura.SetActive(true);
    }

    public void BlockEnd()
    {
        animator.SetBool(block, false);
        Aura.SetActive(false);
        isDefending = false;
        if(!defendSuccess) Discrimination();
    }

    void JumpHeavy()
    {
        HolyCharge.SetActive(true);

        if (!TempStage) ghostDash.makeGhost = true;

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

        if (phaseModify) return;

        transform.DOMove(new Vector3(player.transform.position.x - Dir * 2f , player.transform.position.y + 12f, 1), 0.75f).OnComplete(()=>
        {
            if (phaseModify)
            {
                rigid.gravityScale = 1f;
                return; 
            }

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

        if (TempStage)
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
        animator.SetBool(frontHeavy, true);
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
        float DestinationY = TempStage ? -288.5f : 146.4f;
        float FallTime = TempStage ? 1f : 1f/AnimSpeed;
        transform.DOMoveY(DestinationY, FallTime).SetEase(Ease.InCirc).OnComplete(()=>
        {
            AttackRange.SetActive(false);
            AudioManager.instance.PlayHoly("OnGround", 0.1f);
            rigid.gravityScale = 1f;
            CreateSlash();
            ghostDash.makeGhost = false;
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
                float Dist = TempStage ? 60f : 120f;
                transform.position += new Vector3(Dir * Time.deltaTime * Dist, 0, 0);
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
        defendSuccess = false;
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
        if (isDefending)
        {
            animator.SetBool(block, false);
            AudioManager.instance.PlaySamurai("DefendSuccess", 0.1f);
            defendSuccess = true;
            isDefending = false;
            animator.Play("HolySlash", 0, 0.3f);
            return;
        }




        if (bossHealth > damage)
        {
            bossHealth -= damage;
            //Debug.Log($"남은 보스 체력 : {bossHealth}");

            if (bossHealth < (bossMaxHealth * 2 / 3) && isPhase1)
            {
                phaseModify = true;
                isPhase1 = false;
                isPhase2 = true;
                isPhase3 = false;
                animator.SetBool(airAttack, false);
                rigid.gravityScale = 1f;
                Discrimination();
                //Debug.Log("next phase");
                //StopCoroutine(mainCoroutine);
                //Discrimination();
                //mainCoroutine = StartCoroutine(Iteration());
            }

            if (bossHealth < (bossMaxHealth * 1 / 3) && isPhase2)
            {
                phaseModify = true;
                count = 0;
                isPhase1 = false;
                isPhase3 = true;
                isPhase2 = false;
                isPhase3Start = true;
                animator.SetBool(airAttack, false);
                rigid.gravityScale = 1f;
                Discrimination();
            }

            UIManager.Instance.uiBar.SetBossBar(bossHealth, bossMaxHealth, damage);
            StartCoroutine(ColorChanged());
        }
        else
        {

            if (isBossDie) return;
            if (isPhase3) isBossDie = true;

            animator.Play("KnockBack", -1, 0f);
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

    public void SprintEnd()
    {
        animator.SetBool(sprint, false);
    }

    void StageClear()
    {
        
        this.gameObject.layer = LayerMask.NameToLayer(Define.PLAYERPROJECTILE);
        UIManager.Instance.uiBar.CallBackBossBar();
        CameraManager.Instance.CallStage3CameraInfo("HolyKnight", 3);
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


        //AudioManager.instance.PlaySFX("Success", 0.05f);

        StartCoroutine(ClearAfter());
        //AudioManager.instance.PlaySFX()
    }

    IEnumerator ColorChanged()
    {
        float durTime = 0.2f; // invincibleTime; Have To Change
        spriteRenderer.DOColor(Color.red, durTime);
        yield return new WaitForSeconds(durTime);
        spriteRenderer.DOColor(Color.white, durTime);
    }

    IEnumerator KnockBack()
    {
        float Dir = isFlip ? 1f : -1f;
        float time = 0f;
        earthQuakeEnd = true;
        AudioManager.instance.PlayHoly("HolyDie", 0.1f);
        while (time < 1f)
        {
            transform.position += new Vector3(Dir * Time.deltaTime * 6, 0, 0);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
    }

    IEnumerator ClearAfter()
    {

        this.gameObject.layer = LayerMask.NameToLayer(Define.PLAYERPROJECTILE);
        UIManager.Instance.uiBar.CallBackBossBar();
        Aura.SetActive(false);
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayHoly("HolyBeat", 0.35f);
        rigid.gravityScale = 3f;
        Time.timeScale = 0.3f;
        StartCoroutine(KnockBack());
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1f;
        door1.OpenDoor();
        door2.OpenDoor();
        yield return new WaitForSeconds(2f);

        CameraManager.Instance.CallStage4CameraInfo("Holy Knight");
        AudioManager.instance.PlaySFX("Success", 0.05f);

        yield return new WaitForSeconds(4f);
        Instantiate(holyLight, transform.position, Quaternion.identity);
    }






}
