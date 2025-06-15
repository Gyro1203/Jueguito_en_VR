using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public AttributesManager atm;
    private float lerpSpeed = 0.05f;
    // public float maxHealth = 100f;
    // public float health;

    // void Start()
    // {
    //     health = maxHealth;
    // }

    void Update()
    {
        if(healthSlider.value != atm.currentHealth)
        {
            healthSlider.value = atm.currentHealth; 
        }

        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, atm.currentHealth, lerpSpeed); 
        }
    }
}
