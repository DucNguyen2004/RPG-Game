using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animationName) : base(_player, _stateMachine, _animationName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 1f;

        player.SetVelociy(-5 * player.facingDir, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();

        //player.SetVelociy(xInput * player.moveSpeed, rb.velocity.y);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.airState);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
