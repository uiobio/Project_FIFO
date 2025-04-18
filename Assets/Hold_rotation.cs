using UnityEngine;

public class Hold_rotation : MonoBehaviour
{
    public Vector3 angle = new Vector3(48.59f, -135f, 0f);

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = angle;
    }
}
