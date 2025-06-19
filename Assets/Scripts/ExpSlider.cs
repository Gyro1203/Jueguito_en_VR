using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpSlider : MonoBehaviour
{
    public Slider expSlider;
    private AttributesManager atm;
    private float lerpSpeed = 0.05f;
    [SerializeField] TextMeshProUGUI levelText;

    // Start is called before the first frame update
    void Start()
    {

        atm = GameObject.FindGameObjectWithTag("Player").GetComponent<AttributesManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
        expSlider.maxValue = atm.maxExperience;
        levelText.text = atm.currentLevel.ToString();

        if(expSlider.value != atm.currentExperience)
        {
            expSlider.value = Mathf.Lerp(expSlider.value, atm.currentExperience, lerpSpeed);
        }

    }
}
