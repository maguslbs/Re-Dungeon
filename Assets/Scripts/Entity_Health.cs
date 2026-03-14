using System;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    private Entity_VFX entityVfx;
    private Entity entity;

    [SerializeField] protected float currentHP;
    [SerializeField] protected float maxHP = 100;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockBackPower = new Vector2(7,7);
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;

    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageTreshold = .3f; //percentage damage taken to be considered as heavy damage

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();

        currentHP = maxHP;
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        Vector2 knockback = CalculateKnockback(damage, damageDealer);
        float duration = CalculateDuration(damage);

        entity?.ReceiveKnockback(knockback, duration);
        entityVfx?.PlayOnDamageVfx();
        ReduceHP(damage);
    }

    protected void ReduceHP(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
        
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = isHeavyDamage(damage) ? heavyKnockBackPower : knockbackPower;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage) => isHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool isHeavyDamage(float damage) => damage / maxHP > heavyDamageTreshold;
}
