using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    #region PARAMETERS

    [SerializeField]
    private AudioSource mainAudioSource;

    #endregion

    #region CACHES

    private PlayerController player;

    #endregion

    #region STATES

    private bool musicOn = true;
    private bool soundOn = true;

    #endregion

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (musicOn)
        {
            mainAudioSource.Play();
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (soundOn)
        {
            mainAudioSource.PlayOneShot(clip);
        }
    }

    public void SetMusicOn(bool value)
    {
        musicOn = value;
        if (!value)
        {
            mainAudioSource.Pause();
        }
        else
        {
            mainAudioSource.UnPause();
        }
    }

    public void SetSoundOn(bool value)
    {
        soundOn = value;
    }
}
