using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;

    [Header("Attack info")]
    [SerializeField] private float comboTime;
    private float comboTimer;
    private bool isAttacking;
    private int attackCounter;

    private float xInput;
    private int facingDir = 1;
    private bool isFacingRight = true;

    [Header("Ground detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGrounded;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        Movement();
        CheckInput();
        GroundCheck();

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimer -= Time.deltaTime;



        HandleFlip();
        AnimatorController();
    }
    private void StartAttack()
    {
        if (!isGrounded)
        {
            return;
        }
        if (comboTimer < 0)
            attackCounter = 0;

        isAttacking = true;
        comboTimer = comboTime;
    }
    public void AttackOver()
    {
        isAttacking = false;

        attackCounter++;

        if (attackCounter > 2)
        {
            attackCounter = 0;
        }
    }
    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking)
        {
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }
    private void GroundCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGrounded);
    }
    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttack();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }
    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
        }
        else if (dashTime > 0)
        {   // dash 
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }
    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("attackCounter", attackCounter);
    }
    // can optimize the flip functions
    private void Flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
    private void HandleFlip()
    {
        if (rb.velocity.x > 0 && !isFacingRight)
            Flip();
        else if (rb.velocity.x < 0 && isFacingRight)
            Flip();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
