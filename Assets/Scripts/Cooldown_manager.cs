using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown_manager : MonoBehaviour
{
    public static Cooldown_manager instance;

    private void Awake() //Makes Cooldown_manager callable in any script: Cooldown_manager.instance.[]
    {
        instance = this;
    }

    [Header("Cooldowns")]
    // Units are seconds
    [SerializeField]
    private float DashCooldownLength;

    private bool IsDashOnCooldown;
    private float DashCooldownCompleteTime;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // If dash is on cooldown and the cooldown timer is over, take the dash off cooldown
        if (IsDashOnCooldown && Time.time >= DashCooldownCompleteTime) {
            IsDashOnCooldown = false;
        }
    }

    // Called when dash executed
    public void UpdateDashCooldown()
    {   
        // Put the dash on cooldown and set the time for when it should come off cooldown
        IsDashOnCooldown = true;
        DashCooldownCompleteTime = Time.time + DashCooldownLength;
    }

    // Getters, setters
    public bool getIsDashOnCooldown() {
        return IsDashOnCooldown;
    }

    public void setIsDashOnCooldown(bool setIsDashOnCooldown)
    {
        IsDashOnCooldown = setIsDashOnCooldown;
    }

    public float getDashCooldownLength() { 
        return DashCooldownLength;
    }

    public void setDashCooldownLength(float setDashCooldownLength) { 
        DashCooldownLength = setDashCooldownLength;
    }

    public float getDashCooldownCompleteTime()
    {
        return DashCooldownLength;
    }

    public void setDashCooldownCompleteTime(float setDashCooldownCompleteTime)
    {
        DashCooldownCompleteTime = setDashCooldownCompleteTime;
    }
}
