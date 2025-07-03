using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceLine : MonoBehaviour
{
    public Vector3 FromPos;
    public Vector3 ToPos;
    public RectTransform Line;
    public Image LineImage;

    void Update()
    {
        Vector3 direction = ToPos - FromPos;
        float distance = direction.magnitude;
        ColorUtility.TryParseHtmlString("#87CEEB", out Color color);  // Pale, electric blue
        LineImage.color = color;
        Line.position = FromPos;
        Line.sizeDelta = new Vector2(distance, 1.5f);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Line.rotation = Quaternion.Euler(0, 0, angle);
    }
}