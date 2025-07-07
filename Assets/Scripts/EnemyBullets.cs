using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    [HideInInspector] public GameObject creador;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && creador != null)
        {
            creador.GetComponent<AttributesManager>().DealDamage(other.gameObject);
        }
        
        Destroy(gameObject);
    }
}
