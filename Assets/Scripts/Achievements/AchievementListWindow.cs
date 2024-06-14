using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementListWindow : MonoBehaviour
{

    public RectTransform contentRoot;
    public GameObject elementPrefab;


    public void Init()
    {
        Clear();
        foreach (var achievement in AchievementController.Instance.achievements)
        {
            if (!AchievementStorage.HasAchievement(achievement.name))
                continue;
            var element = Instantiate(elementPrefab, contentRoot);
            element.GetComponentInChildren<TMP_Text>().text = achievement.name;
        }
    }
    

    private void Clear()
    {
        for (int i = 0; i < contentRoot.childCount; i++)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }
    }
}
