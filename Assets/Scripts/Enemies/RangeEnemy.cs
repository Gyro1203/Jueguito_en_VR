using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemy : MonoBehaviour
{
    public NavMeshAgent enemy;
    private Transform player;
    private AttributesManager playerATM;
    private AttributesManager atm;
    public LayerMask playerLayer;
    private Animator animator;
    public float health;

    private bool isMoving, isAttacking, playerInAttackRange;

    public float timeBetweeAttacks;
    public GameObject proyectile;
    public Transform bulletSpawn;

    public float attackRange;
    private Vector3 lookPosition;

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerATM = player.GetComponent<AttributesManager>();
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        atm = GetComponent<AttributesManager>();
        isAttacking = false;
    }

    void Update()
    {
        if(atm.currentHealth > 0)
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
                    animator.SetBool("attack", !isAttacking);
                } 

                animator.SetBool("isMoving", isMoving);
            }
        }
        else
        {
            // if(!dead)
            // {
            //   animator.SetTrigger("dead");
            //   // music.enabled = false;
            //   dead = true;
            //   cronometro = 0;
            // }
            // else
            // {
            //   cronometro += 1 * Time.deltaTime;
            //   if(cronometro > timeAfterDespawn)
            //   {
            //     Destroy(gameObject);
            //   }
            // }
            Destroy(gameObject);
        }
    }

    private void ChasePlayer(){
        enemy.SetDestination(player.position);
        isMoving = true;
    }

    private void AttackPlayer(){
        /// Attack code here
        isAttacking = true;
        bulletSpawn.LookAt(player);
        
        Rigidbody rb = Instantiate(proyectile, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.gameObject.GetComponent<EnemyBullets>().creador = this.gameObject;
        
        rb.AddForce(bulletSpawn.forward * 32f, ForceMode.Impulse);
        rb.AddForce(bulletSpawn.up * 5f, ForceMode.Impulse);

        Invoke(nameof(ResetAttack), timeBetweeAttacks);
    }

    private void ResetAttack(){
        isAttacking = false;
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
