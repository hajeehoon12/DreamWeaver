using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Archer : MonoBehaviour , IDamage
{
    private static readonly int doAttack = Animator.StringToHash("DoAttack");
    private static readonly int doSpecialAttack = Animator.StringToHash("DoSpecialAttack");
    private static readonly int isRunning = Animator.StringToHash("IsRunning");
    private static readonly int isDead = Animator.StringToHash("IsDead");

    private Animator animator;
    private float count = 0;

    public bool isBossDie = false;

    private float bossHealth = 0;
    public float bossMaxHealth = 300;

    SpriteRenderer spriteRenderer;
    Collider2D archerCol;

    public LayerMask ObstacleLayerMask;
    public LayerMask FloorLayerMask;

    public Collider2D BossZoneWall;

    Vector2[] appearPos = { new Vector2(15, 0), new Vector2(-15, 0), new Vector2(14, 1), new Vector2(13, 2), new Vector2(12, 3), new Vector2(-12, 3), new Vector2(-13, 2), new Vector2(-14, 1),new Vector2(-14, -1), new Vector2(14, -1) };

    public bool isPhase1 = true;
    public bool isPhase2 = false;
    public bool isPhase3 = false;

    private int skillPhase3 = 0;

    public GameObject GreenArrow;

    float lastAvoidTime = 0f;
    float lastSkillTime = 0f;

    float skillTime1 = 10f;
    float skillTime2 = 15f;

    //public ItemData dropData;
    //public GameObject ItemLight;

    public GameObject archerLight;




    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        archerCol = GetComponent<Collider2D>();
    }


    private void Start()
    {
        //CallArcherBoss();
        //BossZoneWall.enabled = false;
    }

    public void CallArcherBoss()
    {
        BossZoneWall.enabled = true;   
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX("Nervous", 0.1f);

        //DOTween.To(() => bossHealth, x => bossHealth = x, bossMaxHealth, 2);
        UIManager.Instance.uiBar.CallBossBar("Shadow of Forest");
        StartCoroutine(ArcherBossStageStart());
        
        isPhase1 = true;
        isPhase2 = false;
        isPhase3 = false;
    }

    IEnumerator ArcherBossStageStart()
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
        animator.Play("Special Attack", 0, 0f);
        Discrimination();
        AudioManager.instance.PlayBGM("SilverBird", 0.15f);
        CameraManager.Instance.ModifyCameraInfo(new Vector2(20, 5), new Vector2(142, -38));
        spriteRenderer.flipX = false;
    }



    private void Update()
    {
        if (lastAvoidTime <= skillTime1)
        {
            lastAvoidTime += Time.deltaTime;
        }

        if(lastSkillTime <= skillTime2)
        {
            lastSkillTime += Time.deltaTime;
        }

    }

    void Discrimination()
    {
        if (isBossDie) return;
        StartCoroutine(Iteration());
    }

    IEnumerator Iteration()
    {
        
        if (isPhase1)
        {
            yield return new WaitForSeconds(3f);
            //Flip();
            switch (count % 2)
            {
                case 0:
                    Attack();
                    break;
                case 1:
                    SpecialAttack();
                    break;
            }
        }

        if (isPhase2)
        {
            yield return new WaitForSeconds(2f);
            // Flip();
            switch (count % 2)
            {
                case 0:
                    Attack();
                    break;
                case 1:
                    SpecialAttack();
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
                    Attack();
                    break;
                case 1:
                    SpecialAttack();
                    break;
            }
        }

        count++;
    }

    void Flip() // Look At Player
    {
        //transform.localEulerAngles += new Vector3(0, 180, 0);

        transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 direction = transform.position - CharacterManager.Instance.Player.transform.position;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        

        if (direction.x > 0)
        {
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Euler(0, 180, -rotZ);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 180 + rotZ);
        }
    }

    void AppearPos() //When archer do Special Attack appear near by player
    {
        int tempCount = 0;
        int rand = 0;
        Vector2 targetPos = CharacterManager.Instance.Player.transform.position;
        while (tempCount < 100 || rand >= appearPos.Length-2)
        {
            targetPos = CharacterManager.Instance.Player.transform.position;

            if (isPhase1)
            {
                rand = Random.Range(0, 2);
            }

            if (isPhase2 || isPhase3)
            {
                rand = Random.Range(0, appearPos.Length);
            }


            targetPos += appearPos[rand];

            RaycastHit2D hit = Physics2D.Raycast(CharacterManager.Instance.Player.transform.position+new Vector3(0,CharacterManager.Instance.Player.controller.playerCollider.bounds.extents.y * 2), (targetPos - (Vector2)CharacterManager.Instance.Player.transform.position).normalized, 22f, ObstacleLayerMask);
            
            if (hit.collider?.name == null) break;

            tempCount++;
        }

        if (isPhase1)
        {
            transform.position = new Vector3(targetPos.x, targetPos.y);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, archerCol.bounds.extents.y), new Vector2(0, -1), 10f, FloorLayerMask);

            transform.position = new Vector3(targetPos.x, hit.point.y);
            //Debug.Log(transform.position);
            //Debug.Log(isPhase1);
        }
        else
        {
            transform.position = targetPos;
            if (transform.position.y < -49f)
            {
                transform.position = new Vector3(transform.position.x, -49f);
            }
        }

        if (tempCount == 100)
        {
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, 115f, 168f), Mathf.Clamp(transform.position.y,-49f, -35f));
        }

        Flip();
    }


    void Attack()
    {
        Flip();
        StartCoroutine(DoAttack());
    }

    void SetBossBar()
    {
        UIManager.Instance.uiBar.SetBossBar(bossHealth, bossMaxHealth, 0);
    }

    IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger(doAttack);
        animator.ResetTrigger(doSpecialAttack);
        yield return new WaitForSeconds(1.52f);
        Discrimination();
    }

    void SpecialAttack()
    {
        StartCoroutine(DoSpeciatAttack());
    }

    IEnumerator DoSpeciatAttack()
    {
        animator.SetTrigger(doSpecialAttack);
        animator.ResetTrigger(doAttack);
        yield return new WaitForSeconds(2.62f);
        Discrimination();
    }

    void WhenDie()
    {
        StartCoroutine(DoDie());
    }

    IEnumerator DoDie()
    {
        animator.SetBool(isDead, true);
        yield return new WaitForSeconds(0.8f);
        
        // do die
    }

    void Charge()
    {
        archerCol.enabled = true;
        AudioManager.instance.PlaySFX("Charge1sec", 0.2f);
    }

    void Vanish()
    {
        archerCol.enabled = false;
        AudioManager.instance.PlaySFX("Vanish", 0.2f);
    }

    void Appear()
    {
        if (lastSkillTime >= skillTime2)
        {
            if (isPhase3 && skillPhase3 < 7)
            {
                animator.Play("Special Attack", -1, 0f);

                for (int i = 0; i < 3; i++)
                {
                    GameObject obj = Instantiate(GreenArrow, transform.position + new Vector3(0, 2.5f, 0) + 3 * transform.forward, Quaternion.identity);
                    obj.transform.LookAt(CharacterManager.Instance.Player.transform.position + new Vector3(0, 6*(i-1), 0));
                    obj.transform.localScale = new Vector3(obj.transform.localScale.x * 4, obj.transform.localScale.y * 8, obj.transform.localScale.z * 4);
                }
                skillPhase3++;
            }
            else
            {
                lastSkillTime = 0;
                skillPhase3 = 0;
            }
        }
        AudioManager.instance.PlaySFX("Appear", 0.2f);
    }

    void Laser()
    {
        AudioManager.instance.PlaySFX("Laser", 0.2f);
    }

    IEnumerator AvoidAttack()
    {
        animator.StopPlayback();
        animator.Play("Special Attack", 0, 0f);
        yield return new WaitForSeconds(0.2f);
        //AppearPos();
        //
        //animator.Play("Attack", 0, 0f);

    }

    public void GetDamage(float damage)
    {
        if (!isPhase1)
        {
            if (lastAvoidTime >= skillTime1)
            {
                lastAvoidTime -= skillTime1;
                
                StartCoroutine(AvoidAttack());
                return;
            }
            
        }

        if (bossHealth > damage)
        {
            bossHealth -= damage;
            //Debug.Log($"남은 보스 체력 : {bossHealth}");
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
                lastSkillTime = skillTime2;
                Appear();
            }

            UIManager.Instance.uiBar.SetBossBar(bossHealth, bossMaxHealth, damage);
            StartCoroutine(ColorChanged());
        }
        else
        {
            if (isBossDie) return;
            UIManager.Instance.uiBar.SetBossBar(0, bossMaxHealth, bossHealth);
            CallDie();
            archerCol.enabled = true;
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
        gameObject.layer = LayerMask.NameToLayer(Define.DEFAULT);
        gameObject.tag = Define.PLATFORM;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        //Collider2D[] archers = GetComponentsInChildren<Collider2D>();

        //for (int i = 0; i < archers.Length; i++)
        //{
        //    archers[i].enabled = false;
        //}

        UIManager.Instance.uiBar.CallBackBossBar();
        animator.SetBool(isDead, true);
        isBossDie = true;
        //MonsterDataManager.ChangeCatchStat("ENS00009");
        StartCoroutine(ArcherDie());
    }

    IEnumerator ArcherDie()
    {
        transform.DORotateQuaternion(Quaternion.identity, 0.3f);
        AudioManager.instance.PlaySFX("ArcherDeath", 0.5f);
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX("Success", 0.05f);
        //StopAllCoroutines();
        //DOTween.KillAll();
        AudioManager.instance.StopBGM();


        CameraManager.Instance.CallStage1CameraInfo("archer");

        bool isGround = false;
        while (!isGround)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 0.2f, FloorLayerMask);
            if (hit.collider?.name != null)
            {
                isGround = true;
            }
            yield return new WaitForSeconds(0.01f);
        }
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        //Debug.Log("enabled");
        BossZoneWall.enabled = false;
        //archerCol.isTrigger = true;
        archerCol.enabled = false;
        //ThrowItem();
        Instantiate(archerLight, transform.position,Quaternion.identity);


    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (isBossDie)
    //    {
    //        if (collision.gameObject.layer == LayerMask.NameToLayer(Define.FLOOR_Layer))
    //        {
               
     //       }
    //    }
   // }



}
