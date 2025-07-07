using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    //public LayerMask ground; 
    public float weaponDamage;
    private float speed;
    private Vector3 prevPos, velocity;
    private GameObject player;
    private AttributesManager playerAtm;
    private PlayerMovement pm;
    private Rigidbody rb;
    private Collider weaponCollider;

    public enum Type
    {
        Espada,
        Hacha,
        Lanza,
        Martillo,
        Baston,
        Catalizador
    }
    public Type weaponType;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAtm = player.GetComponent<AttributesManager>();
        pm = player.GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        weaponCollider = GetComponent<Collider>();
        prevPos = transform.position;
    }

    void Update()
    {
        //  No funciona con Kinematic
        //  speed = rb.velocity.magnitude;

        velocity = (transform.position - prevPos) / Time.deltaTime;
        speed = velocity.magnitude;

        //Debug.Log("Is Moving:" + pm.isMoving);
        //if(speed > 1f && pm.isMoving) Debug.Log("RB Speed:" + speed);
        
        // IDEA: if(isMoving) -> mitad de daño
        // Asi se evita el bloquear ataque mientras corres
        // pero no hace tanto daño como un swing

        if(weaponCollider != null)
        {
            weaponCollider.enabled = speed > 1f;
        }

        prevPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Daño");
        if(other.gameObject.tag == "Enemy") playerAtm.DealDamage(other.gameObject);
    }
}
