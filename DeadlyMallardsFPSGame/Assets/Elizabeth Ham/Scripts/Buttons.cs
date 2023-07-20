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
        GameManager.instance.playerScript.resetGuns();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.UnPause();
        
    }

    public void quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        GameManager.instance.playerScript.resetGuns();
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        GameManager.instance.playerScript.resetGuns();
        SceneManager.LoadScene(1);
        GameManager.instance.UnPause();
    }
}
