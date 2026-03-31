using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private bool touchedGround;

    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        player.stamina.UseStamina(player.airAttackCost);

        touchedGround = false;

        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.groundDetected && touchedGround == false)
        {
            touchedGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocity.y);
        }

        if (triggerCalled && player.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
