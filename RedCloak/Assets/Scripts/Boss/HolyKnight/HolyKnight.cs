using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnimationImporter.PyxelEdit;

public class HolyKnight : MonoBehaviour, IDamage
{
    
    private static readonly int notStart = Animator.StringToHash("NotStart");
    private static readonly int isStart = Animator.StringToHash("IsStart");
    private static readonly int castBuff = Animator.StringToHash("CastBuff");

    //private static readonly int isNextPhase = Animator.StringToHash("IsNextPhase");
    //private static readonly int isJump = Animator.StringToHash("IsJump");
    //private static readonly int isRun = Animator.StringToHash("IsRun");
    //private static readonly int isDashAttack = Animator.StringToHash("IsDashAttack");
    //private static readonly int isDead = Animator.StringToHash("IsDead");
    //private static readonly int isAttack = Animator.StringToHash("IsAttack");
    //private static readonly int animSpeed = Animator.StringToHash("AnimSpeed");
    //private static readonly int thunder = Animator.StringToHash("Thunder");



    private float bossHealth = 0;
    public float bossMaxHealth;

    private bool isInvincible = false;
    
    public bool isPhase1 = false;
    public bool isPhase2 = false;
    public bool isPhase3 = false;

    public bool isBossDie = false;

    public bool isStageStart = false;

    SpriteRenderer spriteRenderer;
    Animator animator;
    public GhostDash ghostDash;

    //private int count = 0;

    Coroutine mainCoroutine;
    Coroutine tempCoroutine;

    public bool isFlip = false;
    public bool canFlip = true;

    Vector3 Right = new Vector3(0, 180, 0);
    Vector3 Left = new Vector3(0, 0, 0);

    public bool isDefending = false;

    public float animSpeed = 1.0f;

    public HolyKnightZone zone;

    public Door door;

    Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = CharacterManager.Instance.Player;
    }

    private void Start()
    {
        isStageStart = true;
        animator.SetBool(notStart, true);
    }

    private void Update()
    {
        if (isStageStart && canFlip)
        {
            LookPlayer();
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            CallHolyStage();
        }

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
        animator.SetBool(isStart, true);
        animator.SetBool(notStart, false);
        CameraManager.Instance.MakeCameraShake(transform.position + new Vector3(9, 7, 0) , 4f, 0.05f, 0.1f);
        AudioManager.instance.PlaySFX("Nervous", 0.1f);
        AudioManager.instance.StopBGM();
        isStageStart = true;
        CameraManager.Instance.ModifyCameraInfo(new Vector2(31, 11), new Vector2(8, -275));
        CharacterManager.Instance.Player.controller.cantMove = true;
        CharacterManager.Instance.Player.controller.MakeIdle();



        yield return new WaitForSeconds(1f);
        //AudioManager.instance.PlayHoly("Winter", 0.15f);

        UIManager.Instance.uiBar.CallBossBar("HolyKnight");

        float time = 0f;
        float totalTime = 2f;

        while (time < totalTime)
        {
            bossHealth += (bossMaxHealth * Time.deltaTime / totalTime);
            SetBossBar();
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("Winter", 0.15f);
        CharacterManager.Instance.Player.controller.cantMove = false;
        isPhase1 = true;
        animator.SetBool(castBuff, true);
        yield return new WaitForSeconds(1f);
        Discrimination();
    }

    public void Discrimination()
    {
        if (isBossDie) return;

        if (mainCoroutine != null)
        {
            //Debug.Log("Stop");
            StopCoroutine(mainCoroutine);
        }
        //mainCoroutine = StartCoroutine(Iteration());
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
            //DoCounterAttack();
            return;
        }



        if (bossHealth > damage)
        {
            bossHealth -= damage;
            Debug.Log($"남은 보스 체력 : {bossHealth}");

            if (bossHealth < (bossMaxHealth * 2 / 3) && isPhase1)
            {
                isPhase1 = false;
                isPhase2 = true;
                isPhase3 = false;

                //Debug.Log("next phase");
                //StopCoroutine(mainCoroutine);
                //mainCoroutine = StartCoroutine(Iteration());
            }

            if (bossHealth < (bossMaxHealth * 1 / 3) && isPhase2)
            {
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
        door.OpenDoor();
        //SwordAuraOff();
        CameraManager.Instance.CallStage3CameraInfo();
    }

}
