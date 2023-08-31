using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // [SerializeField] GameObject _activeMenu;
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _levelSelectMenu;
    [SerializeField] GameObject _settings;
    [SerializeField] GameObject _credits;
    [SerializeField] GameObject title;
    private void Awake()
    {
        _settings.SetActive(false);
    }
    public void Resume()
    {
        GameManager.instance.UnPause();
    }

    public void Restart()
    {
        GameManager.instance.UnPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        _settings.SetActive(true);
        _mainMenu.SetActive(false);
        title.SetActive(false);
    }

    public void Credits()
    {
        _mainMenu.SetActive(false);
        _credits.SetActive(true);
        title.SetActive(false);
    }

    public void GoBack()
    {
        _mainMenu.SetActive(true);
        _levelSelectMenu.SetActive(false);
        _credits.SetActive(false);
        title.SetActive(true);
    }

    public void GoBackFromSettings()
    {
        _mainMenu.SetActive(true);
        _settings.SetActive(false);
        title.SetActive(true);
    }

    public void PlayMap1()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Debug.Log("Made it here in ui");
        }

        SceneManager.LoadScene(1);

    }

    public void PlayMap2()
    {
        SceneManager.LoadScene(2);
        GameManager.instance.UnPause();
    }


}
