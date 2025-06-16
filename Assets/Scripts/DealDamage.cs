using UnityEngine;

public class DealDamage : MonoBehaviour
{
    // Fixed base Damage (treat as const, its only not const to allow unity to serialize it)
    public float BaseDamage = 15.0f;

    // How much Damage is actually dealt by the attack
    private float damage = 0;

    private void OnTriggerEnter(Collider c)
    {

        if (c.gameObject.TryGetComponent<Health>(out var health))
        {
            damage = BaseDamage + LevelManager.Instance.precisionUpgradeModifier; // Recalculate Damage on every hit, based on upgrade/pattern modifiers
            health.TakeDamage(damage);
        }
    }

    // Getters, Setters
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}