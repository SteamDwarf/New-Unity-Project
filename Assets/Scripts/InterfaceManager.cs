using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    private GameObject inventory;
    private GameObject pauseMenu;
    private GameManager gm;
    private Inventory inventoryScript;
    private bool inventoryShowed = false;
    private bool pauseMenuShowed = false;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        inventory.SetActive(false);
        pauseMenu.SetActive(false);
    }


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

            if(inventoryScript == null) {
                inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            }
            ShowHideMenu(inventory, ref inventoryShowed, inventoryScript);
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
    private void ShowHideMenu(GameObject menu, ref bool menuStatus, IMenu menuInterface)
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
            menuInterface.CloseMenu();
            /*menu.alpha = 0;
            menu.interactable = false;*/
        }
    }
}
