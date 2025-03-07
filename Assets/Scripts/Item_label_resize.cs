using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item_label_resize : MonoBehaviour
{

    // Force update/reload the canvases and TMP object to ensure the TMP object assigns its preferred height
    // Then use that height to set the height of the canvas
    public void UpdateSize() 
    {
        RectTransform tmpText = (RectTransform)transform.Find("Panel").Find("TMP");
        RectTransform canvasRect = GetComponent<RectTransform>();
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(tmpText);
        canvasRect.sizeDelta = new Vector2(canvasRect.sizeDelta.x, tmpText.rect.height);
    }
}
