using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    public int damageAmount;
    public float timeToExplode = .5f;
    public float blastRange;

    public LayerMask whatIsDestructible;
    public LayerMask whatIsDamagable;

    void Update()
    {
        timeToExplode -= Time.deltaTime;

        if(timeToExplode <= 0)
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            AudioManager.Instance.PlaySFXAdjusted(4);

            Destroy(gameObject);

            DestoryObjects();

            DamageEnemies();
        }
    }

    public void DestoryObjects()
    {

        Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDestructible);

        if (objectsToRemove.Length > 0)
        {
            foreach (Collider2D col in objectsToRemove)
            {
                Destroy(col.gameObject);
            }
        }
    }

    public void DamageEnemies()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDamagable);

        if (enemiesToDamage.Length > 0)
        {
            foreach (Collider2D col in enemiesToDamage)
            {
                EnemyHealthController enemyHealth = col.GetComponent<EnemyHealthController>();

                if (enemyHealth != null)
                {
                    enemyHealth.DamageEnemy(damageAmount);
                }
            }
        }
    }
}
