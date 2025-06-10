using UnityEngine;
using System.Collections;


public class PatternFuncs : MonoBehaviour
{
    [Header("Speed Boost (Pair)")]
    [Tooltip("The speed boost multiplier is Controller by PlayerInputManager script on the player")]
    [SerializeField] private float boostDuration = 30f;
    public bool IsBoosted = false;
    [Header("Damage Sweep (Four of a Kind)")]
    [Tooltip("The prefab created when player uses the Four of A Kind pattern")]
    [SerializeField] private GameObject DamageSweepPrefab;

    //Speed Boost Function
    public void StartSpeedBoost()
    {
        StartCoroutine(SpeedBoost());
    }

    IEnumerator SpeedBoost()
    {
        IsBoosted = true;
        Debug.Log("Speed Boost Activated!");

        yield return new WaitForSeconds(boostDuration); // Wait for 30 seconds

        IsBoosted = false;
        Debug.Log("Speed Boost Ended!");
    }

    public void DamageSweep()
    {
        Instantiate(DamageSweepPrefab, LevelManager.Instance.LeftPoint.position, Quaternion.Euler(0f, -45f, 0f));
    }
}
