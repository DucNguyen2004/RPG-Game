using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animationName) : base(_player, _stateMachine, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (xInput != 0)
            player.SetVelociy(player.moveSpeed * 0.8f * xInput, rb.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
    }
}