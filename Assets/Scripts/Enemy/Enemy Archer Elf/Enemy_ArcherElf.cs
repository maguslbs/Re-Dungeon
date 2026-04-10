using UnityEngine;

public class Enemy_ArcherElf : Enemy
{
    [Header("Arrow Data")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float arrowSpeed = 10f;

    public bool CanBeParried { get => canBeStunned; }
    public Enemy_ArcherElfBattleState elfBattleState { get; set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        deadState = new Enemy_DeadState(this, stateMachine, "empty");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        elfBattleState = new Enemy_ArcherElfBattleState(this, stateMachine, "battle");
        battleState = elfBattleState;
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

    public void ShootArrow()
    {
        GameObject arrowGO = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

        Enemy_ArcherElfArrow arrow = arrowGO.GetComponent<Enemy_ArcherElfArrow>();

        int direction = facingDir;

        arrow.SetUpArrow(direction * arrowSpeed, GetComponent<Entity_Combat>());
    }
}
