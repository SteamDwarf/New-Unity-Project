using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;


public class InventoryCellScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int cellId;
    [SerializeField] private CellType cellType;
    [SerializeField] GameObject inputControllerGO;
    private InputController inputController; 
    private Inventory inventory;

    private void Start() {
        inputController = inputControllerGO.GetComponent<InputController>();
    }

    public void OnPointerClick(PointerEventData eventData) { 

        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            if(inputController.currentState is InventoryState ) {
                inventory.ShowContextMenu(cellId);
                inputController.SwitchState<InventoryItemInteractionState>();
            }
        }

        if(eventData.button == PointerEventData.InputButton.Left){
            if(inputController.currentState is InventoryState) {
                inventory.MoveItem(cellId);
            }
        }
    }
}
