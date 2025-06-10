using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField]
    int value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c){
        if(c.tag == "Player"){
            Debug.Log("Coin hit by player!");
            LevelManager.Instance.GainCoin(value);
            Destroy(gameObject);
        }
    }
}
