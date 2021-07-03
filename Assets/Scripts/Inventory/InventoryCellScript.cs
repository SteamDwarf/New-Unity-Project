using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CellType {
    inventory, hotBar
}
public class InventoryCellScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int cellId;
    [SerializeField] private CellType cellType;
    private Inventory inventory;

    public void OnPointerClick(PointerEventData eventData) { 

        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            inventory.CreateContextMenu(cellId, transform.position);
        }

        if(eventData.button == PointerEventData.InputButton.Left){
            inventory.MoveItem(cellId);
        }
    }
}
