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

                PlayerPrefs.SetInt("DoubleJumpUnlocked", 1);
            }

            if (unlockDash)
            {
                player.canDash = true;

                PlayerPrefs.SetInt("DashUnlocked", 2);
            }

            if (unlockBecomeBall)
            {
                player.canBecomeBall = true;

                PlayerPrefs.SetInt("BallFormUnlocked", 3);
            }

            if (unlockDropBomb)
            {
                player.canDropBomb = true;

                PlayerPrefs.SetInt("BombUnlocked", 4);
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
