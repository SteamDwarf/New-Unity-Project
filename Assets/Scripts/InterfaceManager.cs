using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    private GameObject inventory;
    private GameObject pauseMenu;
    private GameManager gm;
    private Inventory inventoryScript;
    private MainMenu pauseMenuScript;
    private bool inventoryShowed = false;
    private bool pauseMenuShowed = false;
    private bool settingsMenuShowed = false;

    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        inventory.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void ShowHideMenu(GameObject menu, ref bool menuStatus) {
        if(!menuStatus) {
            menuStatus = true;
            gm.isPaused = true;
            menu.SetActive(true);
        } else {
            menuStatus = false;
            gm.isPaused = false;
            menu.SetActive(false);
        }
    }
    private void ShowHideMenu(GameObject menu, ref bool menuStatus, IMenu menuInterface) {
        if(!menuStatus) {
            menuStatus = true;
            gm.isPaused = true;
            menu.SetActive(true);
            menuInterface.OpenMenu();
        } else {
            menuStatus = false;
            gm.isPaused = false;
            menu.SetActive(false);
            menuInterface.CloseMenu();
        }
    }

    public void ShowHideInventory() {
/*         if(pauseMenuShowed) {
            return;
        } */

        if(inventoryScript == null) {
            inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        ShowHideMenu(inventory, ref inventoryShowed, inventoryScript);
    }
    public void ShowHidePauseMenu() {
  /*       if (inventoryShowed) {
            return;
        } */

        if(pauseMenuScript == null) {
            pauseMenuScript = pauseMenu.GetComponentInChildren<MainMenu>();
        }
        ShowHideMenu(pauseMenu, ref pauseMenuShowed, pauseMenuScript);
    }
}
