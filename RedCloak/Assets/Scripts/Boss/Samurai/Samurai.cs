using AnimationImporter.PyxelEdit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Samurai : MonoBehaviour, IDamage
{
    private static readonly int isBaldo = Animator.StringToHash("IsBaldo");
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

    private bool isPhase1 = false;
    private bool isPhase2 = false;
    private bool isPhase3 = false;

    public bool isBossDie = false;

    public bool isStageStart = false;

    SpriteRenderer spriteRenderer;
    Animator animator;

    private int count = 0;

    Coroutine mainCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        bossHealth = 0;
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

        animator.SetBool(isBaldo, true); // temp
    }

    void LookPlayer()
    {
        if (isBossDie) return;
        spriteRenderer.flipX = (CharacterManager.Instance.Player.transform.position.x > transform.position.x) ? false : true;
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

    void SetBossBar()
    {
        UIBar.Instance.SetBossBar(bossHealth, bossMaxHealth, 0);
    }

    public void GetDamage(float damage)
    {
        if (isInvincible) return;

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
