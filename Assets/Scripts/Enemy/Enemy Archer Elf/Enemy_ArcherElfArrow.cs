using UnityEngine;

public class Enemy_ArcherElfArrow : MonoBehaviour, IParryable
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float lifeTime = 5f;

    private Collider col;
    private Rigidbody rb;
    private Entity_Combat combat;

    public bool CanBeParried => true;

    public void SetUpArrow(float xVelocity, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.linearVelocity = new Vector3(xVelocity, 0,0);
        this.combat = combat;

        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsTarget) == 0)
            return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null)
            return;

        Entity entity = collision.GetComponent<Entity>();

        if (entity != null && entity.isInvulnerable)
        {
            Physics.IgnoreCollision(col, collision, true);
            return;
        }

        combat.PerformRangeAttack(damageable);
        Destroy(gameObject);
    }

    public void HandleParry()
    {
        //throw new System.NotImplementedException();
    }

}
