using UnityEngine;
using System.Collections;


public class PatternFuncs : MonoBehaviour
{
    [Header("Speed Boost (Pair)")]
    [Tooltip("The speed boost multiplier is controller by Player_input_manager script on the player")]
    float boostDuration = 30f;
    public bool isBoosted = false;
    //Speed Boost Function
    public void StartSpeedBoost(){
        StartCoroutine(SpeedBoost());
    }

    IEnumerator SpeedBoost()
    {
        isBoosted = true;
        Debug.Log("Speed Boost Activated!");

        yield return new WaitForSeconds(boostDuration); // Wait for 30 seconds

        isBoosted = false;
        Debug.Log("Speed Boost Ended!");
    }
}
