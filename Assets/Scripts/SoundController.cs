using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public static SoundController Instance;

    public AudioSource music;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        DontDestroyOnLoad (this);
        if (Instance == null) {
            Instance = this;
            UpdateSettings();
            music.Play();
        } else {
            DestroyObject(gameObject);
        }
    }

    public void UpdateSettings()
    {
        AudioListener.volume = ES3.Load<float>("volume") / 100f;
    }
    
}