using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public string RoomName;
    private Health health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = gameObject.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.IsDead)
        {
            LevelManager.Instance.ResetPlayerHealth();
            LevelManager.Instance.ResetRoom();
            SceneManager.LoadScene(RoomName);
        }
    }
}
