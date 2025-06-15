using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    public Slider expSlider;
    public AttributesManager atm;
    private float lerpSpeed = 0.05f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(expSlider.value != atm.currentExperience)
        {
            expSlider.value = Mathf.Lerp(expSlider.value, atm.currentExperience, lerpSpeed);
        }


    }
}
