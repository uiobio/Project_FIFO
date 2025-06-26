using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private float damage;
    [SerializeField] private bool isIndestructable = false;

    public float Speed { get => speed; set => speed = value; }
    public float Lifetime { get => lifetime; set => lifetime = value; }
    public float Damage { get => damage; set => damage = value; }
    public bool IsIndestructable { get => isIndestructable; set => isIndestructable = value; }

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

        if (!isIndestructable)
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
        this.isIndestructable = indestructable;
    }
}
