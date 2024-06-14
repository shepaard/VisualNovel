using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ImageDisplayUI : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public Image image;

    private bool isVisible = false;

    public void Show(Sprite sprite)
    {
        image.sprite = sprite;
        image.preserveAspect = true;
        if (isVisible)
            return;
        isVisible = true;
        canvasGroup.alpha = 1f;
    }
    
    public void Hide()
    {
        if (!isVisible)
            return;
        isVisible = false;
        canvasGroup.alpha = 0f;
    }
}
