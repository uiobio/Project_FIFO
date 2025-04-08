using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pattern_UI_manager : MonoBehaviour
{
    [SerializeField]
    public List<Image> Queue = new List<Image>();

    Color transparent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transparent = new Color(0f, 0f, 0f, 0f);

        for(int i =0; i < Queue.Count; i++){
            Queue[i].color = transparent;
        }

        Level_manager.AddPatternUI(gameObject);
    }

    public void UpdateQueueColors(int new_type){
        
        for(int i=Queue.Count-1; i>=1; i--){
            Queue[i].color = Queue[i-1].color;
        }

        Queue[0].color = Level_manager.instance.typeColors[new_type];
    }

}
