using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpforce;
    
    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;

    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;
    
    [Header("Attack Info")]
    [SerializeField] private float comboTime = 0.3f;
    private float comboTimeWindow;
    private bool isAttacking;
    private int comboCounter;


    private float xInput;

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();

        Movement();
        CheckInput();
        CollisionCheck();        

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

        FlipController();
        AnimatorController();
    }

    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;
        
        if(comboCounter > 2)
            comboCounter = 0;
        
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartActtack();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    private void StartActtack()
    {
        if(!isGrounded)
            return;
            
        if(comboTimeWindow < 0)
                comboCounter = 0;

        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void Movement()
    {
        if(isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
            return;
        }

        if(dashTime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if(isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    }


    private void DashAbility()
    {
        if(dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0; //ย่อการใช้ if / else

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("ComboCouter", comboCounter);

    }


    private void FlipController()
    {
        if(rb.velocity.x > 0 && !facingRight) 
            Flip();
        else if(rb.velocity.x < 0 && facingRight) 
            Flip();
        
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();
    }
}
