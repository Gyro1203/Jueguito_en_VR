using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    public float maxDurability;
    [HideInInspector]
    public float currentDurability;
    
    void Start()
    {
        currentDurability = maxDurability;
    }

    void Update()
    {
        if(currentDurability <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    public void TakeDamage(float amount)
    {
        currentDurability -= amount;
    }
}
