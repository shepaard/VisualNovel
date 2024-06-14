using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IDialogueActions
{
    Controls _inputActions;
    Dialogues _dialogues;
    private void OnEnable()
    {
        _dialogues = FindObjectOfType<Dialogues>();
        if (_inputActions!=null)
        {
            return;
        }
        _inputActions = new Controls();
        _inputActions.Dialogue.SetCallbacks(this);
        _inputActions.Dialogue.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Dialogue.Disable();
    }
    public void OnNextPhraseButtonPressed()
    {
        var context = new InputAction.CallbackContext();
        OnNextPhrase(context);
    }
    public void OnNextPhrase(InputAction.CallbackContext context)
    {
        if ((context.action != null && context.started) && _dialogues.DialoguePlay)
        {
            _dialogues.ContinueStory(_dialogues._buttonChoicePanel.activeInHierarchy);
        }
        else if (context.action == null)
        {
            _dialogues.ContinueStory(_dialogues._buttonChoicePanel.activeInHierarchy);
        }
    }
}
