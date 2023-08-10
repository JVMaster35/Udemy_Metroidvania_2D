using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fadeScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject fullScreenMap;

    public string mainMenuScene;
    public float fadeSpeed = 2f;

    private bool fadingToBlack, fadingFromBlack;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("There's more than one UIController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //UpdateHealth(PlayerHealthController.Instance.currentHealth, PlayerHealthController.Instance.maxHealth);
    }

    void Update()
    {
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(
                fadeScreen.color.r, 
                fadeScreen.color.b, 
                fadeScreen.color.g, 
                Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime)
                );

            if(fadeScreen.color.a == 1f)
            {
                fadingToBlack = false;
            }
        }
        else if(fadingFromBlack)
        {
            fadeScreen.color = new Color(
                fadeScreen.color.r,
                fadeScreen.color.b,
                fadeScreen.color.g,
                Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime)
                );

            if (fadeScreen.color.a == 0f)
            {
                fadingFromBlack = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }

    public void StartFadeFromBlack()
    {
        fadingToBlack = false;
        fadingFromBlack = true;
    }

    public void PauseUnpause()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        Destroy(PlayerHealthController.Instance.gameObject);
        PlayerHealthController.Instance = null;

        Destroy(RespawnManager.Instance.gameObject);
        RespawnManager.Instance = null;

        Destroy(MapController.Instance.gameObject);
        MapController.Instance = null;

        Instance = null;
        Destroy(gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public GameObject GetFullscreenMap()
    {
        return fullScreenMap;
    }
}
