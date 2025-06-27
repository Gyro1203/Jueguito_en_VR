using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    public GameObject[] enemy;

    private bool nextWaveScheduled = false;

    private int enemiesKilledCounter;

    private int enemyIndex;

    public int waveCounter;

    [SerializeField]
    private float enemiesAtSameTime;

    [SerializeField]
    private float enemyCounter;

    [SerializeField]
    private float totalEnemiesPerWave;

    [SerializeField]
    private float spawnSpacing = 50f;

    [SerializeField]
    private float spawnRate = 7f;

    private float nextWaveDelay = 10f;

    [SerializeField]
    private float spawnTimer = 0f;

    // Update is called once per frame
    private void Start()
    {
        totalEnemiesPerWave = 10;
        enemiesAtSameTime = 5; 
        enemiesKilledCounter = 0;
        waveCounter = 1;
    }

    void Update()
    {
        if (enemyCounter < totalEnemiesPerWave)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate)
            {
                SpawnWave();
                spawnTimer = 0f;
            }
            nextWaveScheduled = false;
        }

        if (enemiesKilledCounter == totalEnemiesPerWave && !nextWaveScheduled)
        {
            waveCounter += 1;
            Invoke("nextWave", nextWaveDelay);
            nextWaveScheduled = true;
        }
        
    }

    void SpawnWave()
    {
        Vector3 center = gameObject.transform.position;
        for(int i = 0; i < enemiesAtSameTime && enemyCounter < totalEnemiesPerWave; i++){

            Vector2 randomCircle = Random.insideUnitCircle * spawnSpacing;
            Vector3 spawnPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);
            enemyIndex = Random.Range(0, enemy.Length);
            Instantiate(enemy[enemyIndex], spawnPosition, Quaternion.identity);
            enemyCounter ++;
            }
        }

    void nextWave()
    {
        enemiesKilledCounter = 0;
        totalEnemiesPerWave += 5;
        enemiesAtSameTime +=2;
        enemyCounter = 0;
    }

    public void enemiesKilled()
    {
        enemiesKilledCounter ++;
    }

    }

    












