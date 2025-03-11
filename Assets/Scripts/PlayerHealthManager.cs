using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    // Allows this to be freely referenced by other scripts
    public static PlayerHealthManager instance;

    private int health = 100;

    /// <summary>
    /// Clamps the value between 0 and the maximum health before assigning
    /// </summary>
    public int Health
    {
        get => health;
        set
        {
            if (value < 0)
            {
                health = 0;
            }
            else if (value > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health = value;
            }
        }
    }

    public int maxHealth = 100;

    private void Awake() // makes Player_input_manager callable in any script: Player_input_manager.instance.[]
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(int damage)
    {
        Health -= damage;
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;
    }

    public void OnRespawn()
    {
        Health = maxHealth;
    }
}
