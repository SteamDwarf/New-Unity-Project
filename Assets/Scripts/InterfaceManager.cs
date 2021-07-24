using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    private GameObject inventory;
    private GameObject pauseMenu;
    private GameManager gm;
    private Inventory inventoryScript;
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
        if(pauseMenuShowed) {
            return;
        }

        if(inventoryScript == null) {
            inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        ShowHideMenu(inventory, ref inventoryShowed, inventoryScript);
    }
    public void ShowHidePauseMenu() {
        if (inventoryShowed) {
            return;
        }
        ShowHideMenu(pauseMenu, ref pauseMenuShowed);
    }
    public void ShowHideSettingsMenu() {
        ShowHideMenu(settingsMenu, ref settingsMenuShowed);
    }
}
