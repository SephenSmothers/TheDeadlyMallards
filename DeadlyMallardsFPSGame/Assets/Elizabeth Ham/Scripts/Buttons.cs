using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void Resume()
    {
        GameManager.instance.UnPause();
    }

    public void Restart()
    {
        GameManager.instance.shootingScript.resetGuns();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.UnPause();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        GameManager.instance.shootingScript.resetGuns();
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        GameManager.instance.shootingScript.resetGuns();
        SceneManager.LoadScene(1);
        GameManager.instance.UnPause();
    }

    public void Settings()
    {
        GameManager.instance._pauseMenu.SetActive(false);
        GameManager.instance._settings.SetActive(true);
        GameManager.instance._activeMenu = GameManager.instance._settings;
    }

    public void GoBackFromSettings()
    {
        GameManager.instance._pauseMenu.SetActive(true);
        GameManager.instance._settings.SetActive(false);
        GameManager.instance._activeMenu = GameManager.instance._pauseMenu;
    }
}
