using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    private GameObject inventory;
    private GameObject pauseMenu;
    /*private CanvasGroup inventory;
    private CanvasGroup pauseMenu;*/
    private GameManager gm;
    private bool inventoryShowed = false;
    private bool pauseMenuShowed = false;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        /*inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<CanvasGroup>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<CanvasGroup>();*/
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        inventory.SetActive(false);
        pauseMenu.SetActive(false);
        /*inventory.alpha = 0;
        inventory.interactable = false;
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;*/
    }


    //При закытии инвентаря все контекстные менюшк должны закрываться


    void Update()
    {
        MenusManager();
    }

    private void MenusManager()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryShowed)
                return;

            ShowHideMenu(pauseMenu, ref pauseMenuShowed);     
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (pauseMenuShowed)
                return;

            ShowHideMenu(inventory, ref inventoryShowed);
        }   
    }


    private void ShowHideMenu(GameObject menu, ref bool menuStatus)
    {
        if(!menuStatus)
        {
            menuStatus = true;
            gm.isPaused = true;
            menu.SetActive(true);
            /*menu.alpha = 1;
            menu.interactable = true;*/
        } else
        {
            menuStatus = false;
            gm.isPaused = false;
            menu.SetActive(false);
            /*menu.alpha = 0;
            menu.interactable = false;*/
        }
    }
}
