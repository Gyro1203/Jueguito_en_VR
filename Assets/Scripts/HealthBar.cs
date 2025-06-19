using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public AttributesManager atm;
    private float lerpSpeed = 0.05f;
    [SerializeField] TextMeshProUGUI life;
    // public float maxHealth = 100f;
    // public float health;

    // void Start()
    // {
    //     health = maxHealth;
    // }

    void Update()
    {
        // Actualiza los valores de vida maxima
        healthSlider.maxValue = atm.maxHealth;
        easeHealthSlider.maxValue = atm.maxHealth;

        // Texto de la barra de vida
        if(gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            life.text = atm.currentHealth.ToString() + " / " + atm.maxHealth.ToString();
        }

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
