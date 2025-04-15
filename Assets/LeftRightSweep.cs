using UnityEngine;

public class LeftRightSweep : MonoBehaviour
{
    public float tolerance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Projectile P = gameObject.GetComponent<Projectile>();
        P.SetAsIndestructable(true);
        transform.rotation = Quaternion.Euler(0f, -45f, 0f);
    }

    void Update(){
        if( (Level_manager.instance.Right_point.position - transform.position).magnitude <= tolerance){
            Destroy(gameObject);
        }
    }
}
