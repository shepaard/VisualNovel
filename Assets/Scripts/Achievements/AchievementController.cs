using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementController : MonoBehaviour
{

    public static AchievementController Instance;

    public List<AchievementData> achievements;
    public AchievementPopup popup;
    public float achievementDisplayTime = 3f;

    void Awake()
    {
        DontDestroyOnLoad (this);
        if (Instance == null) {
            Instance = this;
        } else {
            DestroyObject(gameObject);
        }
    }

    public void CheckAchievement(string text, AchievementType type)
    {
        foreach (var achievement in achievements)
        {
            if (achievement.type == type && text.Contains(achievement.dialogueKey) && !AchievementStorage.HasAchievement(achievement.name))
            {
                Debug.Log("Получено достижение: " + achievement.name);
                popup.Show("Достижение: " + achievement.name, achievementDisplayTime);
                AchievementStorage.SaveAchievement(achievement.name);
            }
        }
    }

}
