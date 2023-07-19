using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
   // [SerializeField] GameObject _activeMenu;
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _levelSelectMenu;

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
        _mainMenu.SetActive(false);
        _levelSelectMenu.SetActive(true);
    }

    public void GoBack()
    {
        _mainMenu.SetActive(true);
        _levelSelectMenu.SetActive(false);
    }

    public void PlayMap1()
    {
        SceneManager.LoadScene(1);
        GameManager.instance.UnPause();
    }

    public void PlayMap2()
    {
        SceneManager.LoadScene(2);
        GameManager.instance.UnPause();
    }


}
