using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IMenu
{
    [SerializeField] private GameObject inputControllerGO;
    [SerializeField] private GameObject settingsWindow;

    private GameManager GM;
    private GameObject gameManager;
    private InterfaceManager interfaceManager;
    private InputController inputController;

    private void Start()
    {
        try
        {
            gameManager = GameObject.Find("GameManager");
            GM = gameManager.GetComponent<GameManager>();
            inputController = inputControllerGO.GetComponent<InputController>();
            interfaceManager = gameManager.GetComponent<InterfaceManager>();
        }
        catch (System.Exception)
        {
            Debug.Log("In Menu");
        }
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume() {  
        interfaceManager.ShowHidePauseMenu();
        inputController.SwitchState<InGameState>();
    }

    public void ShowSettings() {
        settingsWindow.SetActive(true);
    }
    public void HideSettings() {
        settingsWindow.SetActive(false);
        settingsWindow.GetComponent<AudioSettingsManager>().CloseSettings();
    }
    public void OpenMenu(){}
    public void CloseMenu() {
        if(settingsWindow.activeInHierarchy) {
            settingsWindow.SetActive(false);
            settingsWindow.GetComponent<AudioSettingsManager>().CloseSettings();
        }
    }
}
