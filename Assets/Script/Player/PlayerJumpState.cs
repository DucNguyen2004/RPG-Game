using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animationName) : base(_player, _stateMachine, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();

        player.SetVelociy(xInput * player.moveSpeed, rb.velocity.y);
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
