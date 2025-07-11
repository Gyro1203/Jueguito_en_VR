using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent enemy;
    private Transform player;
    public LayerMask whatIsPlayer;
    public float health;
    //public float speed = 3f;

    private bool isMoving;
    private Animator animator;

    public float timeBetweeAttacks;
    bool alreadyAttacked;
    public GameObject proyectile;
    public Transform bulletSpawn;

    public float attackRange;
    public bool playerInAttackRange;
    private Vector3 lookPosition;

    private void Awake(){
        //player = GameObject.Find("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // void Start()
    // {
    //     player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    //     enemy = GetComponent<NavMeshAgent>();
    // }

    void Update()
    {
        //Check for attack range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        //Debug.Log(playerInAttackRange);

        ChasePlayer();
        
        if(playerInAttackRange) AttackPlayer();
        animator.SetBool("isMoving", isMoving);

        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //transform.LookAt(player);
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void ChasePlayer(){
        enemy.SetDestination(player.position);
        isMoving = true;
    }

    private void AttackPlayer(){
        isMoving = false;
        
        enemy.SetDestination(transform.position);

        lookPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPosition);

        if(!alreadyAttacked)
        {
            /// Attack code here
            animator.SetTrigger("attack");

            Rigidbody rb = Instantiate(proyectile, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.gameObject.GetComponent<BulletScript>().creador = this.gameObject;
                
            rb.AddForce(bulletSpawn.forward * 32f, ForceMode.Impulse);
            rb.AddForce(bulletSpawn.up * 1f, ForceMode.Impulse);


            alreadyAttacked = true;
            //LLama a una funcion luego de un tiempo definido
            Invoke(nameof(ResetAttack), timeBetweeAttacks);
        }
    }

    private void ResetAttack(){
        alreadyAttacked = false;
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
