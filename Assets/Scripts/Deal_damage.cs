using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deal_damage : MonoBehaviour
{
    [SerializeField]
    private float damage = 15;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag.Equals("Enemy"))
        {
            Health health = c.gameObject.GetComponent<Health>();
            health.TakeDamage(damage);
        }
    }
}
