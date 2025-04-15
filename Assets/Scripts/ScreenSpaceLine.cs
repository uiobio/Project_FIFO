using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceLine : MonoBehaviour
{
    public Vector3 fromPos;
    public Vector3 toPos;
    public RectTransform line;
    public Image lineImage;

    void Update()
    {
        Vector3 direction = toPos - fromPos;
        float distance = direction.magnitude;
        ColorUtility.TryParseHtmlString("#87CEEB", out Color color);  // Pale, electric blue
        lineImage.color = color;
        line.position = fromPos;
        line.sizeDelta = new Vector2(distance, 1.5f);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.rotation = Quaternion.Euler(0, 0, angle);
    }
}