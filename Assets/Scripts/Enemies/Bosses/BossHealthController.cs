using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController Instance { get; private set; }

    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private BossBattle battleBoss;

    public int currentHealth = 30;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("There's more than one BossHealthController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    
    void Start()
    {
        bossHealthSlider.maxValue = currentHealth;
        bossHealthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            AudioManager.Instance.PlaySFXAudio(0);

            battleBoss.BatleEnd();

        }
        else
        {
            AudioManager.Instance.PlaySFXAudio(1);
        }

        bossHealthSlider.value = currentHealth;
    }
}
