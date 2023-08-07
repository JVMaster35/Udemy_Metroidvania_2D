using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    [SerializeField] private GameObject pickUpEffect;
    [SerializeField] private TMP_Text unlockTMPTxt;

    public bool unlockDoubleJump, unlockDash, unlockBecomeBall, unlockDropBomb;

    public string unlockMessage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerAbilityTracker player = other.GetComponentInParent<PlayerAbilityTracker>();

            if (unlockDoubleJump)
            {
                player.canDoubleJump = true;
            }

            if (unlockDash)
            {
                player.canDash = true;
            }

            if (unlockBecomeBall)
            {
                player.canBecomeBall = true;
            }

            if (unlockDropBomb)
            {
                player.canDropBomb = true;
            }

            Instantiate(pickUpEffect, transform.position, transform.rotation);

            unlockTMPTxt.transform.parent.SetParent(null);
            unlockTMPTxt.transform.parent.position = transform.position;

            unlockTMPTxt.text = unlockMessage;
            unlockTMPTxt.gameObject.SetActive(true);

            AudioManager.Instance.PlaySFXAudio(5);

            Destroy(unlockTMPTxt.transform.parent.gameObject, 5f);

            Destroy(gameObject);
        }
    }
}
