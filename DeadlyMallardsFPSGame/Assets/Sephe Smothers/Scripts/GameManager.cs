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
    public dynamiteThrowable dynamiteScript;
    [SerializeField] public SaveStats SaveDataStats;
    [Header("-----Player-----")]
    public GameObject _player;
    public GameObject _playerSpawn;
    public GameObject _activeMenu;
    public GameObject _pauseMenu;
    public GameObject _winMenu;
    public GameObject _loseMenu;
    public GameObject _flashScreen;
    public GameObject _finalwinMenu;
    public GameObject _settings;
    public TextMeshProUGUI dynamiteRemaining;
    public TextMeshProUGUI enemiesRemainText;
    public TextMeshProUGUI ammoCountRemaning;
    public TextMeshProUGUI reloadPopUp;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerCash;
    public Image playerStaminaBar;
    public Image playerHpBar;
    public Image playerHpLostBar;
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
    public ObjectiveManager _ObjectiveUi;




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
        UpdatePlayerUI();
        if (!_ObjectiveUi.AllObjectivesCompleted && !isPaused && Input.GetButtonDown("Cancel") && _activeMenu == null)
        {
            Pause();
            _activeMenu = _pauseMenu;
            _activeMenu.SetActive(isPaused);
            scoreManager.ScoreBoard.SetActive(true);
            _ObjectiveUi.ObjectiveUi.SetActive(false);
        }
    }

    void OnApplicationQuit()
    {
        ResetAllStats();
    }

    public void Pause()
    {
        if (!_ObjectiveUi.AllObjectivesCompleted)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            isPaused = !isPaused;
            scoreManager.ScoreBoard.SetActive(true);
            _ObjectiveUi.ObjectiveUi.SetActive(false);
        }

    }

    public void UnPause()
    {
        Time.timeScale = origTimeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        if (_activeMenu != null)
        {
            _activeMenu.SetActive(false);
            _activeMenu = null;
        }
        //_activeMenu.SetActive(false);
        //_activeMenu = null;
        scoreManager.ScoreBoard.SetActive(false);
        _ObjectiveUi.ObjectiveUi.SetActive(true);
    }

    public void YoLose()
    {
        Pause();
        _activeMenu = _loseMenu;
        _activeMenu.SetActive(true);
    }

    public void FinalWin()
    {
        Pause();
        _activeMenu = _finalwinMenu;
        _activeMenu.SetActive(true);
    }

    public IEnumerator FlashScreen()
    {
        //_flashScreen.SetActive(true);
        //yield return new WaitForSeconds(0.1f);
        //_flashScreen.SetActive(false);
        Color startColor = _flashScreen.GetComponent<Image>().color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        _flashScreen.SetActive(true);

        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            elapsedTime = Time.time - startTime;
            float t = elapsedTime / 0.1f;
            _flashScreen.GetComponent<Image>().color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        startTime = Time.time;
        elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            elapsedTime = Time.time - startTime;
            float t = elapsedTime / 0.1f;
            _flashScreen.GetComponent<Image>().color = Color.Lerp(targetColor, startColor, t);
            yield return null;
        }

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
        playerHpLostBar.fillAmount = Mathf.Lerp(playerHpLostBar.fillAmount, playerHpBar.fillAmount, 2 * Time.deltaTime);
        playerStaminaBar.fillAmount = playerScript.stamina / playerScript.maxStamina;
        dynamiteRemaining.SetText($"{dynamiteScript.dynamiteAmount}");
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
        if (GameManager.instance.SaveDataStats._guns.Count == 0)
        {
            GameManager.instance.SaveDataStats._guns = GameManager.instance.shootingScript.usedGuns;
        }
        GameManager.instance.cash = GameManager.instance.SaveDataStats._cash;
        GameManager.instance.shootingScript.gunList = GameManager.instance.SaveDataStats._guns;
        SaveAllStats();
    }
    public void SaveAllStats()
    {
        GameManager.instance.SaveDataStats._cash = GameManager.instance.cash;
        GameManager.instance.SaveDataStats._guns = GameManager.instance.shootingScript.gunList;
    }

    public void ResetAllStats()
    {
        GameManager.instance.SaveDataStats._cash = 0;
        GameManager.instance.SaveDataStats._guns.Clear();
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
