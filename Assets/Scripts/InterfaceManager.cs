using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    private GameObject inventory;
    private GameObject pauseMenu;
    private GameManager gm;
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

    // Update is called once per frame
    void Update()
    {
        MenusManager();
    }

    private void MenusManager()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHideMenu(pauseMenu, ref pauseMenuShowed);     
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
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
        } else
        {
            menuStatus = false;
            gm.isPaused = false;
            menu.SetActive(false);
        }
    }
}
