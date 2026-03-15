using UnityEngine;

public class Enemy_Skeleton : Enemy, IParryable
{
    public bool CanBeParried { get => canBeStunned; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadState(this, stateMachine, "empty");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    [ContextMenu("Stun Enemy")]

    public void HandleParry()
    {
        if (CanBeParried == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }
}
