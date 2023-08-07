using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnim;
    [SerializeField] private Transform exitPoint;

    public float distanceToOpen;
    public float movePlayerSpeed;

    public string levelToLoad;

    private PlayerController player;

    private bool playerExiting;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.Instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

        if(playerExiting)
        {
            PlayerExiting();
        }
    }

    public void OpenDoor()
    {
        doorAnim.SetBool("DoorOpen", true);
    }

    public void CloseDoor()
    {
        doorAnim.SetBool("DoorOpen", false);
    }

    public void PlayerExiting()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, exitPoint.position, movePlayerSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(!playerExiting)
            {
                player.canMove = false;

                StartCoroutine(UseDoorRoutine());
            }
        }
    }

    IEnumerator UseDoorRoutine()
    {
        playerExiting = true;

        player.GetPlayerAnimator().enabled = false;

        UIController.Instance.StartFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        RespawnManager.Instance.SetSpawn(exitPoint.position);

        player.canMove = true;
        player.GetPlayerAnimator().enabled = true;

        UIController.Instance.StartFadeFromBlack();

        SceneManager.LoadScene(levelToLoad);
    }
}
