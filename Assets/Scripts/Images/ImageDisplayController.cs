using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class ImageDisplayController : MonoBehaviour
{
    public static ImageDisplayController Instance;
    
    public List<ImageDisplayData> imageDataList;
    public ImageDisplayUI imageDisplayUI;
    
    void Awake()
    {
        DontDestroyOnLoad (this);
        if (Instance == null) {
            Instance = this;
        } else {
            DestroyObject(gameObject);
        }
    }
    
    public void UpdateDisplayImage(string dialogText, List<Choice> choices)
    {
        List<string> textList = new List<string>() {dialogText};
        textList.AddRange(GetChoicesTextList(choices));
        foreach (var imageData in imageDataList)
        {
            foreach (var text in textList)
            {
                int index = imageData.dialogueKeys.FindIndex(key => text.Contains(key));
                if (index >= 0)
                {
                    imageDisplayUI.Show(imageData.displayImage);
                    return;
                }
            }
        }
        imageDisplayUI.Hide();
    }

    private List<string> GetChoicesTextList(List<Choice> choices)
    {
        List<string> result = new List<string>();
        foreach (var choice in choices)
        {
            result.Add(choice.text);
        }
        return result;
    }
    
    
}
