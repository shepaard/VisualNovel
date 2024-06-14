using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserPrefs
{
    private static string VOLUME_KEY = "SOUND_VOLUME";

    public static void SetVolume(int volume)
    {
        PlayerPrefs.SetInt(VOLUME_KEY, volume);
    }
    
    public static int GetVolume()
    {
        return PlayerPrefs.GetInt(VOLUME_KEY, 100);
    }


}