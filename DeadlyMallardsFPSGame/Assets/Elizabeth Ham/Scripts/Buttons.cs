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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.UnPause();
        GameManager.instance.playerScript.resetGuns();
        
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
        SceneManager.LoadScene(1);
        GameManager.instance.playerScript.resetGuns();
        GameManager.instance.UnPause();
    }
}
