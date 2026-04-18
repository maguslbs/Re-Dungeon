using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;

    [Header("Battle Details")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float attackCooldown = .5f;
    public bool canChasePlayer = true;
    [Space]
    public float battleTimeDuration = 5;
    public float minimumRetreatDistance = 1;
    public Vector2 retretVelocity;

    [Header("Stunned state details")]
    public float stunnedDuration = 1f;
    public float miniStunDuration = 0.35f;
    public Vector2 stunnedVelocity = new Vector2(7, 7);
    [SerializeField] protected bool canBeStunned;
    [SerializeField] private float miniStunCooldown = 0.15f;
    private float lastTimeMiniStunned;
    private Coroutine miniStunCoroutine;
    public bool IsMiniStunned { get; private set; }

    [Header("Movement Details")]
    public float idleTime = 2;
    public float moveSpeed = 1.4f;

    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public float activeSlowMultiplier { get; private set; } = 1;
    public Transform player { get; private set; }

    public void EnableCounterWindow(bool enable) => canBeStunned = enable;
    public float GetBattleMoveSpeed() => battleMoveSpeed * activeSlowMultiplier;

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState)
            return;

        if (stateMachine.currentState == attackState)
            return;

        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetected().transform;

        return player;
    }

    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    public bool CanBeStunned() => canBeStunned;

    public virtual bool OnHit(float damage, Vector2 knockback, float knockbackDuration)
    {
        if (Time.time < lastTimeMiniStunned + miniStunCooldown)
            return false;

        lastTimeMiniStunned = Time.time;

        if (miniStunCoroutine != null)
            StopCoroutine(miniStunCoroutine);

        miniStunCoroutine = StartCoroutine(MiniStunCoroutine(knockback));
        return true;
    }

    private IEnumerator MiniStunCoroutine(Vector2 knockback)
    {
        IsMiniStunned = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(miniStunDuration);

        IsMiniStunned = false;
        miniStunCoroutine = null;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * playerCheckDistance), playerCheck.position.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * attackDistance), playerCheck.position.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * minimumRetreatDistance), playerCheck.position.y));
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}