using UnityEngine;

public class LeftRightSweep : MonoBehaviour
{
    public float Tolerance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Projectile proj = gameObject.GetComponent<Projectile>();
        proj.SetAsIndestructable(true);
        transform.rotation = Quaternion.Euler(0f, -45f, 0f);
    }

    void Update()
    {
        if ((LevelManager.Instance.RightPoint.position - transform.position).magnitude <= Tolerance)
        {
            Destroy(gameObject);
        }
    }
}
