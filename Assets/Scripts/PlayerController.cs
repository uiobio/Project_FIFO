using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Controller;
    public float NormalSpeed = 5f; // Default movement Speed
    public float BoostedSpeed = 7.5f; // 1.5x Speed boost
    public float BoostDuration = 30f * LevelManager.Instance.hardwareAccelUpgradeModifier; // base 30 seconds duration 
    private bool isBoosted = false;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Ensure the player has a Rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        // Move player (Example: Change this if you use different movement logic)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = (isBoosted ? BoostedSpeed : NormalSpeed) * Time.deltaTime * new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.MovePosition(rb.position + movement);

        // Trigger Speed boost when 'R' is pressed
        if (Input.GetKeyDown(KeyCode.R) && !isBoosted)
        {
            StartCoroutine(SpeedBoost());
        }
    }

    public void UpgradeBoost()
    {
        StartCoroutine(SpeedBoost(LevelManager.Instance.bootUpUpgradeModifierValue, LevelManager.Instance.bootUpUpgradeModifier));
    }

    IEnumerator SpeedBoost()
    {
        isBoosted = true;
        Debug.Log("Speed Boost Activated!");

        yield return new WaitForSeconds(BoostDuration); // Wait for 30 seconds

        isBoosted = false;
        Debug.Log("Speed Boost Ended!");
    }

    IEnumerator SpeedBoost(float boost, float duration)
    {
        float temp = BoostedSpeed;
        BoostedSpeed = boost;
        isBoosted = true;
        Debug.Log("Speed Boost Activated!");

        yield return new WaitForSeconds(duration);

        isBoosted = false;
        BoostedSpeed = temp;
        Debug.Log("Speed Boost Ended!");
    }
}
