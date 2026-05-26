using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;

    public Animator anim { get; private set; }
    public Rigidbody rb { get; private set; } //3d
    public StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;
    

    [Header("Collision detection")]
    [SerializeField] protected  LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    [SerializeField] private Transform groundCheck;

    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }
    public bool isInvulnerable { get; private set; }

    private bool isKnocked;
    private Coroutine knockbackCo;


    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>(); //3d

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {

    }

    public void ReceiveKnockback(Vector2 knockback, float duration)
    {
        if (knockbackCo != null)
            StopCoroutine(knockbackCo);

        knockbackCo = StartCoroutine(KnockBackCo(knockback, duration));
    }

    private IEnumerator KnockBackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = new Vector3(knockback.x, knockback.y, 0); //3d

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector3.zero; //3d
        isKnocked = false;
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
            return;

        rb.linearVelocity = new Vector3(xVelocity, yVelocity, 0); //3d
        HandleFlip(xVelocity);
    }

    public  void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight == true)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;

        OnFlipped?.Invoke();
    }

    private void HandleCollisionDetection() //3d
    {


        groundDetected = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, whatIsGround);

        if (secondaryWallCheck != null)
        {
            wallDetected = Physics.Raycast(primaryWallCheck.position, Vector3.right * facingDir, wallCheckDistance, whatIsGround)
                        && Physics.Raycast(secondaryWallCheck.position, Vector3.right * facingDir, wallCheckDistance, whatIsGround);
        }
        else
            wallDetected = Physics.Raycast(primaryWallCheck.position, Vector3.right * facingDir, wallCheckDistance, whatIsGround);

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));

        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
