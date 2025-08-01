using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Buster : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bigBullet;
    public Transform firePoint;
    [SerializeField] private GameObject yellow;
    [SerializeField] private GameObject blue;
    private bool isCharging = false;
    private float cronometro = 0f;  

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
        fireAction.started += OnFireStarted;
        fireAction.canceled += OnFireCanceled;
        // fireAction.performed += OnFire;
    }
    
    void Update()
    {
        if (isHeld && isCharging)
        {
            cronometro += Time.deltaTime;

            if (cronometro > 3f)
            {
                yellow.SetActive(false);
                blue.SetActive(true); // Fully charged
            }
            else
            {
                yellow.SetActive(true); // Charging
                blue.SetActive(false);
            }
        }
    }

    void OnDisable()
    {
        if (fireAction != null)
        {
            // fireAction.performed -= OnFire;
            fireAction.started -= OnFireStarted;
            fireAction.canceled -= OnFireCanceled;
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

    // private void OnFire(InputAction.CallbackContext context)
    // {
    //     if (isHeld)
    //     {
    //         Shoot();
    //     }
    // }

    private void OnFireStarted(InputAction.CallbackContext context)
    {
        if (isHeld)
        {
            Debug.Log("Botón presionado - empieza disparo");
            isCharging = true;
            cronometro = 0f;
        }
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        if (isHeld)
        {
            Debug.Log("Botón soltado - termina disparo");

            if (cronometro > 3f)
            {
                Debug.Log("Disparo cargado");
                GameObject obj = Instantiate(bigBullet, firePoint.position, firePoint.rotation);
                // Opcional: agregar fuerza, efectos, etc.
            }
            else
            {
                Debug.Log("Disparo normal");
                GameObject obj = Instantiate(bullet, firePoint.position, firePoint.rotation);
            }

            cronometro = 0f;
            blue.SetActive(false);
            yellow.SetActive(false);
        }
    }


    // private void Shoot()
    // {
    //     Debug.Log("¡Disparo!");
    //     if (bullet != null && firePoint != null)
    //     {
    //         GameObject obj = Instantiate(bullet, firePoint.position, firePoint.rotation) as GameObject;
    //         obj.transform.position = firePoint.transform.position;
    //         obj.transform.rotation = firePoint.transform.rotation;
    //     }
    // }
}
