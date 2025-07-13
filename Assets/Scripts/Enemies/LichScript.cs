using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichScript : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public float time_rutinas;
    public Animator animator;
    public Quaternion angulo;
    public float grado;
    public GameObject target;
    //public bool atacando;
    // public RangoLich rango;
    public GameObject[] hit;
    public int hit_Select;

    //Ataque distancia
    public GameObject proyectile;
    public Transform bulletSpawn;
    public float attackRange;
    public List<GameObject> pool = new List<GameObject>();

    //Invocacion
    public GameObject[] enemies;
    private int enemyIndex;
    private float spawnSpacing = 10f;

    // TP
    public GameObject tpPoint;

    public UnityEngine.AI.NavMeshAgent enemy;
    private Transform player;
    private AttributesManager playerATM;
    public LayerMask playerLayer;
    public float health;

    private bool isMoving, isAttacking, playerInAttackRange;

    public float timeBetweeAttacks;

    private Vector3 lookPosition;

    private void Awake(){
        target = GameObject.FindGameObjectWithTag("Player");
        player = target.GetComponent<Transform>();
        playerATM = player.GetComponent<AttributesManager>();
        enemy = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        isAttacking = false;
    }

    void Update()
    {
        if(!playerATM.imDying)
        {
            //Check for attack range
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            ChasePlayer();
            
            if(playerInAttackRange)
            {
                isMoving = false;
                enemy.SetDestination(transform.position);
                lookPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(lookPosition);
                // animator.SetBool("attack", !isAttacking);

                switch (rutina)
                {
                    case 0:
                        // Controlador Rutina
                        cronometro += 1 * Time.deltaTime;
                        if(cronometro > timeBetweeAttacks) //time_rutinas
                        {
                            rutina = Random.Range(0, 3);
                            cronometro = 0;
                        }
                        break;
                    case 1:
                        //Invocar minions
                        animator.SetBool("attack", false);
                        animator.SetBool("isMoving", false);
                        animator.SetBool("summon", true);
                        break;
                    case 2:
                        //Ataque Distancia
                        animator.SetBool("isMoving", false);
                        animator.SetBool("summon", false);
                        animator.SetBool("attack", true);
                        break;
                }
            } 

            animator.SetBool("isMoving", isMoving);
        }
    }

    private void ChasePlayer(){
        enemy.SetDestination(player.position);
        isMoving = true;
    }

    public void ResetAttack(){
        animator.SetBool("attack", false);
        animator.SetBool("summon", false);
        rutina = 0;
        isAttacking = false;
    }

    private void AttackPlayer(){
        /// Attack code here
        isAttacking = true;
        bulletSpawn.LookAt(player);
        
        Rigidbody rb = Instantiate(proyectile, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.gameObject.GetComponent<EnemyBullets>().creador = this.gameObject;
        
        rb.AddForce(bulletSpawn.forward * 32f, ForceMode.Impulse);
        rb.AddForce(bulletSpawn.up * 5f, ForceMode.Impulse);

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
        Vector3 center = tpPoint.transform.position;
        Vector2 randomCircle = Random.insideUnitCircle * 50;
        Vector3 tpPosition = center + new Vector3(randomCircle.x, 0, randomCircle.y);
        gameObject.transform.position = tpPosition.transform.position;
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
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
