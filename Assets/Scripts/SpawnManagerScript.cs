using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// comentare este script culiao xq esta gigante 
public class SpawnManagerScript : MonoBehaviour
{
    // declara arrays para spawn de enemigos y jefes
    public GameObject[] enemy;

    public GameObject[] boss;
    // declara variables para controlar el spawn de enemigos y jefes
    private bool bossSpawned = false;

    BackGroundMusic audioManager;

    private bool isCombatMusicPlaying = false;

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
    private float spawnSpacing = 30f;

    [SerializeField]
    private float spawnRate = 7f;

    private float nextWaveDelay = 10f;

    [SerializeField]
    private float spawnTimer = 0f;
    // LayerMask para detectar el suelo
    [SerializeField]
    private LayerMask groundLayer;

    // Update is called once per frame
    private void Start()
    {
        totalEnemiesPerWave = 20; // Cuantos enemigos spawnean por oleada
        enemiesAtSameTime = 5; // Cuantos enemigos spawnean por iteracion
        enemiesKilledCounter = 0;
        waveCounter = 1;
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<BackGroundMusic>();
    }

    void Update()
    {
        // Si el contador de oleadas es impar y no hay jefes, se spawnea un jefe
        if (waveCounter % 3 == 0)
        {
            if(!bossSpawned)
            {
            SpawnBoss();
            audioManager.PlayBossMusic();
            bossSpawned = true;
            nextWaveScheduled = false;
            }
        
        return;
        }else
        {
        // Si el contador de enemigos es menor que el total de enemigos por oleada, se spawnean enemigos
        if (enemyCounter < totalEnemiesPerWave)
        {
             if (!isCombatMusicPlaying)
            {
                audioManager.PlayBattleMusic();
                isCombatMusicPlaying = true;
            }

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate)
            {
                SpawnWave();
                spawnTimer = 0f;
            }
            nextWaveScheduled = false;
        }
        }

        // Si el contador de enemigos asesinados es igual al total de enemigos por oleada, se inicia la siguiente oleada
        if (enemiesKilledCounter == totalEnemiesPerWave && !nextWaveScheduled)
        {
            if (isCombatMusicPlaying)
            {
                // audioManager.backgroundmusicSource.Play();
                audioManager.StopBattleMusic();
                isCombatMusicPlaying = false;
            }
            waveCounter += 1;
            Invoke("nextWave", nextWaveDelay);
            nextWaveScheduled = true;
        }
        
    }
    // Método para spawnear enemigos en una posición aleatoria dentro de un círculo alrededor del objeto
    void SpawnWave()
    {
        Vector3 center = gameObject.transform.position;
        for(int i = 0; i < enemiesAtSameTime && enemyCounter < totalEnemiesPerWave; i++){

            Vector2 randomCircle = Random.insideUnitCircle * spawnSpacing;
            Vector3 spawnPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);
            
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, 20f, groundLayer))
           {
                Vector3 finalPosition = hit.point;
                enemyIndex = Random.Range(0, enemy.Length);
                Instantiate(enemy[enemyIndex], finalPosition, Quaternion.identity);
                enemyCounter++;
           } 
        }
        }
    // Método para reiniciar los contadores y preparar la siguiente oleada
    void nextWave()
    {
        enemiesKilledCounter = 0;
        totalEnemiesPerWave += 5;
        enemiesAtSameTime +=3;
        enemyCounter = 0;
    }
    // Método para incrementar el contador de enemigos asesinados
    public void enemiesKilled()
    {
        enemiesKilledCounter ++;
    }
    // Método para spawnear jefes en una posición aleatoria dentro de un círculo alrededor del objeto
    void SpawnBoss()
        {
            Vector3 center = gameObject.transform.position;
            for (int i = 0; i < boss.Length; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle * spawnSpacing;
                Vector3 spawnPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);
                Instantiate(boss[i], spawnPosition, Quaternion.identity);
            }
            
        }
    // Método que se llama cuando un jefe es asesinado
    public void OnBossKilled()
    {
        audioManager.StopBossMusic();
        enemiesKilledCounter = 0;
        bossSpawned = false;
        waveCounter++;
        Invoke("nextWave", nextWaveDelay);
    }
}

    
    












