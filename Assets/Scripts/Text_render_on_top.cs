using TMPro;
using UnityEngine;

// Ensures FloatingText objects always render in the furthest foreground rendering layer
public class Text_render_on_top : MonoBehaviour
{
    void Start()
    {
        TextMeshPro tmp = GetComponent<TextMeshPro>();
        if (tmp != null)
        {
            Material fontMaterial = tmp.fontSharedMaterial;
            fontMaterial.renderQueue = 4000;
        }
    }
}