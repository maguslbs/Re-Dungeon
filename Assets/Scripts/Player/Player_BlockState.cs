using UnityEngine;

public class Player_BlockState : PlayerState
{
    public Player_BlockState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        if (!player.stamina.HasStamina())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        base.Enter();

    }

    public override void Update()
    {
        base.Update();

        rb.linearVelocity = Vector2.zero;

        if (!player.blockHeld)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (!player.blockHeld || !player.stamina.HasStamina())
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
