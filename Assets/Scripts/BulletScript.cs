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
        //Debug.Log(other.tag);

        // 7 es Player, 8 es Enemy
        if(other.gameObject.layer == 7 || other.gameObject.layer == 8)
        {
            // Debug.Log(creador.GetComponent<AttributesManager>().attack);
            if(creador != null)
            {
                creador.GetComponent<AttributesManager>().DealDamage(other.gameObject);
            }
        }

        Destroy(gameObject);
    }
}
