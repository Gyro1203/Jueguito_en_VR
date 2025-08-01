using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    public NavMeshAgent enemy;
    private Transform player;
    private AttributesManager playerATM;
    public LayerMask playerLayer;
    private Animator animator;
    private AttributesManager atm;
    public bool dead;

    private bool canMove, canAttack, playerInAttackRange;

    public float timeBetweeAttacks;

    public float attackRange;
    private Vector3 lookPosition;

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerATM = player.GetComponent<AttributesManager>();
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        atm = GetComponent<AttributesManager>();
        canAttack = true;
        canMove = true;
    }

    void Update()
    {
        
        if(atm.currentHealth > 0)
        {
            if(!playerATM.imDying){
                //Check for attack range
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

                if(canMove) ChasePlayer();
                
                if(playerInAttackRange)
                {
                    canMove = false;
                    enemy.SetDestination(transform.position);
                    lookPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                    transform.LookAt(lookPosition);
                    if(canAttack)
                    {
                        animator.SetBool("attack", canAttack);
                    }
                }

                animator.SetBool("run", canMove);
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
    }

    public void AttackPlayer(){
        canAttack = false;
        animator.SetBool("attack", canAttack);
        canMove = true;
        Invoke(nameof(ResetAttack), timeBetweeAttacks);
    }

    private void ResetAttack(){
        canAttack = true;
        canMove = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
