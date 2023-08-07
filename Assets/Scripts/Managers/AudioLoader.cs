using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{

    [SerializeField] private AudioManager audioManager;

    private void Awake()
    {
        if(AudioManager.Instance == null)
        {
            AudioManager newAM = Instantiate(audioManager);
            AudioManager.Instance = newAM;
            DontDestroyOnLoad(newAM.gameObject);
        }
    }
}
