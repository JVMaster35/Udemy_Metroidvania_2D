using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private GameObject pickUpEffect;

    public int healthAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerHealthController.Instance.HealPlayer(healthAmount);

            if (pickUpEffect != null)
            {
                Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            }

            AudioManager.Instance.PlaySFXAudio(5);

            Destroy(gameObject);
        }
    }
}
