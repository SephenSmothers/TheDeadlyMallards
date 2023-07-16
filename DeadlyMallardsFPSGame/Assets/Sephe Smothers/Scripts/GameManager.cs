using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("-----Instance-----")]
    public static GameManager instance;
    public playerControl playerScript;
    [Header("-----Player-----")]
    public GameObject _player;
    public GameObject _playerSpawn;
    public GameObject _activeMenu;
    public GameObject _pauseMenu;
    public GameObject _winMenu;
    public GameObject _loseMenu;
    public GameObject _flashScreen;
    public TextMeshProUGUI enemiesRemainText;
    public TextMeshProUGUI ammoCountRemaning;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerCash;
    public Image playerHpBar;
    bool isPaused;
    float origTimeScale;
    int enemiesRemain;
    int ammoCountRemain;
    int score;
    int cash;
   


    void Awake()
    {
        instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerSpawn = GameObject.FindGameObjectWithTag("playerSpawn");
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

    // maybe output whatever string you want with string interpolation of the currentAmmo / totalAmmo or might not even need that
    public int SetAmmoCount(int count)
    {
        ammoCountRemain += count;
        ammoCountRemaning.text = ammoCountRemain.ToString("f0");
        return ammoCountRemain;
    }

    public int AddScore(int _score)
    {
        score += _score;
        scoreText.text = score.ToString("f0");
        return score;
        
    }

    public int AddCash(int _cash) 
    {
        cash += _cash;
        playerCash.text = cash.ToString("f0");
        return cash;
    }



}
