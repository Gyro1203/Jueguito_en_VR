using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiedScreen : MonoBehaviour
{
    public float fadeDuration = 2;
    public Material fadeMat;
    public AnimationCurve fadeCurve;
    public string colorPropertyName = "_Color";
    private Renderer rend;
    private bool isOut = true;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }
    
    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn,alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn,float alphaOut)
    {
        rend.enabled = true;

        float timer = 0;
        while(timer <= fadeDuration)
        {
            rend.material.SetTexture("_BaseMap", fadeMat.GetTexture("_BaseMap"));
            Color newColor = fadeMat.GetColor("_BaseColor");
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, fadeCurve.Evaluate(timer / fadeDuration));

            rend.material.SetColor(colorPropertyName, newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeMat.GetColor("_BaseColor");
        newColor2.a = alphaOut;
        rend.material.SetColor(colorPropertyName, newColor2);

        if(alphaOut == 0)
            rend.enabled = false;

        if(isOut){
            isOut = false;
            fadeDuration = 2;
            Invoke(nameof(FadeIn), 2f);
        }   
    }
}
