using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rgdby;
    [SerializeField] private GameObject impactEffect;

    public int damageAmount;
    public float moveSpeed;


    void Start()
    {
        Vector3 direction = transform.position - PlayerHealthController.Instance.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        AudioManager.Instance.PlaySFXAudio(2);
    }

    void Update()
    {
        rgdby.velocity = -transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealthController.Instance.DamagePlayer(damageAmount);
        }

        if(impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        AudioManager.Instance.PlaySFXAdjusted(3);
    }
}
