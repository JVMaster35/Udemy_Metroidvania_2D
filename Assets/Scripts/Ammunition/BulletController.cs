using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject impactFX;

    public int damageAmount = 1;
    public float bulletSpeed;
    public Rigidbody2D rgdby;

    public Vector2 moveDir;

    private void Update()
    {
        rgdby.velocity = moveDir * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        if(other.tag == "Boss")
        {
            BossHealthController.Instance.TakeDamage(damageAmount);
        }

        if (impactFX != null)
        {
            Instantiate(impactFX, transform.position, Quaternion.identity);
        }

        AudioManager.Instance.PlaySFXAudio(3);

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
