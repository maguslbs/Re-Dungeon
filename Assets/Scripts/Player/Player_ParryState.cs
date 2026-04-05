using UnityEngine;

public class Player_ParryState : PlayerState
{
    private Player_Combat combat;
    private bool parriedSomebody;

    public Player_ParryState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        if (!player.stamina.HasStamina())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        base.Enter();

        player.stamina.UseStamina(player.parryCost);

        stateTimer = combat.GetParryRecoveryDuration();
        parriedSomebody = combat.ParryPerformed();

        anim.SetBool("parryPerformed", parriedSomebody);

    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.linearVelocity.y);

        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && parriedSomebody == false)
            stateMachine.ChangeState(player.idleState);
    }
}
