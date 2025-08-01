using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Buster : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    [Header("Input Action")]
    public InputActionAsset inputAction; // Asignas el mismo asset que usas en el menú
    private InputAction fireAction;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnEnable()
    {
        var map = inputAction.FindActionMap("Shoot"); // Este es el mapa que incluye el gatillo
        fireAction = map.FindAction("Fire"); // "Activate" se usa comúnmente para el trigger
        
        fireAction.Enable();
        fireAction.performed += OnFire;
    }

    void OnDisable()
    {
        if (fireAction != null)
        {
            fireAction.performed -= OnFire;
            fireAction.Disable();
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (isHeld)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("¡Disparo!");
        if (bullet != null && firePoint != null)
        {
            GameObject obj = Instantiate(bullet, firePoint.position, firePoint.rotation) as GameObject;
            obj.transform.position = firePoint.transform.position;
            obj.transform.rotation = firePoint.transform.rotation;
        }
    }
}
