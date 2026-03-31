using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        player.stamina.UseStamina(player.jumpCost);

        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        // make sure not in jump attack state while in fall state. otherwise character fall faster and doesn't attack.
        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}
