using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public float attack;
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    
    [SerializeField] public int currentLevel, currentExperience, maxExperience, expValue;

    private GameObject player;
 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            Debug.Log("VALOR DE EXP AL MATAR UN ENEMIGO: " + expValue);
            ExperienceManager.Instance.AddExperienceHandler(player, expValue);
            Destroy(gameObject);
        }
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();
        if(atm != null)
        {
            atm.TakeDamage(attack);
        }
    }
    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }


 // Cosas relacionadas a la experiencia
    public void AddExperience(int amount)
    {
        currentExperience += amount;
        Debug.Log("VALOR DE EXP AL ENTRAR A ADDEXPERIENCE: " + amount);
        if(currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        maxHealth += 10;
        attack += 100;
        currentHealth = maxHealth;
        currentLevel++;
        currentExperience = 0;

        maxExperience += 100;
    }
}
