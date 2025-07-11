using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject menuUI;

    [Header("UI Pages")]
    public GameObject menu;
    public GameObject restartConfirm;
    public GameObject mainMenuConfirm;

    [Header("Pause Menu Buttons")]
    public Button resumeButtom;
    public Button restartButton;
    public Button confirmRestartButton;
    public Button mainMenuButton;
    public Button confirmMainMenuButton;

    public List<Button> returnButtons;

    private bool activeUI = true;

    public InputActionAsset inputAction;
    private InputAction xButton;

    public GameObject LeftRay;
    public GameObject RightRay;

    // Start is called before the first frame update
    void Start()
    {
        DisplayUI();

        //Hook events
        resumeButtom.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(EnableRestartConfirm);
        confirmRestartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(EnableMainMenuConfirm);
        confirmMainMenuButton.onClick.AddListener(ReturnToMainMenu);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMenu);
        }
    }

    void OnEnable()
    {
        var map = inputAction.FindActionMap("Menu");
        xButton = map.FindAction("MenuPressed");
        xButton.Enable();
        xButton.performed += OnMenuPressed;
    }

    void OnDisable()
    {
        xButton.performed -= OnMenuPressed;
        xButton.Disable();
    }

    private void OnMenuPressed(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            DisplayUI();
        }
    }

    public void DisplayUI()
    {
        if(activeUI)
        {
            menuUI.SetActive(false);
            activeUI = false;
            LeftRay.SetActive(false);
            RightRay.SetActive(false);
            Time.timeScale = 1;
        }
        else if(!activeUI)
        {
            LeftRay.SetActive(true);
            RightRay.SetActive(true);
            menuUI.SetActive(true);
            EnableMenu();
            activeUI = true;
            Time.timeScale = 0;
        }
    }
    
    public void EnableMenu()
    {
        menu.SetActive(true);
        restartConfirm.SetActive(false);
        mainMenuConfirm.SetActive(false);
    }

    public void ResumeGame()
    {
        HideMenu();
        DisplayUI();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        HideMenu();
        DisplayUI();
        SceneTransitionManager.singleton.GoToSceneAsync(0);
    }

    public void HideMenu()
    {
        menu.SetActive(false);
        restartConfirm.SetActive(false);
        mainMenuConfirm.SetActive(false);
    }


    public void EnableRestartConfirm()
    {
        menu.SetActive(false);
        restartConfirm.SetActive(true);
        mainMenuConfirm.SetActive(false);
    }
    public void EnableMainMenuConfirm()
    {
        menu.SetActive(false);
        restartConfirm.SetActive(false);
        mainMenuConfirm.SetActive(true);
    }
}
