using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;

    [SerializeField] private SpriteRenderer[] playerSprites;

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    public float invincabilityLength;
    public float flashLength;

    private float invincabilityCounter;
    private float flashCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("There's more than one PlayerHealthController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        //When Player gets Hurt
        PlayerHurt();
    }

    public void DamagePlayer(int damageAmount)
    {
        //When Player is damaged by Enemy
        if (invincabilityCounter <= 0)
        {
            currentHealth -= damageAmount;

            //Player Dies
            if (currentHealth <= 0)
            {
                currentHealth = 0;

                AudioManager.Instance.PlaySFXAudio(8);

                RespawnManager.Instance.Respawn();
            }
            else
            {
                invincabilityCounter = invincabilityLength;

                AudioManager.Instance.PlaySFXAdjusted(11);
            }

            UIController.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public void PlayerHurt()
    {
        if (invincabilityCounter > 0)
        {
            invincabilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;

            if (flashCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                {
                    sr.enabled = !sr.enabled;
                }

                flashCounter = flashLength;
            }

            if (invincabilityCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                {
                    sr.enabled = true;
                }

                flashCounter = 0f;
            }
        }
    }

    public void FillHealth()
    {
        currentHealth = maxHealth;

        UIController.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    public int GetPlayerHealth()
    {
        return currentHealth;
    }

    public int GetPlayerMaxHealth()
    {
        return maxHealth;
    }
}
