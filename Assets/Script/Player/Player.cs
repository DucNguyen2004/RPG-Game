using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack details")]
    public Vector2[] attackMovement;

    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [Header("Collision detection")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGrounded;

    public int facingDir { get; private set; }
    public bool isFacingRight = true;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        facingDir = 1;
        stateMachine.Initialize(idleState);
    }
    private void Update()
    {
        stateMachine.currentState.Update();

        CheckForDashInput();

    }
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {
        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    public void ZeroVelocity() => rb.velocity = Vector2.zero;
    public void SetVelociy(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        HandleFlip(_xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGrounded);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facingDir, whatIsGrounded);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }

    public void Flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void HandleFlip(float _x)
    {
        if (_x > 0 && !isFacingRight)
            Flip();
        else if (_x < 0 && isFacingRight)
            Flip();
    }
    // [Header("Movement info")]
    // [SerializeField] private float moveSpeed;
    // [SerializeField] private float jumpForce;

    // [Header("Dash info")]
    // [SerializeField] private float dashSpeed;
    // [SerializeField] private float dashDuration;
    // private float dashTime;
    // [SerializeField] private float dashCooldown;
    // private float dashCooldownTimer;

    // [Header("Attack info")]
    // [SerializeField] private float comboTime;
    // private float comboTimer;
    // private bool isAttacking;
    // private int attackCounter;

    // private float xInput;

    // protected override void Start()
    // {
    //     base.Start();
    // }

    // protected override void Update()
    // {
    //     base.Update();
    //     Movement();
    //     CheckInput();

    //     dashTime -= Time.deltaTime;
    //     dashCooldownTimer -= Time.deltaTime;
    //     comboTimer -= Time.deltaTime;

    //     HandleFlip();
    //     AnimatorController();
    // }
    // private void StartAttack()
    // {
    //     if (!isGrounded)
    //     {
    //         return;
    //     }
    //     if (comboTimer < 0)
    //         attackCounter = 0;

    //     isAttacking = true;
    //     comboTimer = comboTime;
    // }
    // public void AttackOver()
    // {
    //     isAttacking = false;

    //     attackCounter++;

    //     if (attackCounter > 2)
    //     {
    //         attackCounter = 0;
    //     }
    // }
    // private void DashAbility()
    // {
    //     if (dashCooldownTimer < 0 && !isAttacking)
    //     {
    //         dashTime = dashDuration;
    //         dashCooldownTimer = dashCooldown;
    //     }
    // }

    // private void CheckInput()
    // {
    //     xInput = Input.GetAxisRaw("Horizontal");

    //     if (Input.GetKeyDown(KeyCode.Mouse0))
    //     {
    //         StartAttack();
    //     }
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Jump();
    //     }

    //     if (Input.GetKeyDown(KeyCode.LeftShift))
    //     {
    //         DashAbility();
    //     }
    // }
    // private void Movement()
    // {
    //     if (isAttacking)
    //     {
    //         rb.velocity = Vector2.zero;
    //     }
    //     else if (dashTime > 0)
    //     {   // dash 
    //         rb.velocity = new Vector2(facingDir * dashSpeed, 0);
    //     }
    //     else
    //     {
    //         rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    //     }
    // }
    // private void Jump()
    // {
    //     if (isGrounded)
    //         rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    // }
    // private void AnimatorController()
    // {
    //     bool isMoving = rb.velocity.x != 0;
    //     anim.SetFloat("yVelocity", rb.velocity.y);

    //     anim.SetBool("isMoving", isMoving);
    //     anim.SetBool("isGrounded", isGrounded);
    //     anim.SetBool("isDashing", dashTime > 0);
    //     anim.SetBool("isAttacking", isAttacking);
    //     anim.SetInteger("attackCounter", attackCounter);
    // }

    // protected override void CollisionCheck()
    // {
    //     base.CollisionCheck();
    // }

}
