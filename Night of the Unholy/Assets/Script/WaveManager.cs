using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    public enum WaveState
    {
        STARTING, WAITING, DELAYED
    }

    [System.Serializable]
    public struct Wave
    {
        public string id;
        public GameObject[] enemies;
        public float enemyDamageMultiplier;
        public float enemyHealthMultiplier;
        public int amount;
        public float spawnRate;
    }

    public Gamemode gamemode;
    public Wave[] randomWaves;
    public Wave[] premadeWaves;
    public bool useRandomizedWaves = true;
    private int nextWave = 0;
    private bool checkAlive = true;

    public float delayedStart = 5f;
    public float startTimer;

    public WaveState currentWaveState = WaveState.DELAYED;

    public void SetupWaves()
    {
        this.enabled = true;
        gamemode = GetComponent<Gamemode>();
        if (useRandomizedWaves)
        {
            randomWaves = new Wave[200];
            for (int i = 0; i < randomWaves.Length; i++)
            {
                randomWaves[i] = new Wave();
                randomWaves[i].id = (i + 1).ToString();
                randomWaves[i].spawnRate = 1 + (i + 1) / 10;
                randomWaves[i].enemies = gamemode.enemyPrefabs;
                randomWaves[i].enemyDamageMultiplier = 1 + 0.05f * i;
                randomWaves[i].enemyHealthMultiplier = 1 + 0.05f * i;

                if (i >= 8)
                {
                    randomWaves[i].amount = i + Random.Range(5, (5+(i/3)));
                }
                else
                {
                    randomWaves[i].amount = (Fib(i));
                }
            }
        }
    }

    private void Start()
    {
        startTimer = delayedStart;     
    }

    private void Update()
    {
        if (currentWaveState == WaveState.WAITING) //wait till all enemies are dead
        {
            if (!IsEnemiesAlive())
            {
                //Start a new wave
                nextWave++;
                currentWaveState = WaveState.DELAYED;
                startTimer = delayedStart;
                Debug.Log("Wave Finished!");
            }
            else
            {
                return;
            }
        }

        if (startTimer <= 0)
        {
            if (currentWaveState != WaveState.STARTING)
            {
                if (!useRandomizedWaves)
                {
                    StartCoroutine(StartWave((premadeWaves[nextWave]))); // start next wave
                }
                else
                {
                    StartCoroutine(StartWave((randomWaves[nextWave]))); // start next wave
                }
            }
        }
        else
        {
            startTimer -= Time.deltaTime;
        }
    }


    private bool IsEnemiesAlive() 
    {
        /*
         * Needs optimizing
         */

        WaitSetBool(5f, checkAlive);
        if (checkAlive)
        {
            //return GameObject.FindGameObjectsWithTag("Enemy").Length == 0 ? false : true; maybe better test
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                return false;
            else
                return true;
        }
        else
            return true;
    }

    private void KillAllEnemies()
    {
        //kill all enemies
    }

    void SpawnEntity(GameObject _entity, Wave _wave)
    {
        GameObject clone = Instantiate(_entity, gamemode.enemySpawnpoints[Random.Range(0, gamemode.enemySpawnpoints.Length)].transform.position, Quaternion.identity, null);
        Enemy enemy = clone.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.health = enemy.health * _wave.enemyHealthMultiplier;
            enemy.damage = enemy.damage * _wave.enemyDamageMultiplier;
        }
        NetworkServer.Spawn(clone);
        Debug.Log("Spawned Entity: " + _entity.name);
    }

    IEnumerator StartWave(Wave _wave)
    {
        Debug.LogWarning("Starting new wave " + _wave.id);
        //GameObject.Find("Player").SendMessage("UpdateRound", _wave.id);
        currentWaveState = WaveState.STARTING;

        for (int i = 0; i < _wave.amount; i++)
        {
            SpawnEntity(_wave.enemies[0], _wave); // change later so different zombies can spawn
            yield return new WaitForSeconds(1f / _wave.spawnRate);
            //delay spawn if needed
        }

        currentWaveState = WaveState.WAITING;

        yield break;
    }

    private IEnumerator WaitSetBool(float time, bool check)
    {
        yield return new WaitForSeconds(time);
        check = true;
        yield break;
    }

    public int Fib(int n)
    {
        if (n <= 2)
        {
            return 1;
        }
        int a = 1, b = 1, c;
        for (int i = 0; i < n - 2; i++)
        {
            c = a + b;
            b = a;
            a = c;
        }
        return a;
    }
}

