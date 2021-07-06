using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private GameObject gm;
    private InterfaceManager interfaceManager;

    void Start()
    {
        interfaceManager = gm.GetComponent<InterfaceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UseCell();
        CallUI();
    }

    private void UseCell()
    {
        int cellInd;

        if (Input.GetKeyDown("1"))
            cellInd = 0;
        else if (Input.GetKeyDown("2"))
            cellInd = 1;
        else if (Input.GetKeyDown("3"))
            cellInd = 2;
        else if (Input.GetKeyDown("4"))
            cellInd = 3;
        else if (Input.GetKeyDown("5"))
            cellInd = 4;
        else{
            return;
        }

        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        inventory.UseItem(cellInd);
    }

    private void CallUI() {
        if (Input.GetKeyDown(KeyCode.I)){
            interfaceManager.ShowHideInventory();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            interfaceManager.ShowHidePauseMenu();
        }
    }

}
