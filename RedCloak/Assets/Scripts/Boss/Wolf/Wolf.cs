using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wolf : MonoBehaviour
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        //DOTween.To(() => bossHealth, x => bossHealth = x, bossMaxHealth, 2);
        UIBar.Instance.CallBossBar("Cave Wolf");
        StartCoroutine(WolfBossStageStart());

        isPhase1 = true;
        isPhase2 = false;
        isPhase3 = false;
    }

    IEnumerator WolfBossStageStart()
    {

        CameraManager.Instance.MakeCameraShake(transform.position, 3f, 0.05f, 0.1f);
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


}
