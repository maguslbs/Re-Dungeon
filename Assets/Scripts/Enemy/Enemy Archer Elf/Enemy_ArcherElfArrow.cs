using UnityEngine;

public class Enemy_ArcherElfArrow : MonoBehaviour, IParryable
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float lifeTime = 5f;

    private Collider2D col;
    private Rigidbody2D rb;
    private Entity_Combat combat;

    public bool CanBeParried => true;

    public void SetUpArrow(float xVelocity, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.linearVelocity = new Vector2(xVelocity, 0);
        this.combat = combat;

        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsTarget) == 0)
            return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null)
            return;

        Entity entity = collision.GetComponent<Entity>();

        // 🔥 Kalau invulnerable → tembus TANPA destroy
        if (entity != null && entity.isInvulnerable)
        {
            Physics2D.IgnoreCollision(col, collision, true);
            return;
        }

        // ✔ valid hit
        combat.PerformRangeAttack(damageable);
        Destroy(gameObject);
    }

    public void HandleParry()
    {
        //throw new System.NotImplementedException();
    }

}
