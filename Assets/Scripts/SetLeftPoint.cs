using UnityEngine;

public class SetLeftPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Level_manager.instance.SetLeftPoint(transform);   
    }
}
