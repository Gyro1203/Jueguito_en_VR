using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public AttributesManager playerAtm;
    public AttributesManager enemyAtm;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            if(enemyAtm.currentHealth <= 0){
                Destroy(enemyAtm.gameObject);
            }else
            {
                playerAtm.DealDamage(enemyAtm.gameObject);
            }
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            if(playerAtm.currentHealth <= 0){
                Destroy(playerAtm.gameObject);
            }else
            {
                enemyAtm.DealDamage(playerAtm.gameObject);
            }
        }
    }
}
