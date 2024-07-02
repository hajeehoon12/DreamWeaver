using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    private static readonly int isRunning = Animator.StringToHash("IsRunning");
    private static readonly int isJumping = Animator.StringToHash("IsJumping");
    private static readonly int isFalling = Animator.StringToHash("IsFalling");
    private static readonly int isRolling = Animator.StringToHash("IsRolling");
    private static readonly int isAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int isDashing = Animator.StringToHash("IsDashing");
    private static readonly int isWallClimbing = Animator.StringToHash("IsWallClimbing");


    public Animator animator;

    public float maxSpeed;// 
    public float jumpPower;
    public Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    GhostDash ghostDash;
    Collider2D playerCollider;

    bool Jumping = false;           // AM i Jumping?
    //bool Falling = false;
    public bool Rolling = false;           // AM i rolling?
    public bool isGrounded = true;  // AM i on the ground?
    bool canCombo = false;          // AM i doing combo attack

    private float playerGravityScale;
    
    public LayerMask enemyLayerMask;
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;
    public LayerMask objectLayerMask;

    public bool canRoll = true;           // skill on / off
    public bool canDash = true;           // skill on / off
    public bool canComboAttack = true;    // skill on / off
    public bool canWallJump = true;       // skill on / off


    public bool canDoubleJump = true;      // not skill on / off

    public bool isWall = false;

    Vector2 boundPlayer;

    Coroutine dashCoroutine;

    public float attackRate = 10f;  //attack Damage
    public int ComboCount;          // current combo Count

    public GameObject SnowEffect;
    public ParticleSystem Snow;

    private float lastWallJumpTime = 0.3f;
    private float wallJumpDelayTime = 0.3f;

    public float attackedTime = 0.2f;

    public float slopeSpeed = 0.5f;
    public float originSlopeSpeed = 0.5f;

    private bool isAttacked = false;


    void Awake()
    {
        instance = this;


        rigid = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ghostDash = GetComponent<GhostDash>();
        playerCollider = GetComponent<Collider2D>();
        
    }

    private void Start()
    {
        SnowEffect.SetActive(false);
        playerGravityScale = rigid.gravityScale;
        boundPlayer = playerCollider.bounds.extents;
        canDoubleJump = true;
    }

    private void FixedUpdate()
    {
        Move();
        
    }
    void Update()
    {
        
        JumpCheck(); // Checking whether can jump
        WallClimb();


        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        FallDoubleCheck();
        
    }






    public void FallDoubleCheck() // idle to jump
    {
        if (rigid.velocity.y < -5f)
        {
            animator.SetBool(isFalling, true);
            Jumping = true;
            isGrounded = false;
        }
    }

    public void WallClimb()
    {
        float dir = spriteRenderer.flipX ? -1 : 1;
        isWall = Physics2D.Raycast(transform.position + new Vector3(dir * playerCollider.bounds.extents.x,playerCollider.bounds.extents.y), Vector2.right * dir, 0.05f, wallLayerMask);

        if (lastWallJumpTime >= wallJumpDelayTime-0.02f && lastWallJumpTime <= wallJumpDelayTime)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y); 
        }

        if (lastWallJumpTime < wallJumpDelayTime)
        {
            lastWallJumpTime += Time.deltaTime;
        }


        if (isWall)
        {
            
            if (lastWallJumpTime >= wallJumpDelayTime)
            {
                if (dir * Input.GetAxisRaw("Horizontal") > 0)
                {
                    animator.SetBool(isWallClimbing, true);
                    animator.SetBool(isJumping, false);
                    animator.SetBool(isDashing, false);
                    if(dashCoroutine != null) StopCoroutine(dashCoroutine);
                    DashOff();

                    if (rigid.velocity.y == 0.5f)
                    {
                        rigid.velocity = new Vector2(rigid.velocity.x, 0f); // init;
                    }
                    //rigid.velocity = new Vector2(rigid.velocity.x, -slopeSpeed); // init;
                    
                }
                if (Input.GetAxis("Jump") != 0)
                {
                    lastWallJumpTime -= 0.3f;

                    animator.SetBool(isFalling, false);
                    animator.SetBool(isJumping, true);
                    AudioManager.instance.PlaySFX("Jump", 0.2f);
                    rigid.velocity = new Vector2(-dir * jumpPower * 0.4f, jumpPower);
                    StartCoroutine(CallSnowEffect());
                    //spriteRenderer.flipX = !spriteRenderer.flipX;
                    animator.SetBool(isWallClimbing, false);
                    animator.SetBool(isJumping, true);
                }
            }
        }
        else
        {
            animator.SetBool(isWallClimbing, false);
        }

    }
  

    public void CheckHit() // Execute In attack Animation
    {
        //Debug.Log("I'm hitting!!");
        float CheckDir = 1f;

        AudioManager.instance.PlayPitchSFX("SwordAttack", 0.05f);

        if (spriteRenderer.flipX) CheckDir = -1f;

        
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, playerCollider.bounds.extents.y, 0), new Vector2(1, 0) * CheckDir, 3f, enemyLayerMask);
        {
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.name);
                if (hit.transform.gameObject.TryGetComponent(out Monster monster))
                {
                    monster.GetDamage(attackRate);
                }
            }
                
        }

        RaycastHit2D hits = Physics2D.Raycast(transform.position+new Vector3(0, playerCollider.bounds.extents.y, 0), new Vector2(1, 0) * CheckDir, 3f, objectLayerMask);
        {

            if (hits.collider == null) return;
            //Debug.Log(hit.collider.name);
            if (hits.transform.gameObject.TryGetComponent(out Rigidbody2D rigid2D))
            {
                rigid2D.AddForce(CheckDir * new Vector2(1, 0) * rigid2D.mass * 10f, ForceMode2D.Impulse);
                Debug.Log("hit object!!");
                AudioManager.instance.PlayPitchSFX("BoxHit", 0.05f);
            }

        }


    }

    public void DoubleJump()
    {
        StartCoroutine(CallSnowEffect());

    }

    IEnumerator CallSnowEffect()
    {
        SnowEffect.SetActive(true);
        Snow.Play();
        yield return new WaitForSeconds(0.2f);
        Snow.Stop();
        SnowEffect.SetActive(false);


    }

    void OnDash() // when C keyboard input do dash
    {
        if (!canDash) return;
        if (animator.GetBool(isWallClimbing)) return;

        if (!ghostDash.makeGhost)
        {
            AudioManager.instance.PlayPitchSFX("Dash", 0.1f);
            rigid.gravityScale = 0f;
            rigid.velocity = new Vector2(rigid.velocity.x, 0); // gravity done;
            ghostDash.makeGhost = true;
            animator.SetBool(isDashing, true);
            animator.SetBool(isAttacking, false);
            if(dashCoroutine != null) StopCoroutine(dashCoroutine);
            dashCoroutine = StartCoroutine(DoingDash());
        }


    }

    void OnClick() // When Clicked
    {
        OnAttack();
    }

    IEnumerator DoingDash() // Do Coroutine During Dash
    {
        while (ghostDash.makeGhost)
        {
            if (spriteRenderer.flipX)
            {
                transform.position -= new Vector3(0.05f * transform.localScale.x, 0, 0);
            }
            else
            {
                transform.position += new Vector3(0.05f * transform.localScale.x, 0, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }
        
        yield return null;
    }

    public void DashOff() //When Dash Animation End
    {
        rigid.gravityScale = playerGravityScale;
        ghostDash.makeGhost = false;
        animator.SetBool(isDashing, false);
        if (!isGrounded)
        {
            animator.SetBool(isJumping, true);
        }
    }
    
  

    void OnRoll() // When Shift called Do Rolling
    {
        if (!canRoll) return;

        if (!Rolling && !Jumping)
        {  
            animator.SetBool(isRolling, true);
            Rolling = true;
        }
    }

    

    void RollSound()
    {
        AudioManager.instance.PlayPitchSFX("Roll", 0.1f);
        
    }


    void ComboStart()
    {
        ComboCount = 0;    
    }

    void ComboSum()
    {
        ComboCount++;
        
    }

    void OnAttack() // Z button called
    {
            //Debug.Log("Attack!!");
        animator.SetBool(isAttacking, true);
        
        if (canCombo && canComboAttack) animator.SetTrigger("NextCombo");

    }

    public void ComboEnable()
    {

        canCombo = true;
        //Debug.Log("ComboEnable");
    }

    public void ComboDisAble()
    {
        canCombo = false;        
    }


    public void AttackEnd()
    {
        //Debug.Log("Combo!!");
        animator.SetBool(isAttacking, false);
    }
    

    public void RollEnd() // When Roll Animation End Called
    {
        animator.SetBool(isRolling, false);
        Rolling = false;
    }


    private void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (isAttacked)
        {
            Debug.Log("Can't move!!");
            return;
        }
        if (ghostDash.makeGhost) return;
        

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            animator.SetBool(isRunning, true);
            moveVelocity = Vector3.left;
            spriteRenderer.flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            animator.SetBool(isRunning, true);
            moveVelocity = Vector3.right;
            spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetBool(isRunning, false);
        }

        float dir = spriteRenderer.flipX ? -1 : 1;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(dir * boundPlayer.x, boundPlayer.y), new Vector2(dir, 0), 0.02f, groundLayerMask);
        if (hit.collider?.name != null) return;

        if (Rolling)
        {
            //transform.position += moveVelocity * 1.2f * maxSpeed * Time.deltaTime;
            return;
        }

        if (lastWallJumpTime < wallJumpDelayTime)
        {
            return;
        }

        transform.position += moveVelocity * maxSpeed * Time.deltaTime;
    }

    public void OnJump() // Space Button = Jump
    {
        //Debug.Log(rigid.velocity.y);
        if (Rolling) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // && !Jumping
        {
            isGrounded = false;
            
            StartCoroutine(DoJump());
            return;
            //Debug.Log("Try Jumping");
        }
    }

    void JumpCheck() // need to check When player Start Falling Down
    {
        if (rigid.velocity.y < 0 && !isGrounded && Jumping)
        {
            animator.SetBool(isJumping, false);
            animator.SetBool(isFalling, true);
            //Falling = true;
        }
    }



    IEnumerator DoJump() // Give Power
    {
        AudioManager.instance.PlaySFX("Jump", 0.2f);
        if (ghostDash.makeGhost) // While During Dash
        {
            rigid.gravityScale = playerGravityScale;
        }
        rigid.AddForce(Vector2.up * jumpPower * rigid.mass, ForceMode2D.Impulse);
        animator.SetBool(isJumping, true);
        yield return new WaitForSeconds(0.1f);
        Jumping = true;
    }

    public void OnGetAttacked()
    {
        GetAttacked();
    }

    private void GetAttacked() // When Player Get Attacked
    {
        //Debug.Log("Do Red");

        if (isAttacked) return;

        float knockBackPower = 8f;
        float Dir = spriteRenderer.flipX ? -1 : 1;
        
        StartCoroutine(ColorChanged());
        StartCoroutine(GetAttackedCheck());

        
        rigid.velocity = Vector3.zero;
        rigid.AddForce((Vector3.up + Dir * new Vector3(3f, 0, 0)) * rigid.mass * knockBackPower, ForceMode2D.Impulse); // Vector3.up + Dir * new Vector3(3f, 0, 0)

    }

    IEnumerator GetAttackedCheck()
    {
        isAttacked = true;
        yield return new WaitForSeconds(attackedTime); // Get Hit Time; Have to Change
        isAttacked = false;
    }

    IEnumerator ColorChanged()
    {
        float durTime = attackedTime; // invincibleTime; Have To Change
        spriteRenderer.DOColor(Color.red, durTime);
        yield return new WaitForSeconds(durTime);
        spriteRenderer.DOColor(Color.white, durTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);

        //collision.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            GetAttacked();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Trigger detected with " + collider.gameObject.name);
    }

    private void OnCollisionStay2D(Collision2D collider) // Jump and wall Climb check
    {
        //Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.CompareTag("Floor") || collider.gameObject.CompareTag("Monster") || collider.gameObject.CompareTag("Platform")) // 
        {
            //Debug.Log(boundPlayer.x);
            //Debug.Log(boundPlayer.y);
            //Debug.Log("Floor");




            for (int i = -1; i < 2; i++)
            {

                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(playerCollider.bounds.extents.x * i * 0.85f,0.1f), new Vector2(0, -1), 0.5f, groundLayerMask); // is Grounded Check
                if (hit.collider?.name != null)
                {
                    //Debug.Log(hit.collider.name);
                    if (!isGrounded && Jumping)
                    {
                        //Debug.Log("Down");
                        //Falling = false;
                        isGrounded = true;
                        Jumping = false;
                        canDoubleJump = true;
                        animator.SetBool(isFalling, false);
                        animator.SetBool(isJumping, false);
                        break;

                    }
                    //return;
                }
            }

            //for (int i = -1; i < 2; i += 2) // wall Climbing Check
            //{
                //Debug.Log(i);
                //RaycastHit2D wallHit = Physics2D.Raycast(transform.position+new Vector3(boundPlayer.x * i, boundPlayer.y, 0), new Vector2(i, 0), 0.1f, groundLayerMask);
                //if (wallHit.collider?.name != null)
                //{
                    //if (Input.GetAxisRaw("Horizontal") * i > 0)// Two Case right wall right key, left wall left key
                    //{
                        //Debug.Log("I'm Climbing");
                        //animator.SetBool(isWallClimbing, true);
                        //rigid.gravityScale = 0f;
                        //return;
                    //}
                //}

            //}
            //animator.SetBool(isWallClimbing, false);
            //rigid.gravityScale = playerGravityScale;
        }
        
    }



}
