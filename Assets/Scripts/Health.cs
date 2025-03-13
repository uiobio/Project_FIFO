using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    [SerializeField]
    private float health = 100;

    [SerializeField]
    private bool isDead = false;

    void Update()
    {
        isDead = (health < 0) ? true : false;
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        UpdateIfPlayer(true, false);
    }

    public void SetHealth(float SetTo){
        health = SetTo;
        UpdateIfPlayer(false, true);
    }

    public float GetHealth(){
        return health;
    }

    public void Heal(float heal)
    {
        if (health < maxHealth)
        {
            health += heal;
        }
        UpdateIfPlayer();
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
        UpdateIfPlayer();
    }

    // Functions to keep GameState up to date with player's health
    private void UpdateIfPlayer(){
        UpdateIfPlayer(true, true);
    }
    private void UpdateIfPlayer(bool setMax, bool setHealth)
    {
        if(gameObject.tag == "Player")
        {
            if(setMax) { GameState.Instance.PlayerMaxHealth = maxHealth; }
            if(setHealth) { GameState.Instance.PlayerHealth = health; }
        }
    }
}
