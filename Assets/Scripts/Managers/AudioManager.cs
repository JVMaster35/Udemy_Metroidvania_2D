using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource[] musicAudio;
    [SerializeField] private AudioSource[] sfxAudio;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("There's more than one AudioManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlayMainMenuMusic()
    {
        musicAudio[0].Stop();
        musicAudio[1].Stop();

        musicAudio[2].Play();
    }

    public void PlayLevelMusic()
    {
        if(musicAudio[0].isPlaying || musicAudio[2].isPlaying)
        {
            musicAudio[0].Stop();
            musicAudio[2].Stop();
        }

        musicAudio[1].Play();
    }

    public void PlayBossMusic()
    {
        if (musicAudio[1].isPlaying || musicAudio[2].isPlaying)
        {
            musicAudio[2].Stop();
            musicAudio[1].Stop();
        }

        musicAudio[0].Play();
    }

    public void PlaySFXAudio(int sfxIndex)
    {
        sfxAudio[sfxIndex].Stop();
        sfxAudio[sfxIndex].Play();
    }

    public void PlaySFXAdjusted(int sfxToAdjust)
    {
        sfxAudio[sfxToAdjust].pitch = Random.Range(.8f, 1.2f);
        PlaySFXAudio(sfxToAdjust);
    }
}
