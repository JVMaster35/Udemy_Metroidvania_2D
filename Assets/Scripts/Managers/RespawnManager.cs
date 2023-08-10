using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    [SerializeField] private GameObject deathEffect;

    public float waitToRespawn;

    private Vector3 respawnPoint;
    private GameObject player;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("There's more than one RespawnManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.Instance.gameObject;

        respawnPoint = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        player.SetActive(false);

        if (deathEffect != null)
        {
            Instantiate(deathEffect, player.transform.position, player.transform.rotation);
        }

        yield return new WaitForSeconds(waitToRespawn);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        player.transform.position = respawnPoint;

        player.SetActive(true);

        PlayerHealthController.Instance.FillHealth();
    }
}
