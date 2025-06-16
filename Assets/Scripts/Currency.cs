using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField]
    int Value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            Debug.Log("Coin hit by player!");
            LevelManager.Instance.GainCoin(Value);
            Destroy(gameObject);
        }
    }
}
