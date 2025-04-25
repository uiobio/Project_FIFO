using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_death : MonoBehaviour
{
    private Health H;
    public string room_name;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        H = gameObject.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if( H.isDead ){
            Level_manager.instance.ResetPlayerHealth();
            SceneManager.LoadScene(room_name);
        }
    }
}
