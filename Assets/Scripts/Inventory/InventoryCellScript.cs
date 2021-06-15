using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCellScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject contextMenu;
    [SerializeField] private GameObject useBtn;
    [SerializeField] private GameObject descriptionBtn;
    [SerializeField] private GameObject dropBtn;
    [SerializeField] private int cellId;

    private Inventory inventory;

    private void Start()
    {
        
        //inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void OnPointerClick(PointerEventData eventData)
    { 
        if(inventory == null)
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            inventory.ShowContextMenu(cellId);
        }

        /*if(eventData.button == PointerEventData.InputButton.Left)
        {
            inventory.MoveItem(cellId);
        }*/

    }

    public void Use()
    {
        inventory.UpdateItemInformation();
    }

    /*public void ShowContextMenu()
    {
        inventory.ShowContextMenu(cellId);
    }*/
}
