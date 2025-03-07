using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    [SerializeField]
    private float health = 100;

    [SerializeField]
    private bool isDead = false;

    void Update()
    {
        isDead = (health < 0) ? true : false;
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
    }

    public void Heal(float heal)
    {
        if (health < maxHealth)
        {
            health += heal;
        }
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
    }
}
