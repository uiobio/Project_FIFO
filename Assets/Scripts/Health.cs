using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    [SerializeField]
    private float health = 100;

    public bool IsDead = false;

    public ProgressBar HealthBar;
    public GameObject DmgMessage;

    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            maxHealth = LevelManager.Instance.PlayerMaxHealth;
            health = LevelManager.Instance.PlayerHealth;
            if (health == -1)
            {
                health = LevelManager.Instance.PlayerMaxHealth;
                UpdateHealth();
            }
        }
    }

    void Update()
    {
        IsDead = health <= 0;
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        UpdateMax();
    }

    public void Heal(float heal)
    {
        if (health < maxHealth)
        {
            health += heal;
            UpdateHealth();
            ShowDamageUI((int)heal);
        }
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            UpdateHealth();
            ShowDamageUI((int)(-1 * damage));
        }
    }

    public float GetHealthPercentage()
    {
        float per = health / maxHealth;
        return (per < 0) ? 0 : ((per > 1) ? 1 : per);
    }

    //Functions used to update the UI Healthbar
    private void UpdateMax()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("PLAYER MAX HEALTH SET");
            LevelManager.Instance.SetMaxHealth(maxHealth);
        }
        else if (HealthBar != null)
        {
            HealthBar.SetProgress(GetHealthPercentage());
        }
    }

    private void UpdateHealth()
    {
        if (gameObject.CompareTag("Player"))
        {
            LevelManager.Instance.SetHealth(health);
        }
        else if (HealthBar != null)
        {
            HealthBar.SetProgress(GetHealthPercentage());
        }
    }

    private void ShowDamageUI(int dmg)
    {
        if (DmgMessage == null)
        {
            return;
        }

        //Create the Damage number that appears by the enemy
        Vector3 damageMessagePosition = gameObject.transform.position;
        if (HealthBar != null)
        {
            damageMessagePosition = HealthBar.gameObject.transform.position;
        }
        GameObject GO = Instantiate(DmgMessage, damageMessagePosition, transform.rotation);
        ShowDamage SD = GO.GetComponent<ShowDamage>();
        SD.UpdateText(dmg);
        if (gameObject.tag == "Player")
        {
            if (dmg < 0)
            {
                SD.SetColor(Color.red);
            }
            else
            {
                SD.SetColor(Color.green);
            }
        }
        else
        {
            SD.SetColor(Color.red);
        }
    }
}
