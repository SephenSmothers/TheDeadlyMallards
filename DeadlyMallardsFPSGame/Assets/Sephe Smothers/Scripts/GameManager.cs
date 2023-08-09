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
    public int cash;
    public int zombiesKilled;



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
        //SaveAllStats();
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
        }
    }

    void OnApplicationQuit()
    {
        ResetAllStats();
    }

    public void Pause()
    {
        //LoadAllStats();
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
        if (shootingScript.gunList.Count > 0)
        {
            ammoCountRemaning.SetText($"{shootingScript.gunList[shootingScript.selectedGun].bulletsLeft} / {shootingScript.gunList[shootingScript.selectedGun].totalAmmo}");
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
        //EnemySpawner.instance = GameManager.instance.SaveDataStats._spawnerRef;
    }
    public void SaveAllStats()
    {
        GameManager.instance.SaveDataStats._cash = GameManager.instance.cash;
        GameManager.instance.SaveDataStats._guns = GameManager.instance.shootingScript.gunList;
        //EnemySpawner.instance = GameManager.instance.SaveDataStats._spawnerRef;
    }

    public void ResetAllStats()
    {
        GameManager.instance.SaveDataStats._cash = 0;
        GameManager.instance.SaveDataStats._guns = GameManager.instance.shootingScript.usedGuns;
       // GameManager.instance.SaveDataStats._guns = new List<GunsManager>();

    }

}
