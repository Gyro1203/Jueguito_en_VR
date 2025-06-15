using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    /// Todo lo del Start no estaba incluido, pero ya que el prefab no guarda el GameObject del player
    /// lo agregue para evitar errores. Sin esto, se debe arratrar directo player hasta cam en el inspector.
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerCamera.transform;
    }

    // La barra sige la camara (vista) del jugador
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
