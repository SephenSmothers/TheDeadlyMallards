using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public TextMeshProUGUI RoundIndicatorText;
    public enum SpawnState { SPAWNING, WAITING, COUNTING }

    public static EnemySpawner instance;

    [SerializeField] public List<WaveSpawner> waves;
    [SerializeField] private List<EnemeyAI> enemyList;

    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float waveCountdown = 0f;

    private SpawnState state = SpawnState.COUNTING;

    public int currentWave;
    int round;

    [SerializeField] private Transform[] spawners;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        currentWave = 0;
        round = 0;
        instance = this;

        RoundIndicatorText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
    }

    private void UpdateRoundDisplay()
    {
        RoundIndicatorText.text = "Round: " + GetRoundForText();
    }


    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemiesAreDead())
            {
                return;
            }
            else
            {
                CompleteWave();
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
                //something goes here
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

        UpdateRoundDisplay();
    }

    private IEnumerator SpawnWave(WaveSpawner wave)
    {
        int randZombie = 0;
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.enemiesAmmount; i++)
        {
            randZombie = Random.Range(0, wave.enemy.Length);
            SpawnZombie(wave.enemy[randZombie]);
            yield return new WaitForSeconds(wave.delay);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    private void SpawnZombie(EnemeyAI enemy)
    {
        int randomInt = Random.Range(1, spawners.Length);
        Transform randomSpawner = spawners[randomInt];
        EnemeyAI newEnemy = Instantiate(enemy, randomSpawner.position, randomSpawner.rotation);
        enemyList.Add(newEnemy);
    }

    private bool EnemiesAreDead()
    {
        bool allEnemiesAreDead = true;

        foreach (EnemeyAI enemy in enemyList)
        {
            if (enemy != null)
            {
                allEnemiesAreDead = false;
                break;
            }
        }

        return allEnemiesAreDead;
    }

    private void CompleteWave()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        AddNewWave();
        currentWave++;
        round++;
        UpdateRoundDisplay();
        enemyList.Clear();      
    }

    private void AddNewWave()
    {
        WaveSpawner wave = new WaveSpawner();

        wave.enemy = waves[currentWave].enemy;
        wave.name = "Round " + currentWave;
        wave.delay = waveCountdown;
        wave.enemiesAmmount += (int)(waves[currentWave].enemiesAmmount * 1.5);
        

        //for (int i = 0; i < waves[currentWave].enemy.Length; i++)
        //{
        //    wave.enemy[i].hp *= (int)(wave.enemy[i].hp * 0.1);
        //}
        waves.Add(wave);
    }

    public int GetCurrentWave()
    {
        return currentWave + 1;
    }
    public int GetRoundForText()
    {
        return round + 1;
    }
}
