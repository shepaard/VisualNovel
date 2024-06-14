using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using ES3Internal;
using UnityEngine.UI;

public class Dialogues : MonoBehaviour
{
    private Story _story;
    private TextAsset _inkJson;
    
    private GameObject _dialoguesPanel;
    private TextMeshProUGUI _dialogueText;
    private TextMeshProUGUI _nameText;

    [HideInInspector]public GameObject _buttonChoicePanel;
    [SerializeField]private GameObject _buttonChoice;
    private List<TextMeshProUGUI> _choiseText = new();
    private List<Character>  characters = new();
    private float multiplier = 1.1f;
    private bool isLoadStarted;
    [Header("Настройки загрузки")]
    [Tooltip("Загрузка между сценами")]
    public bool isNeed;

    public bool DialoguePlay {  get; private set; }
    [Inject]
    public void Construct(DialogueInstaller dialogueInstaller)
    {
        _inkJson = dialogueInstaller.inkJson;
        _dialoguesPanel = dialogueInstaller.dialoguesPanel;
        _dialogueText = dialogueInstaller.dialogueText;
        _nameText = dialogueInstaller.nameText;
        _buttonChoicePanel = dialogueInstaller.buttonChoicePanel;
        _buttonChoice = dialogueInstaller.buttonChoice;
    }    
    
    private void Awake()
    {
        #region SaveSystem
        int curSave = ES3.Load("curSave", 1);

        if (curSave == 1)
            ES3.Save("scene_save1", SceneManager.GetActiveScene().name);

        else if (curSave == 2)
            ES3.Save("scene_save2", SceneManager.GetActiveScene().name);

        else if (curSave == 3)
            ES3.Save("scene_save3", SceneManager.GetActiveScene().name);
        #endregion

        _story = new Story(_inkJson.text);
    }
    
    void Start()
    {
        foreach(var character in FindObjectsOfType<Character>())
        {
            characters.Add(character);
        }
        StartDialogues();
    }

    public void StartDialogues()
    {
        DialoguePlay = true;
        _dialoguesPanel.SetActive(true);
        ContinueStory();
    }

    public void ContinueStory(bool choiseBefore =false)
    {
        if (_story.canContinue)
        {
            ShowDialogue();
            ShowChoiseButton();
        }
        else if(!choiseBefore && !isLoadStarted)
        {
            if (isNeed)
            {
                GetComponent<MainMenu>().loadingScreen.SetActive(!GetComponent<MainMenu>().loadingScreen.activeSelf);
                StartCoroutine(LoadAsyncOperation());
            }

            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void ShowDialogue()
    {
        AchievementController.Instance?.CheckAchievement(_dialogueText.text, AchievementType.ReadText);
        _dialogueText.text = _story.Continue();
        _nameText.text = (string)_story.variablesState["charаcterName"];
        var index = characters.FindIndex(character => character.characterName.Contains(_nameText.text));
        characters[index].ChangeEmotion((int)_story.variablesState["characherEmotions"]);
        ChangeCharacterScale(index);
        ImageDisplayController.Instance?.UpdateDisplayImage(_dialogueText.text, _story.currentChoices);
    }

    private void ChangeCharacterScale(int indexCharacter)
    {
        if (indexCharacter >= 0)
        {
            foreach (var character in characters)
            {
                if (character != characters[indexCharacter])
                {
                    character.ResetScale();
                }
                else if (character.DefaultScale == character.transform.localScale)
                {
                    character.ResetScale();
                    character.ChangeScale(multiplier);
                }
                else
                {
                    character.ResetScale();
                    character.ChangeScale(multiplier);
                }
            }
        }
        else
        {
            characters.ForEach(character => character.ResetScale());
        }
     }
    private void ShowChoiseButton() 
    {
        List<Choice> currentChoices = _story.currentChoices;
        _buttonChoicePanel.SetActive(currentChoices.Count != 0);
        if (currentChoices.Count <= 0 ) {return;}
        _buttonChoicePanel.transform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        _choiseText.Clear();
        for (int i = 0; i < currentChoices.Count; i++)
        {
            GameObject choice = Instantiate(_buttonChoice);
            choice.GetComponent<ButtonAction>().index = i;
            choice.transform.SetParent(_buttonChoicePanel.transform);

            TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = currentChoices[i].text;
            _choiseText.Add(choiceText);
        }
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].gameObject.SetActive(false);
        }
    }


    public void ChoiseButtonAvtion(int choiceIndex)
    {
        AchievementController.Instance?.CheckAchievement(_choiseText[choiceIndex].text, AchievementType.SelectOption);
        _story.ChooseChoiceIndex(choiceIndex);
        ContinueStory(true);
    }
    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation nextSceneIndex = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        nextSceneIndex.allowSceneActivation = false;
        isLoadStarted = true;

        while (!nextSceneIndex.isDone)
        {
            DialoguePlay = false;
            _dialoguesPanel.SetActive(false);

            Slider slider = GameObject.Find("ProgressSlider").GetComponent<Slider>();
            TextMeshProUGUI text = GameObject.Find("ProgressTextValue").GetComponent<TextMeshProUGUI>();
            slider.value = nextSceneIndex.progress;
            text.text = (int)(slider.value * 100f) + "%";

            if (nextSceneIndex.progress >= .9f &&
                !nextSceneIndex.allowSceneActivation &&
                SceneManager.GetActiveScene().buildIndex + 1 <= SceneManager.sceneCount)
            {
                slider.value = 1;
                text.text = "100%";
                yield return new WaitForSeconds(1.0f);
                nextSceneIndex.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}