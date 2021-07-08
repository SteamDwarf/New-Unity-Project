using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager GM;
    private GameObject gameManager;
    private InterfaceManager interfaceManager;
    [SerializeField] private GameObject inputControllerGO;
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
        Debug.Log("���� ���������");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
       interfaceManager.ShowHidePauseMenu();
        inputController.SwitchState<InGameState>();
    }
}
