using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pattern_UI_manager : MonoBehaviour
{
    [SerializeField]
    public List<Image> Queue = new List<Image>();
    [SerializeField]
    public TMP_Text t_pattern;

    Color transparent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transparent = new Color(0f, 0f, 0f, 0f);

        ClearQueue();

        Level_manager.AddPatternUI(gameObject);
    }

    public void UpdateQueueColors(int new_type){
        
        for(int i=Queue.Count-1; i>=1; i--){
            Queue[i].color = Queue[i-1].color;
        }

        Queue[0].color = Level_manager.instance.typeColors[new_type];
    }

    public void UpdatePatternName(string name){
        t_pattern.text = name;
    }

    public void ClearQueue(){
        for(int i =0; i < Queue.Count; i++){
            Queue[i].color = transparent;
        }
        t_pattern.text = "";
    }

}
