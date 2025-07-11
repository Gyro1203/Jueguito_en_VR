using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiedScreen : MonoBehaviour
{
    public float fadeDuration = 2;
    public Image YouDied;
    private bool isOut = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void FadeIn()
    {
        StartCoroutine(FadeRoutine(YouDied, fadeDuration));
    }

    private IEnumerator FadeRoutine(Image imagen, float duracion)
    {
        Color color = imagen.color;
        color.a = 0f;             // Asegura que empieza completamente transparente
        imagen.color = color;

        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Clamp01(tiempo / duracion);

            color.a = alpha;
            imagen.color = color;

            yield return null;
        }

        // Asegura que quede totalmente visible al final
        color.a = 1f;
        imagen.color = color;
    }
}
