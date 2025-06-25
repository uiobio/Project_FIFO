using UnityEngine;
using System.Collections.Generic;

public class SpriteSelect : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public List<Sprite> SpriteOptions = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (SpriteOptions != null && SpriteOptions.Count > 0) {
            int r = (int) Random.Range(0f, SpriteOptions.Count - 0.01f);
            spriteRenderer.sprite = SpriteOptions[r];
        }
    }
}
