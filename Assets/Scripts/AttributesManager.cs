using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttributesManager : MonoBehaviour
{
    public float attack;
    public float maxHealth;
    public FadeScreen fadeScreen;
    public DiedScreen diedScreen;
     public bool inmortal = false;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool imPlayer, imDying;

    [SerializeField] public int currentLevel, currentExperience, maxExperience, expValue;

    private GameObject player;

    private SpawnManagerScript spawnManager;
 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(gameObject.CompareTag("Player")){
            imPlayer = true;
            imDying = false;
        }
        currentHealth = maxHealth;
        spawnManager = FindObjectOfType<SpawnManagerScript>();
    }

    void Update()
    {
        if(currentHealth <= 0)
        { 

            if(spawnManager != null)
            {
                spawnManager.enemiesKilled();
            }

            if(!imPlayer && !imDying)
            {
                imDying = true;
                Debug.Log("VALOR DE EXP AL MATAR UN ENEMIGO: " + expValue);
                ExperienceManager.Instance.AddExperienceHandler(player, expValue);
                if (gameObject.layer == LayerMask.NameToLayer("Boss"))
                {
                    spawnManager.OnBossKilled();
                }
                //Destroy(gameObject);
            }
            else if(!imDying)
            {
                imDying = true;
                fadeScreen.fadeDuration = 10;
                diedScreen.gameObject.SetActive(true);
                diedScreen.FadeIn();
                player.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
                var interactors = player.GetComponentsInChildren<XRBaseInteractor>();
                foreach (var interactor in interactors)
                {
                    interactor.enabled = false;
                }
                
                SceneTransitionManager.singleton.GoToSceneAsync(0);
            }

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
        if (!inmortal)
        {
            currentHealth -= amount;
        }
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
        attack += 10;
        currentHealth = maxHealth;
        currentLevel++;
        currentExperience = 0;

        maxExperience += 100;
    }
}
