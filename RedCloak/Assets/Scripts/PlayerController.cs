using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private static readonly int isRunning = Animator.StringToHash("IsRunning");
    private static readonly int isJumping = Animator.StringToHash("IsJumping");
    private static readonly int isFalling = Animator.StringToHash("IsFalling");
    private static readonly int isRolling = Animator.StringToHash("IsRolling");
    private static readonly int isAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int isDashing = Animator.StringToHash("IsDashing");
    private static readonly int isWallClimbing = Animator.StringToHash("IsWallClimbing");


    Animator animator;

    public float maxSpeed;// 최대속도 설정
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    GhostDash ghostDash;
    Collider2D playerCollider;

    bool Jumping = false;           // AM i Jumping?
    //bool Falling = false;
    bool Rolling = false;           // AM i rolling?
    public bool isGrounded = true;  // AM i on the ground?
    bool canCombo = false;          // AM i doing combo attack

    private float playerGravityScale;
    
    public LayerMask enemyLayerMask;
    public LayerMask groundLayerMask;

    public bool canRoll = true;           // skill on / off
    public bool canDash = true;           // skill on / off
    public bool canComboAttack = true;    // skill on / off

    Vector2 boundPlayer;


    public float attackRate = 10f;  //attack Damage
    public int ComboCount;          // current combo Count





    void Awake()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ghostDash = GetComponent<GhostDash>();
        playerCollider = GetComponent<Collider2D>();
        
    }

    private void Start()
    {
        playerGravityScale = rigid.gravityScale;
        boundPlayer = playerCollider.bounds.extents;
    }

    private void FixedUpdate()
    {
        Move();
    }
    void Update()
    {
        

        JumpCheck(); // Checking wheter can jump

        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

    }

   

  

    public void CheckHit() // Execute In attack Animation
    {
        //Debug.Log("I'm hitting!!");
        float CheckDir = 1f;
        

        if (spriteRenderer.flipX) CheckDir = -1f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1, 0) * CheckDir, 2f, enemyLayerMask);
        {
            
            if (hit.collider == null) return;
            //Debug.Log(hit.collider.name);
            if (hit.transform.gameObject.TryGetComponent(out Monster monster))
            {
                monster.GetDamage(attackRate);
            }
                
        }

    }


    void OnDash() // when C keyboard input do dash
    {
        if (!canDash) return;

        if (!ghostDash.makeGhost)
        {
            rigid.gravityScale = 0f;
            rigid.velocity = new Vector2(rigid.velocity.x, 0); // gravity done;
            ghostDash.makeGhost = true;
            animator.SetBool(isDashing, true);
            animator.SetBool(isAttacking, false);
            StartCoroutine(DoingDash());
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
                transform.position -= new Vector3(0.2f, 0, 0);
            }
            else
            {
                transform.position += new Vector3(0.2f, 0, 0);
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
            transform.position += moveVelocity * 1.2f * maxSpeed * Time.deltaTime;
            return;
        }



        transform.position += moveVelocity * maxSpeed * Time.deltaTime;
    }

    private void OnJump() // Space Button = Jump
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
        if (ghostDash.makeGhost) // While During Dash
        {
            rigid.gravityScale = playerGravityScale;
        }
        rigid.AddForce(Vector2.up * jumpPower * rigid.mass, ForceMode2D.Impulse);
        animator.SetBool(isJumping, true);
        yield return new WaitForSeconds(0.1f);
        Jumping = true;
    }


    private void OnCollisionStay2D(Collision2D collider) // Jump and wall Climb check
    {
        //Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.CompareTag("Floor"))
        {
            //Debug.Log(boundPlayer.x);
            //Debug.Log(boundPlayer.y);


            for (int i = -1; i < 2; i++)
            {

                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(playerCollider.bounds.extents.x * i,0), new Vector2(0, -1), 0.3f, groundLayerMask); // is Grounded Check
                if (hit.collider?.name != null)
                {
                    //Debug.Log(hit.collider.name);
                    if (!isGrounded && Jumping)
                    {
                        //Falling = false;
                        isGrounded = true;
                        Jumping = false;
                        animator.SetBool(isFalling, false);
                        animator.SetBool(isJumping, false);

                    }
                    return;
                }
            }

            for (int i = -1; i < 2; i += 2) // wall Climbing Check
            {
                //Debug.Log(i);
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position+new Vector3(boundPlayer.x * i, boundPlayer.y, 0), new Vector2(i, 0), 0.1f, groundLayerMask);
                if (wallHit.collider?.name != null)
                {
                    if (Input.GetAxisRaw("Horizontal") * i > 0)// Two Case right wall right key, left wall left key
                    {
                        //Debug.Log("I'm Climbing");
                        //animator.SetBool(isWallClimbing, true);
                        //rigid.gravityScale = 0f;
                        return;
                    }
                }

            }
            //animator.SetBool(isWallClimbing, false);
            //rigid.gravityScale = playerGravityScale;
        }
        
    }



}
