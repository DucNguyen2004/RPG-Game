using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animationName) : base(_player, _stateMachine, _animationName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (yInput < 0)
            player.SetVelociy(0, rb.velocity.y);
        else
            player.SetVelociy(0, rb.velocity.y * .7f);

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
