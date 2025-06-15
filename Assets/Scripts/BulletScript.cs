using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //public LayerMask ground; 
    private AttributesManager playerAtm;
    public GameObject creador;
    
    void Start()
    {
        playerAtm = GameObject.FindGameObjectWithTag("Player").GetComponent<AttributesManager>();
        // Debug.Log("Fui creado por: " + creador);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer);
        // Actualmente el layer nueve es para el suelo, el if me devuelve el numero del layer
        if(other.gameObject.layer == 9) {
            Destroy(gameObject);
        }

        // 7 es Player, 8 es Enemy
        if(other.gameObject.layer == 7 || other.gameObject.layer == 8) {
            // Debug.Log(creador.GetComponent<AttributesManager>().attack);
            creador.GetComponent<AttributesManager>().DealDamage(other.gameObject);
        }
    }
}
