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

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        //if(context.performed) 
        Debug.Log("BOTON PRESIONADO");
        DisplayUI();
    }

    public void DisplayUI()
    {
        if(activeUI)
        {
            HideMenu();
            menuUI.SetActive(false);
            activeUI = false;
        }
        else if(!activeUI)
        {
            menuUI.SetActive(true);
            EnableMenu();
            activeUI = true;
        }
    }
    
    public void EnableMenu()
    {
        if(activeUI)
        menu.SetActive(true);
        restartConfirm.SetActive(false);
        mainMenuConfirm.SetActive(false);
    }

    public void ResumeGame()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        HideMenu();
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
