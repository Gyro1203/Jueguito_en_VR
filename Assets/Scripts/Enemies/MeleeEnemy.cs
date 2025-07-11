using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    public NavMeshAgent enemy;
    private Transform player;
    public LayerMask playerLayer;
    private Animator animator;
    public float health;

    private bool canMove, canAttack, playerInAttackRange;

    public float timeBetweeAttacks;

    public float attackRange;
    private Vector3 lookPosition;

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        canAttack = true;
        canMove = true;
    }

    void Update()
    {
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
