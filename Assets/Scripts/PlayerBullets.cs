using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets : MonoBehaviour
{
    //public LayerMask ground; 
    private AttributesManager playerAtm;
    
    void Start()
    {
        playerAtm = GameObject.FindGameObjectWithTag("Player").GetComponent<AttributesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy") playerAtm.DealDamage(other.gameObject);
        if(other.gameObject.tag != "Player") Destroy(gameObject);
    }
}

