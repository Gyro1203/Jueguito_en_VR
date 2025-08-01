using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets : MonoBehaviour
{
    //public LayerMask ground; 
    public float damage;
    private float cronometro = 0;

    private AttributesManager playerAtm;
    
    void Start()
    {
        playerAtm = GameObject.FindGameObjectWithTag("Player").GetComponent<AttributesManager>();
    }

    void Update()
    {
        cronometro += 1 * Time.deltaTime;
        if(cronometro > 3)
        {
            cronometro = 0;
            Destroy(gameObject);
        }
        transform.Translate(Vector3.forward * 10 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy") other.GetComponent<AttributesManager>().TakeDamage(damage);
        // playerAtm.DealDamage(other.gameObject);
    }
}

