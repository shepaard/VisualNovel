using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AchievementStorage
{

    public static void SaveAchievement(string name)
    {
        PlayerPrefs.SetInt("achievement_" + name, 1);
    }
    
    public static bool HasAchievement(string name)
    {
        return PlayerPrefs.GetInt("achievement_" + name, 0) == 1;
    }
}
