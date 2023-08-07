using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{

    [SerializeField] private GameObject deathEffect;

    public int totalHealth = 3;

    public void DamageEnemy(int damageAmount)
    {
        totalHealth -= damageAmount;

        if(totalHealth <= 0)
        {
            if(deathEffect != null )
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            AudioManager.Instance.PlaySFXAudio(4);

            Destroy(gameObject);
        }
    }
}
