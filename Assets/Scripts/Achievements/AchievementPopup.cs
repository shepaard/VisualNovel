using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class AchievementPopup : MonoBehaviour
{
    
    public CanvasGroup canvasGroup;
    public RectTransform messageWindow;
    public TMP_Text messageText;
    public float animationTime = 0.35f;

    private bool showing = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(string message, float time)
    {
        if (showing)
        {
            StopAllCoroutines();
            canvasGroup.alpha = 0f;
        }
        messageText.text = message;
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageWindow);
        StartCoroutine(ShowCoroutine(time));
    }

    IEnumerator ShowCoroutine(float time)
    {
        showing = true;
        StartCoroutine(PopupAnimation(true));
        yield return new WaitForSeconds(time);
        StartCoroutine(PopupAnimation(false));
        showing = false;
    }

    protected IEnumerator PopupAnimation(bool show, UnityAction onComplete = null)
    {
        if (show)
        {
            messageWindow.localScale = Vector3.zero;
            canvasGroup.alpha = 1f;
        }
        canvasGroup.blocksRaycasts = false;
        for (float t = 0f; t < animationTime; t += Time.deltaTime)
        {
            float nt = t / animationTime;
            if (!show)
                nt = 1f - nt;
            float easeValue = EaseFunctions.OutBack(nt);
            messageWindow.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, easeValue);
            yield return null;
        }
        messageWindow.localScale = show ? Vector3.one : Vector3.zero;
        if (show)
            canvasGroup.blocksRaycasts = true;
        else
            canvasGroup.alpha = 0f;
        onComplete?.Invoke();
    }
    
    
}
