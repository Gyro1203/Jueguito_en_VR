using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichBullets : MonoBehaviour
{
    public float damage;
    private float cronometro;

    [HideInInspector] public GameObject target;

    // Update is called once per frame
    void Update()
    {
        cronometro += 1 * Time.deltaTime;
        if(cronometro > 3)
        {
            gameObject.SetActive(false);
            cronometro = 0;
        }
        transform.Translate(Vector3.forward * 20 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && target != null)
        {
            target.GetComponent<AttributesManager>().TakeDamage(damage);
        }
        
        // Destroy(gameObject);
    }
}
