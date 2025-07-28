using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichScript : MonoBehaviour
{
    // Variables Base Boss
    public int rutina;
    public float cronometro, timeBetweeAttacks, timeAfterDespawn = 5f;
    public Animator animator;
    public bool isAttacking;
    public AudioSource music;

    public UnityEngine.AI.NavMeshAgent enemy;
    private GameObject player;
    private AttributesManager playerATM;
    public LayerMask playerLayer;

    // Fases
    [Header("Fase")]
    public int fase = 1;
    public float currHp, maxHp;
    public bool dead;

    //Ataque distancia
    [Header("Ataque a distancia")]
    public GameObject fireBall;
    public GameObject chargedFireBall;
    public GameObject superChargedFireBall;
    public Transform bulletSpawn;
    public List<GameObject> firePool = new List<GameObject>();
    public List<GameObject> chargedPool = new List<GameObject>();
    public List<GameObject> superChargedPool = new List<GameObject>();
    private GameObject mainCam;

    // Invocacion
    [Header("Invocacion")]
    public GameObject[] enemies;
    private int enemyIndex;
    private float spawnSpacing = 10f;

    // TP
    [Header("Teleport")]
    public GameObject tpPoint;
    public float warningZone, tpCounter, timeBeforeEscape = 5f;
    private bool playerInWarningZone;

    // Knockback (NO IMPLEMENTADO)
    [Header("Knockback")]
    public float dangerZone;
    private Rigidbody pjRb;
    private bool playerInDangerZone;

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player"); // Actually no se usa pero lo dejo por si acaso
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        // pjRb = player.GetComponent<Rigidbody>();
        playerATM = player.GetComponent<AttributesManager>();
        enemy = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        isAttacking = false;

        // music.enabled = true;
        maxHp = GetComponent<AttributesManager>().maxHealth;
        currHp = GetComponent<AttributesManager>().currentHealth;
    }

    void Update()
    {
        // PARCHE MOMENTANEO (hay que ver como manejar aqui la hp)
        currHp = GetComponent<AttributesManager>().currentHealth;

        if(currHp > 0)
        {
            Life();
        }
        else
        {
            if(!dead)
            {
              animator.SetTrigger("dead");
              // music.enabled = false;
              dead = true;
              cronometro = 0;
            }
            else
            {
              cronometro += 1 * Time.deltaTime;
              if(cronometro > timeAfterDespawn)
              {
                Destroy(gameObject);
              }
            }
        }
    }

    public void Life()
    {
        if(currHp < 100)
        {
            fase = 2;
            timeBetweeAttacks = 1;
        }

        BoosBehavior();
    }

    public void BoosBehavior()
    {
        if(!playerATM.imDying)
        {
            var lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            bulletSpawn.LookAt(mainCam.transform.position);  // O player.transform.position CORREGIR ROTACION EN X
            // Parche momentaneo. Posible solucion -> Añadir objeto vacio como target del enemigo


            // Detecta si el jugador esta demaciado cerca del enemigo
            playerInWarningZone = Physics.CheckSphere(transform.position, warningZone, playerLayer);
            playerInDangerZone = Physics.CheckSphere(transform.position, dangerZone, playerLayer);
            if(playerInWarningZone && !isAttacking) tpCounter += 1 * Time.deltaTime;
            
            if(tpCounter > timeBeforeEscape)
            {
              Debug.Log("NIGERUNDAYOOOOO");
              cronometro = 0;
              tpCounter = 0;
              rutina = 3; // Escapa del jugador luego de X tiempo
            }
            else if(!isAttacking)
            {
                enemy.SetDestination(transform.position);

                switch (rutina)
                {
                    case 0:
                        // Controlador Rutina
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 5f);
                        
                        cronometro += 1 * Time.deltaTime;
                        if(cronometro > timeBetweeAttacks)
                        {
                            rutina = Random.Range(0, 6); // Valor maximo + 1 (Val. máximo es exclusivo [documentación])
                            cronometro = 0;
                        }
                        break;
                    case 1:
                        //Ataque 1

                        if(fase == 2 )
                        {
                          animator.SetBool("attack", false);
                          animator.SetBool("superAttack", true);
                          animator.SetFloat("skillsF2", 0);
                        }
                        else{
                          animator.SetBool("superAttack", false);
                          animator.SetBool("attack", true);
                          animator.SetFloat("skillsF1", 0);
                        }
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                        break;
                    case 2:
                        //Ataque 2

                        if(fase == 2 )
                        {
                          animator.SetBool("attack", false);
                          animator.SetBool("superAttack", true);
                          animator.SetFloat("skillsF2", 0.25f);
                        }
                        else{
                          animator.SetBool("superAttack", false);
                          animator.SetBool("attack", true);
                          animator.SetFloat("skillsF1", 0.25f);
                        }
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                        break;
                    case 3:
                        //Ataque Cargado

                        if(fase == 2 )
                        {
                          animator.SetBool("attack", false);
                          animator.SetBool("superAttack", true);
                          animator.SetFloat("skillsF2", 0.5f);
                        }
                        else{
                          animator.SetBool("superAttack", false);
                          animator.SetBool("attack", true);
                          animator.SetFloat("skillsF1", 0.5f);
                        }
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                        break;
                    case 4:
                        //Invocar minions
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2f);

                        if(fase == 2 )
                        {
                          animator.SetBool("attack", false);
                          animator.SetBool("superAttack", true);
                          animator.SetFloat("skillsF2", 0.75f);
                        }
                        else{
                          animator.SetBool("superAttack", false);
                          animator.SetBool("attack", true);
                          animator.SetFloat("skillsF1", 0.75f);
                        }
                        break;
                    case 5:   
                        //TP

                        if(fase == 2 )
                        {
                          animator.SetBool("attack", false);
                          animator.SetBool("superAttack", true);
                          animator.SetFloat("skillsF2", 1);
                        }
                        else{
                          animator.SetBool("superAttack", false);
                          animator.SetBool("attack", true);
                          animator.SetFloat("skillsF1", 0.5f); 
                        }
                        break;
                }
            }
        }
    }

    public void ResetAttack(){
        animator.SetBool("attack", false);
        animator.SetBool("superAttack", false);
        rutina = 0;
        isAttacking = false;
    
    }

    public GameObject GetFireBall()
    {
        // Este codigo recicla los proyectiles en caso de lanzar varios
        for(int i = 0; i < firePool.Count; i++)
        {
            if(!firePool[i].activeInHierarchy)
            {
              firePool[i].SetActive(true);
              return firePool[i];
            }
        }
        GameObject obj = Instantiate(fireBall, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        obj.GetComponent<LichBullets>().target = player;
        firePool.Add(obj);
        return obj;
    }

    private void FireBallAttack(){
        /// Attack code here
        isAttacking = true;
        // bulletSpawn.LookAt(player.transform);
        
        // Rigidbody rb = Instantiate(fireBall, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        // rb.gameObject.GetComponent<EnemyBullets>().creador = this.gameObject;
        
        // rb.AddForce(bulletSpawn.forward * 32f, ForceMode.Impulse);
        // rb.AddForce(bulletSpawn.up * 5f, ForceMode.Impulse);

        GameObject obj = GetFireBall();
        obj.transform.position = bulletSpawn.transform.position;
        obj.transform.rotation = bulletSpawn.transform.rotation;
    }

    public GameObject GetPinkFireBall()
    {
        // Este codigo recicla los proyectiles en caso de lanzar varios
        for(int i = 0; i < chargedPool.Count; i++)
        {
            if(!chargedPool[i].activeInHierarchy)
            {
              chargedPool[i].SetActive(true);
              return chargedPool[i];
            }
        }
        GameObject obj = Instantiate(chargedFireBall, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        obj.GetComponent<LichBullets>().target = player;
        chargedPool.Add(obj);
        return obj;
    }

    private void ChargedAttack(){
        /// Attack code here
        isAttacking = true;
        GameObject obj = GetPinkFireBall();
        obj.transform.position = bulletSpawn.transform.position;
        obj.transform.rotation = bulletSpawn.transform.rotation;
    }

    public GameObject GetRedFireBall()
    {
        // Este codigo recicla los proyectiles en caso de lanzar varios
        for(int i = 0; i < superChargedPool.Count; i++)
        {
            if(!superChargedPool[i].activeInHierarchy)
            {
              superChargedPool[i].SetActive(true);
              return superChargedPool[i];
            }
        }
        GameObject obj = Instantiate(superChargedFireBall, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        obj.GetComponent<LichBullets>().target = player;
        superChargedPool.Add(obj);
        return obj;
    }

    private void SuperChargedAttack(){
        /// Attack code here
        isAttacking = true;
        GameObject obj = GetRedFireBall();
        obj.transform.position = bulletSpawn.transform.position;
        obj.transform.rotation = bulletSpawn.transform.rotation;
    }

    private void SummonMinions(){
        /// Summoning code here
        
        isAttacking = true;
        for(int i = 0; i < 3; i++){
            Vector3 center = gameObject.transform.position;
            Vector2 randomCircle = Random.insideUnitCircle * spawnSpacing;
            Vector3 spawnPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);
            enemyIndex = Random.Range(0, enemies.Length);
            Instantiate(enemies[enemyIndex], spawnPosition, Quaternion.identity);
        }
    }

    private void Teleport()
    {
        isAttacking = true;
        Vector3 center = tpPoint.transform.position;
        Vector2 randomCircle = Random.insideUnitCircle * 50;
        Vector3 tpPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);
        gameObject.transform.position = tpPosition;
    }

    private void Knockback()
    {
        isAttacking = true;
        if(pjRb)
        {
            Vector3 direction = (pjRb.transform.position - transform.position).normalized;
            pjRb.AddForce(direction * 10f, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("No player found in danger zone for knockback.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, warningZone);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerZone);
    }
}
