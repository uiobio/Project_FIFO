using UnityEngine;

public class SniperEnemy : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform FirePoint;
    public float FireRate = 2f;
    public float ProjectileSpeed = 50f;
    public float DetectionRange = 30f;

    private Transform player;
    private float fireCooldown = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        fireCooldown -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (fireCooldown <= 0f && distanceToPlayer <= DetectionRange)
        {
            ShootAtPlayer();
            fireCooldown = FireRate;
        }
    }

    void ShootAtPlayer()
    {
        Vector3 direction = (player.position - FirePoint.position).normalized;
        GameObject bullet = Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * ProjectileSpeed;
    }
}
