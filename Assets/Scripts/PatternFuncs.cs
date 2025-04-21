using UnityEngine;
using System.Collections;


public class PatternFuncs : MonoBehaviour
{
    [Header("Speed Boost (Pair)")]
    [Tooltip("The speed boost multiplier is controller by Player_input_manager script on the player")]
    [SerializeField]
    float boostDuration = 30f;
    public bool isBoosted = false;
    [Header("Damage Sweeo (Four of a Kind)")]
    [Tooltip("The prefab created when player uses the Four of A Kind pattern")]
    [SerializeField]
    GameObject DamageSweepPrefab;

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

    public void DamageSweep(){
        Instantiate(DamageSweepPrefab, Level_manager.instance.Left_point.position, Quaternion.Euler(0f, -45f, 0f));
    }
}
