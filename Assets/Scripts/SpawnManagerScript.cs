using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    public GameObject enemy;

    [SerializeField]
    private float enemiesAtSameTime = 5;

    [SerializeField]
    private float enemyCounter;

    [SerializeField]
    private float totalEnemiesPerWave;

    [SerializeField]
    private float spawnSpacing = 50f;

    [SerializeField]
    private float spawnRate = 7f;

    [SerializeField]
    private float spawnTimer = 0f;

    // Update is called once per frame
    private void Start()
    {
        totalEnemiesPerWave = 39;
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
        }
        
    }

    void SpawnWave()
    {
        Vector3 center = gameObject.transform.position;
        for(int i = 0; i < enemiesAtSameTime && enemyCounter < totalEnemiesPerWave; i++){
            Vector2 randomCircle = Random.insideUnitCircle * spawnSpacing;
            Vector3 spawnPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);
            Instantiate(enemy, spawnPosition, Quaternion.identity);
            enemyCounter ++;
            }
        }
    }














