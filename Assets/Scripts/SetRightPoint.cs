using UnityEngine;

public class SetRightPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Level_manager.instance.SetRightPoint(transform);   
    }
}
