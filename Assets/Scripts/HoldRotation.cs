using UnityEngine;

public class HoldRotation : MonoBehaviour
{
    public Vector3 Angle = new Vector3(48.59f, -135f, 0f);

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Angle;
    }
}
