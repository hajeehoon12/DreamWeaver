using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnimationImporter.PyxelEdit;

public class HolyKnight : MonoBehaviour, IDamage
{

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
    }

    private void Update()
    {
        if (isStageStart && canFlip)
        {
            LookPlayer();
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
            //Debug.Log($"남은 보스 체력 : {bossHealth}");
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

            animator.Play("Death", -1, 0f);
            AudioManager.instance.PlaySamurai("SamuraiDie", 0.15f);
            UIBar.Instance.SetBossBar(0, bossMaxHealth, bossHealth);

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
        //SwordAuraOff();
        CameraManager.Instance.CallStage3CameraInfo();
    }

}
