using System.Collections.Generic;
using UnityEngine;

public class SupportEnemy : MonoBehaviour
{
    public float supportRadius = 5f;
    public float moveSpeed = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, supportRadius);
        List<Transform> supportedAllies = new List<Transform>();
        Vector3 center = Vector3.zero;

        foreach (var hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy") && hit.gameObject != this.gameObject)
            {
                Health health = hit.GetComponent<Health>();
                if (health != null)
                {
                    health.isSupported = true;
                    supportedAllies.Add(hit.transform);
                    center += hit.transform.position;
                }
            }
        }

        // Move toward the center of supported allies
        if (supportedAllies.Count > 0)
        {
            center /= supportedAllies.Count;
            Vector3 direction = (center - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnDisable()
    {
        // Remove support buff when this enemy is destroyed or disabled
        Collider[] hits = Physics.OverlapSphere(transform.position, supportRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy") && hit.gameObject != this.gameObject)
            {
                Health health = hit.GetComponent<Health>();
                if (health != null)
                {
                    health.isSupported = false;
                }
            }
        }
    }
}
