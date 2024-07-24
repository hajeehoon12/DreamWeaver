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

    private int count = 0;

    Coroutine mainCoroutine;

    private bool isFlip = false;

    Vector3 Right = new Vector3(0, 180, 0);
    Vector3 Left = new Vector3(0, 0, 0);

    public GameObject AttackRange;
    public GameObject SpinBlade;
    public GameObject ChargeEffect;
    public GameObject Baldo;

    public bool isDefending = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        bossHealth = 0;
        AttackRange.SetActive(false);
        Baldo.SetActive(false);
        ChargeEffect.SetActive(false);
        //CallSamurai();
    }

    private void Update()
    {
        if (isStageStart)
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
        }
        else
        {
            transform.localEulerAngles = Right;
        }

    }


    public void CallSamurai()
    {
       
        StartCoroutine(SamuraiStageOn());
        
    }

    IEnumerator SamuraiStageOn()
    {
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
        AudioManager.instance.PlayBGM("StarSky", 0.2f);
        CharacterManager.Instance.Player.controller.cantMove = false;
        isPhase1 = true;
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
        if (isPhase1)
        {
           
            yield return new WaitForSeconds(1.5f);

            switch (count % 2)
            {
                case 0:
                    CounterAttack();
                    break;
                case 1:
                    BalDo();
                    break;
                case 2:

                    //DefendEnd();
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

    void ThrowSpinBlade()
    {
        GameObject spinProjectile = Instantiate(SpinBlade);

        
    }

    void CounterAttack()
    {
        animator.SetBool(isDefend, true);
    }

    void DefendStart()
    {
        isDefending = true;
    }

    public void DefendEnd()
    {
        isDefending = false;
        animator.SetBool(isDefend, false);
        Discrimination();
    }

    void DoCounterAttack()
    {
        isDefending = false;
        animator.SetBool(isDefend, false);
        animator.SetTrigger(revenge);
    }

    void DoAttack()
    { 
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
