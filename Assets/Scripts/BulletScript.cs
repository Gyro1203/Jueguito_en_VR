using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    //public LayerMask ground; 
    private AttributesManager playerAtm;
    
    void Start()
    {
        playerAtm = GameObject.FindGameObjectWithTag("Player").GetComponent<AttributesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer);
        // Actualmente el layer nueve es para el suelo, el if me devuelve el numero del layer
        if(other.gameObject.layer == 9) {
            Destroy(gameObject);
        }

        if(other.gameObject.layer == 8) {
            playerAtm.DealDamage(other.gameObject);
        }
    }
}
