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

    public bool isBossDie = false;

    public GameObject wolfZone1;
    public GameObject wolfZone2;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wolfCol = GetComponent<Collider2D>();
    }

    private void Start()
    {
        animator.SetBool(isDead, true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            CallWolfBoss();
        }
    }

    public void CallWolfBoss()
    {
        //BossZoneWall.enabled = true;
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX("Nervous", 0.1f);
        animator.SetBool(isDead, false);
        animator.SetBool(isRun, true);
        //DOTween.To(() => bossHealth, x => bossHealth = x, bossMaxHealth, 2);
        UIBar.Instance.CallBossBar("Cave Wolf");
        StartCoroutine(WolfBossStageStart());
        CameraManager.Instance.ModifyCameraInfo(new Vector2(20, 10), new Vector2(308, -145));
        isPhase1 = true;
        isPhase2 = false;
        isPhase3 = false;

        transform.DOMove(new Vector3(306, -146, 0), 3f);

        spriteRenderer.DOFade(1, 3f).OnComplete(() =>
        {
            animator.SetBool(isRun, false);
        });
    }

    IEnumerator WolfBossStageStart()
    {

        CameraManager.Instance.MakeCameraShake(new Vector3(306, -146, 0), 3f, 0.05f, 0.1f);
        yield return new WaitForSeconds(1f);

        float time = 0f;

        while (time < 1)
        {
            bossHealth += (bossMaxHealth * Time.deltaTime);
            SetBossBar();
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }
        //animator.Play("Special Attack", 0, 0f);
        //Discrimination();
        AudioManager.instance.StopBGM();
        AudioManager.instance.StopBGM2();
        AudioManager.instance.PlayBGM("Wolf", 0.1f);
        //CameraManager.Instance.ModifyCameraInfo(new Vector2(20, 5), new Vector2(142, -38));
        spriteRenderer.flipX = false;
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
        animator.SetBool(isDead, true);
        isBossDie = true;
        AudioManager.instance.StopBGM();
        //TODO Boss Die
    }

}
