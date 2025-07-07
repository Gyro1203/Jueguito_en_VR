using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    //public LayerMask ground; 
    public float weaponDamage;
    private AttributesManager enemyAtm;
    private PlayerMovement playerMove;
    
    void Start()
    {
        enemyAtm = GetComponentInParent<AttributesManager>();
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(playerMove != null && playerMove.isProtected)
        {
            playerMove.ProtectDamage(enemyAtm.attack);
        }
        else if(other.gameObject.tag == "Player") 
        {
            enemyAtm.DealDamage(other.gameObject);
        }
    }
}
