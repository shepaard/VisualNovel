using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ES3Internal;
using Unity.VisualScripting;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    [HideInInspector]
    public GameObject tittleScreen, mainScreen, loadScreen, loadingScreen, settingsScreen;
    private GameObject settingSlider, settingText, mainButton;
    private GameObject save1, save2, save3;
    private float volume;
    private string curScene, scene_save1, scene_save2, scene_save3;
    private bool isNewGame, isMainScene;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (SceneManager.GetActiveScene().name == "MainMenu")
            isMainScene = true;

        if (isMainScene)
        {
            tittleScreen = GameObject.Find("TittleScreen");
            loadScreen = GameObject.Find("LoadScreen");

            save1 = GameObject.Find("Save1");
            save2 = GameObject.Find("Save2");
            save3 = GameObject.Find("Save3");

            tittleScreen.SetActive(true);
            loadScreen.SetActive(false);

            if (ES3.KeyExists("scene_save1"))
            {
                scene_save1 = ES3.Load<string>("scene_save1");
                save1.GetComponentInChildren<TextMeshProUGUI>().text = $"Сохранение 1\nПрогресс: {scene_save1}";
            }

            if (ES3.KeyExists("scene_save2"))
            {
                scene_save2 = ES3.Load<string>("scene_save2");
                save2.GetComponentInChildren<TextMeshProUGUI>().text = $"Сохранение 2\nПрогресс: {scene_save2}";
            }

            if (ES3.KeyExists("scene_save3"))
            {
                scene_save3 = ES3.Load<string>("scene_save3");
                save3.GetComponentInChildren<TextMeshProUGUI>().text = $"Сохранение 3\nПрогресс: {scene_save3}";
            }
        }

        if (GameObject.Find("MainButton") != null)
        {
            mainButton = GameObject.Find("MainButton");
        }

        mainScreen = GameObject.Find("MainScreen");
        loadingScreen = GameObject.Find("LoadingScreen");
        settingsScreen = GameObject.Find("SettingsScreen");

        settingSlider = GameObject.Find("SettingSlider");
        settingText = GameObject.Find("SettingTextValue");

        mainScreen.SetActive(false);
        loadingScreen.SetActive(false);
        settingsScreen.SetActive(false);

        volume = ES3.Load<float>("volume", 100);
        settingText.GetComponent<TextMeshProUGUI>().text = volume.ToString();
        settingSlider.GetComponent<Slider>().value = volume;
    }

    private void Update()
    {
        if (settingsScreen.activeSelf == true)
        {
            volume = settingSlider.GetComponent<Slider>().value;
            settingText.GetComponent<TextMeshProUGUI>().text = volume.ToString();
            ES3.Save("volume", volume);
            SoundController.Instance?.UpdateSettings();
        }
    }

    #region ButtonControl
    public void Save1()
    {
        if (isNewGame)
        {
            ES3.Save<int>("curSave", 1);
            ES3.Save<string>("scene_save1", "Scene1");
            curScene = ES3.Load<string>("scene_save1");
            loadScreen.SetActive(false);
            loadingScreen.SetActive(true);
            mainScreen.SetActive(false);
            StartCoroutine(LoadAsyncOperation());
        }

        else
        {
            ES3.Save<int>("curSave", 1);
            curScene = ES3.Load<string>("scene_save1");
            loadScreen.SetActive(false);
            loadingScreen.SetActive(true);
            mainScreen.SetActive(false);
            StartCoroutine(LoadAsyncOperation());
        }
    }

    public void Save2()
    {
        if (isNewGame)
        {
            ES3.Save<int>("curSave", 2);
            ES3.Save<string>("scene_save2", "Scene1");
            curScene = ES3.Load<string>("scene_save2");
            loadScreen.SetActive(false);
            loadingScreen.SetActive(true);
            mainScreen.SetActive(false);
            StartCoroutine(LoadAsyncOperation());
        }

        else
        {
            ES3.Save<int>("curSave", 2);
            curScene = ES3.Load<string>("scene_save2");
            loadScreen.SetActive(false);
            loadingScreen.SetActive(true);
            mainScreen.SetActive(false);
            StartCoroutine(LoadAsyncOperation());
        }
    }

    public void Save3()
    {
        if (isNewGame)
        {
            ES3.Save<int>("curSave", 3);
            ES3.Save<string>("scene_save3", "Scene1");
            curScene = ES3.Load<string>("scene_save3");
            loadScreen.SetActive(false);
            loadingScreen.SetActive(true);
            mainScreen.SetActive(false);
            StartCoroutine(LoadAsyncOperation());
        }

        else
        {
            ES3.Save<int>("curSave", 3);
            curScene = ES3.Load<string>("scene_save3");
            loadScreen.SetActive(false);
            loadingScreen.SetActive(true);
            mainScreen.SetActive(false);
            StartCoroutine(LoadAsyncOperation());
        }
    }

    public void TittleButton()
    {
        mainScreen.SetActive(!mainScreen.activeSelf);

        if (isMainScene)
            tittleScreen.SetActive(false);
        else
            mainButton.SetActive(!mainButton.activeSelf);
    }

    public void LoadButton()
    {
        if (isMainScene)
        {
            isNewGame = false;
            loadScreen.SetActive(!loadScreen.activeSelf);
        }

        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void NewGameButton()
    {
        isNewGame = true;
        mainScreen.SetActive(!mainScreen.activeSelf);
        loadScreen.SetActive(!loadScreen.activeSelf);
    }

    public void SettingsButton()
    {
        mainScreen.SetActive(!mainScreen.activeSelf);
        settingsScreen.SetActive(!settingsScreen.activeSelf);
    }

    public void ExitButton()
    {
        if (isMainScene)
            Application.Quit();

        else
        {
            loadingScreen.SetActive(!loadingScreen.activeSelf);
            mainScreen.SetActive(!mainScreen.activeSelf);
            StartCoroutine(LoadAsyncOperationExit());
        }

    }
    #endregion

    #region Loading
    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(curScene);
        gameLevel.allowSceneActivation = false;

        while (!gameLevel.isDone)
        {
            Slider slider = GameObject.Find("ProgressSlider").GetComponent<Slider>();
            TextMeshProUGUI text = GameObject.Find("ProgressTextValue").GetComponent<TextMeshProUGUI>();
            slider.value = gameLevel.progress;
            text.text = (int)(slider.value * 100f) + "%";

            if (gameLevel.progress >= .9f && !gameLevel.allowSceneActivation)
            {
                slider.value = 1;
                text.text = "100%";
                yield return new WaitForSeconds(1.0f);
                gameLevel.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    IEnumerator LoadAsyncOperationExit()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("MainMenu");
        gameLevel.allowSceneActivation = false;

        while (!gameLevel.isDone)
        {
            Slider slider = GameObject.Find("ProgressSlider").GetComponent<Slider>();
            TextMeshProUGUI text = GameObject.Find("ProgressTextValue").GetComponent<TextMeshProUGUI>();
            slider.value = gameLevel.progress;
            text.text = (int)(slider.value * 100f) + "%";

            if (gameLevel.progress >= .9f && !gameLevel.allowSceneActivation)
            {
                slider.value = 1;
                text.text = "100%";
                yield return new WaitForSeconds(1.0f);
                gameLevel.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    #endregion
}
