using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichScript : MonoBehaviour
{
    // Variables Base Boss
    public int rutina;
    public float cronometro;
    public float timeBetweeAttacks;
    public Animator animator;
    public Quaternion angulo;
    public float grado;
    public GameObject target;
    public bool isAttacking;
    public GameObject[] hit;
    public int hit_Select;

    //Ataque distancia
    public GameObject fireBall;
    public Transform bulletSpawn;
    public float dangerZone;
    public List<GameObject> pool = new List<GameObject>();

    // Invocacion
    public GameObject[] enemies;
    private int enemyIndex;
    private float spawnSpacing = 10f;

    // TP
    public GameObject tpPoint;
    public float tpCounter, timeBeforeEscape = 5f;

    // Fases
    public int fase = 1;
    public float currHp, maxHp;
    public bool dead;

    public AudioSource music;


    //  Todo lo demas
    public UnityEngine.AI.NavMeshAgent enemy;
    private Transform player;
    private AttributesManager playerATM;
    public LayerMask playerLayer;
    public float health;

    private bool isMoving, playerInDangerZone;

    private Vector3 lookPosition;

    private void Awake(){
        target = GameObject.FindGameObjectWithTag("Player");
        player = target.GetComponent<Transform>();
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
              // AUN NO ESTA IMPLEMENTADO
              // animator.SetTrigger("dead");
              // music.enabled = false;
              // dead = true;
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
            var lookPos = player.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            bulletSpawn.LookAt(player.position);  // O targer.transform.position CORREGIR ROTACION EN X
            
            // Detecta si el jugador esta demaciado cerca del enemigo
            playerInDangerZone = Physics.CheckSphere(transform.position, dangerZone, playerLayer);
            if(playerInDangerZone) tpCounter += 1 * Time.deltaTime;
            
            // ChasePlayer();
            
            if(tpCounter > timeBeforeEscape)
            {
              Debug.Log("NIGERUNDAYOOOOO");
              cronometro = 0;
              tpCounter = 0;
              rutina = 3; // Escapa del jugador luego de X tiempo
            }
            else if(!isAttacking)
            {
            //     isMoving = false;
                enemy.SetDestination(transform.position);
                lookPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                //transform.LookAt(lookPosition);
                // animator.SetBool("attack", !isAttacking);

                switch (rutina)
                {
                    case 0:
                        // Controlador Rutina
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 5f);
                        
                        cronometro += 1 * Time.deltaTime;
                        if(cronometro > timeBetweeAttacks) //time_rutinas
                        {
                            rutina = Random.Range(0, 4); // Valor maximo + 1 (Val. máximo es exclusivo [documentación])
                            cronometro = 0;
                        }
                        break;
                    case 1:
                        //Invocar minions
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2f);

                        // animator.SetBool("isMoving", false);
                        animator.SetBool("attack", false);
                        animator.SetBool("teleport", false);
                        // animator.SetBool("skills", 0);
                        if(fase == 1) animator.SetBool("summon", true);
                        else animator.SetBool("summon", true);
                        break;
                    case 2:
                        //Ataque Distancia
                        // animator.SetBool("isMoving", false);
                        animator.SetBool("summon", false);
                        animator.SetBool("teleport", false);
                        animator.SetBool("attack", true);
                        // animator.SetBool("skills", 0);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                        break;
                    case 3:
                        //TP
                        // animator.SetBool("isMoving", false);
                        animator.SetBool("summon", false);
                        animator.SetBool("attack", false);
                        animator.SetBool("teleport", true);
                        break;
                }
            }

            // animator.SetBool("isMoving", isMoving);
        }
    }

    private void ChasePlayer(){
        enemy.SetDestination(player.position);
        isMoving = true;
    }

    public void ResetAttack(){
        animator.SetBool("attack", false);
        animator.SetBool("summon", false);
        animator.SetBool("teleport", false);
        rutina = 0;
        isAttacking = false;
    
    }

    public GameObject GetFireBall()
    {
        // Este codigo recicla los proyectiles en caso de lanzar varios
        for(int i = 0; i < pool.Count; i++)
        {
          if(!pool[i].activeInHierarchy)
          {
            pool[i].SetActive(true);
            return pool[i];
          }
        }
        GameObject obj = Instantiate(fireBall, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        obj.GetComponent<LichBullets>().creador = this.gameObject;
        pool.Add(obj);
        return obj;
    }

    private void AttackPlayer(){
        /// Attack code here
        isAttacking = true;
        // bulletSpawn.LookAt(player);
        
        // Rigidbody rb = Instantiate(fireBall, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        // rb.gameObject.GetComponent<EnemyBullets>().creador = this.gameObject;
        
        // rb.AddForce(bulletSpawn.forward * 32f, ForceMode.Impulse);
        // rb.AddForce(bulletSpawn.up * 5f, ForceMode.Impulse);

        GameObject obj = GetFireBall();
        obj.transform.position = bulletSpawn.transform.position;
        obj.transform.rotation = bulletSpawn.transform.rotation;

        //Invoke(nameof(ResetAttack), timeBetweeAttacks);
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

        //Invoke(nameof(ResetAttack), timeBetweeAttacks);
    }

    private void Teleport()
    {
        isAttacking = true;
        Vector3 center = tpPoint.transform.position;
        Vector2 randomCircle = Random.insideUnitCircle * 50;
        Vector3 tpPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);
        gameObject.transform.position = tpPosition;
    }
    

    public void TakeDamage(int damage){
        health -= damage;
        
        if(health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerZone);
    }
}
