using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private PlayerAbilityTracker player;

    public string newGameScene;

    void Start()
    {
        if (PlayerPrefs.HasKey("ContinueLevel"))
        {
            continueBtn.SetActive(true);
        }

        AudioManager.Instance.PlayMainMenuMusic();
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(newGameScene);
    }

    public void ContinueGame()
    {
        player.gameObject.SetActive(true);
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));

        if (PlayerPrefs.HasKey("DoubleJumpUnlocked"))
        {
            if(PlayerPrefs.GetInt("DoubleJumpUnlocked") == 1)
            {
                player.canDoubleJump = true;
            }
        }

        if (PlayerPrefs.HasKey("DashUnlocked"))
        {
            if (PlayerPrefs.GetInt("DashUnlocked") == 2)
            {
                player.canDash = true;
            }
        }

        if (PlayerPrefs.HasKey("BallFormUnlocked"))
        {
            if (PlayerPrefs.GetInt("BallFormUnlocked") == 3)
            {
                player.canBecomeBall = true;
            }
        }

        if (PlayerPrefs.HasKey("BombUnlocked"))
        {
            if (PlayerPrefs.GetInt("BombUnlocked") == 4)
            {
                player.canDropBomb = true;
            }
        }

        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Quit!");
    }
}
