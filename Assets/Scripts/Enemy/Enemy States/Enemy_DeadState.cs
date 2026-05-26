using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider col; //3d

    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        col = enemy.GetComponent<Collider>(); //3d
    }

    public override void Enter()
    {
        anim.enabled = false;
        col.enabled = false;

        rb.useGravity = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 15,0); //3d

        stateMachine.SwitchOffStateMachine();
    }
}
