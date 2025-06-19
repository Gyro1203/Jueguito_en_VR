using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{

public static ExperienceManager Instance;

// Check para asegurarse que solamente exista un manager de experiencia al mismo tiempo
  private void Awake(){
    if(Instance != null && Instance != this)
    {
        Destroy(this);
    }else
    {
        Instance = this;
    }
  }

  public void AddExperienceHandler(GameObject target, int amount)
  {
    Debug.Log("EXPERIENCIA AL ENTRAR AL HANDLER: " + amount + target);
    var atm = target.GetComponent<AttributesManager>();
    if (atm != null)
    {
        atm.AddExperience(amount);
    }
  }

}
