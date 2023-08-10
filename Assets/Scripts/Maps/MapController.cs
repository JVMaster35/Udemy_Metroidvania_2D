using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController Instance;

    [SerializeField] private GameObject[] maps;
    [SerializeField] private GameObject fullMapCamera;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("There's more than one MapController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        foreach (GameObject map in maps)
        {
            if(PlayerPrefs.GetInt("Map_" + map.name) == 1)
            {
                map.SetActive(true);
            }
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!UIController.Instance.GetFullscreenMap().activeInHierarchy)
            {
                Time.timeScale = 0f;
                UIController.Instance.GetFullscreenMap().SetActive(true);
                fullMapCamera.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                UIController.Instance.GetFullscreenMap().SetActive(false);
                fullMapCamera.SetActive(false);
            }
        }
    }

    public void ActivateMap(string mapToActivate)
    {
        foreach(GameObject map in maps)
        {
            if(map.name == mapToActivate)
            {
                map.SetActive(true);
                PlayerPrefs.SetInt("Map_" + mapToActivate, 1);
            }
        }
    }
}
