using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING }

    WaveSpawner wave = new WaveSpawner();

    [SerializeField] public WaveSpawner[] waves;
    [SerializeField] private List<CharacterStats> enemyList;

    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float waveCountdown = 0f;

    private SpawnState state = SpawnState.COUNTING;

    private int currentWave;

    [SerializeField] private Transform[] spawners;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        currentWave = 0;
    }

    private void Update()
    {
        if(state == SpawnState.WAITING)
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

        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnWave(WaveSpawner wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.enemiesAmmount; i++)
        {
            SpawnZombie(wave.enemy);
            yield return new WaitForSeconds(wave.delay);          
        }
        state = SpawnState.WAITING;
        yield break;
    }

    private void SpawnZombie(GameObject enemy)
    {
        int randomInt = Random.Range(1, spawners.Length);
        Transform randomSpawner = spawners[randomInt];
        GameObject newEnemy = Instantiate(enemy, randomSpawner.position, randomSpawner.rotation);
        CharacterStats newEnemyStats = newEnemy.GetComponent<CharacterStats>();
        enemyList.Add(newEnemyStats);
    }

    private bool EnemiesAreDead()
    {
        int i = 0;
        foreach (CharacterStats enemy in enemyList)
        {
            if (enemy.IsDead())
            {
                i++;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private void CompleteWave()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(currentWave + 1 > waves.Length - 1)
        {
            currentWave = 0;
            Debug.Log("Completed All the waves!");
            SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            currentWave++;
        }        
    }

}
