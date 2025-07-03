using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatternUIManager : MonoBehaviour
{
    public List<Image> Queue = new();
    public TMP_Text PatternText;

    public List<Sprite> SpriteUnused = new();
    public List<Sprite> SpriteUsed = new();
    public Sprite SpriteEmpty;
    [SerializeField] private Vector2 emptySize;
    [SerializeField] private Vector2 spriteSize;
    [SerializeField] private float middleDiff;
    private float emptyY;
    private float fullY;
    private List<RectTransform> spriteRectTransformList = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Image img in Queue)
        {
            spriteRectTransformList.Add(img.gameObject.GetComponent<RectTransform>());
        }

        fullY = spriteRectTransformList[0].anchoredPosition.y;
        emptyY = fullY + middleDiff;
        
        ClearQueue();

        LevelManager.AddPatternUI(gameObject);
    }

    public void UpdateQueueColors(int minUsed, int maxUsed)
    {
        //Iterates through the pattern record of levelmanager and sets all ElementTypes/sprites -- Level_manager.Instance.PatternRecord.Count
        for (int i = 0; i < LevelManager.Instance.PatternRecord.Count; i++)
        {
            bool used = (minUsed >= i && i >= maxUsed);
            int index = LevelManager.MAX_PATTERN_LEN - LevelManager.Instance.PatternRecord.Count + i;
            SetType(index, LevelManager.Instance.PatternRecord[i], used);
        }
    }

    public void UpdatePatternName(string name)
    {
        PatternText.text = name;
    }

    public void ClearQueue()
    {
        for (int i = 0; i < Queue.Count; i++)
        {
            SetEmpty(i);
        }
        PatternText.text = "";
    }

    void SetType(int imgIndex, int elementIndex, bool use)
    {
        // Sets img imgIndex to element elementIndex. uses SpriteUsed or SpriteUnused based on 'use'
        Queue[imgIndex].sprite = use ? SpriteUsed[elementIndex] : SpriteUnused[elementIndex];
        spriteRectTransformList[imgIndex].sizeDelta = spriteSize;
        spriteRectTransformList[imgIndex].anchoredPosition = SetPos(imgIndex, fullY);
    }

    void SetEmpty(int imgIndex)
    {
        // Sets img imgIndex to empty
        Queue[imgIndex].sprite = SpriteEmpty;
        spriteRectTransformList[imgIndex].sizeDelta = emptySize;
        spriteRectTransformList[imgIndex].anchoredPosition = SetPos(imgIndex, emptyY);
    }

    Vector2 SetPos(int index, float y)
    {
        // Returns the position of image index changing just the y
        return new Vector2(spriteRectTransformList[index].anchoredPosition.x, y);
    }

}
