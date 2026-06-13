using System.Collections;
using UnityEngine;

public class Enemy_ArcherElfArrow : MonoBehaviour, IParryable
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float arcHeight = 5f;
    [SerializeField] private float groundDestroyDelay = 1f;

    private Collider col;
    private Rigidbody rb;
    private Entity_Combat combat;

    private bool hitGround = false;
    private bool hitPlayer = false;

    public bool CanBeParried => true;

    public void SetUpArrow(float xVelocity, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        this.combat = combat;

        rb.linearVelocity = new Vector3(xVelocity, arcHeight, 0);

        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter(Collider collision)
    {

        if (hitPlayer || hitGround)
            return;

        if (((1 << collision.gameObject.layer) & whatIsTarget) != 0)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable == null)
                return;

            Entity entity = collision.GetComponent<Entity>();

            if (entity != null && entity.isInvulnerable)
            {
                if (col != null)
                    Physics.IgnoreCollision(col, collision, true);
                return;
            }

            hitPlayer = true;
            combat.PerformRangeAttack(damageable);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hitGround = true;
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
            StartCoroutine(DestroyeAfterDelay());
        }
    }

    private IEnumerator DestroyeAfterDelay()
    {
        yield return new WaitForSeconds(groundDestroyDelay);
        Destroy(gameObject);
    }

    public void HandleParry()
    {
        //throw new System.NotImplementedException();
    }

}
