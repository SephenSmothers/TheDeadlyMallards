using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("-----Instance-----")]
    public static GameManager instance;
    [Header("-----Player-----")]
    public GameObject _player;
    public GameObject _activeMenu;
    public GameObject _pauseMenu;
    public GameObject _winMenu;
    public GameObject _loseMenu;
    public GameObject _flashScreen;
    public TextMeshProUGUI enemiesRemainText;
    public TextMeshProUGUI ammoCountRemaning;
    public Image playerHpBar;
    bool isPaused;
    float origTimeScale;
    int enemiesRemain;
    int ammoCountRemain;
   


    void Awake()
    {
        instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
        origTimeScale = Time.timeScale;
    }

   // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && _activeMenu == null)
        {
            Pause();
            _activeMenu = _pauseMenu;
            _activeMenu.SetActive(isPaused);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    }

    public void UnPause()
    {
        Time.timeScale = origTimeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        _activeMenu.SetActive(false);
        _activeMenu = null;
    }

    public void YoLose()
    {
        Pause();
        _activeMenu = _loseMenu;
        _activeMenu.SetActive(true);
    }

    public IEnumerator FlashScreen()
    {
        _flashScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _flashScreen.SetActive(false);
    }

    public int ReturnEnemyCount(int ammount)
    {
        enemiesRemain += ammount;
        enemiesRemainText.text = enemiesRemain.ToString("f0");
        return enemiesRemain;
    }

    public int SetAmmoCount(int count)
    {
        ammoCountRemain = count;
        ammoCountRemaning.text = ammoCountRemain.ToString();
        return ammoCountRemain;
    }

}
