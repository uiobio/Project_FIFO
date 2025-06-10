using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private float damage;
    [SerializeField] private bool indestructable = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Moves the projectile forward every frame
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Optional: use layers to restrict collisions via physics matrix
        Health health = other.GetComponent<Health>();
        Debug.Log($"HIT! {gameObject.name} hit {other.gameObject.name} with Health: {health != null}");
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        if (!indestructable)
        { 
            Destroy(gameObject);
        }
    }
    
    public void SetLifetime(float newLifetime)
    {
        lifetime = newLifetime;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetAsIndestructable(bool indestructable)
    {
        this.indestructable = indestructable;
    }
}
