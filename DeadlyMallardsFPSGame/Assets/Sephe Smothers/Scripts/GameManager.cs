using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("-----Instance-----")]
    public static GameManager instance;
    public playerControl playerScript;
    public shootingControl shootingScript;
    [SerializeField] public SaveStats SaveDataStats;
    [Header("-----Player-----")]
    public GameObject _player;
    public GameObject _playerSpawn;
    public GameObject _activeMenu;
    public GameObject _pauseMenu;
    public GameObject _winMenu;
    public GameObject _loseMenu;
    public GameObject _flashScreen;
    public GameObject _settings;
    public TextMeshProUGUI enemiesRemainText;
    public TextMeshProUGUI ammoCountRemaning;
    public TextMeshProUGUI reloadPopUp;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerCash;
    public Image playerStaminaBar;
    public Image playerHpBar;
    bool isPaused;
    float origTimeScale;
    int enemiesRemain;
    int ammoCountRemain;
    int score;
    public int cash;
    public int zombiesKilled;
    public GameObject damageIndicator;
    public ScoreManager scoreManager;
    public SliderSettings sliderSettings;


    void Awake()
    {
        instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerSpawn = GameObject.FindGameObjectWithTag("playerSpawn");
        origTimeScale = Time.timeScale;
        zombiesKilled = 0;
    }
    private void Start()
    {
        _settings.SetActive(false);
        LoadAllStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && _activeMenu == null)
        {
            Pause();
            _activeMenu = _pauseMenu;
            _activeMenu.SetActive(isPaused);
            if (scoreManager != null)
            {
                scoreManager.ScoreBoard.SetActive(true);
            }

        }
    }

    void OnApplicationQuit()
    {
        ResetAllStats();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
        if (scoreManager != null)
        {
            scoreManager.ScoreBoard.SetActive(true);
        }


    }

    public void UnPause()
    {
        Time.timeScale = origTimeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        _activeMenu.SetActive(false);
        _activeMenu = null;
        if (scoreManager != null)
        {
            scoreManager.ScoreBoard.SetActive(false);
        }

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

    public IEnumerator DamageDirection()
    {
        damageIndicator.SetActive(true);
        yield return new WaitForSeconds(1f);
        damageIndicator.SetActive(false);
    }

    public int ReturnEnemyCount(int ammount)
    {
        enemiesRemain += ammount;
        enemiesRemainText.text = enemiesRemain.ToString("f0");
        return enemiesRemain;
    }

    // maybe output whatever string you want with string interpolation of the currentAmmo / totalAmmo or might not even need that
    public void SetAmmoCount(int currentAmmo, int maxAmmo)
    {
        ammoCountRemain = currentAmmo;
        ammoCountRemaning.text = $"{currentAmmo} / {maxAmmo}";
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
    public int RemoveCash(int _cash)
    {
        cash -= _cash;
        playerCash.text = cash.ToString("f0");
        return cash;
    }
    public void UpdatePlayerUI()
    {
        playerHpBar.fillAmount = (float)playerScript.hp / playerScript.maxHP;
        playerStaminaBar.fillAmount = playerScript.stamina / playerScript.maxStamina;
        if (shootingScript.gunList.Count > 0)
        {
            ammoCountRemaning.SetText($"{shootingScript.gunList[shootingScript.selectedGun].bulletsLeft} / {shootingScript.gunList[shootingScript.selectedGun].totalAmmo}");
            LowAmmoColorChange();
        }
    }
    public int GetZombiesKilled()
    {
        return zombiesKilled;
    }
    public void OnZombieKilled()
    {
        zombiesKilled++;
    }

    public void LoadAllStats()
    {
        GameManager.instance.cash = GameManager.instance.SaveDataStats._cash;
        GameManager.instance.shootingScript.gunList = GameManager.instance.SaveDataStats._guns;
    }
    public void SaveAllStats()
    {
        GameManager.instance.SaveDataStats._cash = GameManager.instance.cash;
        GameManager.instance.SaveDataStats._guns = GameManager.instance.shootingScript.gunList;
    }

    public void ResetAllStats()
    {
        GameManager.instance.SaveDataStats._cash = 0;
        GameManager.instance.SaveDataStats._guns = GameManager.instance.shootingScript.usedGuns;

    }

    private void LowAmmoColorChange()
    {
        if (shootingScript.gunList[shootingScript.selectedGun].bulletsLeft > (int)(shootingScript.gunList[shootingScript.selectedGun].magSize / 3) * 2)
        {
            ammoCountRemaning.SetText($"{shootingScript.gunList[shootingScript.selectedGun].bulletsLeft} / {shootingScript.gunList[shootingScript.selectedGun].totalAmmo}");
            ammoCountRemaning.color = Color.white;
            reloadPopUp.enabled = false;
        }
        else if (shootingScript.gunList[shootingScript.selectedGun].bulletsLeft < (int)(shootingScript.gunList[shootingScript.selectedGun].magSize / 3))
        {
            ammoCountRemaning.color = Color.red;
            reloadPopUp.enabled = true;
        }
        else
        {
            ammoCountRemaning.color = Color.yellow;
        }

        
    }
}
