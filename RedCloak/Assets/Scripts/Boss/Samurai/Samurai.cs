using AnimationImporter.PyxelEdit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Samurai : MonoBehaviour, IDamage
{
    private static readonly int isBaldo = Animator.StringToHash("IsBaldo");
    private static readonly int isDefend = Animator.StringToHash("IsDefend");
    private static readonly int revenge = Animator.StringToHash("Revenge");
    private static readonly int isSpinBlade = Animator.StringToHash("IsSpinBlade");
    //private static readonly int isDead = Animator.StringToHash("IsDead");
    private static readonly int isAttack = Animator.StringToHash("IsAttack");
    private static readonly int isRun = Animator.StringToHash("IsRun");
    //private static readonly int thunder = Animator.StringToHash("Thunder");
    private static readonly int isDashAttack = Animator.StringToHash("IsDashAttack");


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

    private int count = 0;

    Coroutine mainCoroutine;
    Coroutine tempCoroutine;

    public bool isFlip = false;
    public bool canFlip = true;

    Vector3 Right = new Vector3(0, 180, 0);
    Vector3 Left = new Vector3(0, 0, 0);

    public GameObject AttackRange;
    public GameObject SpinBlade;
    public GameObject ChargeEffect;
    public GameObject Baldo;
    public GameObject SwordAura;
    public GameObject MagicSword;

    public bool isDefending = false;

    public float animSpeed = 1.0f;

    Player player;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ghostDash = GetComponent<GhostDash>();
    }

    private void Start()
    {
        bossHealth = 0;
        AttackRange.SetActive(false);
        Baldo.SetActive(false);
        ChargeEffect.SetActive(false);
        SwordAura.SetActive(false);
        player = CharacterManager.Instance.Player.GetComponent<Player>();
        //CallSamurai();
    }

    private void Update()
    {
        if (isStageStart && canFlip)
        {
            LookPlayer();
        }

        if (Input.GetKeyUp(KeyCode.P)) // temp
        {
            CallSamurai();
        }

        
        if (Input.GetKeyUp(KeyCode.G))
        {
            animator.SetBool(isDefend, true);
        }
        
    }

    void LookPlayer()
    {
        if (isBossDie) return;

       
        //spriteRenderer.flipX = (CharacterManager.Instance.Player.transform.position.x > transform.position.x) ? false : true;

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


    public void CallSamurai()
    {
       
        StartCoroutine(SamuraiStageOn());
        
    }

    IEnumerator SamuraiStageOn()
    {
        AudioManager.instance.PlaySFX("Nervous", 0.1f);
        AudioManager.instance.StopBGM();
        isStageStart = true;
        CameraManager.Instance.ModifyCameraInfo(new Vector2(38, 10), new Vector2(268, -478));
        CharacterManager.Instance.Player.controller.cantMove = true;


        
        yield return new WaitForSeconds(1f);

        UIBar.Instance.CallBossBar("Samurai");

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
        AudioManager.instance.PlayBGM("StarSky", 0.15f);
        CharacterManager.Instance.Player.controller.cantMove = false;
        isPhase1 = true;
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
        mainCoroutine = StartCoroutine(Iteration());
    }

    IEnumerator Iteration()
    {
        if (isPhase1)
        {
            
            yield return new WaitForSeconds(1.5f);
            
            switch (count % 6)
            {
                case 0:
                    DoDashAttack();
                    break;
                case 1:
                    BalDo();
                    break;
                case 2:
                    BackStepAttack();
                    //DefendEnd();
                    break;
                case 3:
                    CounterAttack();
                    break;
                case 4:
                    Running();
                    break;
                case 5:
                    DoNormalAttack();
                    break;
                default:
                    CounterAttack();
                    break;
            }


        }

        if (isPhase2)
        {
            
            yield return new WaitForSeconds(1f);

            switch (count % 3)
            {
                case 0:
                    
                    break;
                case 1:
                    
                    break;
                case 2:
                   
                    break;
                default:
                    break;
            }


        }

        if (isPhase3)
        {
            yield return new WaitForSeconds(0.5f);
           
            switch (count % 7)
            {
                case 0:
                    yield return new WaitForSeconds(0.5f);
                   
                    break;
                case 1:
                case 4:
                    
                    break;
                case 2:
                case 5:
                    
                    break;
                case 3:
                case 6:
                    
                    break;
                default:
                    //Phase3Start();
                    break;
            }
        }
        count++;
    }

    void DoDashAttack()
    {
        animator.SetBool(isDashAttack, true);
        ghostDash.makeGhost = true;
    }

    void DoingDash()
    {
        canFlip = false;
        float Dir = isFlip ? -1f : 1f;
        float Destination;
        Destination = transform.position.x + 30 * Dir;
        Destination = Mathf.Clamp(Destination, 232, 305);
        transform.DOMoveX(Destination, 1.33f / animSpeed).OnComplete(() =>
        {
            ghostDash.makeGhost = false;

        }
        );
    }

    public void DashAttackEnd()
    {
        animator.SetBool(isDashAttack, false);
        Discrimination();
    }


    void Running()
    {
        StartCoroutine(RunningPattern());
    }

    IEnumerator RunningPattern()
    {
        RunStart();
        yield return new WaitForSeconds(1.5f);
        RunEnd();
        Discrimination();
    }


    void RunStart()
    {
        animator.SetBool(isRun, true);
    }

    void RunEnd()
    {
        animator.SetBool(isRun, false);
    }


    void DoNormalAttack()
    {
        animator.SetBool(isAttack, true);
        SwordAuraOn();
    }

    public void NormalAttackEnd()
    {
        animator.SetBool(isAttack, false);
    }

    void ThrowSpinBlade()
    {
        float Dir = isFlip ? -1f : 1f;
        AudioManager.instance.PlaySamurai("SpinBlade", 0.2f);
        GameObject spinProjectile = Instantiate(SpinBlade, transform.position + new Vector3(0, 1.5f, 0) + Dir * new Vector3(3, 0, 0), Quaternion.identity);

        spinProjectile.transform.DOMove(player.transform.position  , 1f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            AudioManager.instance.PlaySamurai("Boomerang", 0.3f);
            spinProjectile.transform.DOMove(transform.position + new Vector3(-Dir * 8, 12, 0), 1.5f).SetEase(Ease.InExpo);
        }
        );
        Destroy(spinProjectile, 3f);
    }

    public void BackStepAttackEnd()
    {
        animator.SetBool(isSpinBlade, false);
    }


    void BackStepAttack()
    {
        animator.SetBool(isSpinBlade, true);
        AudioManager.instance.PlaySamurai("BackStepSlash", 0.2f);
    }

    void BackStepStart()
    {
        float Dir = isFlip ? -1f : 1f;
        SwordAuraOn();
        ghostDash.makeGhost = true;
        tempCoroutine = StartCoroutine(DoingBackDash(Dir));
  
    }

    IEnumerator DoingBackDash(float Dir)
    {
        float time = 0f;
        //float timeElapsed = 0.05f;
        while (time < 0.3f)
        {
            transform.position += new Vector3(-Dir * Time.deltaTime * 25f, 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }
    }

    void BackStepEnd()
    {
        SwordAuraOff();
        if (tempCoroutine != null) StopCoroutine(tempCoroutine);
        ghostDash.makeGhost = false;
    }

    void SwordAuraOn()
    {
        SwordAura.SetActive(true);
    }

    void SwordAuraOff()
    {
        SwordAura.SetActive(false);
    }




    void CounterAttack()
    {
        animator.SetBool(isDefend, true);
    }

    void DefendStart()
    {
        isDefending = true;
    }

    void DefenseSound()
    {
        AudioManager.instance.PlaySamurai("Defense", 0.15f);
    }

    public void DefendEnd()
    {
        isDefending = false;
        animator.SetBool(isDefend, false);
        
    }

    void DoCounterAttack()
    {
        AudioManager.instance.PlaySamurai("DefendSuccess", 0.2f);
        isDefending = false;
        animator.SetBool(isDefend, false);
        animator.SetTrigger(revenge);
    }

    void DoAttack()
    {
        AudioManager.instance.PlaySamurai("CounterAttack", 0.2f);
        AttackRange.SetActive(true);
    }

    void AttackEnd()
    {
        AttackRange.SetActive(false);
    }

    void BalDo()
    {
        animator.SetBool(isBaldo, true);
    }

    public void BalDoEnd()
    {
        animator.SetBool(isBaldo, false);
    }

    void StartCharging()
    {
        ChargeEffect.SetActive(true);
        AudioManager.instance.PlaySamurai("Baldo", 0.2f);
        Baldo.SetActive(false);
    }

    void EndCharging()
    {
        ChargeEffect.SetActive(false);
    }

    void BaldoEffect()
    {
        AudioManager.instance.PlaySamurai("BaldoEnd", 0.5f);
        Baldo.SetActive(true);
    }

    void BaldoDamage()
    {
        Debug.Log("Attack!!");
        if(!CharacterManager.Instance.Player.controller.Rolling && Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position) < 28f)
        CharacterManager.Instance.Player.battle.ChangeHealth(-1);
    }



    void SetBossBar()
    {
        UIBar.Instance.SetBossBar(bossHealth, bossMaxHealth, 0);
    }

    public void GetDamage(float damage)
    {
        if (isInvincible) return;

        if (isDefending)
        {
            DoCounterAttack();
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

            UIBar.Instance.SetBossBar(bossHealth, bossMaxHealth, damage);
            StartCoroutine(ColorChanged());
        }
        else
        {
            
            if (isBossDie) return;
            if (isPhase3) isBossDie = true;

            isStageStart = false;
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
        
    }

}
